using Database.VMA.Entities;
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

    public virtual DbSet<GstcalculationMaster> GstcalculationMasters { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VenderPaymentNote> VenderPaymentNotes { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    public virtual DbSet<VendorDetail> VendorDetails { get; set; }

    public virtual DbSet<VendorPayment> VendorPayments { get; set; }

    public virtual DbSet<VendorService> VendorServices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GstcalculationMaster>(entity =>
        {
            entity.HasKey(e => e.SrNo).HasName("PK__GSTCalcu__C3A4D3AC16F8356B");

            entity.ToTable("GSTCalculationMaster");

            entity.Property(e => e.CgstPercentage).HasColumnName("CGST_Percentage");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.GstCategoryName).HasColumnName("GST_Category_Name");
            entity.Property(e => e.IgstPercentage).HasColumnName("IGST_Percentage");
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.SgstPercentage).HasColumnName("SGST_Percentage");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__InvoiceD__D796AAD590836049");

            entity.Property(e => e.InvoiceId).HasColumnName("InvoiceID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07A9DF6A75");

            entity.ToTable("User");
        });

        modelBuilder.Entity<VenderPaymentNote>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("PK__VenderPa__EACE355F3C6447DF");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FkVendorId).HasColumnName("FK_VendorID");
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentNoteDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.VendorId).HasName("PK__Vendors__FC8618D31F71923A");

            entity.Property(e => e.VendorId).HasColumnName("VendorID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.VendorGstnumber).HasColumnName("VendorGSTNumber");
            entity.Property(e => e.VendorIfsccode).HasColumnName("VendorIFSCCode");
        });

        modelBuilder.Entity<VendorDetail>(entity =>
        {
            entity.HasKey(e => e.VendorDetailId).HasName("PK__VendorDe__6458FAB5A0BE2E7B");

            entity.Property(e => e.VendorDetailId).HasColumnName("VendorDetailID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DetailsYear).HasMaxLength(20);
            entity.Property(e => e.FkVendorId).HasColumnName("FK_VendorID");
            entity.Property(e => e.FkVendorServiceId).HasColumnName("FK_VendorServiceID");
            entity.Property(e => e.IsAmc).HasColumnName("IsAMC");
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<VendorPayment>(entity =>
        {
            entity.HasKey(e => e.VendorPaymentId).HasName("PK__VendorPa__68C7C3D20C7588B7");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FkInvoiceId).HasColumnName("FK_InvoiceID");
            entity.Property(e => e.FkNoteId).HasColumnName("FK_NoteId");
            entity.Property(e => e.FkVendorDetailId).HasColumnName("FK_VendorDetailID");
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentYear).HasMaxLength(20);
            entity.Property(e => e.VendorPaymentCgst)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("VendorPaymentCGST");
            entity.Property(e => e.VendorPaymentDate).HasColumnType("datetime");
            entity.Property(e => e.VendorPaymentIsGst).HasColumnName("VendorPaymentIsGST");
            entity.Property(e => e.VendorPaymentIsTdsapplicable).HasColumnName("VendorPaymentIsTDSApplicable");
            entity.Property(e => e.VendorPaymentRtgsAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.VendorPaymentSgst)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("VendorPaymentSGST");
            entity.Property(e => e.VendorPaymentTdsamount)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("VendorPaymentTDSAmount");
            entity.Property(e => e.VendorPaymentUtrnumber).HasColumnName("VendorPaymentUTRNumber");
        });

        modelBuilder.Entity<VendorService>(entity =>
        {
            entity.HasKey(e => e.VendorServiceId).HasName("PK__VendorSe__5116B960D8CAAE81");

            entity.Property(e => e.VendorServiceId).HasColumnName("VendorServiceID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FkVendorId).HasColumnName("FK_VendorID");
            entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
