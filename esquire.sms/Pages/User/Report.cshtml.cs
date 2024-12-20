using esquire.common.Models;
using esquire.services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace esquire.sms.Pages.User
{
    public class ReportModel : PageModel
    {
        [BindProperty]
        public List<SMSData> SMSData { get; set; }

        private readonly IUsersService _userService;


        public ReportModel(IUsersService usersService)
        {
            _userService = usersService;
        }
        public void OnGet()
        {
            SMSData = _userService.GetAllMobile();
        }
    }
}
