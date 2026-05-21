using LoginClipboardApp.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading;

namespace LoginClipboardApp.ViewModel
{
    public class AppViewModel : BaseViewModel
    {
        public IList<UserCredentialsViewModel> UserCredentials { get; set; }
        public UserCredentialsViewModel CurrentTab { get; set; }
        public AppViewModel(IConfiguration configuration)
        {
            UserCredentials = new List<UserCredentialsViewModel>();
            var profilesSection = configuration.GetSection("Profiles");
            if (profilesSection != null)
            {
                foreach (var child in profilesSection.GetChildren())
                {
                    var profile = new UserCredentialsProfile
                    {
                        email = child["email"],
                        password = child["password"],
                        totpSecret = child["totpSecret"]
                    };
                    UserCredentials.Add(new UserCredentialsViewModel(profile));
                }

                if (UserCredentials.Any())
                {
                    CurrentTab = UserCredentials.First();
                }
            }
        }
    }
}
