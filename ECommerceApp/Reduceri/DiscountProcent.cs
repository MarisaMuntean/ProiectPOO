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
        public override void AplicareReducereProdus()
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

                        //pret initial
                        int pretInitial = (int)pretCmd.ExecuteScalar();
                        Console.WriteLine("Pretul inainte de reducere: " + pretInitial);

                        //calculare pret dupa reducere
                        int pretRedus = (int)(pretInitial - (procent / 100.0 * pretInitial));
                        Console.WriteLine("Pretul dupa reducere: " + pretRedus);

                        reducereProdusCmd.Parameters.AddWithValue("@Reducere", $"{procent} %");
                        reducereProdusCmd.Parameters.AddWithValue("@Pret", pretRedus);
                        reducereProdusCmd.Parameters.AddWithValue("@ID", id);
                        int produsAfectat = reducereProdusCmd.ExecuteNonQuery();
                        if (produsAfectat == 1)
                            Console.WriteLine("A fost aplicata reducerea discount %.");
                        else
                        {
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
                string categorie = Console.ReadLine().ToLower();
                categorie = PrimaLitera(categorie);

                int procent;
                Console.WriteLine("Ce procent se va aplica pentru reducere?");
                string input = Console.ReadLine();
                bool succes = int.TryParse(input, out procent);

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
                                int pretRedus = (int)(pretInitial - (procent / 100.0 * pretInitial));

                                //mai intai se sterg parametrii pentru a nu se confunda cu valori introduse in trecut
                                reducereProdusCmd.Parameters.Clear();
                                reducereProdusCmd.Parameters.AddWithValue("@Reducere", $"{procent} %");
                                reducereProdusCmd.Parameters.AddWithValue("@Pret", pretRedus);
                                reducereProdusCmd.Parameters.AddWithValue("@ID", id);
                                reducereProdusCmd.ExecuteNonQuery();
                            }
                        }
                        Console.WriteLine($"Reducerea de {procent}% a fost aplicata produselor din categoria {categorie}.");
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