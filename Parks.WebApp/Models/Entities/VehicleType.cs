using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Parks.WebApp.Models.EntityBase;

namespace Parks.WebApp.Models.Entities
{
    public class VehicleType : IEntityBase
    {
        public VehicleType()
        {
            VehiclesList = new List<Vehicle>();
        }

       
        public int Id { get; set; }

        [Required (ErrorMessage = "Enter Vehicle Type.")]
        [Display(Name = "Vehicle Type")]
        public string Name { get; set; }

        public virtual ICollection<Vehicle> VehiclesList{ get; set; }
    }
}
