namespace Catalog.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Plate> Plates { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Status table and seed data
            modelBuilder.Entity<Status>().ToTable("Statuses");
            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 10, Name = "Available" },
                new Status { Id = 20, Name = "Reserved" },
                new Status { Id = 30, Name = "Sold" }
            );

            // Plate configuration
            modelBuilder.Entity<Plate>().ToTable("Plates");

            modelBuilder.Entity<Plate>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Registration)
                      .HasMaxLength(20); // optional, but safe default

                entity.Property(p => p.PurchasePrice)
                      .HasPrecision(18, 2);

                entity.Property(p => p.SalePrice)
                      .HasPrecision(18, 2);

                entity.Property(p => p.FinalSalePrice)
                      .HasPrecision(18, 2);

                entity.Property(p => p.Letters)
                      .HasMaxLength(10); // optional

                entity.Property(p => p.PromoCodeUsed)
                      .HasMaxLength(50); // optional

                entity.Property(p => p.StatusId)
                      .HasDefaultValue(10);

                entity.HasOne(p => p.Status)
                      .WithMany(s => s.Plates)
                      .HasForeignKey(p => p.StatusId)
                      .IsRequired();
            });

            // AuditLog configuration
            modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.Plate)
                      .WithMany(p => p.AuditLogs)
                      .HasForeignKey(a => a.PlateId);
            });
        }
    }
}
