using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace lbdbackend.Data.Configurations {
    public class UserConfigurations : IEntityTypeConfiguration<IdentityUser> {
        public void Configure(EntityTypeBuilder<IdentityUser> builder) {
            builder.Property(b => b.UserName).HasMaxLength(25).IsRequired(true);
            builder.Property(b => b.Email).HasMaxLength(25).IsRequired(true);
            builder.Property(b => b.PasswordHash).HasMaxLength(1000).IsRequired(true);
        }
    }
}
