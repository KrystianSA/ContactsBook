﻿namespace RecruitmentTask.Entities
{
    public class Contact
    {
        public int Id { get; set; }    
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public double PhoneNumber { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
