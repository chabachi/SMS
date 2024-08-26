using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace esquire.sms.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public string ExternalAuthenticationUrl { get; set; }
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        public string GoogleLoginUrl { get; private set; }
        public string GoogleRegisterUrl { get; private set; }
        public IndexModel(IConfiguration configuration, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            var clientId = _configuration.GetSection("ConfigurationSettings").GetSection("GoogleClientId").Value;
            var baseUrl = _configuration.GetSection("ConfigurationSettings").GetSection("BaseUrl").Value;
            var scope = "openid profile email"; // Add additional scopes as needed

            var redirectUri = $"{baseUrl}/{_configuration.GetSection("ConfigurationSettings").GetSection("GoogleRegistertUri").Value}";
            var googleRegisterUrl = $"https://accounts.google.com/o/oauth2/auth?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope={scope}&state=STATE";
            GoogleRegisterUrl = googleRegisterUrl;

        }
    }
}
