using Defra.Trade.Address.V1.ApiClient.Model;
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

    /// <summary>
    /// provides api endpoints for establishments
    /// </summary>
    /// <param name="establishmentsService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public EstablishmentsController(IEstablishmentsService establishmentsService)
    {
        _establishmentsService = establishmentsService ?? throw new ArgumentNullException(nameof(establishmentsService));
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
    /// <returns>IEnumerable of LogisticLocationDTO</returns>
    [HttpGet("Party/{tradePartyId}", Name = "GetLogisticsLocationsForTradePartyAsync")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LogisticsLocationDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> GetLogisticsLocationsForTradePartyAsync(Guid tradePartyId, [FromQuery(Name = "isRejected")] bool isRejected)
    {
        IEnumerable<LogisticsLocationDto>? result;
        try
        {
            if (isRejected) result = await _establishmentsService.GetAllLogisticsLocationsForTradePartyAsync(tradePartyId);
            else result = await _establishmentsService.GetLogisticsLocationsForTradePartyAsync(tradePartyId);
            if (result == null)
            {
                return NotFound("No trade party found found");
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
        LogisticsLocationDto? createdLocation;

        try
        {
            if (await _establishmentsService.EstablishmentAlreadyExists(dto))
            {
                return BadRequest("Establishment already exists");
            }

            createdLocation = await _establishmentsService.AddLogisticsLocationAsync(tradePartyId, dto);
            if (createdLocation != null)
            {
                return CreatedAtRoute("GetLogisticsLocationByIdAsync", new { id = createdLocation.Id }, createdLocation.Id);
            }
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return BadRequest("No establishment added");
    }

    /// <summary>
    /// Updates logistics location
    /// </summary>
    /// <param name="id"></param>
    /// <param name="logiticsLocationRequest"></param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateLogisticsLocationAsync(Guid id, LogisticsLocationDto logiticsLocationRequest)
    {
        LogisticsLocationDto? establishmentDto;
        try
        {
            if (!logiticsLocationRequest.IsRemoved && await _establishmentsService.EstablishmentAlreadyExists(logiticsLocationRequest))
            {
                return BadRequest("Establishment already exists");
            }

            establishmentDto = await _establishmentsService.UpdateLogisticsLocationAsync(id, logiticsLocationRequest);
            if (establishmentDto == null)
            {
                return NotFound("No establishments found");
            }
        }
        catch(Exception ex)
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
        LogisticsLocationDto? result;
        try
        {
            result = _establishmentsService.GetLogisticsLocationByUprnAsync(uprn);

            if (result == null)
            {
                return NotFound("No establishments found");
            }  
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(result);
    }

    /// <summary>
    /// Updates logistics location
    /// </summary>
    /// <param name="id"></param>
    /// <param name="logiticsLocationRequest"></param>
    /// <returns>No content</returns>
    [HttpPut("SelfServe/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateLogisticsLocationSelfServeAsync(Guid id, LogisticsLocationDto logiticsLocationRequest)
    {
        LogisticsLocationDto? establishmentDto;
        try
        {
            if (!logiticsLocationRequest.IsRemoved && await _establishmentsService.EstablishmentAlreadyExists(logiticsLocationRequest))
            {
                return BadRequest("Establishment already exists");
            }

            establishmentDto = await _establishmentsService.UpdateLogisticsLocationSelfServeAsync(id, logiticsLocationRequest);
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
