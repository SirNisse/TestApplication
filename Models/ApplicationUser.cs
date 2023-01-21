using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestApplication.ViewModels;

namespace TestApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }




        public UserViewModel ToViewModel(ApplicationUser user)
        {
            return new UserViewModel
            {
                Name = user.Name,
                City = user.City,
                Address = user.Address,
            };
        }
    }
}
