﻿// <auto-generated />
using System;
using API_Parque_Privado2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API_Parque_Privado2.Migrations
{
    [DbContext(typeof(API_Parque_Privado2Context))]
    partial class API_Parque_Privado2ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("API_Parque_Privado2.Models.Cliente", b =>
                {
                    b.Property<int>("Nif")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Nif");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("API_Parque_Privado2.Models.Freguesia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Freguesias");
                });

            modelBuilder.Entity("API_Parque_Privado2.Models.Lugar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Andar")
                        .HasColumnType("int");

                    b.Property<string>("Fila")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Numero")
                        .HasColumnType("int");

                    b.Property<int>("ParqueId")
                        .HasColumnType("int");

                    b.Property<double>("Preco")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ParqueId");

                    b.ToTable("Lugares");
                });

            modelBuilder.Entity("API_Parque_Privado2.Models.Parque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("FreguesiaId")
                        .HasColumnType("int");

                    b.Property<string>("Rua")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FreguesiaId");

                    b.ToTable("Parques");
                });

            modelBuilder.Entity("API_Parque_Privado2.Models.Reserva", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Fim")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Inicio")
                        .HasColumnType("datetime2");

                    b.Property<int>("LugarId")
                        .HasColumnType("int");

                    b.Property<int>("NifCliente")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LugarId");

                    b.HasIndex("NifCliente");

                    b.ToTable("Reservas");
                });

            modelBuilder.Entity("API_Parque_Privado2.Models.Lugar", b =>
                {
                    b.HasOne("API_Parque_Privado2.Models.Parque", "Parque")
                        .WithMany()
                        .HasForeignKey("ParqueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parque");
                });

            modelBuilder.Entity("API_Parque_Privado2.Models.Parque", b =>
                {
                    b.HasOne("API_Parque_Privado2.Models.Freguesia", "Freguesia")
                        .WithMany()
                        .HasForeignKey("FreguesiaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Freguesia");
                });

            modelBuilder.Entity("API_Parque_Privado2.Models.Reserva", b =>
                {
                    b.HasOne("API_Parque_Privado2.Models.Lugar", "Lugar")
                        .WithMany()
                        .HasForeignKey("LugarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API_Parque_Privado2.Models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("NifCliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Lugar");
                });
#pragma warning restore 612, 618
        }
    }
}