using esquire.common.Models;
using esquire.services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace esquire.sms.Pages.User
{
    public class ReportModel : PageModel
    {
        [BindProperty]
        public List<SMSDataList> SmsData { get; set; }

        private readonly IUsersService _userService;

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }
        [BindProperty(SupportsGet = true)]
        public string DateFrom { get; set; }
        [BindProperty(SupportsGet = true)]
        public string DateEnd { get; set; }

        public ReportModel(IUsersService usersService)
        {
            _userService = usersService;
        }
        public void OnGet()
        {
            SmsData = _userService.GetAllMobile();
        }
        public void OnPost()
        {
            // Parse dates from the form inputs
            DateTime? fromDate = string.IsNullOrEmpty(DateFrom) ? null : DateTime.Parse(DateFrom);
            DateTime? toDate = string.IsNullOrEmpty(DateEnd) ? null : DateTime.Parse(DateEnd);

            // Get all SMS data and apply filtering
            var smsData = _userService.GetAllMobile();

            if (fromDate.HasValue)
            {
                smsData = smsData.Where(x => x.DateTimeSent.Date >= fromDate.Value.Date).ToList();
            }

            if (toDate.HasValue)
            {
                smsData = smsData.Where(x => x.DateTimeSent.Date <= toDate.Value.Date).ToList();
            }
            SmsData = smsData;
        }
    }
}
