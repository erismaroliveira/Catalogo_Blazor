using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Catalogo_Blazor.Server.Utils
{
    public static class HttpContextExtensions
    {
        public static async Task InserirParametroEmPageResponse<T>(this HttpContext context, 
            IQueryable<T> queryable, int quantidadeTotalRegistrosAExibir)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            double quantidadeRegistrosTotais = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(quantidadeRegistrosTotais / quantidadeTotalRegistrosAExibir);

            // salvando as informações no header do response
            context.Response.Headers.Add("quantidadeRegistrosTotais", quantidadeRegistrosTotais.ToString());
            context.Response.Headers.Add("totalPaginas", totalPaginas.ToString());
        }
    }
}
