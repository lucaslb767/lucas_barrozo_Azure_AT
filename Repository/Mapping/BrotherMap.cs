using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Mapping
{
    class BrotherMap : IEntityTypeConfiguration<Brother>
    {
        public void Configure(EntityTypeBuilder<Brother> builder)
        {
            builder.ToTable("Brother");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Telephone).IsRequired().HasMaxLength(200);
            builder.HasOne<Friend>(x => x.Friend);
        }
    }
}
