﻿// <auto-generated />
using System;
using AuthAPI903.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AuthAPI903.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AuthAPI903.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("AuthAPI903.Models.AsignacionPaciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("IdPaciente")
                        .HasColumnType("int");

                    b.Property<int?>("IdProfesional")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPaciente");

                    b.HasIndex("IdProfesional");

                    b.ToTable("AsignacionPacientes");
                });

            modelBuilder.Entity("AuthAPI903.Models.Cita", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Estatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan?>("Horario")
                        .HasColumnType("time");

                    b.Property<int?>("IdAsignacion")
                        .HasColumnType("int");

                    b.Property<string>("Notas")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdAsignacion");

                    b.ToTable("Citas");
                });

            modelBuilder.Entity("AuthAPI903.Models.DocumentoProfesional", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Estudios")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdProfesional")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdProfesional");

                    b.ToTable("DocumentoProfesionales");
                });

            modelBuilder.Entity("AuthAPI903.Models.Domicilio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Calle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CodigoPostal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Colonia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdPersona")
                        .HasColumnType("int");

                    b.Property<string>("NumeroExterior")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroInterior")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdPersona");

                    b.ToTable("Domicilios");
                });

            modelBuilder.Entity("AuthAPI903.Models.Emocion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmocionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Factores")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Emociones");
                });

            modelBuilder.Entity("AuthAPI903.Models.Entrada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Entradas");
                });

            modelBuilder.Entity("AuthAPI903.Models.Medicion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("IdEmocion")
                        .HasColumnType("int");

                    b.Property<int?>("IdEntrada")
                        .HasColumnType("int");

                    b.Property<int?>("Nivel")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdEmocion");

                    b.HasIndex("IdEntrada");

                    b.ToTable("Mediciones");
                });

            modelBuilder.Entity("AuthAPI903.Models.Notificacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contenido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Notificaciones");
                });

            modelBuilder.Entity("AuthAPI903.Models.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Estatus")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FechaRegistro")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IdPersona")
                        .HasColumnType("int");

                    b.Property<string>("NotasAdicionales")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPersona")
                        .IsUnique()
                        .HasFilter("[IdPersona] IS NOT NULL");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("AuthAPI903.Models.Padecimiento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NombrePadecimiento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Padecimientos");
                });

            modelBuilder.Entity("AuthAPI903.Models.PadecimientoPaciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("IdPaciente")
                        .HasColumnType("int");

                    b.Property<int?>("IdPadecimiento")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPaciente");

                    b.HasIndex("IdPadecimiento");

                    b.ToTable("PadecimientoPacientes");
                });

            modelBuilder.Entity("AuthAPI903.Models.Persona", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApellidoMaterno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApellidoPaterno")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EstadoCivil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FechaNacimiento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Foto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ocupacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Personas");
                });

            modelBuilder.Entity("AuthAPI903.Models.Profesional", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Estatus")
                        .HasColumnType("bit");

                    b.Property<int?>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Profesionales");
                });

            modelBuilder.Entity("AuthAPI903.Models.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RolesU");
                });

            modelBuilder.Entity("AuthAPI903.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contrasena")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdAppUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("IdPersona")
                        .HasColumnType("int");

                    b.Property<int?>("IdRol")
                        .HasColumnType("int");

                    b.Property<string>("IdentificadorUnico")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TipoUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdAppUser")
                        .IsUnique();

                    b.HasIndex("IdPersona")
                        .IsUnique()
                        .HasFilter("[IdPersona] IS NOT NULL");

                    b.HasIndex("IdRol");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("AuthAPI903.Models.AsignacionPaciente", b =>
                {
                    b.HasOne("AuthAPI903.Models.Paciente", "Paciente")
                        .WithMany("AsignacionPacientes")
                        .HasForeignKey("IdPaciente")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AuthAPI903.Models.Profesional", "Profesional")
                        .WithMany("AsignacionPacientes")
                        .HasForeignKey("IdProfesional")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Paciente");

                    b.Navigation("Profesional");
                });

            modelBuilder.Entity("AuthAPI903.Models.Cita", b =>
                {
                    b.HasOne("AuthAPI903.Models.AsignacionPaciente", "AsignacionPaciente")
                        .WithMany()
                        .HasForeignKey("IdAsignacion")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("AsignacionPaciente");
                });

            modelBuilder.Entity("AuthAPI903.Models.DocumentoProfesional", b =>
                {
                    b.HasOne("AuthAPI903.Models.Profesional", "Profesional")
                        .WithMany("DocumentoProfesionales")
                        .HasForeignKey("IdProfesional")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Profesional");
                });

            modelBuilder.Entity("AuthAPI903.Models.Domicilio", b =>
                {
                    b.HasOne("AuthAPI903.Models.Persona", "Persona")
                        .WithMany("Domicilios")
                        .HasForeignKey("IdPersona")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Persona");
                });

            modelBuilder.Entity("AuthAPI903.Models.Entrada", b =>
                {
                    b.HasOne("AuthAPI903.Models.Usuario", "Usuario")
                        .WithMany("Entradas")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("AuthAPI903.Models.Medicion", b =>
                {
                    b.HasOne("AuthAPI903.Models.Emocion", "Emocion")
                        .WithMany()
                        .HasForeignKey("IdEmocion")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AuthAPI903.Models.Entrada", "Entrada")
                        .WithMany()
                        .HasForeignKey("IdEntrada")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Emocion");

                    b.Navigation("Entrada");
                });

            modelBuilder.Entity("AuthAPI903.Models.Notificacion", b =>
                {
                    b.HasOne("AuthAPI903.Models.Usuario", "Usuario")
                        .WithMany("Notificaciones")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("AuthAPI903.Models.Paciente", b =>
                {
                    b.HasOne("AuthAPI903.Models.Persona", "Persona")
                        .WithOne("Paciente")
                        .HasForeignKey("AuthAPI903.Models.Paciente", "IdPersona")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AuthAPI903.Models.Usuario", null)
                        .WithMany("Pacientes")
                        .HasForeignKey("UsuarioId");

                    b.Navigation("Persona");
                });

            modelBuilder.Entity("AuthAPI903.Models.PadecimientoPaciente", b =>
                {
                    b.HasOne("AuthAPI903.Models.Paciente", "Paciente")
                        .WithMany("PadecimientoPacientes")
                        .HasForeignKey("IdPaciente")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AuthAPI903.Models.Padecimiento", "Padecimiento")
                        .WithMany("PadecimientoPacientes")
                        .HasForeignKey("IdPadecimiento")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Paciente");

                    b.Navigation("Padecimiento");
                });

            modelBuilder.Entity("AuthAPI903.Models.Profesional", b =>
                {
                    b.HasOne("AuthAPI903.Models.Usuario", "Usuario")
                        .WithMany("Profesionales")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("AuthAPI903.Models.Usuario", b =>
                {
                    b.HasOne("AuthAPI903.Models.AppUser", "AuthUser")
                        .WithOne("Usuario")
                        .HasForeignKey("AuthAPI903.Models.Usuario", "IdAppUser")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AuthAPI903.Models.Persona", "Persona")
                        .WithOne("Usuario")
                        .HasForeignKey("AuthAPI903.Models.Usuario", "IdPersona")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AuthAPI903.Models.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("AuthUser");

                    b.Navigation("Persona");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AuthAPI903.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AuthAPI903.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuthAPI903.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AuthAPI903.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AuthAPI903.Models.AppUser", b =>
                {
                    b.Navigation("Usuario")
                        .IsRequired();
                });

            modelBuilder.Entity("AuthAPI903.Models.Paciente", b =>
                {
                    b.Navigation("AsignacionPacientes");

                    b.Navigation("PadecimientoPacientes");
                });

            modelBuilder.Entity("AuthAPI903.Models.Padecimiento", b =>
                {
                    b.Navigation("PadecimientoPacientes");
                });

            modelBuilder.Entity("AuthAPI903.Models.Persona", b =>
                {
                    b.Navigation("Domicilios");

                    b.Navigation("Paciente");

                    b.Navigation("Usuario")
                        .IsRequired();
                });

            modelBuilder.Entity("AuthAPI903.Models.Profesional", b =>
                {
                    b.Navigation("AsignacionPacientes");

                    b.Navigation("DocumentoProfesionales");
                });

            modelBuilder.Entity("AuthAPI903.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("AuthAPI903.Models.Usuario", b =>
                {
                    b.Navigation("Entradas");

                    b.Navigation("Notificaciones");

                    b.Navigation("Pacientes");

                    b.Navigation("Profesionales");
                });
#pragma warning restore 612, 618
        }
    }
}
