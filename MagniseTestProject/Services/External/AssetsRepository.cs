using MagniseTask.Data;
using MagniseTask.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagniseTask.Services;

public class AssetsRepository : IAssetsRepository
{
	private readonly MarketDbContext _context;

	public AssetsRepository(MarketDbContext context)
	{
		_context = context;
	}
	
	
	public async Task<IEnumerable<Asset>> GetSupportedAssets()
	{
		return await _context.Assets
			.Include(a => a.Mappings)
			.ToListAsync();
	}

	public async Task<Asset> GetAsset(string symbol)
	{
		return await _context.Assets.Where(a => a.Symbol.Contains(symbol))
			.FirstOrDefaultAsync() ?? throw new Exception($"Asset with symbol {symbol} not found");
	}

	public Task AddAsset(Asset asset)
	{
		Task.FromResult(_context.Assets.AddAsync(asset));
		_context.SaveChanges();
		return Task.CompletedTask;
	}

	public Task AddAssets(IEnumerable<Asset> assets)
	{
		_context.Assets.AddRangeAsync(assets);
		_context.SaveChanges();
		return Task.CompletedTask;
	}

	public Task UpdateAsset(Asset asset)
	{
		_context.Assets.Update(asset);
		_context.SaveChanges();
		return Task.CompletedTask;
	}

	public Task DeleteAsset(string id)
	{
		_context.Assets.Remove(_context.Assets.Find(id));
		_context.SaveChanges();
		return Task.CompletedTask;
	}
}