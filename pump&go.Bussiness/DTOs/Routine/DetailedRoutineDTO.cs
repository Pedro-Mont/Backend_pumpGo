namespace pump_go.pump_go.Bussiness.DTOs.Routine
{
    public class DetailedRoutineDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public bool IsPublic { get; set; }
        public List<DetailedRoutineItemDTO> RoutineItems { get; set; }
    }
}
