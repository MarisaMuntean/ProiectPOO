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
        internal void FisaDeComanda(Client clientGasit)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Facultate/ANUL_2/POO/CommerceAPP/ECommerceApp.accdb";
            string query = "INSERT INTO COMENZI (Nume_CLient, Prenume_CLient, Adresa_livrare, Status, Data_Plasare, Data_livrarare, Valoarea_Comenzii) " +
               "VALUES (@nume, @prenume, @Adresa, @Status, @DataP, @DataL, @Valoare)";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);

                // Parametrii pentru query
                command.Parameters.Add("@nume", OleDbType.VarWChar).Value = clientGasit.Nume ?? "";
                command.Parameters.Add("@prenume", OleDbType.VarWChar).Value = clientGasit.Prenume ?? "";
                command.Parameters.Add("@Adresa", OleDbType.VarWChar).Value = clientGasit.Adresa ?? "";
                command.Parameters.Add("@Status", OleDbType.VarWChar).Value = "in procesare";
                command.Parameters.Add("@DataP", OleDbType.Date).Value = DateTime.Now;
                command.Parameters.Add("@DataL", OleDbType.Date).Value = DateTime.Now.AddDays(3);
                command.Parameters.Add("@Valoare", OleDbType.Currency).Value = 0;

                try
                {
                    connection.Open();  // Deschide conexiunea
                    command.ExecuteNonQuery();  // Execută interogarea
                    //Console.WriteLine("Ati fost inregistrat/a cu succes!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
            }

        }
    }
}
