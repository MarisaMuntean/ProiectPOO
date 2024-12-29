﻿using ECommerceApp.Utilizatori;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp
{
    internal class CreeareCONT
    {
        internal void CreareCont(Client clientNou)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:/Facultate/ANUL_2/POO/CommerceAPP/ECommerceApp.accdb";
            string query = "INSERT INTO UTILIZATOR ( Nume, Prenume, [E-mail], NrTelefon, Adresa, Administrator) VALUES (@nume, @prenume, @mail,@telefon,@adresa, @admin)";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(query, connection);

                // Parametrii pentru query
                command.Parameters.AddWithValue("@nume", clientNou.Nume);
                command.Parameters.AddWithValue("@prenume", clientNou.Prenume);
                command.Parameters.AddWithValue("@mail", clientNou.Email);
                command.Parameters.AddWithValue("@telefon", clientNou.Telefon);
                command.Parameters.AddWithValue("@adresa", clientNou.Adresa);
                command.Parameters.AddWithValue("@admin", clientNou.Tip);

                try
                {
                    connection.Open();  // Deschide conexiunea
                    command.ExecuteNonQuery();  // Execută interogarea (inserarea în baza de date)
                    Console.WriteLine("Ati fost inregistrat/a cu succes!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
            }
        }
    }
}
