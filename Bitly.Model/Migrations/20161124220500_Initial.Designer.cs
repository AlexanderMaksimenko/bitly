using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Bitly.Model;

namespace BitlyService.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20161124220500_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BitlyService.Link", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("JumpsCount");

                    b.Property<string>("ShortLink")
                        .HasMaxLength(8);

                    b.Property<string>("SourceLink")
                        .HasMaxLength(2000);

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ShortLink")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("BitlyService.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BitlyService.Link", b =>
                {
                    b.HasOne("BitlyService.User", "User")
                        .WithMany("Links")
                        .HasForeignKey("UserId");
                });
        }
    }
}
