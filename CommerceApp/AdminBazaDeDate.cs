using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp
{
    internal class AdminBazaDeDate
    {
        public Admin Admin_; 
        public AdminBazaDeDate(Admin admin)
        {
            this.Admin_ = admin;
        }

        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= E:\VisualStudioCommunityProjects\ProiectPOO\CommerceApp\E_commerce1.accdb";

        public void AdaugareProduse(Produse produs)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                string query = "INSERT INTO PRODUS (NumeProdus, Descriere, Pret,Stoc, Evaluare, Categorie) VALUES (@NumeProdus, @Descriere, @Pret, @Stoc, @Evaluare, @Categorie)";
                OleDbCommand command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("@NumeProdus", produs.Nume);
                command.Parameters.AddWithValue("@Descriere", produs.Descriere);
                command.Parameters.AddWithValue("@Pret", produs.Pret.);
                command.Parameters.AddWithValue("@Stoc", produs.Stoc);
                command.Parameters.AddWithValue("@Evaluare", produs.Evaluare);
                command.Parameters.AddWithValue("@Evaluare", produs.Categorie);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


    }
}
