using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp
{
    internal class MetodeWISHLIST
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Facultate/ANUL_2/POO/CommerceAPP/ECommerceApp.accdb";

        // Metoda pentru a adauga un produs în Wishlist
        // Metoda pentru a adauga un produs în Wishlist
        internal void AdaugaInWishlist(int idClient, int idProdus)
        {
            string query = "INSERT INTO WISHLIST (ID_Client, ID_Produs) VALUES (@idClient, @idProdus)";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                    command.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;
                    command.ExecuteNonQuery();

                    Console.WriteLine("Produsul a fost adaugat în Wishlist cu succes.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
            }
        }
        // Metoda pentru a muta produsele din Wishlist în Cos
        internal void MutaDinWishlistInCos(int idClient, int idProdus, int cantitate)
        {
            string selectWishlistQuery = "SELECT ID_Produs FROM WISHLIST WHERE ID_Client = @idClient AND ID_Produs = @idProdus";
            string selectStocQuery = "SELECT Stoc FROM PRODUS WHERE ID = @idProdus";
            string updateStocQuery = "UPDATE PRODUS SET Stoc = Stoc - @cantitate WHERE ID = @idProdus";
            string insertCosQuery = "INSERT INTO COS (ID_Client, ID_Produs, Cantitate) VALUES (@idClient, @idProdus, @cantitate)";
            string deleteWishlistQuery = "DELETE FROM WISHLIST WHERE ID_Client = @idClient AND ID_Produs = @idProdus";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Verificam daca produsul exista in Wishlist
                    OleDbCommand selectWishlistCommand = new OleDbCommand(selectWishlistQuery, connection);
                    selectWishlistCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                    selectWishlistCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                    object resultWishlist = selectWishlistCommand.ExecuteScalar();

                    if (resultWishlist != null) // Produsul exista in Wishlist
                    {
                        // Verificam stocul produsului
                        OleDbCommand selectStocCommand = new OleDbCommand(selectStocQuery, connection);
                        selectStocCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                        object stocObj = selectStocCommand.ExecuteScalar();
                        if (stocObj == null)
                        {
                            Console.WriteLine("Produsul specificat nu exista in baza de date.");
                            return;
                        }

                        int stoc = Convert.ToInt32(stocObj);

                        if (stoc < cantitate)
                        {
                            Console.WriteLine($"Stoc insuficient pentru produsul cu ID-ul {idProdus}. Stoc disponibil: {stoc}");
                            return;
                        }

                        // Scadem stocul
                        OleDbCommand updateStocCommand = new OleDbCommand(updateStocQuery, connection);
                        updateStocCommand.Parameters.Add("@cantitate", OleDbType.Integer).Value = cantitate;
                        updateStocCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;
                        updateStocCommand.ExecuteNonQuery();

                        // Adaugam produsul în Coș
                        OleDbCommand insertCosCommand = new OleDbCommand(insertCosQuery, connection);
                        insertCosCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                        insertCosCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;
                        insertCosCommand.Parameters.Add("@cantitate", OleDbType.Integer).Value = cantitate;
                        insertCosCommand.ExecuteNonQuery();

                        // stergem produsul din Wishlist
                        OleDbCommand deleteWishlistCommand = new OleDbCommand(deleteWishlistQuery, connection);
                        deleteWishlistCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                        deleteWishlistCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;
                        deleteWishlistCommand.ExecuteNonQuery();

                        Console.WriteLine($"Produsul (ID: {idProdus}) a fost mutat din Wishlist în Cos cu cantitatea {cantitate}. Stoc actualizat: {stoc - cantitate}");
                    }
                    else
                    {
                        Console.WriteLine("Produsul specificat nu există în Wishlist.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
            }
        }


        // Metoda pentru a vizualiza Wishlist-ul
        internal void VizualizeazaWishlist(int idClient)
        {
            string query = "SELECT P.ID, P.NumeProdus, P.Pret FROM WISHLIST W INNER JOIN PRODUS P ON W.ID_Produs = P.ID WHERE W.ID_Client = @idClient";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Wishlist-ul clientului:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID Produs: {reader["ID"]}, Nume: {reader["NumeProdus"]}, Pret: {reader["Pret"]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
            }
        }


        internal void StergeProdusDinWishlist(int idClient, int idProdus)
        {
            string deleteQuery = "DELETE FROM WISHLIST WHERE ID_Client = @idClient AND ID_Produs = @idProdus";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Ștergem produsul din Wishlist
                    OleDbCommand deleteCommand = new OleDbCommand(deleteQuery, connection);
                    deleteCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                    deleteCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Produsul cu ID-ul {idProdus} a fost șters din Wishlist-ul clientului cu ID-ul {idClient}.");
                    }
                    else
                    {
                        Console.WriteLine($"Produsul cu ID-ul {idProdus} nu a fost găsit în Wishlist-ul clientului cu ID-ul {idClient}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
            }
        }


    }
}
