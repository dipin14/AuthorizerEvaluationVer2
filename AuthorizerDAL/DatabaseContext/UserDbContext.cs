using AuthorizerDAL.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizerDAL.DatabaseContext
{
    public class UserDbContext : DbContext
    {
        public UserDbContext() : base(nameOrConnectionString: "Default") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);

            //Fluent API that shows many to many relations
            modelBuilder.Entity<Role>()
    .HasMany(rol => rol.Pages)
    .WithMany(page => page.Roles)
    .Map(mc =>
    {
        mc.ToTable("T_Role_Page");
        mc.MapLeftKey("roleId");
        mc.MapRightKey("pageId");
    });
            modelBuilder.Entity<User>().Property(c => c.userName).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("US_Name") { IsUnique = true }));

            modelBuilder.Entity<Role>().Property(c => c.roleName).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RO_Name") { IsUnique = true }));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Page> Pages { get; set; }
    }
}
