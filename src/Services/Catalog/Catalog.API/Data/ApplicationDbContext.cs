namespace Catalog.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Plate> Plates { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: Map AuditLog to table explicitly
            modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");

            // Set up foreign key relationship: AuditLog → Plate
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.Plate)
                .WithMany(p => p.AuditLogs)
                .HasForeignKey(a => a.PlateId);
        }
    }
}
