using Microsoft.Data.SqlClient;

namespace HotelAccesoDatos
{
    public class Conexion
    {
        // Cadena de conexión hacia tu SQL Server Express local
        private readonly string _cadenaConexion = "Server=localhost\\SQLEXPRESS;Database=HotelUNEDDb;Trusted_Connection=True;TrustServerCertificate=True;";

        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(_cadenaConexion);
        }
    }
}