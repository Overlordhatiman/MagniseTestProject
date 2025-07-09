using MagniseTask.Data;

namespace MagniseTask.Interfaces;

public interface IFintachartsDataService
{
	Task<IEnumerable<Asset>> GetAllAssets(string authHeaderValue);
}