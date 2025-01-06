using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Reduceri
{
    internal class DiscountFix : Reducere
    {
        public override void AplicareReducereProdus()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                int id, fix;
                Console.WriteLine("Introduceti ID-ul produsului: ");
                string input = Console.ReadLine();
                bool succes = int.TryParse(input, out id);

                Console.WriteLine("Ce discount se va aplica pentru reducere?");
                input = Console.ReadLine();
                if (succes != int.TryParse(input, out fix))
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
                        int pretInitial = (int)pretCmd.ExecuteScalar();
                        if (pretInitial != null)
                        {
                            Console.WriteLine("Pretul inainte de reducere: " + pretInitial);
                            int pretRedus = pretInitial - fix;
                            Console.WriteLine("Pretul dupa reducere: " + pretRedus);

                            reducereProdusCmd.Parameters.AddWithValue("@Reducere", $"-{fix}");
                            reducereProdusCmd.Parameters.AddWithValue("@Pret", pretRedus);
                            reducereProdusCmd.Parameters.AddWithValue("@ID", id);
                            int produsAfectat = reducereProdusCmd.ExecuteNonQuery();
                            if (produsAfectat == 1)
                                Console.WriteLine("A fost aplicata reducerea discount fix.");
                            else
                                Console.WriteLine("Nu a fost adaugata reducerea discount %");

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

        public override void AplicareReducereCategorie()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                Console.WriteLine("Introduceti categoria: ");
                string categorie = Console.ReadLine();
                categorie = PrimaLitera(categorie);

                int fix;
                Console.WriteLine("Ce discount se va aplica pentru reducere?");
                string input = Console.ReadLine();
                bool succes = int.TryParse(input, out fix);

                if (succes)
                {
                    string pretQuery = "SELECT ID, Pret FROM PRODUS WHERE Categorie = ?";
                    OleDbCommand pretCmd = new OleDbCommand(pretQuery, connection);
                    pretCmd.Parameters.AddWithValue("@Categorie", categorie);

                    string reducereProdusQuery = "UPDATE PRODUS SET Reducere = ?, Pret = ? WHERE ID = ?";
                    OleDbCommand reducereProdusCmd = new OleDbCommand(reducereProdusQuery, connection);


                    try
                    {
                        connection.Open();

                        //se parcurg coloanele id si pret pentru categoria cautata
                        using (OleDbDataReader reader = pretCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                int pretInitial = reader.GetInt32(1);
                                int pretRedus = pretInitial - fix;

                                //mai intai se sterg parametrii pentru a nu se confunda cu valori introduse in trecut
                                reducereProdusCmd.Parameters.Clear();
                                reducereProdusCmd.Parameters.AddWithValue("@Reducere", $"-{fix}");
                                reducereProdusCmd.Parameters.AddWithValue("@Pret", pretRedus);
                                reducereProdusCmd.Parameters.AddWithValue("@ID", id);
                                reducereProdusCmd.ExecuteNonQuery();
                            }
                        }
                        Console.WriteLine($"Reducerea de -{fix} a fost aplicata produselor din categoria {categorie}.");
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