﻿using Newtonsoft.Json;

namespace FamilyHubs.ServiceDirectory.Core.Postcode.Model;

// slightly leaky re: clean architecture, but keeps things simple and optimised

/// 
public sealed class PostcodeInfo
{
    [JsonProperty("postcode")]
    public string Postcode { get; set; } = default!;

    public string AdminArea => string.Equals(Codes.AdminCounty, "E99999999", StringComparison.InvariantCultureIgnoreCase) ? Codes.AdminDistrict : Codes.AdminCounty;

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonProperty("outcode")]
    public string? OutCode { get; set; }
    
    [JsonProperty("codes")]
    public Codes Codes { get; set; } = default!;
}

public sealed class Codes
{
    [JsonProperty("admin_district")]
    public string AdminDistrict { get; set; } = default!;

    [JsonProperty("admin_county")]
    public string AdminCounty { get; set; } = default!;
}