using pump_go.Entities.Enums;

namespace pump_go.Entities
{
    public class Exercise
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoURL { get; set; }
        public Guid MuscleGroupId { get; set; }
        public MuscleGroup MuscleGroup { get; set; }
        public PlaceType PlaceType { get; set; }
    }
}
