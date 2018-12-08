namespace GraphLabs.Backend.Domain
{
    public class Student : User
    {
        public virtual string Group { get; set; }
    }
}