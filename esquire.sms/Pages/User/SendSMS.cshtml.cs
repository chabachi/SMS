using esquire.common.Models;
using esquire.services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.FileIO;
using System.Security.Claims;
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

        public IActionResult OnPost()
        {
            try
            {
                var file = Request.Form.Files[0];
                string email = User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault() != null ? User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault().Value : "";
                SMSSendingDTO.ExecutedBy = email;

                if (file != null)
                {
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
                            var responsebatch = _userService.SendAndSave(SMSSendingDTO);
                            if (!responsebatch.IsSuccess)
                            {
                                errormessage += SMSSendingDTO.MobileNumber + "(FAILED),";
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
            }
            catch
            {
                return new JsonResult(new { success = false, message = "ERROR while sending message" });
            }
            
        }
    }
}
