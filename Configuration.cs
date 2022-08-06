namespace Micro;

public record class Configuration(
    bool Configured = false,
    string? Name = null,
    string? Username = null,
    string? Password = null,
    string? Token = null
);