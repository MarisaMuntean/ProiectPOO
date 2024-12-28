using System;
using System.Deployment.Internal;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class Produse
{
	internal string Nume { get; set};
	internal string Descriere { get; set; }
	internal string Categorie{ get; set; }
	internal int Pret { get; set; }
	internal int Stoc { get; set; }
	internal int Evaluare { get; set; }
		
	public Produse( string nume , string descriere, string categorie, int pret,int stoc, int evaluare )
	{
		Nume = nume;
		Descriere = descriere;
		Categorie = categorie;
		Pret = pret;
		Stoc = stoc;
		Evaluare = evaluare;
	}
	
	public AddProduse(Produse produs)
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
