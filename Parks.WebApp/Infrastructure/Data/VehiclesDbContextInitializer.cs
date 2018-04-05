using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parks.WebApp.Models.Entities;

namespace Parks.WebApp.Data
{
    public static class VehiclesDbContextInitializer
    {
        public static void Initializer(VehicleDbContext context)
        {
            //initialize  the database
           VehicleInitializer(context);

        }

        public static void VehicleInitializer(VehicleDbContext context)
        { 
                //VehicleType
                var vehicleType = new VehicleType[]
                {
                    new VehicleType{  Name = "Car"},
                    new VehicleType{ Name= "Bus"},
                    new VehicleType{  Name="MotorCycle"},
                    new VehicleType{  Name= "Truck"},
                    new VehicleType{ Name ="Sport car"}
                };

                context.VehicleTypes.AddRange(vehicleType);
                context.SaveChanges();
                       
        }

    }
}
