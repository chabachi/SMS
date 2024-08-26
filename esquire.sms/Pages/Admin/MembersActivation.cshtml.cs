using esquire.common.Models;
using esquire.services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace esquire.sms.Pages.Admin
{
    public class MembersActivationModel : PageModel
    {
        private readonly IUsersService _memberService;
        public MembersActivationModel(IUsersService memberService)
        {
            _memberService = memberService;
        }
        public void OnGet()
        {
        }

        public IActionResult OnGetGetHOAMembers()
        {
            return new JsonResult(_memberService.GetAllMembers());
        }

        public IActionResult OnPostActivateHOAMember([FromBody] UsersListDTO member)
        {
            // Implement activation logic here
            // You can update the member's Active status in the database or perform any other action
            _memberService.ActivateDeactivateMember(member.Email, member.Active ? false : true);
            // Return success response
            return new JsonResult(new { success = true });
        }
    }
}
