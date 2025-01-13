using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp
{
    internal class CautareClient
    {
        internal object CautareUtilizator(string raspuns)
        {
            
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\ECommerceApp.accdb";
            if (raspuns == "da" || raspuns == "Da" || raspuns == "DA" || raspuns == "dA")
            {
                Console.WriteLine("introduceti numele de familie: ");
                string nume = Console.ReadLine();
                Console.WriteLine("introduceti prenumele: ");
                string prenume = Console.ReadLine();

                // Conexiunea către baza de date
                string query = "SELECT * FROM UTILIZATOR WHERE Nume = @nume AND Prenume = @prenume";

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    OleDbCommand command = new OleDbCommand(query, connection);

                    // Adăugarea parametrilor pentru query
                    command.Parameters.AddWithValue("@nume", nume);
                    command.Parameters.AddWithValue("@prenume", prenume);

                    try
                    {
                        connection.Open(); // Deschide conexiunea

                        // Execută interogarea și preia rezultatele
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                object nr = reader["ID"];
                                return nr;
                            }
                            else
                            {
                                Console.WriteLine("Nu s-a gasit niciun utilizator cu acest nume.");
                                return -1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Eroare: {ex.Message}");
                        return -2;
                    }
                }
            }
            return -2;
        }
    }
}
