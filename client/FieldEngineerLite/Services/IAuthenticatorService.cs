using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;

namespace FieldEngineerLite
{
    public interface IAuthenticatorService {
        Task<MobileServiceUser> Authorize(MobileServiceAuthenticationProvider provider);

        void Logout();
    }
}
