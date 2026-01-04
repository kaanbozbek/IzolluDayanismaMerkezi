namespace IzolluVakfi.Services;

public class AuthService
{
    private const string USERNAME = "izollu";
    private const string PASSWORD = "Malatya44";
    
    public bool IsAuthenticated { get; private set; }
    
    public event Action? OnAuthStateChanged;
    
    public bool Login(string username, string password)
    {
        if (username == USERNAME && password == PASSWORD)
        {
            IsAuthenticated = true;
            OnAuthStateChanged?.Invoke();
            return true;
        }
        return false;
    }
    
    public void Logout()
    {
        IsAuthenticated = false;
        OnAuthStateChanged?.Invoke();
    }
}
