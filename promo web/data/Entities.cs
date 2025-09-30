using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace Promos.Web.Data;

public class Categoria
{
    public int Id { get; set; }
    [MaxLength(50)] public string? Descripcion { get; set; }
    public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
}
public class Marca
{
    public int Id { get; set; }
    [MaxLength(50)] public string? Descripcion { get; set; }
    public ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();
}
public class Articulo
{
    public int Id { get; set; }
    [MaxLength(50)] public string? Codigo { get; set; }
    [MaxLength(50)] public string? Nombre { get; set; }
    [MaxLength(150)] public string? Descripcion { get; set; }
    public int? IdMarca { get; set; }
    public int? IdCategoria { get; set; }
    [Column(TypeName = "money")] public decimal? Precio { get; set; }
    public Marca? Marca { get; set; }
    public Categoria? Categoria { get; set; }
    public ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();
}
public class Imagen
{
    public int Id { get; set; }
    public int IdArticulo { get; set; }
    [MaxLength(1000)] public string ImagenUrl { get; set; } = null!;
    public Articulo Articulo { get; set; } = null!;
}
public class Cliente
{
    public int Id { get; set; }
    [MaxLength(50)] public string Documento { get; set; } = null!;
    [MaxLength(50)] public string Nombre { get; set; } = null!;
    [MaxLength(50)] public string Apellido { get; set; } = null!;
    [MaxLength(50)] public string Email { get; set; } = null!;
    [MaxLength(50)] public string Direccion { get; set; } = null!;
    [MaxLength(50)] public string Ciudad { get; set; } = null!;
    public int CP { get; set; }
    public ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
}
public class Voucher
{
    [Key, MaxLength(50)] public string CodigoVoucher { get; set; } = null!;
    public int? IdCliente { get; set; }
    public DateOnly? FechaCanje { get; set; }
    public int? IdArticulo { get; set; }
    public Cliente? Cliente { get; set; }
    public Articulo? Articulo { get; set; }
}

public class PromosDbContext : DbContext
{
    public PromosDbContext(DbContextOptions<PromosDbContext> o) : base(o) { }
    public DbSet<Categoria> CATEGORIAS => Set<Categoria>();
    public DbSet<Marca> MARCAS => Set<Marca>();
    public DbSet<Articulo> ARTICULOS => Set<Articulo>();
    public DbSet<Imagen> IMAGENES => Set<Imagen>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Categoria>().ToTable("CATEGORIAS");
        b.Entity<Marca>().ToTable("MARCAS");
        b.Entity<Articulo>().ToTable("ARTICULOS");
        b.Entity<Imagen>().ToTable("IMAGENES");
        b.Entity<Cliente>().ToTable("Clientes");
        b.Entity<Voucher>().ToTable("Vouchers");

        b.Entity<Articulo>()
            .HasOne(a => a.Marca).WithMany(m => m.Articulos).HasForeignKey(a => a.IdMarca);
        b.Entity<Articulo>()
            .HasOne(a => a.Categoria).WithMany(c => c.Articulos).HasForeignKey(a => a.IdCategoria);
        b.Entity<Imagen>()
            .HasOne(i => i.Articulo).WithMany(a => a.Imagenes).HasForeignKey(i => i.IdArticulo);
        b.Entity<Voucher>()
            .HasOne(v => v.Cliente).WithMany(c => c.Vouchers).HasForeignKey(v => v.IdCliente);
        b.Entity<Voucher>()
            .HasOne(v => v.Articulo).WithMany().HasForeignKey(v => v.IdArticulo);

        b.Entity<Cliente>().HasIndex(c => c.Documento).IsUnique();
        b.Entity<Voucher>().HasIndex(v => v.IdCliente); // canjeados
    }
}
