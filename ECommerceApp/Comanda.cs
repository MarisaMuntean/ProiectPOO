using ECommerceApp.Utilizatori;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ECommerceApp
{
    internal class Comanda
    {
        internal void FisaDeComanda(int idClient, string metodaPlata, string adresaLivrare)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Facultate/ANUL_2/POO/CommerceAPP/ECommerceApp.accdb";

            // Query pentru detalii client
            string selectClientQuery = "SELECT Nume, Prenume FROM UTILIZATOR WHERE ID = @idClient";

            // Query pentru produse din coș
            string selectCosQuery = "SELECT ID_Produs, Cantitate FROM COS WHERE ID_Client = @idClient";

            // Query pentru inserarea comenzii
            string insertComandaQuery = "INSERT INTO COMENZI (Nume_Client, Prenume_Client,Status, Adresa_livrare,  Data_Plasare, Data_livrarare, Valoarea_Comenzii, MetodaPlata) VALUES (@nume, @prenume, @status, @adresa, @dataPlasare, @dataLivrare, @valoare, @metodaPlata)";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Preluăm datele clientului
                    OleDbCommand selectClientCommand = new OleDbCommand(selectClientQuery, connection);
                    selectClientCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;

                    string nume = "", prenume = "";
                    using (OleDbDataReader clientReader = selectClientCommand.ExecuteReader())
                    {
                        if (clientReader.Read())
                        {
                            nume = clientReader["Nume"].ToString();
                            prenume = clientReader["Prenume"].ToString();
                        }
                        else
                        {
                            Console.WriteLine("Nu s-a gasit clientul specificat.");
                            return;
                        }
                    }

                    // Inserăm comanda în tabelul COMENZI
                    OleDbCommand insertCommand = new OleDbCommand(insertComandaQuery, connection);
                    insertCommand.Parameters.Add("@nume", OleDbType.VarWChar).Value = nume;
                    insertCommand.Parameters.Add("@prenume", OleDbType.VarWChar).Value = prenume;
                    insertCommand.Parameters.Add("@status", OleDbType.VarWChar).Value = "în procesare";
                    insertCommand.Parameters.Add("@adresa", OleDbType.VarWChar).Value = adresaLivrare; // Folosim adresa ca parametru
                    insertCommand.Parameters.Add("@dataPlasare", OleDbType.Date).Value = DateTime.Now;
                    insertCommand.Parameters.Add("@dataLivrare", OleDbType.Date).Value = DateTime.Now.AddDays(3);
                    insertCommand.Parameters.Add("@valoare", OleDbType.Currency).Value = 0; // Valoarea va fi calculată mai jos
                    insertCommand.Parameters.Add("@metodaPlata", OleDbType.VarWChar).Value = metodaPlata;

                    insertCommand.ExecuteNonQuery();

                    // Obținem ID-ul comenzii inserate
                    OleDbCommand getIdCommand = new OleDbCommand("SELECT @@IDENTITY", connection);
                    int idComanda = Convert.ToInt32(getIdCommand.ExecuteScalar());

                    // Preluăm produsele din coș
                    OleDbCommand selectCosCommand = new OleDbCommand(selectCosQuery, connection);
                    selectCosCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;

                    int valoareTotala = 0;
                    using (OleDbDataReader cosReader = selectCosCommand.ExecuteReader())
                    {
                        while (cosReader.Read()) // Parcurgem fiecare rând din COS
                        {
                            int idProdus = Convert.ToInt32(cosReader["ID_Produs"]);
                            int cantitate = Convert.ToInt32(cosReader["Cantitate"]);

                            // Inserăm detaliile comenzii
                            string insertDetaliiQuery = "INSERT INTO DETALII_COMENZI (ID_Comanda, ID_Produs, Cantitate) VALUES (@idComanda, @idProdus, @cantitate)";
                            OleDbCommand insertDetaliiCommand = new OleDbCommand(insertDetaliiQuery, connection);
                            insertDetaliiCommand.Parameters.Add("@idComanda", OleDbType.Integer).Value = idComanda;
                            insertDetaliiCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;
                            insertDetaliiCommand.Parameters.Add("@cantitate", OleDbType.Integer).Value = cantitate;

                            insertDetaliiCommand.ExecuteNonQuery();

                            // Calculăm valoarea totală a comenzii pentru fiecare produs
                            string selectPretQuery = "SELECT Pret FROM PRODUS WHERE ID = @idProdus";
                            OleDbCommand selectPretCommand = new OleDbCommand(selectPretQuery, connection);
                            selectPretCommand.Parameters.Add("@idProdus", OleDbType.Integer).Value = idProdus;

                            object pret = selectPretCommand.ExecuteScalar();
                            if (pret != null)
                            {
                                valoareTotala += Convert.ToInt32(pret) * cantitate;
                            }
                        }
                    }

                    // Actualizăm valoarea totală a comenzii
                    string updateValoareQuery = "UPDATE COMENZI SET Valoarea_Comenzii = @valoareTotala WHERE ID = @idComanda";
                    OleDbCommand updateValoareCommand = new OleDbCommand(updateValoareQuery, connection);
                    updateValoareCommand.Parameters.Add("@valoareTotala", OleDbType.Currency).Value = valoareTotala;
                    updateValoareCommand.Parameters.Add("@idComanda", OleDbType.Integer).Value = idComanda;
                    updateValoareCommand.ExecuteNonQuery();

                    // Golim coșul clientului
                    string deleteCosQuery = "DELETE FROM COS WHERE ID_Client = @idClient";
                    OleDbCommand deleteCosCommand = new OleDbCommand(deleteCosQuery, connection);
                    deleteCosCommand.Parameters.Add("@idClient", OleDbType.Integer).Value = idClient;
                    deleteCosCommand.ExecuteNonQuery();

                    Console.WriteLine("Comanda a fost plasata cu succes!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
            }
        }



    }
}
