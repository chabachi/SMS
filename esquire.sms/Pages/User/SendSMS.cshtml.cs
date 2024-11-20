using esquire.common.Models;
using esquire.services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.FileIO;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static Google.Apis.Requests.BatchRequest;

namespace esquire.sms.Pages.User
{
    public class DropdownItem
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }
    public class SendSMSModel : PageModel
    {
        [BindProperty]
        public SMSSendingDTO SMSSendingDTO { get; set; }
        public List<SMSSendingDTO> SMSSendingListDTO { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        private readonly IUsersService _userService;

        private const long MaxFileSizePerFile = 50 * 1024 * 1024; // 80MB
        public SendSMSModel(IUsersService usersService)
        {
            _userService = usersService;
        }


        public void OnGet()
        {
        }
        public bool ValidateMobileNumber(string mobileNumber)
        {
            // Regular expression to match a mobile number starting with 09 and exactly 11 digits
            string pattern = @"^09\d{9}$";
            return Regex.IsMatch(mobileNumber, pattern);
        }
        public IActionResult OnPost()
        {
            try
            {

                string email = User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault() != null ? User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault().Value : "";
                SMSSendingDTO.ExecutedBy = email;

                if (Request.Form.Files.Count > 0)
                {
                    SMSSendingListDTO = new List<SMSSendingDTO>();
                    var file = Request.Form.Files[0];
                    string errormessage = "";
                    if (file.Length > MaxFileSizePerFile)
                    {
                        ModelState.AddModelError("files", $"File '{file.FileName}' exceeds 50MB.");
                        return new JsonResult(new { success = false, message = $"File '{file.FileName}' exceeds 80MB." });
                    }

                    // Process the uploaded file
                    using (var stream = new StreamReader(file.OpenReadStream()))
                    using (var csvReader = new TextFieldParser(stream))
                    {
                        csvReader.SetDelimiters(new string[] { "," });
                        csvReader.HasFieldsEnclosedInQuotes = true;

                        // Skip the first row (header row)
                        csvReader.ReadLine();

                        // Read and process the rest of the rows
                        while (!csvReader.EndOfData)
                        {
                            string[] fields = csvReader.ReadFields();
                            SMSSendingDTO.MobileNumber = fields[0];
                            SMSSendingDTO.Customer = fields[1];

                            if (!ValidateMobileNumber(SMSSendingDTO.MobileNumber))
                            {
                                errormessage += SMSSendingDTO.MobileNumber + " (Invalid mobile number format.)<br />";
                            }

                            SMSSendingListDTO.Add(SMSSendingDTO);
                        }

                        if (errormessage.Length > 0)
                        {
                            return new JsonResult(new { success = false, message = errormessage });
                        }
                        errormessage = "";
                        foreach (var row in SMSSendingListDTO)
                        {
                            var responsebatch = _userService.SendAndSave(SMSSendingDTO);
                            if (!responsebatch.IsSuccess)
                            {
                                errormessage += SMSSendingDTO.MobileNumber + "(FAILED)<br />";
                            }
                        }


                        if (errormessage.Length == 0)
                        {
                            return new JsonResult(new { success = true });
                        }
                        else
                        {
                            return new JsonResult(new { success = false, message = errormessage });
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SMSSendingDTO.MobileNumber) || string.IsNullOrEmpty(SMSSendingDTO.Customer))
                    {
                        return new JsonResult(new { success = false, message = "Mobile number/Customer field is required." });
                    }

                    if (ValidateMobileNumber(SMSSendingDTO.MobileNumber))
                    {
                        var response = _userService.SendAndSave(SMSSendingDTO);
                        if (response.IsSuccess)
                        {
                            return new JsonResult(new { success = true });
                        }
                        else
                        {
                            return new JsonResult(new { success = false, message = response.Description });
                        }
                    }
                    else
                    {
                        return new JsonResult(new { success = false, message = "Invalid mobile number format." });
                    }

                }
            }
            catch
            {
                return new JsonResult(new { success = false, message = "ERROR while sending message" });
            }

        }
    }
}
