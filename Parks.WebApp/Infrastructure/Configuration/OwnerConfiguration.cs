using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parks.WebApp.Models.Entities;


namespace Parks.WebApp.Configuration
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Name).IsRequired().HasMaxLength(60);
            builder.Property(v => v.Email).IsRequired().HasMaxLength(100);
            builder.Property(v => v.Address).IsRequired().HasMaxLength(100);
            builder.Property(v => v.PhoneNumber).IsRequired().HasMaxLength(50);

        }
    }
}
