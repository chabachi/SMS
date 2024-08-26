using esquire.common.Models;
using esquire.services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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

        private readonly IUsersService _userService;


        public SendSMSModel(IUsersService usersService)
        {
            _userService = usersService;
        }

       
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string email = User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault() != null ? User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault().Value : "";
            SMSSendingDTO.ExecutedBy = email;
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
}
