using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using HotelServiciosAPI.Models; // Asegúrate de tener la referencia de proyectos lista

namespace HotelAccesoDatos
{
    public class ClienteDatos
    {
        private readonly Conexion _conexion = new Conexion();

        // 1. OBTENER TODOS LOS CLIENTES
        public List<Cliente> ObtenerTodos()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "SELECT Identificacion, TipoIdentificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento FROM Clientes";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Cliente
                        {
                            Identificacion = dr["Identificacion"].ToString(),
                            TipoIdentificacion = dr["TipoIdentificacion"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            PrimerApellido = dr["PrimerApellido"].ToString(),
                            SegundoApellido = dr["SegundoApellido"] != DBNull.Value ? dr["SegundoApellido"].ToString() : string.Empty,
                            FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"])
                        });
                    }
                }
            }
            return lista;
        }

        // 2. BUSCAR CLIENTE POR IDENTIFICACIÓN
        public Cliente ObtenerPorId(string identificacion)
        {
            Cliente cliente = null;

            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "SELECT Identificacion, TipoIdentificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento FROM Clientes WHERE Identificacion = @Identificacion";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Identificacion", identificacion);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        cliente = new Cliente
                        {
                            Identificacion = dr["Identificacion"].ToString(),
                            TipoIdentificacion = dr["TipoIdentificacion"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            PrimerApellido = dr["PrimerApellido"].ToString(),
                            SegundoApellido = dr["SegundoApellido"] != DBNull.Value ? dr["SegundoApellido"].ToString() : string.Empty,
                            FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"])
                        };
                    }
                }
            }
            return cliente;
        }

        // 3. CREAR CLIENTE
        public bool Agregar(Cliente cliente)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = @"INSERT INTO Clientes (Identificacion, TipoIdentificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento) 
                                 VALUES (@Identificacion, @TipoIdentificacion, @Nombre, @PrimerApellido, @SegundoApellido, @FechaNacimiento)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                cmd.Parameters.AddWithValue("@TipoIdentificacion", cliente.TipoIdentificacion);
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@PrimerApellido", cliente.PrimerApellido);
                cmd.Parameters.AddWithValue("@SegundoApellido", (object)cliente.SegundoApellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 4. ACTUALIZAR CLIENTE
        public bool Actualizar(Cliente cliente)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = @"UPDATE Clientes 
                                 SET TipoIdentificacion = @TipoIdentificacion, 
                                     Nombre = @Nombre, 
                                     PrimerApellido = @PrimerApellido, 
                                     SegundoApellido = @SegundoApellido, 
                                     FechaNacimiento = @FechaNacimiento 
                                 WHERE Identificacion = @Identificacion";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                cmd.Parameters.AddWithValue("@TipoIdentificacion", cliente.TipoIdentificacion);
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@PrimerApellido", cliente.PrimerApellido);
                cmd.Parameters.AddWithValue("@SegundoApellido", (object)cliente.SegundoApellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 5. ELIMINAR CLIENTE
        public bool Eliminar(string identificacion)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "DELETE FROM Clientes WHERE Identificacion = @Identificacion";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Identificacion", identificacion);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}