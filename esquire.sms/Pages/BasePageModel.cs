using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;


namespace esquire.sms.Pages
{
    public class BasePageModel : PageModel
    {
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            // Check if the user is authenticated
            if (User?.Identity?.IsAuthenticated != true || !User.Claims.Any())
            {
                // Redirect to login page if no valid claims
                context.Result = new RedirectToPageResult("/Index");
            }

            base.OnPageHandlerExecuting(context);
        }
    }
}
