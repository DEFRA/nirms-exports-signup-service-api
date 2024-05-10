using Defra.Trade.Address.V1.ApiClient.Model;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Helpers;
using Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Defra.Trade.ReMoS.AssuranceService.API.V1.Controllers;

/// <summary>
/// class for establishments
/// </summary>
[ApiVersion("1")]
[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class EstablishmentsController : ControllerBase
{
    private readonly IEstablishmentsService _establishmentsService;
    private readonly ILogger<EstablishmentsController> _logger;

    /// <summary>
    /// provides api endpoints for establishments
    /// </summary>
    /// <param name="establishmentsService"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public EstablishmentsController(IEstablishmentsService establishmentsService, ILogger<EstablishmentsController> logger)
    {
        _establishmentsService = establishmentsService ?? throw new ArgumentNullException(nameof(establishmentsService));
        _logger = logger;
    }

    /// <summary>
    /// Get a location using Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>LogisticsLocationDTO</returns>
    [HttpGet("{id}", Name = "GetLogisticsLocationByIdAsync")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LogisticsLocationDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetLogisticsLocationByIdAsync(Guid id)
    {
        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(GetLogisticsLocationByIdAsync));

        LogisticsLocationDto? result;
        try
        {
            result = await _establishmentsService.GetLogisticsLocationByIdAsync(id);
            if (result == null)
            {
                return NotFound("Establishment not found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    /// <summary>
    /// Get all locations in a post code
    /// </summary>
    /// <param name="postcode"></param>
    /// <returns>List of logistic locations</returns>
    [HttpGet("Postcode/{postcode}", Name = "GetLogisticsLocationsByPostcodeAsync")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LogisticsLocationDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetLogisticsLocationsByPostcodeAsync(string postcode)
    {
        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(GetLogisticsLocationsByPostcodeAsync));
        IEnumerable<LogisticsLocationDto>? result;
        try
        {
            result = await _establishmentsService.GetLogisticsLocationsByPostcodeAsync(postcode);
            if (result == null || result.ToList().Count < 1)
            {
                return NotFound("No establishments found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);

    }

    /// <summary>
    /// Get all locations belonging to the trade party
    /// </summary>
    /// <param name="tradePartyId"></param>
    /// <param name="isRejected"></param>
    /// <param name="searchTerm"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns>IEnumerable of LogisticLocationDTO</returns>
    [HttpGet("Party/{tradePartyId}", Name = "GetLogisticsLocationsForTradePartyAsync")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LogisticsLocationDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetLogisticsLocationsForTradePartyAsync(
        Guid tradePartyId,
        [FromQuery(Name = "isRejected")] bool isRejected,
        [FromQuery(Name = "searchTerm")] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        const int maxPageSize = 50;
        pageSize = (pageSize > maxPageSize) ? maxPageSize : pageSize;

        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(GetLogisticsLocationsForTradePartyAsync));

        PagedList<LogisticsLocationDto>? result;
        try
        {
            if (isRejected) result = await _establishmentsService.GetAllLogisticsLocationsForTradePartyAsync(tradePartyId, pageNumber, pageSize);
            else result = await _establishmentsService.GetLogisticsLocationsForTradePartyAsync(tradePartyId, pageNumber, pageSize);
            if (result == null)
            {
                return NotFound("No trade party found");
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                result.Items = result.Items.Where(logisticslocation => logisticslocation.Name!.Contains(searchTerm) ||
                                      logisticslocation.RemosEstablishmentSchemeNumber!.Contains(searchTerm) ||
                                      logisticslocation.Address!.PostCode!.Contains(searchTerm)).ToList();
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    /// <summary>
    /// Get all locations
    /// </summary>
    /// <returns>List of logistic locations</returns>
    [HttpGet(Name = "GetAllLogisticsLocationsAsync")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LogisticsLocationDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetAllLogisticsLocationsAsync()
    {
        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(GetAllLogisticsLocationsAsync));

        IEnumerable<LogisticsLocationDto>? result;
        try
        {
            result = await _establishmentsService.GetAllLogisticsLocationsAsync();
            if (result == null || result.ToList().Count < 1)
            {
                return NotFound("No establishments found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    /// <summary>
    /// Add a location to a trade party
    /// </summary>
    /// <param name="tradePartyId"></param>
    /// <param name="dto"></param>
    /// <returns>logistic location id</returns>
    [HttpPost("Party/{tradePartyId}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> AddLogisticsLocationToTradePartyAsync(Guid tradePartyId, LogisticsLocationDto dto)
    {
        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(AddLogisticsLocationToTradePartyAsync));

        LogisticsLocationDto? createdLocation;

        try
        {
            if (await _establishmentsService.EstablishmentAlreadyExists(dto, tradePartyId))
            {
                return BadRequest("Establishment already exists");
            }

            createdLocation = await _establishmentsService.AddLogisticsLocationAsync(tradePartyId, dto);

            if (createdLocation != null)
            {
                return CreatedAtRoute("GetLogisticsLocationByIdAsync", new { id = createdLocation.Id }, createdLocation.Id);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return BadRequest("No establishment added");
    }

    /// <summary>
    /// Updates logistics location
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateLogisticsLocationAsync(Guid id, LogisticsLocationDto request)
    {
        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(UpdateLogisticsLocationAsync));

        LogisticsLocationDto? establishmentDto;
        try
        {
            if (!request.IsRemoved && await _establishmentsService.EstablishmentAlreadyExists(request, request.TradePartyId!.Value))
            {
                return BadRequest("Establishment already exists");
            }

            establishmentDto = await _establishmentsService.UpdateLogisticsLocationAsync(id, request);
            if (establishmentDto == null)
            {
                return NotFound("No establishments found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return NoContent();
    }

    /// <summary>
    /// Deletes a location with a given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<ActionResult> RemoveEstablishmentAsync(Guid id)
    {
        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(RemoveEstablishmentAsync));

        try
        {
            if (!await _establishmentsService.RemoveLogisticsLocationAsync(id))
            {
                return NotFound("No establishments found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return NoContent();
    }

    /// <summary>
    /// Get Trade.Address addresses by postcode
    /// </summary>
    /// <param name="postcode"></param>
    /// <returns></returns>
    [HttpGet("Trade/Postcode/{postcode}", Name = "GetTradeAddressApiLocations")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AddressDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public IActionResult GetTradeAddressApiLocations(string postcode)
    {
        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(GetTradeAddressApiLocations));

        List<AddressDto> result;
        try
        {
            result = _establishmentsService.GetTradeAddressApiByPostcode(postcode);

            if (result == null)
                return NotFound("No establishments found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    /// <summary>
    /// Get a logistics location by UPRN
    /// </summary>
    /// <param name="uprn"></param>
    /// <returns></returns>
    [HttpGet("Trade/Uprn/{uprn}", Name = "GetLogisticsLocationByUprn")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LogisticsLocationDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public IActionResult GetLogisticsLocationByUprn(string uprn)
    {
        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(GetLogisticsLocationByUprn));

        LogisticsLocationDto? result;
        try
        {
            result = _establishmentsService.GetLogisticsLocationByUprnAsync(uprn);

            if (result == null)
            {
                return NotFound("No establishments found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    /// <summary>
    /// Updates logistics location
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns>No content</returns>
    [HttpPut("SelfServe/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateLogisticsLocationSelfServeAsync(Guid id, LogisticsLocationDto request)
    {
        _logger.LogInformation("Entered {Class}.{Method}", nameof(EstablishmentsController), nameof(UpdateLogisticsLocationSelfServeAsync));

        LogisticsLocationDto? establishmentDto;
        try
        {
            if (!request.IsRemoved && await _establishmentsService.EstablishmentAlreadyExists(request, request.TradePartyId!.Value))
            {
                return BadRequest("Establishment already exists");
            }

            establishmentDto = await _establishmentsService.UpdateLogisticsLocationSelfServeAsync(id, request);
            if (establishmentDto == null)
            {
                return NotFound("No establishments found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return NoContent();
    }
}
