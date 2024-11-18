using esquire.common.Models;
using esquire.services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace esquire.sms.Pages.User
{
    public class ReportDetailsModel : PageModel
    {
        [BindProperty]
        public List<SMSDetail> SMSData { get; set; }

        private readonly IUsersService _userService;


        public ReportDetailsModel(IUsersService usersService)
        {
            _userService = usersService;
        }
        public void OnGet(string id)
        {
            SMSData = _userService.GetDetailsMobile(id);
        }
    }
}
