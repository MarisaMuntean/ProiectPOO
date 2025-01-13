using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace ECommerceApp
{
    internal class NavigareProduse
    {

        public void VizualizeazaProduse()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\ECommerceApp.accdb";

            using OleDbConnection connection = new OleDbConnection(connectionString) ;
            string query = "SELECT  ID, NumeProdus, Descriere, Pret, Stoc,Evaluare, Categorie FROM PRODUS";
            OleDbCommand command = new OleDbCommand(query, connection);

            try
            {
                connection.Open();

                using OleDbDataReader reader = command.ExecuteReader();
                Console.WriteLine("ID | Nume Produs| Descriere Produs | Pret | Stoc | Evaluare |Categorie");
                Console.WriteLine("-------------------------------------------------------");


                while (reader.Read())
                {
                    Console.WriteLine($"{reader["ID"],-5} | {reader["NumeProdus"],-20} | {reader["Descriere"],-10} | {reader["Pret"],-5}| {reader["Stoc"],-5}| {reader["Evaluare"],-5} |{reader["Categorie"],-20}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare la încarcarea produselor: {ex.Message}");
            }
        }
    }
}
