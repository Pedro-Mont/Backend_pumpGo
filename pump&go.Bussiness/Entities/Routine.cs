namespace pump_go.Entities
{
    public class Routine
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public List<RoutineItem> RoutineItems { get; set; } 
    }
}
