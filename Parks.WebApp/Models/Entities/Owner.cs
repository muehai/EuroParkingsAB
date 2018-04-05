using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Parks.WebApp.Models.EntityBase;

namespace Parks.WebApp.Models.Entities
{
    public class Owner : IEntityBase
    {
        public Owner()
        {
            VehiclesOwnerList = new List<Vehicle>();
        }

        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{6}-\d{4}$", ErrorMessage = "Enter your person nummer on format YYMMDD-1234.")]
        [Display( Name="Person number")]
        public string OwnerPersonalId { get; set; }

        [Required(ErrorMessage = "Enter your Name.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage ="Enter Email.")]
        [Display(Name="Email")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Enter your telefon number.")] 
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
       
        [Required(ErrorMessage = "Enter your address.")]
        [Display(Name = "Address")]
        public string Address { get; set; }
      
        public  virtual ICollection<Vehicle> VehiclesOwnerList { get; set; }

    }
}
