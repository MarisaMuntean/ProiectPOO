using System;
using System.Deployment.Internal;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

public class Produse
{
	public string Nume { get; private set};
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
	

}
