namespace GraphLabs.Backend.DAL
{
    public interface IUserInfoService
    {
        string UserId { get; }
        
        string UserRole { get; }
    }
}