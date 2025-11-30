namespace pump_go.pump_go.Bussiness.DTOs.Routine
{
    public class RoutineUpdateDTO : RoutineCreateDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPublic { get; set; }
        public List<RoutineItemDTO> RoutineItems { get; set; } = new();
    }
}
