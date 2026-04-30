using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProcurmentProject.Data.Models;

public partial class ProcurmentSystemContext : DbContext
{
    public ProcurmentSystemContext()
    {
    }

    public ProcurmentSystemContext(DbContextOptions<ProcurmentSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<PrProduct> PrProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PurchasedRequisition> PurchasedRequisitions { get; set; }

    public virtual DbSet<Rfq> Rfqs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierQuotation> SupplierQuotations { get; set; }

    public virtual DbSet<SuppliesDelivery> SuppliesDeliveries { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCompany> UserCompanies { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VwSupplierDelivery> VwSupplierDeliveries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__companie__3213E83FE857E30E");

            entity.ToTable("companies");

            entity.HasIndex(e => new { e.NtnNumber, e.Name }, "uq_user").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.NtnNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ntn_number");
            entity.Property(e => e.RegisterIn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("register_in");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__document__3213E83F595073EB");

            entity.ToTable("documents");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BelongId).HasColumnName("belong_id");
            entity.Property(e => e.BelongName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("belong_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.EncodedFileName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("encoded_file_name");
            entity.Property(e => e.OriginalFileName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("original_file_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Url)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("url");
        });

        modelBuilder.Entity<PrProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pr_produ__3213E83FB8692964");

            entity.ToTable("pr_products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.PrId).HasColumnName("pr_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Pr).WithMany(p => p.PrProducts)
                .HasForeignKey(d => d.PrId)
                .HasConstraintName("FK__pr_produc__pr_id__09A971A2");

            entity.HasOne(d => d.Product).WithMany(p => p.PrProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__pr_produc__produ__0A9D95DB");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__products__3213E83F6226AE71");

            entity.ToTable("products");

            entity.HasIndex(e => e.Upc, "uq_product")
                .IsUnique()
                .HasFilter("([deleted]=(0))");

            entity.HasIndex(e => e.Upc, "uq_upc").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Company)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("company");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Upc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("upc");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PurchasedRequisition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__purchase__3213E83F2BF0119A");

            entity.ToTable("purchased_requisitions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.DeliveryDate).HasColumnName("delivery_date");
            entity.Property(e => e.EstimatedBudget)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estimated_budget");
            entity.Property(e => e.Note)
                .HasColumnType("text")
                .HasColumnName("note");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.PurchasedRequisitions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__purchased__user___607251E5");
        });

        modelBuilder.Entity<Rfq>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rfqs__3213E83F707AE414");

            entity.ToTable("rfqs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.PrId).HasColumnName("pr_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Pr).WithMany(p => p.Rfqs)
                .HasForeignKey(d => d.PrId)
                .HasConstraintName("FK__rfqs__pr_id__0F624AF8");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_roles");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Permission)
                .HasColumnType("text")
                .HasColumnName("permission");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supplier__3213E83F577AB82C");

            entity.ToTable("suppliers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("company_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.NtnTaxNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ntn_tax_number");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__suppliers__user___6D0D32F4");
        });

        modelBuilder.Entity<SupplierQuotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supplier__3213E83F298E7B3D");

            entity.ToTable("supplier_quotations");

            entity.HasIndex(e => e.UnitPrice, "id_unit_price");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.FinalPrice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("final_price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RfqId).HasColumnName("rfq_id");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.UnitPrice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unit_price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Product).WithMany(p => p.SupplierQuotations)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__supplier___produ__160F4887");

            entity.HasOne(d => d.Rfq).WithMany(p => p.SupplierQuotations)
                .HasForeignKey(d => d.RfqId)
                .HasConstraintName("FK__supplier___rfq_i__17036CC0");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierQuotations)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__supplier___suppl__151B244E");
        });

        modelBuilder.Entity<SuppliesDelivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__supplies__3213E83F733FA4D4");

            entity.ToTable("supplies_delivery");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.Note)
                .HasColumnType("text")
                .HasColumnName("note");
            entity.Property(e => e.RecevingDatetime)
                .HasColumnType("datetime")
                .HasColumnName("receving_datetime");
            entity.Property(e => e.RecivedBy)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("recived_by");
            entity.Property(e => e.RfqId).HasColumnName("rfq_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.SuplierId).HasColumnName("suplier_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Rfq).WithMany(p => p.SuppliesDeliveries)
                .HasForeignKey(d => d.RfqId)
                .HasConstraintName("FK__supplies___rfq_i__1CBC4616");

            entity.HasOne(d => d.Suplier).WithMany(p => p.SuppliesDeliveries)
                .HasForeignKey(d => d.SuplierId)
                .HasConstraintName("FK__supplies___supli__1DB06A4F");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FC086247D");

            entity.ToTable("users", tb => tb.HasTrigger("deleteTrigger"));

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164EFFE789E").IsUnique();

            entity.HasIndex(e => e.Email, "email_ind");

            entity.HasIndex(e => e.Email, "uq_email")
                .IsUnique()
                .HasFilter("([deleted]=(0))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deleted)
                .HasDefaultValue((byte)0)
                .HasColumnName("deleted");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasColumnType("text")
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<UserCompany>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_com__3213E83FC2A1192F");

            entity.ToTable("user_companies");

            entity.HasIndex(e => new { e.EffectToDatetime, e.EffectFromDatetime }, "uq_effect_date").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EffectFromDatetime)
                .HasColumnType("datetime")
                .HasColumnName("effect_from_datetime");
            entity.Property(e => e.EffectToDatetime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("effect_to_datetime");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("update_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Company).WithMany(p => p.UserCompanies)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK__user_comp__compa__797309D9");

            entity.HasOne(d => d.User).WithMany(p => p.UserCompanies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__user_comp__user___787EE5A0");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__user_rol__3213E83FDF3A04D5");

            entity.ToTable("user_roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_roles");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_role__user___6754599E");
        });

        modelBuilder.Entity<VwSupplierDelivery>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_SupplierDelivery");

            entity.Property(e => e.DeliveryNote).HasColumnType("text");
            entity.Property(e => e.DeliveryStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FinalPrice)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("finalPrice");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.RecevingDatetime)
                .HasColumnType("datetime")
                .HasColumnName("receving_datetime");
            entity.Property(e => e.RecivedByName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RfqStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("rfqStatus");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UnitPrice)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
