using HotelServiciosAPI.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace HotelAccesoDatos
{
    public class HabitacionDatos
    {
        private readonly Conexion _conexion = new Conexion();

        // 1. OBTENER TODAS
        public List<Habitacion> ObtenerTodas()
        {
            List<Habitacion> lista = new List<Habitacion>();
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "SELECT Numero, Tipo, PrecioPorNoche, Estado FROM Habitaciones";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Habitacion
                        {
                            Numero = dr["Numero"].ToString(),
                            Tipo = dr["Tipo"].ToString(),
                            PrecioPorNoche = Convert.ToDecimal(dr["PrecioPorNoche"]),
                            Estado = dr["Estado"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        // 2. OBTENER POR NÚMERO
        public Habitacion ObtenerPorNumero(string numero)
        {
            Habitacion hab = null;
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "SELECT Numero, Tipo, PrecioPorNoche, Estado FROM Habitaciones WHERE Numero = @Numero";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Numero", numero);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hab = new Habitacion
                        {
                            Numero = dr["Numero"].ToString(),
                            Tipo = dr["Tipo"].ToString(),
                            PrecioPorNoche = Convert.ToDecimal(dr["PrecioPorNoche"]),
                            Estado = dr["Estado"].ToString()
                        };
                    }
                }
            }
            return hab;
        }

        // 3. AGREGAR
        public bool Agregar(Habitacion hab)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "INSERT INTO Habitaciones (Numero, Tipo, PrecioPorNoche, Estado) VALUES (@Numero, @Tipo, @PrecioPorNoche, @Estado)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Numero", hab.Numero);
                cmd.Parameters.AddWithValue("@Tipo", hab.Tipo);
                cmd.Parameters.AddWithValue("@PrecioPorNoche", hab.PrecioPorNoche);
                cmd.Parameters.AddWithValue("@Estado", hab.Estado);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 4. ACTUALIZAR EN SQL SERVER
        public bool Actualizar(Habitacion hab)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = @"UPDATE Habitaciones 
                                SET Tipo = @Tipo, 
                                    PrecioPorNoche = @PrecioPorNoche, 
                                    Estado = @Estado 
                                WHERE Numero = @Numero";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Numero", hab.Numero);
                cmd.Parameters.AddWithValue("@Tipo", hab.Tipo);
                cmd.Parameters.AddWithValue("@PrecioPorNoche", hab.PrecioPorNoche);
                cmd.Parameters.AddWithValue("@Estado", hab.Estado);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 5. ELIMINAR EN SQL SERVER
        public bool Eliminar(string numero)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "DELETE FROM Habitaciones WHERE Numero = @Numero";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Numero", numero);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}