﻿namespace IdentityServer.Configurations.ConfigurationModels;

public class CorsOptions
{
    public const string SectionName = "Cors";

    public string[] Origins { get; set; } = default!;
    public string[] Methods { get; set; } = default!;
    public string[] Headers { get; set; } = default!;
}
