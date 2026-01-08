using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<MemberLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Photo>()
                .HasQueryFilter(p => p.IsApproved);

            modelBuilder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole { Id = "member-id", Name = "Member", NormalizedName = "MEMBER", ConcurrencyStamp = "a1111111-1111-1111-1111-111111111111" },
                    new IdentityRole { Id = "moderator-id", Name = "Moderator", NormalizedName = "MODERATOR", ConcurrencyStamp = "b2222222-2222-2222-2222-222222222222" },
                    new IdentityRole { Id = "admin-id", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "c3333333-3333-3333-3333-333333333333" }
                );

            modelBuilder.Entity<Message>()
                .HasOne(x => x.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(x => x.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberLike>()
                .HasKey(x => new { x.SourceMemberId, x.TargetMemberId });

            modelBuilder.Entity<MemberLike>()
                .HasOne(s => s.SourceMember)
                .WithMany(t => t.LikedMembers)
                .HasForeignKey(s => s.SourceMemberId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberLike>()
                .HasOne(s => s.TargetMember)
                .WithMany(t => t.LikedByMembers)
                .HasForeignKey(s => s.TargetMemberId)
                .OnDelete(DeleteBehavior.NoAction);

            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
             );

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
             );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // If Member.Id is intended to be both the PK and the FK to AppUser.Id:
        //    modelBuilder.Entity<Member>()
        //        .HasKey(m => m.Id);

        //    modelBuilder.Entity<AppUser>()
        //        .HasOne(u => u.Member)
        //        .WithOne(m => m.User)
        //        .HasForeignKey<Member>(m => m.Id);

        //    // other configuration...
        //}
    }
}
