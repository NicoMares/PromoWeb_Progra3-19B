using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace PromoWeb.DAL
{
    public class ArticuloDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string PrimeraImagenUrl { get; set; }
    }

    public class ClienteDto
    {
        public int Id { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public int CP { get; set; }
    }

    public class PromoRepository
    {
        private readonly string _cs = ConfigurationManager.ConnectionStrings["PromosDb"].ConnectionString;

        public bool TryValidarVoucher(string codigo, out bool yaUsado)
        {
            yaUsado = false;
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("SELECT IdCliente FROM Vouchers WHERE CodigoVoucher=@c", cn))
            {
                cmd.Parameters.AddWithValue("@c", codigo);
                cn.Open();
                var r = cmd.ExecuteScalar();
                if (r == null) return false;
                yaUsado = (r != DBNull.Value);
                return true;
            }
        }

        public List<ArticuloDto> GetArticulos()
        {
            var list = new List<ArticuloDto>();
            var sql = @"
SELECT a.Id, a.Nombre, a.Descripcion,
       (SELECT TOP 1 ImagenUrl FROM IMAGENES i WHERE i.IdArticulo = a.Id ORDER BY i.Id) AS PrimeraImagenUrl
FROM ARTICULOS a
ORDER BY a.Nombre";
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new ArticuloDto
                        {
                            Id = rd.GetInt32(0),
                            Nombre = rd.IsDBNull(1) ? "" : rd.GetString(1),
                            Descripcion = rd.IsDBNull(2) ? "" : rd.GetString(2),
                            PrimeraImagenUrl = rd.IsDBNull(3) ? "" : rd.GetString(3)
                        });
                    }
                }
            }
            return list;
        }

        public ClienteDto GetClienteByDni(string dni)
        {
            var sql = @"SELECT TOP 1 Id, Documento, Nombre, Apellido, Email, Direccion, Ciudad, CP
                        FROM Clientes WHERE Documento=@dni";
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@dni", dni);
                cn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return new ClienteDto
                        {
                            Id = rd.GetInt32(0),
                            Documento = rd.GetString(1),
                            Nombre = rd.GetString(2),
                            Apellido = rd.GetString(3),
                            Email = rd.GetString(4),
                            Direccion = rd.GetString(5),
                            Ciudad = rd.GetString(6),
                            CP = rd.GetInt32(7)
                        };
                    }
                }
            }
            return null;
        }

        public int UpsertCliente(ClienteDto c)
        {
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("SELECT Id FROM Clientes WHERE Documento=@dni", cn))
            {
                cmd.Parameters.AddWithValue("@dni", c.Documento);
                cn.Open();
                var id = cmd.ExecuteScalar();
                if (id == null)
                {
                    var ins = new SqlCommand(@"
INSERT INTO Clientes(Documento,Nombre,Apellido,Email,Direccion,Ciudad,CP)
VALUES(@Documento,@Nombre,@Apellido,@Email,@Direccion,@Ciudad,@CP);
SELECT SCOPE_IDENTITY();", cn);
                    ins.Parameters.AddWithValue("@Documento", c.Documento);
                    ins.Parameters.AddWithValue("@Nombre", c.Nombre);
                    ins.Parameters.AddWithValue("@Apellido", c.Apellido);
                    ins.Parameters.AddWithValue("@Email", c.Email);
                    ins.Parameters.AddWithValue("@Direccion", c.Direccion);
                    ins.Parameters.AddWithValue("@Ciudad", c.Ciudad);
                    ins.Parameters.AddWithValue("@CP", c.CP);
                    return Convert.ToInt32(ins.ExecuteScalar());
                }
                else
                {
                    int existingId = Convert.ToInt32(id);
                    var upd = new SqlCommand(@"
UPDATE Clientes SET Nombre=@Nombre,Apellido=@Apellido,Email=@Email,Direccion=@Direccion,Ciudad=@Ciudad,CP=@CP
WHERE Id=@Id", cn);
                    upd.Parameters.AddWithValue("@Nombre", c.Nombre);
                    upd.Parameters.AddWithValue("@Apellido", c.Apellido);
                    upd.Parameters.AddWithValue("@Email", c.Email);
                    upd.Parameters.AddWithValue("@Direccion", c.Direccion);
                    upd.Parameters.AddWithValue("@Ciudad", c.Ciudad);
                    upd.Parameters.AddWithValue("@CP", c.CP);
                    upd.Parameters.AddWithValue("@Id", existingId);
                    upd.ExecuteNonQuery();
                    return existingId;
                }
            }
        }

        public void CanjearVoucher(string codigo, int clienteId, int articuloId)
        {
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand(@"
UPDATE Vouchers
SET IdCliente=@cli, IdArticulo=@art, FechaCanje=CAST(GETDATE() AS date)
WHERE CodigoVoucher=@c", cn))
            {
                cmd.Parameters.AddWithValue("@cli", clienteId);
                cmd.Parameters.AddWithValue("@art", articuloId);
                cmd.Parameters.AddWithValue("@c", codigo);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public string GetArticuloNombre(int id)
        {
            using (var cn = new SqlConnection(_cs))
            using (var cmd = new SqlCommand("SELECT TOP 1 Nombre FROM ARTICULOS WHERE Id=@id", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                var r = cmd.ExecuteScalar();
                return r == null ? "" : r.ToString();
            }
        }
    }
}