namespace CommerceApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Utilizator admin1 = new Admin(
            id: "A001",
            nume: "Popescu",
            prenume: "Ion",
            email: "ion.popescu@example.com",
            telefon: "0712345678",
            adresa: "Str. Exemplu, Nr. 10, București",
            tip: "Admin");

            AdminBazaDeDate adminDb = new AdminBazaDeDate(admin1); 
            Produse produs = new Produse( nume: "Laptop", descriere: "Laptop performant pentru gaming", pret: 4500.99m, stoc: 10, evaluare: 4.5, categorie: "Electronice" ); 
            // Call the AdaugareProduse method admin
            Db.AdaugareProduse(produs);
            Console.WriteLine("Produsul a fost adăugat cu succes.");

        }
    }
}
