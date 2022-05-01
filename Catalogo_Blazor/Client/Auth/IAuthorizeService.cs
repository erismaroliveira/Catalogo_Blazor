using System.Threading.Tasks;

namespace Catalogo_Blazor.Client.Auth
{
    public interface IAuthorizeService
    {
        Task Login(string token);
        Task Logout();
    }
}
