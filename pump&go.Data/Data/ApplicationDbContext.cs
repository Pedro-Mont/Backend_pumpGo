using Microsoft.EntityFrameworkCore;
using pump_go.Entities;
using System;

namespace pump_go.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Usuarios { get; set; }
        public DbSet<Exercise> Exercicios { get; set; }
        public DbSet<MuscleGroup> GruposMusculares { get; set; }
        public DbSet<Routine> RotinasDeTreino { get; set; }
        public DbSet<RoutineItem> ItensRotina { get; set; }
        public DbSet<Plan> Planos { get; set; }
        public DbSet<Signature> Assinaturas { get; set; }
        public DbSet<Professional> Profissionais { get; set; }
        public DbSet<Conversation> Conversas { get; set; }
        public DbSet<Message> Mensagens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Plan>().HasData(
                new Plan { Id = 1, Name = "Básico", Price = 0.00m, Description = "Plano gratuito com acesso a criação de rotinas." },
                new Plan { Id = 2, Name = "Pro", Price = 95.00m, Description = "Acesso a personal trainers." },
                new Plan { Id = 3, Name = "Ultra", Price = 150.00m, Description = "Acesso a personais e nutricionistas." }
            );

            modelBuilder.Entity<Exercise>()
                .Property(e => e.PlaceType) 
                .HasConversion<string>();

            modelBuilder.Entity<Professional>()
                .Property(p => p.Specialty)
                .HasConversion<string>();

            modelBuilder.Entity<Signature>()
                .Property(a => a.Status)
                .HasConversion<string>();
        }
    }
}