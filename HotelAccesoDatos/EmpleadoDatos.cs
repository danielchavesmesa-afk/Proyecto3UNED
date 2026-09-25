using HotelServiciosAPI.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace HotelAccesoDatos
{
    public class EmpleadoDatos
    {
        private readonly Conexion _conexion = new Conexion();

        // 1. OBTENER TODOS
        public List<Empleado> ObtenerTodos()
        {
            List<Empleado> lista = new List<Empleado>();
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "SELECT Identificacion, Nombre, PrimerApellido, SegundoApellido, Puesto, Provincia, Canton, Distrito FROM Empleados";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Empleado
                        {
                            Identificacion = dr["Identificacion"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            PrimerApellido = dr["PrimerApellido"].ToString(),
                            SegundoApellido = dr["SegundoApellido"] != DBNull.Value ? dr["SegundoApellido"].ToString() : string.Empty,
                            Puesto = dr["Puesto"].ToString(),
                            Provincia = dr["Provincia"].ToString(),
                            Canton = dr["Canton"].ToString(),
                            Distrito = dr["Distrito"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        // 2. OBTENER POR ID
        public Empleado ObtenerPorId(string identificacion)
        {
            Empleado empleado = null;
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "SELECT Identificacion, Nombre, PrimerApellido, SegundoApellido, Puesto, Provincia, Canton, Distrito FROM Empleados WHERE Identificacion = @Identificacion";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Identificacion", identificacion);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        empleado = new Empleado
                        {
                            Identificacion = dr["Identificacion"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            PrimerApellido = dr["PrimerApellido"].ToString(),
                            SegundoApellido = dr["SegundoApellido"] != DBNull.Value ? dr["SegundoApellido"].ToString() : string.Empty,
                            Puesto = dr["Puesto"].ToString(),
                            Provincia = dr["Provincia"].ToString(),
                            Canton = dr["Canton"].ToString(),
                            Distrito = dr["Distrito"].ToString()
                        };
                    }
                }
            }
            return empleado;
        }

        // 3. AGREGAR
        public bool Agregar(Empleado emp)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = @"INSERT INTO Empleados (Identificacion, Nombre, PrimerApellido, SegundoApellido, Puesto, Provincia, Canton, Distrito) 
                                 VALUES (@Identificacion, @Nombre, @PrimerApellido, @SegundoApellido, @Puesto, @Provincia, @Canton, @Distrito)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Identificacion", emp.Identificacion);
                cmd.Parameters.AddWithValue("@Nombre", emp.Nombre);
                cmd.Parameters.AddWithValue("@PrimerApellido", emp.PrimerApellido);
                cmd.Parameters.AddWithValue("@SegundoApellido", (object)emp.SegundoApellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Puesto", emp.Puesto);
                cmd.Parameters.AddWithValue("@Provincia", emp.Provincia);
                cmd.Parameters.AddWithValue("@Canton", emp.Canton);
                cmd.Parameters.AddWithValue("@Distrito", emp.Distrito);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 4. ACTUALIZAR EN SQL SERVER
        public bool Actualizar(Empleado emp)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = @"UPDATE Empleados 
                                SET Nombre = @Nombre, 
                                    PrimerApellido = @PrimerApellido, 
                                    SegundoApellido = @SegundoApellido, 
                                    Puesto = @Puesto, 
                                    Provincia = @Provincia, 
                                    Canton = @Canton, 
                                    Distrito = @Distrito 
                                WHERE Identificacion = @Identificacion";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Identificacion", emp.Identificacion);
                cmd.Parameters.AddWithValue("@Nombre", emp.Nombre);
                cmd.Parameters.AddWithValue("@PrimerApellido", emp.PrimerApellido);
                cmd.Parameters.AddWithValue("@SegundoApellido", (object)emp.SegundoApellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Puesto", emp.Puesto);
                cmd.Parameters.AddWithValue("@Provincia", emp.Provincia);
                cmd.Parameters.AddWithValue("@Canton", emp.Canton);
                cmd.Parameters.AddWithValue("@Distrito", emp.Distrito);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 5. ELIMINAR EN SQL SERVER
        public bool Eliminar(string id)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "DELETE FROM Empleados WHERE Identificacion = @Identificacion";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Identificacion", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}