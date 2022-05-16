namespace API.Models {
    public class Employee {
        public Employee(Guid empId, string firstName, string lastName, string birthDay, string sex, int salary, Guid superId, Guid branchId)
        {
            Emp_id = empId;
            First_name = firstName;
            Last_name = lastName;
            Birth_day = birthDay;
            Sex = sex;
            Salary = salary;
            Super_id = superId;
            Branch_id = branchId;
        }

        public Guid Emp_id { get; set; }
        public string First_name  { get; set; }
        public string Last_name  { get; set; }
        public string Birth_day  { get; set; }
        public string Sex  { get; set; }
        public int Salary  { get; set; }
        public Guid Super_id  { get; set; } 
        public Guid Branch_id  { get; set; }
    
   
    }
}