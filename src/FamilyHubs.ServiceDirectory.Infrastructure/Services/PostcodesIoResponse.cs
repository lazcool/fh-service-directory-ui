﻿using System.Text.Json.Serialization;
using FamilyHubs.ServiceDirectory.Core.Postcode.Model;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Services;

//todo: can we flatten the result during deserialization?
public sealed record PostcodesIoResponse(
    //todo: do we need status from the body - should always match the http code?
    //[property: JsonPropertyName("status")] int Status,
    [property: JsonPropertyName("error")] string? Error,
    [property: JsonPropertyName("result")] PostcodeInfo PostcodeInfo);
