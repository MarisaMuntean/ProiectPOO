using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Reduceri
{
    internal class DiscountProcent : Reducere
    {
        public override void AplicaReducerea()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                int id, procent;
                Console.WriteLine("Introduceti ID-ul produsului: ");
                string input = Console.ReadLine();
                bool succes = int.TryParse(input, out id);

                Console.WriteLine("Ce procent se va aplica pentru reducere?");
                input = Console.ReadLine();
                if (succes != int.TryParse(input, out procent))
                    succes = false;

                if (succes)
                {
                    string pretQuery = "SELECT Pret FROM PRODUS WHERE ID = ?";
                    OleDbCommand pretCmd = new OleDbCommand(pretQuery, connection);
                    pretCmd.Parameters.AddWithValue("@ID", id);

                    string reducereProdusQuery = "UPDATE PRODUS SET Reducere = ?, Pret = ? WHERE ID = ?";
                    OleDbCommand reducereProdusCmd = new OleDbCommand(reducereProdusQuery, connection);


                    try
                    {
                        connection.Open();
                        object pretObj = pretCmd.ExecuteScalar();
                        if (pretObj != DBNull.Value)
                        {
                            double pretInitial = Convert.ToDouble(pretObj);
                            Console.WriteLine("Pretul inainte de reducere: " + pretInitial);
                            double pretRedus = pretInitial - (procent / 100 * pretInitial);
                            Console.WriteLine("Pretul dupa reducere: " + pretRedus);

                            reducereProdusCmd.Parameters.AddWithValue("@Reducere", $"{procent} %");
                            reducereProdusCmd.Parameters.AddWithValue("@Pret", pretRedus);
                            reducereProdusCmd.Parameters.AddWithValue("@ID", id);
                            int produsAfectat = reducereProdusCmd.ExecuteNonQuery();
                            if (produsAfectat == 1)
                                Console.WriteLine("A fost aplicata reducerea discount % pentru " + produsAfectat + " produs.");
                            else
                            {
                                Console.WriteLine("Niciun produs nu a fost afectat.");
                            }
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
