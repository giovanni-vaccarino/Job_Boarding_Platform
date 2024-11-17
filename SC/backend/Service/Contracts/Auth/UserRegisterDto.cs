using backend.Shared.Enums;

namespace backend.Dtos.Auth;

public class UserRegisterDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public ProfileType ProfileType { get; set; }
}