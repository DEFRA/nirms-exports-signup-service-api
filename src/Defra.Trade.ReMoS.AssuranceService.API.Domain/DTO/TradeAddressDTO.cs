﻿using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Trade.ReMoS.AssuranceService.API.Domain.DTO;

public class TradeAddressDto
{
    public Guid Id { get; set; }
    public string? LineOne { get; set; }
    public string? LineTwo { get; set; }
    public string? LineThree { get; set; }
    public string? LineFour { get; set; }
    public string? LineFive { get; set; }
    public string? PostCode { get; set; }
    public string? CityName { get; set; }
    public string? County { get; set; }
    public string? TradeCountry { get; set; }
}

public class TradeAddressAndBusinessNameDto
{
    public TradeAddressDto? TradeAddress { get; set; }
    public string? BusinessName { get; set; }
}
