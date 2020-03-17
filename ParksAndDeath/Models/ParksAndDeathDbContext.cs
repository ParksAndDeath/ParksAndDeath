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
        public virtual DbSet<ParksToVisit> ParksToVisit { get; set; }
        public virtual DbSet<ParksVisited> ParksVisited { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }

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

            modelBuilder.Entity<ParksToVisit>(entity =>
            {
                entity.HasKey(e => e.DesiredParkId)
                    .HasName("PK__ParksToV__801589840488142C");

                entity.Property(e => e.DesiredParkId).HasColumnName("desiredParkID");

                entity.Property(e => e.AAddress)
                    .HasColumnName("a_address")
                    .HasMaxLength(1);

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("decimal(3, 2)");

                entity.Property(e => e.CurrentUserId).HasColumnName("currentUserID");

                entity.Property(e => e.ParkCode)
                    .IsRequired()
                    .HasColumnName("parkCode")
                    .HasMaxLength(1);

                entity.Property(e => e.ParkName)
                    .IsRequired()
                    .HasColumnName("parkName")
                    .HasMaxLength(1);

                entity.Property(e => e.SState)
                    .IsRequired()
                    .HasColumnName("s_state")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.HasOne(d => d.CurrentUser)
                    .WithMany(p => p.ParksToVisit)
                    .HasForeignKey(d => d.CurrentUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ParksToVi__curre__6C190EBB");
            });

            modelBuilder.Entity<ParksVisited>(entity =>
            {
                entity.HasKey(e => e.VisitedParkId)
                    .HasName("PK__ParksVis__6C52C9B4200A3D4F");

                entity.Property(e => e.VisitedParkId).HasColumnName("visitedParkID");

                entity.Property(e => e.AAddress)
                    .HasColumnName("a_address")
                    .HasMaxLength(1);

                entity.Property(e => e.CurrentUserId).HasColumnName("currentUserID");

                entity.Property(e => e.ParkCode)
                    .IsRequired()
                    .HasColumnName("parkCode")
                    .HasMaxLength(1);

                entity.Property(e => e.ParkName)
                    .IsRequired()
                    .HasColumnName("parkName")
                    .HasMaxLength(1);

                entity.Property(e => e.SState)
                    .IsRequired()
                    .HasColumnName("s_state")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.HasOne(d => d.CurrentUser)
                    .WithMany(p => p.ParksVisited)
                    .HasForeignKey(d => d.CurrentUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ParksVisi__curre__6EF57B66");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__1788CC4C6E9367BC");

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("date");

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.OwnerId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.UserInfo)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserInfo__OwnerI__6383C8BA");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
