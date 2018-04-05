using Parks.WebApp.Abstact;
using Parks.WebApp.Data;
using Parks.WebApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parks.WebApp.Repositories
{
    public class VehicleTypeRepository : VehicleEntityBaseRepository<VehicleType>, IVehicleTypeRepository
    {
        public VehicleTypeRepository(VehicleDbContext context) : base(context)
        { }
    }
}
