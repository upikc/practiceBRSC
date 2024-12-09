using System.ComponentModel.DataAnnotations;

namespace practiceAPI.Models
{
    public class Usersdata
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int Role_id { get; set; }
    }

    public class UserDetailsView
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string passwordhash { get; set; }
        public string role { get; set; }
    }

    public class RegisterModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
    public class UpdateUserModelWithPass
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role_id { get; set; }
    }

}
