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
        public override void AplicareReducereProdus()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                int id;
                Console.WriteLine("Introduceti ID-ul produsului: ");
                string input = Console.ReadLine();
                bool succes = int.TryParse(input, out id);

                if (succes)
                {
                    //adauga reducerea doar acolo unde sunt minim 3 produse pe stoc
                    string reducereProdusQuery = "UPDATE PRODUS SET Reducere = ? WHERE ID = ? AND Stoc > 3";
                    OleDbCommand reducereProdusCmd = new OleDbCommand(reducereProdusQuery, connection);
                    reducereProdusCmd.Parameters.AddWithValue("@Reducere", "2+1");
                    reducereProdusCmd.Parameters.AddWithValue("@ID", id);

                    try
                    {
                        connection.Open();

                        int produse = reducereProdusCmd.ExecuteNonQuery();
                        if (produse == 1)
                            Console.WriteLine("A fost aplicata reducerea 2+1.");
                        else
                            Console.WriteLine("Nu a fost aplicata reducerea 2+1.");

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
                //prima litera o transforma in majuscula
                categorie = PrimaLitera(categorie);

                try
                {
                    //adauga reducerea doar acolo unde sunt minim 3 produse pe stoc
                    connection.Open();
                    string query = "UPDATE PRODUS SET Reducere = '2+1' WHERE Categorie = ? AND Stoc > 3";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Categorie", categorie);
                        int produseAfectate = command.ExecuteNonQuery();

                        if (produseAfectate > 0)
                        {
                            Console.WriteLine($"Reducerea 2+1 a fost aplicata pentru {produseAfectate} produse din categoria {categorie}.");
                        }
                        else
                        {
                            Console.WriteLine("Nu s-au gasit produse care sa indeplineasca conditiile.");
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
        }
    }
}