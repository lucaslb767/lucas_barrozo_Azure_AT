using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Repository.Mapping
{
    public class FriendMap : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.ToTable("Friend");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Photo).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Telephone).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Bday).IsRequired();
            builder.HasMany<Brother>(x => x.Friends);
            builder.HasOne<Country>(x => x.Country);
            builder.HasOne<State>(x => x.State);

        }
    }
}
