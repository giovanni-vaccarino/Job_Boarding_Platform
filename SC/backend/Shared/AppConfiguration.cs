namespace backend.Shared;

public record ConnectionStrings
{
    public string DefaultConnection { get; set; }
    public string DatabaseVersion { get; set; }
}

public record JwtConfig
{
    public string Key { get; set; }
    public int ExpiryAccessToken { get; set; }
    public int ExpiryRefreshToken { get; set; }
}