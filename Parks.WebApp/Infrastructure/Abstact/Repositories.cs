using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parks.WebApp.Models.Entities;
using Parks.WebApp.Abstact;


namespace Parks.WebApp.Abstact
{

    public interface IVehicleRepository : IEntityBaseRepository<Vehicle>{}
    public interface IOwnerRepository : IEntityBaseRepository<Owner> { }
    public interface IVehicleTypeRepository : IEntityBaseRepository<VehicleType> { }

}
