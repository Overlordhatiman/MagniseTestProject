using MagniseTask.Data;

namespace MagniseTask.Interfaces;

public interface IAssetsRepository
{
	Task<IEnumerable<Asset>> GetSupportedAssets();
	Task<Asset> GetAsset(string symbol);
	Task AddAsset(Asset asset);
	Task AddAssets(IEnumerable<Asset> assets);
	Task UpdateAsset(Asset asset);
	Task DeleteAsset(string id);
	
}