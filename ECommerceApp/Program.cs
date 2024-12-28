using ECommerceApp;

namespace ECommerceApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Admin admin1 = new Admin(
                id: "A001",
                nume: "Popescu",
                prenume: "Ion",
                email: "ion.popescu@example.com",
                telefon: "0712345678",
                adresa: "Str. Exemplu, Nr. 10, București",
                tip: "Admin"
            );

            AdminBD adminDb = new AdminBD(admin1);
            Produse produs = new Produse(
                nume: "Laptop",
                descriere: "Laptop performant pentru gaming",
                pret: 4500,
                stoc: 10,
                evaluare: 4,
                categorie: "Electronice"
            );

            // Call the AdaugareProduse method
            adminDb.AdaugareProduse(produs);
            Console.WriteLine("Produsul a fost adăugat cu succes.");
        }
    }
}
