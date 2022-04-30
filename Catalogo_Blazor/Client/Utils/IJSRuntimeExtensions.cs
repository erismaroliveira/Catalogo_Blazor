using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Catalogo_Blazor.Client.Utils
{
    public static class IJSRuntimeExtensions
    {
        public static ValueTask<object> SetInLocalStorage(this IJSRuntime js, string key, string content) =>
            js.InvokeAsync<object>("localstorage.setItem", key, content);

        public static ValueTask<string> GetInLocalStorage(this IJSRuntime js, string key) =>
            js.InvokeAsync<string>("localstorage.getItem", key);

        public static ValueTask<object> RemoveItem(this IJSRuntime js, string key) =>
            js.InvokeAsync<object>("localstorage.removeItem", key);
    }
}
