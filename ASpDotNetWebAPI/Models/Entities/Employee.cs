namespace ASpDotNetWebAPI.Models.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public  required string Age { get; set; }

        public required string Email {get; set; }

        public required string Phone { get; set; }
         
         
        public decimal Salary { get; set; }

    }
}
