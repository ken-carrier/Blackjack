using Microsoft.EntityFrameworkCore;
using BlackjackStats.Data.Entities;

namespace BlackjackStats.Data
{
    public class BlackjackStatsContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Deal> Deals { get; set; }

        public BlackjackStatsContext(DbContextOptions<BlackjackStatsContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            BuildGameTable(builder);
            BuildDealTable(builder);
        }

        private void BuildGameTable(ModelBuilder builder)
        {
            builder.Entity<Game>()
            .ToTable("Game");
            builder.Entity<Game>()
                .HasKey(g => g.ID)
                .HasName("GameID");
            builder.Entity<Game>().Property(g => g.StartDateTime).IsRequired(true);
            builder.Entity<Game>().Property(g => g.EndDateTime).IsRequired(true);
            builder.Entity<Game>().Property(g => g.Ip).IsRequired(false);
            builder.Entity<Game>().HasMany(g => g.Deals).WithOne();
            builder.Entity<Game>()
                .UsePropertyAccessMode(PropertyAccessMode.Property);

        }

        private void BuildDealTable(ModelBuilder builder)
        {
            builder.Entity<Deal>()
           .ToTable("Deal");
            builder.Entity<Deal>()
                .HasKey(d => d.ID)
                .HasName("DealID");
            builder.Entity<Deal>().Property(d => d.DateTime).IsRequired(true);
            builder.Entity<Deal>().Property(d => d.Player).HasMaxLength(6).HasConversion<string>();
            builder.Entity<Deal>().Property(d => d.Card).IsRequired(true);
            builder.Entity<Deal>().Property(d => d.SplitScore).IsRequired(true);
            builder.Entity<Deal>().Property(d => d.Action).HasMaxLength(10).HasConversion<string>();
            builder.Entity<Deal>().Property(d => d.Outcome).HasMaxLength(15).HasConversion<string>();

            builder.Entity<Deal>().HasOne(d => d.Game).WithMany(g => g.Deals).HasForeignKey(d => d.GameID);
            builder.Entity<Deal>()
                .UsePropertyAccessMode(PropertyAccessMode.Property);

        }
    }
}