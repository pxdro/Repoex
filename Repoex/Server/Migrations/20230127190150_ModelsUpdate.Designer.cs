﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repoex.Server.Context;

#nullable disable

namespace Repoex.Server.Migrations
{
    [DbContext(typeof(RepoexContext))]
    [Migration("20230127190150_ModelsUpdate")]
    partial class ModelsUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PermissaoUsuario", b =>
                {
                    b.Property<Guid>("PermissoesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsuariosId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PermissoesId", "UsuariosId");

                    b.HasIndex("UsuariosId");

                    b.ToTable("PermissaoUsuario");
                });

            modelBuilder.Entity("Repoex.Shared.Models.Permissao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Relatorio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Permissoes", (string)null);
                });

            modelBuilder.Entity("Repoex.Shared.Models.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Admin")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("PermissaoUsuario", b =>
                {
                    b.HasOne("Repoex.Shared.Models.Permissao", null)
                        .WithMany()
                        .HasForeignKey("PermissoesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Repoex.Shared.Models.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UsuariosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
