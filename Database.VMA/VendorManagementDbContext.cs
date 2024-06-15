using System;
using System.Collections.Generic;
using Database.VMA.Models;
using Microsoft.EntityFrameworkCore;

namespace Database.VMA;

public partial class VendorManagementDbContext : DbContext
{
    public VendorManagementDbContext()
    {
    }

    public VendorManagementDbContext(DbContextOptions<VendorManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=SUMITSLAPTOP\\SQLEXPRESS;Database=VendorManagementDB;Trusted_Connection=true;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07A2A57614");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E4D3A90032").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534E1ED7423").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
