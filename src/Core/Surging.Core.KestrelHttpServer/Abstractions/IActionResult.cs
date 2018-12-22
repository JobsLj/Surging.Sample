using System.Threading.Tasks;

namespace Surging.Core.KestrelHttpServer
{
    public interface IActionResult
    {
        Task ExecuteResultAsync(ActionContext context);
    }
}