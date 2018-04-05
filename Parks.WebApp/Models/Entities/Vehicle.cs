using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Parks.WebApp.Models.EntityBase;

namespace Parks.WebApp.Models.Entities
{
    public class Vehicle : IEntityBase
    {
       
        public int Id { get; set; }

        [Required (ErrorMessage = "Enter tree characters and tree numbers")]
        [Display(Name = "Vehicle registerNr")]
        public string RegisterNr { get; set; }

        [Required (ErrorMessage = "Enter Vehicle Color")]
        [Display(Name = "Vehicle Color")]
        public string Color { get; set; }
        
        [Required (ErrorMessage = "Enter Vehicle Brand")]
        [Display(Name = "Vehicle Brand")]
        public string Brand { get; set; }

        [Required (ErrorMessage = "Enter Vehicle Model")]
        [Display(Name = "Vehicle Model")]
        public string Model { get; set; }

        public int ParkeringSpaceId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date of parking time")]
        public DateTime ParkingTime { get; set; }
       
        public int VehicleOwnersId { get; set; }
        //Navigation properties
        public virtual Owner VehicleOwners { get; set; }

        //public int VehicleTypeId { get; set; }
        
        [Display(Name = "Vehicle Type")]
        public string VehicleTypeName { get; set; }
        //Navigation properties
        public virtual VehicleType VehicleTypesNames { get; set; }

    }
}
