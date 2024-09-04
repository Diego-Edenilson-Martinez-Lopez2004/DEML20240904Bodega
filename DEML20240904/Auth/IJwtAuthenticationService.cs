namespace DEML20240904.Auth
{
    public interface IJwtAuthenticationService
    {
        string Authenticate(string Username);
    }

}
