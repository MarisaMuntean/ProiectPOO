using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Security.Cryptography.X509Certificates;

namespace ECommerceApp
{
    internal class MetodeCOS
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\ECommerceApp.accdb";
        internal void AdaugaInCos(int idClient, int idProdus, int cantitate)
        {
            

            string selectStocQuery = "SELECT Stoc FROM PRODUS WHERE ID = @idProdus";
            string insertCosQuery = "INSERT INTO COS (ID_Client, ID_Produs, Cantitate) VALUES (@idClient, @idProdus, @cantitate)";
            string updateStocQuery = "UPDATE PRODUS SET Stoc = Stoc - @cantitate WHERE ID = @idProdus";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand selectStocCommand = new OleDbCommand(selectStocQuery, connection);
                selectStocCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                connection.Open();

                object stocObj = selectStocCommand.ExecuteScalar();
                if (stocObj != null)
                {
                    int stoc = Convert.ToInt32(stocObj);

                    if (stoc >= cantitate)
                    {
                        // Adaugă produsul în coș
                        OleDbCommand insertCosCommand = new OleDbCommand(insertCosQuery, connection);
                        insertCosCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                        insertCosCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;
                        insertCosCommand.Parameters.Add("@cantitate", OleDbType.Integer).Value = cantitate;

                        insertCosCommand.ExecuteNonQuery();

                        // Scade stocul
                        OleDbCommand updateStocCommand = new OleDbCommand(updateStocQuery, connection);
                        updateStocCommand.Parameters.Add("@cantitate", OleDbType.Integer).Value = cantitate;
                        updateStocCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                        updateStocCommand.ExecuteNonQuery();
                        Console.WriteLine("Produsul a fost adăugat în coș!");
                    }
                    else
                    {
                        Console.WriteLine("Stoc insuficient! Nu puteți adăuga această cantitate.");
                    }
                }
            }
        }

       

        internal void ModificaCantitate(int idClient, int idProdus, int cantitateNoua)
        {
         
            string selectCosQuery = "SELECT Cantitate FROM COS WHERE ID_Client = @idClient AND ID_Produs = @idProdus";
            string selectStocQuery = "SELECT Stoc FROM PRODUS WHERE ID = @idProdus";
            string updateCosQuery = "UPDATE COS SET Cantitate = @cantitateNoua WHERE ID_Client = @idClient AND ID_Produs = @idProdus";
            string updateStocQuery = "UPDATE PRODUS SET Stoc = Stoc + @diferenta WHERE ID= @idProdus";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand selectCosCommand = new OleDbCommand(selectCosQuery, connection);
                selectCosCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                selectCosCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                connection.Open();

                object cantitateVecheObj = selectCosCommand.ExecuteScalar();
                if (cantitateVecheObj != null)
                {
                    int cantitateVeche = Convert.ToInt32(cantitateVecheObj);
                    int diferenta = cantitateNoua - cantitateVeche;

                    OleDbCommand selectStocCommand = new OleDbCommand(selectStocQuery, connection);
                    selectStocCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                    object stocObj = selectStocCommand.ExecuteScalar();
                    if (stocObj != null)
                    {
                        int stoc = Convert.ToInt32(stocObj);

                        if (diferenta > 0 && diferenta > stoc)
                        {
                            Console.WriteLine("Stoc insuficient pentru a crește cantitatea!");
                        }
                        else
                        {
                            // Actualizează cantitatea în coș
                            OleDbCommand updateCosCommand = new OleDbCommand(updateCosQuery, connection);
                            updateCosCommand.Parameters.Add("@cantitateNoua", OleDbType.Integer).Value = cantitateNoua;
                            updateCosCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                            updateCosCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                            updateCosCommand.ExecuteNonQuery();

                            // Ajustează stocul
                            OleDbCommand updateStocCommand = new OleDbCommand(updateStocQuery, connection);
                            updateStocCommand.Parameters.Add("@diferenta", OleDbType.Integer).Value = -diferenta;
                            updateStocCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                            updateStocCommand.ExecuteNonQuery();
                            Console.WriteLine("Cantitatea a fost actualizată cu succes!");
                        }
                    }
                }
            }
        }

        
        internal void EliminaDinCos(int idClient, int idProdus)
        {
            string selectCosQuery = "SELECT Cantitate FROM COS WHERE ID_Client = @idClient AND ID_Produs = @idProdus";
            string deleteCosQuery = "DELETE FROM COS WHERE ID_Client = @idClient AND ID_Produs = @idProdus";
            string updateStocQuery = "UPDATE PRODUS SET Stoc = Stoc + @cantitate WHERE ID = @idProdus";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand selectCosCommand = new OleDbCommand(selectCosQuery, connection);
                selectCosCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                selectCosCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                connection.Open();

                object cantitateObj = selectCosCommand.ExecuteScalar();
                if (cantitateObj != null)
                {
                    int cantitate = Convert.ToInt32(cantitateObj);

                    // Șterge produsul din coș
                    OleDbCommand deleteCosCommand = new OleDbCommand(deleteCosQuery, connection);
                    deleteCosCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                    deleteCosCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                    deleteCosCommand.ExecuteNonQuery();

                    // Adaugă cantitatea în stoc
                    OleDbCommand updateStocCommand = new OleDbCommand(updateStocQuery, connection);
                    updateStocCommand.Parameters.Add("@cantitate", OleDbType.Integer).Value = cantitate;
                    updateStocCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                    updateStocCommand.ExecuteNonQuery();
                    Console.WriteLine("Produsul a fost șters din coș și stocul a fost actualizat!");
                }
                else
                {
                    Console.WriteLine("Produsul nu a fost găsit în coș.");
                }
            }
        }


        internal void AfiseazaCos(int idClient)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string query = "SELECT P.NumeProdus, C.Cantitate FROM COS C INNER JOIN PRODUS P ON C.ID_Produs = P.ID WHERE C.ID_Client = @idClient";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@idClient", idClient);

                connection.Open();
                using (OleDbDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Produse în coș:");
                    while (reader.Read())
                    {
                        Console.WriteLine($" - {reader["NumeProdus"]}: {reader["Cantitate"]}");
                    }
                }
            }
        }



    }
}
