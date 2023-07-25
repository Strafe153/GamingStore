﻿namespace Domain.Shared;

public static class LoggerConstants
{
    public const string OUTPUT_TEMPLATE = "[{Timestamp:dd-MM-yyyy HH:mm:ss}, {Level:u3}] {Message:lj}{NewLine}{Exception}";
}
