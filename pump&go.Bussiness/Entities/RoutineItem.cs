using System.ComponentModel.DataAnnotations.Schema;

namespace pump_go.Entities
{
    public class RoutineItem
    {
        public Guid Id { get; set; }
        public Guid ExerciseId { get; set; }
        public Exercise Exercise { get; set; }
        public int Order { get; set; }
        public string Series { get; set; }
        public int RestTime { get; set; }
        public Guid RoutineId { get; set; }

        [ForeignKey("RoutineId")] 
        public Routine Routine { get; set; }
    }
}
