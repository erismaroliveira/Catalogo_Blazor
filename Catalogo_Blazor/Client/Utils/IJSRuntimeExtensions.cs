using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Catalogo_Blazor.Client.Utils
{
    public static class IJSRuntimeExtensions
    {
        public static async ValueTask<object> SetInLocalStorage(this IJSRuntime js, string key, string content) =>
            await js.InvokeAsync<object>("localstorage.setItem", key, content);

        public static async ValueTask<string> GetInLocalStorage(this IJSRuntime js, string key) =>
            await js.InvokeAsync<string>("localstorage.getItem", key);

        public static async ValueTask<object> RemoveItem(this IJSRuntime js, string key) =>
            await js.InvokeAsync<object>("localstorage.removeItem", key);
    }
}
