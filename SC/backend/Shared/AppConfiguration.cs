namespace backend.Shared;

public record ConnectionStrings
{
    public required string DefaultConnection { get; set; }
    public required string DatabaseVersion { get; set; }
}

public record JwtConfig
{
    public required string Key { get; set; }
    public int ExpiryAccessToken { get; set; }
    public int ExpiryRefreshToken { get; set; }
}