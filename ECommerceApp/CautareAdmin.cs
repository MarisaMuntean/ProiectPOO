using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp
{
    internal class CautareAdmin
    {
        public bool Cautare(string nume, string prenume)
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\ECommerceApp.accdb;Persist Security Info=True";
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                string query = "SELECT COUNT(*) FROM UTILIZATOR WHERE Nume = @nume AND Prenume = @prenume AND Administrator = True";

                PrimaLitera(nume);
                PrimaLitera(prenume);

                try
                {
                    connection.Open();

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nume", nume);
                        command.Parameters.AddWithValue("@prenume", prenume);

                        int linieAfectata = (int)command.ExecuteScalar();   
                        return linieAfectata > 0;
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine("A aparut o eroare: " + ex.Message);
                    return false;
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
        }
        private static string PrimaLitera(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }

    }
}
