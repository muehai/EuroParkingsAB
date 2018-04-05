using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parks.WebApp.Data;
namespace Parks.WebApp.Infrastructure
{
    public class DisposeExtension: IDisposable
    {
        public VehicleDbContext _vehicleDbContext = null;

        public DisposeExtension()
        {
            if(_vehicleDbContext == null)
            _vehicleDbContext = new VehicleDbContext();
        }

        public void Dispose()
        {
            if (_vehicleDbContext != null)
            {
                _vehicleDbContext.Dispose();
            }
        }    
    }
}
