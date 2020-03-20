using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ParksAndDeath.Models
{
    public partial class ParksAndDeathDbContext : DbContext
    {
        public IConfiguration Configuration { get; }
        public ParksAndDeathDbContext()
        {
        }

        public ParksAndDeathDbContext(DbContextOptions<ParksAndDeathDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Parks> Parks { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserParks> UserParks { get; set; }
        public virtual DbSet<UserPreferences> UserPreferences { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Parks>(entity =>
            {
                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Designation).HasMaxLength(30);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Longitude).HasMaxLength(50);

                entity.Property(e => e.ParkCode).HasMaxLength(6);

                entity.Property(e => e.States).HasMaxLength(50);

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__CB9A1CFF0498ADD3");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.Country)
                    .HasColumnName("country")
                    .HasMaxLength(100);

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.Drinker).HasColumnName("drinker");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.OwnerId)
                    .HasColumnName("ownerId")
                    .HasMaxLength(450);

                entity.Property(e => e.Smoker).HasColumnName("smoker");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.UserInfo)
                    .HasForeignKey(d => d.OwnerId)
                    .HasConstraintName("FK__UserInfo__ownerI__08B54D69");
            });

            modelBuilder.Entity<UserParks>(entity =>
            {
                entity.HasKey(e => e.UsersParkIds)
                    .HasName("PK__UserPark__D0A542C543D61E02");

                entity.Property(e => e.UsersParkIds).HasColumnName("usersParkIDs");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(40);

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("decimal(2, 2)");

                entity.Property(e => e.CurrentUserId)
                    .IsRequired()
                    .HasColumnName("currentUserID")
                    .HasMaxLength(450);

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasMaxLength(50);

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasMaxLength(50);

                entity.Property(e => e.ParkCode)
                    .IsRequired()
                    .HasColumnName("parkCode")
                    .HasMaxLength(10);

                entity.Property(e => e.ParkName)
                    .IsRequired()
                    .HasColumnName("parkName")
                    .HasMaxLength(50);

                entity.Property(e => e.ParkVisited).HasColumnName("parkVisited");

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(100);

                entity.HasOne(d => d.CurrentUser)
                    .WithMany(p => p.UserParks)
                    .HasForeignKey(d => d.CurrentUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserParks__curre__02FC7413");
            });

            modelBuilder.Entity<UserPreferences>(entity =>
            {
                entity.HasKey(e => e.PreferencesId)
                    .HasName("PK__UserPref__F62A20A044A43B7A");

                entity.Property(e => e.PreferencesId).HasColumnName("preferencesId");

                entity.Property(e => e.CurrentUserId)
                    .HasColumnName("currentUserID")
                    .HasMaxLength(450);

                entity.Property(e => e.EndYear)
                    .HasColumnName("endYear")
                    .HasColumnType("date");

                entity.Property(e => e.Frequency).HasColumnName("frequency");

                entity.Property(e => e.StartYear)
                    .HasColumnName("startYear")
                    .HasColumnType("date");

                entity.HasOne(d => d.CurrentUser)
                    .WithMany(p => p.UserPreferences)
                    .HasForeignKey(d => d.CurrentUserId)
                    .HasConstraintName("FK__UserPrefe__curre__0E6E26BF");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
