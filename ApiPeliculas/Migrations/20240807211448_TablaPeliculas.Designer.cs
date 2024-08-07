﻿// <auto-generated />
using System;
using ApiPeliculas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApiPeliculas.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240807211448_TablaPeliculas")]
    partial class TablaPeliculas
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ApiPeliculas.Modelos.Categorias", b =>
                {
                    b.Property<int>("idCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("idCategoria"));

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("NombreCategoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idCategoria");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("ApiPeliculas.Modelos.Peliculas", b =>
                {
                    b.Property<int>("PeliculaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PeliculaID"));

                    b.Property<int>("Clasificacion")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duracion")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("NombrePelicula")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RutaImagen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("categoriaID")
                        .HasColumnType("int");

                    b.Property<int?>("idCategoria")
                        .HasColumnType("int");

                    b.HasKey("PeliculaID");

                    b.HasIndex("idCategoria");

                    b.ToTable("Peliculas");
                });

            modelBuilder.Entity("ApiPeliculas.Modelos.Peliculas", b =>
                {
                    b.HasOne("ApiPeliculas.Modelos.Categorias", "Categorias")
                        .WithMany()
                        .HasForeignKey("idCategoria");

                    b.Navigation("Categorias");
                });
#pragma warning restore 612, 618
        }
    }
}
