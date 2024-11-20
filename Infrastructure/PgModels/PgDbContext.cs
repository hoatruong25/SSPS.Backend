using Microsoft.EntityFrameworkCore;

namespace Infrastructure.PgModels
{
    public class PgDbContext : DbContext
    {
        public PgDbContext(DbContextOptions<PgDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set auto increase id
            modelBuilder.Entity<PgChatBoxData>().Property(x => x.Id).ValueGeneratedOnAdd();
        }

        public DbSet<PgUser> Users { get; set; }
        public DbSet<PgCategoryUsageMoney> Categories { get; set; }
        public DbSet<PgMoneyPlan> MoneyPlans { get; set; }
        public DbSet<PgUsageMoney> UsageMoneys { get; set; }
        public DbSet<PgNote> Notes { get; set; }
        public DbSet<PgToDoNote> ToDoNotes { get; set; }
        public DbSet<PgToDoCard> ToDoCards { get; set; }
        public DbSet<PgForgotPassword> ForgotPasswords { get; set; }
        public DbSet<PgOTP> OTP { get; set; }
        public DbSet<PgChatBoxData> ChatBoxData { get; set; }
    }
}
