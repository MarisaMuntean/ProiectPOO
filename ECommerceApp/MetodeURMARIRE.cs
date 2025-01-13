using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp
{
    internal class MetodeURMARIRE
    {
        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\ECommerceApp.accdb";
        internal bool AfiseazaComenziClient(string nume, string prenume)
        {
            string query = @" SELECT ID FROM COMENZI WHERE Nume_Client = @nume AND Prenume_Client = @prenume";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.AddWithValue("@nume", nume);
                    command.Parameters.AddWithValue("@prenume", prenume);

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine($"Comenzile plasate de {nume} {prenume}:");
                        bool hasResults = false;

                        while (reader.Read())
                        {
                            Console.WriteLine($"ID Comanda: {reader["ID"]}");
                            hasResults = true;
                        }

                        if (!hasResults)
                        {
                            Console.WriteLine($"Clientul {nume} {prenume} nu are comenzi plasate.");
                        }
                        return hasResults;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                    return false;
                }
            }
        }
        internal string StatusComanda(int idComanda)
        {
            string query = "SELECT Status FROM COMENZI WHERE ID= @idComanda";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.Add("@idComanda", OleDbType.Integer).Value = idComanda;

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }
                    else
                    {
                        return "Comanda nu a fost gasita.";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                    return "Eroare la interogare.";
                }
            }
        }
        internal void AcordaEvaluariProduse(int idComanda)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Verificăm statusul comenzii
                    string checkStatusQuery = "SELECT Status FROM COMENZI WHERE ID = @idComanda";
                    OleDbCommand checkStatusCommand = new OleDbCommand(checkStatusQuery, connection);
                    checkStatusCommand.Parameters.Add("@idComanda", OleDbType.Integer).Value = idComanda;

                    object statusResult = checkStatusCommand.ExecuteScalar();
                    if (statusResult == null || statusResult.ToString() != "Livrata")
                    {
                        Console.WriteLine("Comanda nu este eligibila pentru evaluare. Statusul trebuie sa fie 'Livrata'.");
                        return;
                    }

                    // Obținem produsele din comandă împreună cu numele lor
                    string getProduseQuery = @"SELECT P.ID, P.NumeProdus, P.Evaluare, P.NrEvaluari FROM DETALII_COMENZI DC INNER JOIN PRODUS P ON DC.ID_Produs = P.ID  WHERE DC.ID_Comanda = @idComanda";
                    OleDbCommand getProduseCommand = new OleDbCommand(getProduseQuery, connection);
                    getProduseCommand.Parameters.Add("@idComanda", OleDbType.Integer).Value = idComanda;

                    using (OleDbDataReader reader = getProduseCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idProdus = Convert.ToInt32(reader["ID"]);
                            string numeProdus = reader["NumeProdus"].ToString();
                            int evaluareVeche = Convert.ToInt32(reader["Evaluare"]);
                            int nrEvaluari = Convert.ToInt32(reader["NrEvaluari"]);

                            // Solicităm utilizatorului să acorde o evaluare
                            Console.Write($"Acordați o evaluare pentru produsul '{numeProdus}' ( 1-5): ");
                            int evaluareNoua = Convert.ToInt32(Console.ReadLine());
                            if (evaluareNoua < 1 || evaluareNoua > 5)
                            {
                                Console.WriteLine("Evaluarea trebuie să fie între 1 și 5. Reîncercați.");
                                continue;
                            }

                            // Calculăm noua medie a evaluării
                            int evaluareNouaMedie = ((evaluareVeche * nrEvaluari) + evaluareNoua) / (nrEvaluari + 1);

                            // Actualizăm evaluarea în tabelul PRODUS
                            string updateEvaluareQuery = "UPDATE PRODUS SET Evaluare = @evaluareNoua, NrEvaluari = @nrEvaluari WHERE ID = @idProdus";
                            OleDbCommand updateEvaluareCommand = new OleDbCommand(updateEvaluareQuery, connection);
                            updateEvaluareCommand.Parameters.Add("@evaluareNoua", OleDbType.Integer).Value = evaluareNouaMedie;
                            updateEvaluareCommand.Parameters.Add("@nrEvaluari", OleDbType.Integer).Value = nrEvaluari + 1;
                            updateEvaluareCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                            updateEvaluareCommand.ExecuteNonQuery();
                            Console.WriteLine($"Evaluarea pentru produsul '{numeProdus}' a fost actualizata.");
                        }
                    }

                    Console.WriteLine("Evaluarile au fost procesate cu succes.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
            }
        }


        internal void AnuleazaComanda(int idComanda)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Verificam statusul comenzii
                    string checkStatusQuery = "SELECT Status FROM COMENZI WHERE ID = @idComanda";
                    OleDbCommand checkStatusCommand = new OleDbCommand(checkStatusQuery, connection);
                    checkStatusCommand.Parameters.Add("@idComanda", OleDbType.Integer).Value = idComanda;

                    object statusResult = checkStatusCommand.ExecuteScalar();
                    if (statusResult == null)
                    {
                        Console.WriteLine("Comanda nu exista.");
                        return;
                    }
                    if (statusResult.ToString() != "In procesare")
                    {
                        Console.WriteLine("Comanda nu poate fi anulata deoarece nu este in statusul 'In procesare'.");
                        return;
                    }

                    // Obtinem produsele din comanda și cantitatile acestora
                    string getProduseQuery = "SELECT ID_Produs, Cantitate FROM DETALII_COMENZI WHERE ID_Comanda = @idComanda";
                    OleDbCommand getProduseCommand = new OleDbCommand(getProduseQuery, connection);
                    getProduseCommand.Parameters.Add("@idComanda", OleDbType.Integer).Value = idComanda;

                    using (OleDbDataReader reader = getProduseCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idProdus = Convert.ToInt32(reader["ID_Produs"]);
                            int cantitate = Convert.ToInt32(reader["Cantitate"]);

                            // Actualizăm stocul pentru produs
                            string updateStockQuery = "UPDATE PRODUS SET Stoc = Stoc + @cantitate WHERE ID = @idProdus";
                            OleDbCommand updateStockCommand = new OleDbCommand(updateStockQuery, connection);
                            updateStockCommand.Parameters.Add("@cantitate", OleDbType.Integer).Value = cantitate;
                            updateStockCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                            updateStockCommand.ExecuteNonQuery();
                            //Console.WriteLine($"Stocul pentru produsul cu ID-ul {idProdus} a fost actualizat (+{cantitate}).");
                        }
                    }

                    // Modificam statusul comenzii în 'Anulata'
                    string updateStatusQuery = "UPDATE COMENZI SET Status = 'Anulata' WHERE ID = @idComanda";
                    OleDbCommand updateStatusCommand = new OleDbCommand(updateStatusQuery, connection);
                    updateStatusCommand.Parameters.Add("@idComanda", OleDbType.Integer).Value = idComanda;

                    int rowsAffected = updateStatusCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Comanda a fost anulată, iar stocurile au fost actualizate.");
                    }
                    else
                    {
                        Console.WriteLine("A apărut o eroare la actualizarea statusului comenzii.");
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
