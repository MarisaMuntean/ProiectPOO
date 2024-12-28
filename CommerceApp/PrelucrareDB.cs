using System;

public class PrelucrareDB
{
    public void AddProduse(Produse produs)
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=:/Facultate/ANUL_2/POO/Proiect/CommerceApp/E_commerce1.accdb";
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
