using System.Configuration;
using System.Data.SqlClient;

namespace YemekKarti
{
    public class Db
    {
        public static SqlConnection Baglanti()
        {
            string baglantiCumlesi = ConfigurationManager.ConnectionStrings["YemekDB"].ConnectionString;
            return new SqlConnection(baglantiCumlesi);
        }
    }
}