using System;

namespace Sample.Data.DTO
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string? Username { get; set; }            // required
        public string? Password { get; set; }            // required
        public string? ConfirmPassword { get; set; }     // required
        public string? Remarks { get; set; }
        public string? Phone { get; set; }
        public string? FullName { get; set; }
        //public string? CompanyName { get; set; }        // now nullable
        public bool IsActive { get; set; }
        public string? Email { get; set; }              // now nullable
        public string? Gender { get; set; }             // now nullable
    }
}
