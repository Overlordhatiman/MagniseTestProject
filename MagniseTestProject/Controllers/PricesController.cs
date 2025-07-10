using MagniseTask.Data;
using MagniseTask.DTOs;
using MagniseTask.Services.RealTime;
using Microsoft.AspNetCore.Mvc;
using MagniseTask.Interfaces;
using AutoMapper;

namespace MagniseTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PricesController : ControllerBase
    {
        private readonly IAssetsRepository _assetsRepository;
        private readonly IFintachartsDataService _fintachartsDataService;
        private readonly IMapper _mapper;
        private readonly MarketDataService _marketDataService;

        public PricesController(
            IAssetsRepository assetsRepository,
            IFintachartsDataService fintachartsDataService,
            IMapper mapper,
            MarketDataService marketDataService)
        {
            _assetsRepository = assetsRepository;
            _fintachartsDataService = fintachartsDataService;
            _mapper = mapper;
            _marketDataService = marketDataService;
        }

        /// <summary>
        /// Gets the real-time price of a specified asset symbol.
        /// </summary>
        [HttpGet("price/{symbol}")]
        public IActionResult GetPrice(string symbol)
        {
            var result = _marketDataService.GetPrice(symbol);
            return result != null ? Ok(result) : NotFound($"Price for symbol '{symbol}' not found.");
        }

        /// <summary>
        /// Gets the list of supported market assets.
        /// </summary>
        [HttpGet("assets")]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetSupportedAssets()
        {
            var supportedAssets = await _assetsRepository.GetSupportedAssets();

            if (supportedAssets == null || !supportedAssets.Any())
            {
                var authHeaderValue = Request.Headers["Authorization"].ToString();

                if (string.IsNullOrWhiteSpace(authHeaderValue))
                {
                    return Unauthorized("Authorization token is missing or invalid.");
                }

                var token = authHeaderValue.Substring("Bearer ".Length).Trim();
                var assets = await _fintachartsDataService.GetAllAssets(token);
                await _assetsRepository.AddAssets(assets);
                return Ok(_mapper.Map<IEnumerable<AssetDto>>(assets));
            }

            return Ok(_mapper.Map<IEnumerable<AssetDto>>(supportedAssets));
        }
    }
}
