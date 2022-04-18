using System.Threading.Tasks;
using Finance.PciDss.Bridge.IceMarket.Server.Models;

namespace Finance.PciDss.Bridge.IceMarket.Server.Interfaces
{
    public interface IPageStorage
    {
        Task<ResultModel<string>> SavePageAsync(string html, string id);
    }
}