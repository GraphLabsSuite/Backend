namespace GraphLabs.Backend.Domain
{
    /// <summary> Пользователь </summary>
    public abstract class User
    {
        public virtual long Id { get; set; }
        
        public virtual string Email { get; set; }
        
        public virtual string FirstName { get; set; }
        
        public virtual string LastName { get; set; }
        
        public virtual string FatherName { get; set; }
        
        public virtual byte[] PasswordHash { get; set; }
        
        public virtual byte[] PasswordSalt { get; set; }

        public virtual string Token { get; set; }
    }
}