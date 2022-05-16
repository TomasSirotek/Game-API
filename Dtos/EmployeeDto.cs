namespace API.Dtos {
    public class EmployeeDto {
        public Guid Emp_id { get; set; }
        public string First_name  { get; set; }
        public string last_name  { get; set; }
        public string birth_day  { get; set; }
        public string sex  { get; set; }
        public int salary  { get; set; }
        public Guid super_id  { get; set; } 
        public Guid branch_id  { get; set; }
    }
}