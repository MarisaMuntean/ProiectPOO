using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Reduceri
{
    internal class DoiPlusUnu : Reducere 
    {
        public override void AplicaReducerea()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                int id;
                Console.WriteLine("Introduceti ID-ul produsului: ");
                string input = Console.ReadLine();
                bool succes = int.TryParse(input, out id);

                if (succes)
                {
                    string stocQuery = "SELECT Stoc FROM PRODUS WHERE ID = ?";
                    OleDbCommand stocCmd = new OleDbCommand(stocQuery, connection);
                    stocCmd.Parameters.AddWithValue("@ID", id);

                    string reducereProdusQuery = "UPDATE PRODUS SET Reducere = ? WHERE ID = ?";
                    OleDbCommand reducereProdusCmd = new OleDbCommand(reducereProdusQuery, connection);
                    reducereProdusCmd.Parameters.AddWithValue("@Reducere", "2+1");
                    reducereProdusCmd.Parameters.AddWithValue("@ID", id);

                    try
                    {
                        connection.Open();
                        int stoc = (int)stocCmd.ExecuteScalar();
                        if (stoc >= 3)
                        {
                            int produse = reducereProdusCmd.ExecuteNonQuery();
                            Console.WriteLine("A fost aplicata reducerea 2+1 pentru " + produse + " produs.");
                        }
                        else
                        {
                            Console.WriteLine("Nu mai sunt destule produse pe stoc.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Eroare: " + ex.Message);
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Produsul nu exista.");
                }
            }
        }
    }
}
