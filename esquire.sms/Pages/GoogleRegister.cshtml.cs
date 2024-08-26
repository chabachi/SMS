using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using esquire.services.Services;
using esquire.common;

namespace esquire.sms.Pages
{
    public class GoogleRegisterModel : PageModel
    {
        private readonly IUsersService _memberService;
        private readonly IGoogleService _googleService;
        private readonly ConfigurationSettings _configuration;
        public string Message { get; private set; }
        public GoogleRegisterModel(IUsersService memberService, IGoogleService googleService, ConfigurationSettings config)
        {
            _memberService = memberService;
            _googleService = googleService;
            _configuration = config;

        }
        public async Task<IActionResult> OnGetAsync(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return RedirectToPage("/Error");
                }

                var userInfo = await _googleService.GetUserInfoAsync(_configuration.BaseUrl, code, _configuration.GoogleRegistertUri);

                if (string.IsNullOrEmpty(userInfo.id))
                {
                    return RedirectToPage("/Error");
                }

                else
                {
                    ////PENDING //UPDATING //FORAPPROVAL //APPROVED
                    ///
                    //Check if email already registered
                    var validateUser = _memberService.FindByEmail(userInfo.email);
                    if (validateUser != null)
                    {
                        if (!validateUser.Active)
                        {
                            Message = "Your account/email is PENDING for approval. Please contact administrator.";
                            return RedirectToPage("/PendingUser");
                        }
                        else
                        {
                            string sessionId = (Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12)).ToUpper();
                            UpdateIdentity(validateUser.Email, sessionId);
                            if (validateUser.UserType == "MEMBER")
                            {
                                return RedirectToPage("/User/SendSMS");
                            }
                            if (validateUser.UserType == "ADMIN")
                            {
                                return RedirectToPage("/Admin/Index");
                            }
                        }

                        return RedirectToPage("/Error");
                    }
                    else
                    {
                        //Register
                        _memberService.Registration(
                            new common.Models.Users
                            {
                                LastName = userInfo.family_name,
                                FirstName = userInfo.given_name,
                                Email = userInfo.email,
                                CreateDate = DateTime.Now,
                                Active = false,
                                UserType = "MEMBER"
                            });

                        return RedirectToPage("/PendingUser");
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle exception
                return RedirectToPage("/Error");
            }
        }

        public void UpdateIdentity(string email, string sessionId)
        {
            var member = _memberService.FindByEmail(email);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.PrimarySid, member.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, member.Email));
            claims.Add(new Claim(ClaimTypes.GivenName, member.FirstName));
            claims.Add(new Claim(ClaimTypes.Role, member.UserType));
            claims.Add(new Claim(ClaimTypes.Upn, sessionId));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var props = new AuthenticationProperties();
            props.IsPersistent = true;
            props.ExpiresUtc = DateTime.UtcNow.AddMinutes(_configuration.AuthenticationMinutes);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
        }
    }
}
