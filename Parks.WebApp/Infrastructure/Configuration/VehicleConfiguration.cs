using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parks.WebApp.Models.Entities;

namespace Parks.WebApp.Configuration
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
      public void Configure(EntityTypeBuilder<Vehicle> builder)
      {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.RegisterNr).IsRequired().HasMaxLength(6);
            builder.Property(v => v.Model).IsRequired().HasMaxLength(25);
            builder.Property(v => v.VehicleTypeName).IsRequired().HasMaxLength(25);
            builder.Property(v => v.Color).IsRequired().HasMaxLength(25);
            builder.Property(v => v.Brand).IsRequired().HasMaxLength(25);
        

       
      }
    }
}
