using pump_go.Entities;

namespace pump_go.pump_go.Bussiness.DTOs.Routine
{
    public class RoutineCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public List<RoutineItemDTO> RoutineItems {get; set;}
    }
}
