using Parks.WebApp.Abstact;
using Parks.WebApp.Data;
using Parks.WebApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parks.WebApp.Repositories
{
    public class OwnerRepository: VehicleEntityBaseRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(VehicleDbContext context) : base(context)
        { }
    }
}
