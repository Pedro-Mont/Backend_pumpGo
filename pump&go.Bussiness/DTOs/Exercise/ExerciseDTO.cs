using pump_go.Entities.Enums;

namespace pump_go.pump_go.Bussiness.DTOs.Exercise
{
    public class ExerciseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoURL { get; set; }
        public string MuscleGroupName { get; set; }
        public PlaceType Place { get; set; }
    }
}
