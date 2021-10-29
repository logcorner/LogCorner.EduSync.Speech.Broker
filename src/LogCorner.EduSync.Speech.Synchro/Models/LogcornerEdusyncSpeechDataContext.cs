using LogCorner.EduSync.Speech.SharedKernel.Events;
using Microsoft.EntityFrameworkCore;

namespace LogCorner.EduSync.Speech.Synchro.Models
{
    public partial class LogcornerEdusyncSpeechDataContext : DbContext
    {
        public LogcornerEdusyncSpeechDataContext()
        {
        }

        public LogcornerEdusyncSpeechDataContext(DbContextOptions<LogcornerEdusyncSpeechDataContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LogCorner.EduSync.Speech.Data;Integrated Security=True");
            }
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        public virtual DbSet<EventStore> EventStore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<EventStore>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.OccurredOn).HasColumnType("datetime");

                entity.Property(e => e.PayLoad)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}