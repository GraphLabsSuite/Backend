namespace GraphLabs.Backend.Domain
{
    /// <summary> Вариант задания </summary>
    public class TaskVariant
    {
        public long Id { get; set; }
        
        public string Name { get; set; }
        
        public string VariantData { get; set; }
        
        public TaskModule TaskModule { get; set; }
    }
}