namespace GraphLabs.Backend.Domain
{
    /// <summary> Пользователь </summary>
    public abstract class User
    {
        public virtual long Id { get; set; }
        
        public virtual string Email { get; set; }
        
        public virtual string Name { get; set; }
    }
}