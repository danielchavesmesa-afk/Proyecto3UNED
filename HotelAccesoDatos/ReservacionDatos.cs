using HotelServiciosAPI.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace HotelAccesoDatos
{
    public class ReservacionDatos
    {
        private readonly Conexion _conexion = new Conexion();

        // 1. OBTENER TODAS
        public List<Reservacion> ObtenerTodas()
        {
            List<Reservacion> lista = new List<Reservacion>();
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "SELECT CodigoReservacion, ClienteId, HabitacionNumero, FechaInicio, FechaSalida, Estado, SolicitudesEspeciales, TarifaCalculada, PorcentajeDescuento, MontoTotal, FechaRealizacion FROM Reservaciones";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Reservacion
                        {
                            CodigoReservacion = dr["CodigoReservacion"].ToString(),
                            ClienteId = dr["ClienteId"].ToString(),
                            HabitacionNumero = dr["HabitacionNumero"].ToString(),
                            FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                            FechaSalida = Convert.ToDateTime(dr["FechaSalida"]),
                            Estado = dr["Estado"].ToString(),
                            SolicitudesEspeciales = dr["SolicitudesEspeciales"] != DBNull.Value ? dr["SolicitudesEspeciales"].ToString() : string.Empty,
                            TarifaCalculada = Convert.ToDecimal(dr["TarifaCalculada"]),
                            PorcentajeDescuento = Convert.ToDecimal(dr["PorcentajeDescuento"]),
                            MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                            FechaRealizacion = Convert.ToDateTime(dr["FechaRealizacion"])
                        });
                    }
                }
            }
            return lista;
        }

        // 2. AGREGAR
        public bool Agregar(Reservacion res)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = @"INSERT INTO Reservaciones 
                                (CodigoReservacion, ClienteId, HabitacionNumero, FechaInicio, FechaSalida, Estado, SolicitudesEspeciales, TarifaCalculada, PorcentajeDescuento, MontoTotal, FechaRealizacion) 
                                VALUES 
                                (@CodigoReservacion, @ClienteId, @HabitacionNumero, @FechaInicio, @FechaSalida, @Estado, @SolicitudesEspeciales, @TarifaCalculada, @PorcentajeDescuento, @MontoTotal, @FechaRealizacion)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CodigoReservacion", res.CodigoReservacion);
                cmd.Parameters.AddWithValue("@ClienteId", res.ClienteId);
                cmd.Parameters.AddWithValue("@HabitacionNumero", res.HabitacionNumero);
                cmd.Parameters.AddWithValue("@FechaInicio", res.FechaInicio);
                cmd.Parameters.AddWithValue("@FechaSalida", res.FechaSalida);
                cmd.Parameters.AddWithValue("@Estado", res.Estado);
                cmd.Parameters.AddWithValue("@SolicitudesEspeciales", (object)res.SolicitudesEspeciales ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TarifaCalculada", res.TarifaCalculada);
                cmd.Parameters.AddWithValue("@PorcentajeDescuento", res.PorcentajeDescuento);
                cmd.Parameters.AddWithValue("@MontoTotal", res.MontoTotal);
                cmd.Parameters.AddWithValue("@FechaRealizacion", res.FechaRealizacion);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 3. BUSCAR POR CÓDIGO (Cumple requisito de búsqueda específica)
        public Reservacion ObtenerPorCodigo(string codigo)
        {
            Reservacion reservacion = null;

            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "SELECT CodigoReservacion, ClienteId, HabitacionNumero, FechaInicio, FechaSalida, Estado, SolicitudesEspeciales, TarifaCalculada, PorcentajeDescuento, MontoTotal, FechaRealizacion FROM Reservaciones WHERE CodigoReservacion = @Codigo";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Codigo", codigo);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        reservacion = new Reservacion
                        {
                            CodigoReservacion = dr["CodigoReservacion"].ToString(),
                            ClienteId = dr["ClienteId"].ToString(),
                            HabitacionNumero = dr["HabitacionNumero"].ToString(),
                            FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                            FechaSalida = Convert.ToDateTime(dr["FechaSalida"]),
                            Estado = dr["Estado"].ToString(),
                            SolicitudesEspeciales = dr["SolicitudesEspeciales"] != DBNull.Value ? dr["SolicitudesEspeciales"].ToString() : null,
                            TarifaCalculada = Convert.ToDecimal(dr["TarifaCalculada"]),
                            PorcentajeDescuento = Convert.ToDecimal(dr["PorcentajeDescuento"]),
                            MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                            FechaRealizacion = Convert.ToDateTime(dr["FechaRealizacion"])
                        };
                    }
                }
            }
            return reservacion;
        }

        // 4. ACTUALIZAR EN SQL SERVER
        public bool Actualizar(Reservacion res)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = @"UPDATE Reservaciones 
                                SET ClienteId = @ClienteId, 
                                    HabitacionNumero = @HabitacionNumero, 
                                    FechaInicio = @FechaInicio, 
                                    FechaSalida = @FechaSalida, 
                                    Estado = @Estado, 
                                    SolicitudesEspeciales = @SolicitudesEspeciales, 
                                    TarifaCalculada = @TarifaCalculada, 
                                    PorcentajeDescuento = @PorcentajeDescuento, 
                                    MontoTotal = @MontoTotal 
                                WHERE CodigoReservacion = @CodigoReservacion";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CodigoReservacion", res.CodigoReservacion);
                cmd.Parameters.AddWithValue("@ClienteId", res.ClienteId);
                cmd.Parameters.AddWithValue("@HabitacionNumero", res.HabitacionNumero);
                cmd.Parameters.AddWithValue("@FechaInicio", res.FechaInicio);
                cmd.Parameters.AddWithValue("@FechaSalida", res.FechaSalida);
                cmd.Parameters.AddWithValue("@Estado", res.Estado);
                cmd.Parameters.AddWithValue("@SolicitudesEspeciales", (object)res.SolicitudesEspeciales ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TarifaCalculada", res.TarifaCalculada);
                cmd.Parameters.AddWithValue("@PorcentajeDescuento", res.PorcentajeDescuento);
                cmd.Parameters.AddWithValue("@MontoTotal", res.MontoTotal);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // 5. ELIMINAR EN SQL SERVER
        public bool Eliminar(string codigo)
        {
            using (SqlConnection con = _conexion.ObtenerConexion())
            {
                string query = "DELETE FROM Reservaciones WHERE CodigoReservacion = @Codigo";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Codigo", codigo);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}