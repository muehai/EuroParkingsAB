using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parks.WebApp.Models.EntityBase
{
    //Mapping entity primary key id to database
   public interface IEntityBase
    {
        int Id { get; set; }
    }
}
