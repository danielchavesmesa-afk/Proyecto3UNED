using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using HotelServiciosAPI.Models;

namespace HotelServiciosAPI.Services
{
    public class DataRepository
    {
        private readonly string _cadenaConexion = "Server=localhost\\SQLEXPRESS;Database=HotelUNEDDb;Trusted_Connection=True;TrustServerCertificate=True;";

        // ==========================================
        // 1. CLIENTES
        // ==========================================
        public List<Cliente> Clientes
        {
            get
            {
                var lista = new List<Cliente>();
                using (var con = new SqlConnection(_cadenaConexion))
                {
                    string query = "SELECT Identificacion, TipoIdentificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento FROM Clientes";
                    var cmd = new SqlCommand(query, con);
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
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
        }

        public Cliente ObtenerCliente(string id)
        {
            return Clientes.Find(c => c.Identificacion == id);
        }

        public bool AgregarCliente(Cliente cliente)
        {
            using (var con = new SqlConnection(_cadenaConexion))
            {
                string query = @"INSERT INTO Clientes (Identificacion, TipoIdentificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento) 
                                 VALUES (@Identificacion, @TipoIdentificacion, @Nombre, @PrimerApellido, @SegundoApellido, @FechaNacimiento)";

                var cmd = new SqlCommand(query, con);
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

        // ==========================================
        // 2. EMPLEADOS
        // ==========================================
        public List<Empleado> Empleados
        {
            get
            {
                var lista = new List<Empleado>();
                using (var con = new SqlConnection(_cadenaConexion))
                {
                    string query = "SELECT Identificacion, Nombre, PrimerApellido, SegundoApellido, Puesto, Provincia, Canton, Distrito FROM Empleados";
                    var cmd = new SqlCommand(query, con);
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
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
        }

        public Empleado ObtenerEmpleado(string id)
        {
            return Empleados.Find(e => e.Identificacion == id);
        }

        public bool AgregarEmpleado(Empleado emp)
        {
            using (var con = new SqlConnection(_cadenaConexion))
            {
                string query = @"INSERT INTO Empleados (Identificacion, Nombre, PrimerApellido, SegundoApellido, Puesto, Provincia, Canton, Distrito) 
                         VALUES (@Identificacion, @Nombre, @PrimerApellido, @SegundoApellido, @Puesto, @Provincia, @Canton, @Distrito)";

                var cmd = new SqlCommand(query, con);
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

        // ==========================================
        // 3. HABITACIONES
        // ==========================================
        public List<Habitacion> Habitaciones
        {
            get
            {
                var lista = new List<Habitacion>();
                using (var con = new SqlConnection(_cadenaConexion))
                {
                    string query = "SELECT Numero, Tipo, PrecioPorNoche, Estado FROM Habitaciones";
                    var cmd = new SqlCommand(query, con);
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
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
        }

        public Habitacion ObtenerHabitacion(string numero)
        {
            return Habitaciones.Find(h => h.Numero == numero);
        }
        public bool AgregarHabitacion(Habitacion hab)
        {
            using (var con = new SqlConnection(_cadenaConexion))
            {
                string query = "INSERT INTO Habitaciones (Numero, Tipo, PrecioPorNoche, Estado) VALUES (@Numero, @Tipo, @PrecioPorNoche, @Estado)";
                var cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Numero", hab.Numero);
                cmd.Parameters.AddWithValue("@Tipo", hab.Tipo);
                cmd.Parameters.AddWithValue("@PrecioPorNoche", hab.PrecioPorNoche);
                cmd.Parameters.AddWithValue("@Estado", hab.Estado);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        // ==========================================
        // 4. RESERVACIONES
        // ==========================================
        public List<Reservacion> Reservaciones
        {
            get
            {
                var lista = new List<Reservacion>();
                using (var con = new SqlConnection(_cadenaConexion))
                {
                    string query = "SELECT CodigoReservacion, ClienteId, HabitacionNumero, FechaInicio, FechaSalida, Estado, SolicitudesEspeciales, TarifaCalculada, PorcentajeDescuento, MontoTotal, FechaRealizacion FROM Reservaciones";
                    var cmd = new SqlCommand(query, con);
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
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
        }

        public Reservacion ObtenerReservacion(string codigo)
        {
            return Reservaciones.Find(r => r.CodigoReservacion == codigo);
        }

    

        public bool AgregarReservacion(Reservacion res)
        {
            using (var con = new SqlConnection(_cadenaConexion))
            {
                string query = @"INSERT INTO Reservaciones 
                        (CodigoReservacion, ClienteId, HabitacionNumero, FechaInicio, FechaSalida, Estado, SolicitudesEspeciales, TarifaCalculada, PorcentajeDescuento, MontoTotal, FechaRealizacion) 
                        VALUES 
                        (@CodigoReservacion, @ClienteId, @HabitacionNumero, @FechaInicio, @FechaSalida, @Estado, @SolicitudesEspeciales, @TarifaCalculada, @PorcentajeDescuento, @MontoTotal, @FechaRealizacion)";

                var cmd = new SqlCommand(query, con);
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
    }
}