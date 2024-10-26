using AuthAPI903.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI903.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AsignacionPaciente> AsignacionPacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<DocumentoProfesional> DocumentoProfesionales { get; set; }
        public DbSet<Domicilio> Domicilios { get; set; }
        public DbSet<Emocion> Emociones { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Medicion> Mediciones { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Padecimiento> Padecimientos { get; set; }
        public DbSet<PadecimientoPaciente> PadecimientoPacientes { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Profesional> Profesionales { get; set; }
        public DbSet<Rol> RolesU { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AppUser> UsuariosAuth { get; set; }
        public DbSet<Cliente> Clientes {  get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<PlanSuscripcion> PlanesSuscripcion { get; set; }
        public DbSet<Suscripcion> Suscripciones { get; set; }
        public DbSet<TipoMetodoPago> TiposMetodoPago { get; set; }
        public DbSet<MetodoPago> MetodosPago { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsignacionPaciente>()
                .HasOne(ap => ap.Paciente)
                .WithMany(p => p.AsignacionPacientes)
                .HasForeignKey(ap => ap.IdPaciente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AsignacionPaciente>()
                .HasOne(ap => ap.Profesional)
                .WithMany(p => p.AsignacionPacientes)
                .HasForeignKey(ap => ap.IdProfesional)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.AsignacionPaciente)
                .WithMany()
                .HasForeignKey(c => c.IdAsignacion)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DocumentoProfesional>()
                .HasOne(dp => dp.Profesional)
                .WithMany(p => p.DocumentoProfesionales)
                .HasForeignKey(dp => dp.IdProfesional)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Domicilio>()
                .HasOne(d => d.Persona)
                .WithMany(p => p.Domicilios)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entrada>()
                .HasOne(e => e.Paciente)
                .WithMany(u => u.Entradas)
                .HasForeignKey(e => e.IdPaciente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medicion>()
                .HasOne(m => m.Emocion)
                .WithMany()
                .HasForeignKey(m => m.IdEmocion)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Medicion>()
                .HasOne(m => m.Entrada)
                .WithMany()
                .HasForeignKey(m => m.IdEntrada)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Usuario)
                .WithMany(u => u.Notificaciones)
                .HasForeignKey(n => n.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Persona)
                .WithOne(u => u.Paciente)
                .HasForeignKey<Paciente>(p => p.IdPersona)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PadecimientoPaciente>()
                .HasOne(pp => pp.Padecimiento)
                .WithMany(p => p.PadecimientoPacientes)
                .HasForeignKey(pp => pp.IdPadecimiento)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PadecimientoPaciente>()
                .HasOne(pp => pp.Paciente)
                .WithMany(p => p.PadecimientoPacientes)
                .HasForeignKey(pp => pp.IdPaciente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Profesional>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Profesionales)
                .HasForeignKey(p => p.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Persona)
                .WithOne(p => p.Usuario)
                .HasForeignKey <Usuario> (e => e.IdPersona)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.AuthUser)
                .WithOne(r => r.Usuario)
                .HasForeignKey<Usuario>(u => u.IdAppUser)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Suscripciones)
                .WithOne(s => s.Cliente)
                .HasForeignKey(s => s.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.MetodosPago)
                .WithOne(mp => mp.Cliente)
                .HasForeignKey(mp => mp.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de PlanDeSuscripcion
            modelBuilder.Entity<PlanSuscripcion>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<PlanSuscripcion>()
                .Property(p => p.DuracionMeses)
                .IsRequired();  

            modelBuilder.Entity<PlanSuscripcion>()
                .HasMany(p => p.Suscripciones)
                .WithOne(s => s.PlanSuscripcion)
                .HasForeignKey(s => s.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de Suscripcion
            modelBuilder.Entity<Suscripcion>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Suscripcion>()
                .HasMany(s => s.Pagos)
                .WithOne(p => p.Suscripcion)
                .HasForeignKey(p => p.SuscripcionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de Pago
            modelBuilder.Entity<Pago>()
                .HasKey(p => p.Id);

            // Configuración de MetodoDePago
            modelBuilder.Entity<MetodoPago>()
                .HasKey(mp => mp.Id);

            modelBuilder.Entity<MetodoPago>()
                .HasOne(mp => mp.Cliente)
                .WithMany(c => c.MetodosPago)
                .HasForeignKey(mp => mp.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MetodoPago>()
                .HasOne(mp => mp.TipoMetodoPago)
                .WithMany(tm => tm.MetodosPago)
                .HasForeignKey(mp => mp.TipoDeMetodoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de TipoDeMetodo
            modelBuilder.Entity<TipoMetodoPago>()
                .HasKey(tm => tm.Id);

            modelBuilder.Entity<TipoMetodoPago>()
                .HasMany(tm => tm.MetodosPago)
                .WithOne(mp => mp.TipoMetodoPago)
                .HasForeignKey(mp => mp.TipoDeMetodoId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
