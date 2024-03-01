using Defra.Trade.ReMoS.AssuranceService.API.Core.Interfaces;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.Constants;
using Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.Net;

namespace Defra.Trade.ReMoS.AssuranceService.API.V1.Controllers;

/// <summary>
/// api controller for trade parties
/// </summary>
[ApiVersion("1")]
[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class TradePartiesController : ControllerBase
{
    private readonly ITradePartiesService _tradePartiesService;

    /// <summary>
    /// provides endpoints for trade parties information
    /// </summary>
    /// <param name="tradePartiesService"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public TradePartiesController(ITradePartiesService tradePartiesService)
    {
        _tradePartiesService = tradePartiesService ?? throw new ArgumentNullException(nameof(tradePartiesService));
    }

    /// <summary>
    /// Gets all trade parties in the db
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetTradePartiesAsync")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TradePartyDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> GetTradePartiesAsync()
    {
        IEnumerable<TradePartyDto> results;
        try
        {
            results = await _tradePartiesService.GetTradePartiesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(results);
    }

    /// <summary>
    /// Gets individual trade party
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetTradePartyAsync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<ActionResult<TradePartyDto>> GetTradePartyAsync(Guid id)
    {
        TradePartyDto? tradeParty;
        try
        {
            tradeParty = await _tradePartiesService.GetTradePartyAsync(id);
            if (tradeParty == null)
            {
                return NotFound("No trade party found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(tradeParty);
    }

    /// <summary>
    /// Get saved details for a DEFRA org
    /// </summary>
    /// <param name="orgid"></param>
    /// <returns></returns>
    [HttpGet("Organisation/{orgid}", Name = "GetTradePartyByDefraOrgId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<ActionResult<TradePartyDto>> GetTradePartyByDefraOrgId(Guid orgid)
    {
        TradePartyDto? tradeParty;
        try
        {
            tradeParty = await _tradePartiesService.GetTradePartyByDefraOrgIdAsync(orgid);
            if (tradeParty == null)
            {
                return NotFound("No trade party found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(tradeParty);
    }

    /// <summary>
    /// Adds a new trade party
    /// </summary>
    /// <param name="tradePartyRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public async Task<IActionResult> AddTradeParty(TradePartyDto tradePartyRequest)
    {
        TradePartyDto? tradePartyDto;
        try
        {
            tradePartyDto = await _tradePartiesService.AddTradePartyAsync(tradePartyRequest);
            if (tradePartyDto != null)
            {
                return CreatedAtRoute("GetTradePartyAsync", new { id = tradePartyDto.Id }, tradePartyDto.Id);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return BadRequest("No tradeparty added");
    }

    /// <summary>
    /// Updates an existing trade party
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tradePartyRequest"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateTradeParty(Guid id, TradePartyDto tradePartyRequest)
    {
        TradePartyDto? tradePartyDto;
        try
        {
            tradePartyDto = await _tradePartiesService.UpdateTradePartyAsync(id, tradePartyRequest);

            if (tradePartyDto == null)
            {
                return NotFound("No trade party found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(tradePartyDto.Id);
    }

    /// <summary>
    /// Updates an existing trade party's address
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tradePartyRequest"></param>
    /// <returns></returns>
    [HttpPut("Address/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateTradePartyAddress(Guid id, TradePartyDto tradePartyRequest)
    {
        TradePartyDto? tradePartyDto;
        try
        {
            tradePartyDto = await _tradePartiesService.UpdateTradePartyAddressAsync(id, tradePartyRequest);
            if (tradePartyDto == null)
            {
                return NotFound("No trade party found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(tradePartyDto.Id);
    }

    /// <summary>
    /// Creates a new address and adds to a trade party
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tradeAddressRequest"></param>
    /// <returns></returns>
    [HttpPost("{id}/Address")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> AddTradePartyAddress(Guid id, TradeAddressDto tradeAddressRequest)
    {
        TradePartyDto? tradePartyDto;
        try
        {
            tradePartyDto = await _tradePartiesService.AddTradePartyAddressAsync(id, tradeAddressRequest);

            if (tradePartyDto == null)
            {
                return NotFound("No trade party found");
            }
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(tradePartyDto.Id);
    }

    /// <summary>
    /// Updates trade party contact
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tradePartyRequest"></param>
    /// <returns></returns>
    [HttpPut("Contact/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateTradePartyContact(Guid id, TradePartyDto tradePartyRequest)
    {
        TradePartyDto? tradePartyDto;
        
        try
        {
            tradePartyDto = await _tradePartiesService.UpdateTradePartyContactAsync(id, tradePartyRequest);
            if (tradePartyDto == null)
            {
                return NotFound("No trade party found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(tradePartyDto.Id);

    }

    /// <summary>
    /// Updates authorised signatory
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tradePartyRequest"></param>
    /// <returns>An Action Result response object</returns>
    [HttpPut("Authorised-Signatory/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TradePartyDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateAuthorisedSignatory(Guid id, TradePartyDto tradePartyRequest)
    {
        TradePartyDto? tradePartyDto;
        try
        {
            tradePartyDto = await _tradePartiesService.UpdateAuthorisedSignatoryAsync(id, tradePartyRequest);
            if (tradePartyDto == null)
            {
                return NotFound("No Trade Party found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(tradePartyDto);
    }

    /// <summary>
    /// Updates authorised signatory in self serve
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tradePartyRequest"></param>
    /// <returns>An Action Result response object</returns>
    [FeatureGate(FeatureFlags.SelfServe)]
    [HttpPut("SelfServe/Authorised-Signatory/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TradePartyDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateAuthorisedSignatorySelfServe(Guid id, TradePartyDto tradePartyRequest)
    {
        TradePartyDto? tradePartyDto;
        try
        {
            tradePartyDto = await _tradePartiesService.UpdateAuthRepSelfServeAsync(id, tradePartyRequest);
            if (tradePartyDto == null)
            {
                return NotFound("No Trade Party found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(tradePartyDto.Id);
    }

    /// <summary>
    /// Updates trade party contact
    /// </summary>
    /// <param name="id"></param>
    /// <param name="tradePartyRequest"></param>
    /// <returns></returns>
    [FeatureGate(FeatureFlags.SelfServe)]
    [HttpPut("SelfServe/Contact/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateTradePartyContactSelfServe(Guid id, TradePartyDto tradePartyRequest)
    {
        TradePartyDto? tradePartyDto;

        try
        {
            tradePartyDto = await _tradePartiesService.UpdateContactSelfServeAsync(id, tradePartyRequest);
            if (tradePartyDto == null)
            {
                return NotFound("No trade party found");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        return Ok(tradePartyDto.Id);

    }
}
