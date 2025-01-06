using ECommerceApp.Utilizatori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp
{
    internal class Inregistrare
    {
        internal void Inscriere(string raspuns)
        {
            if (raspuns == "da" || raspuns == "Da" || raspuns == "DA" || raspuns == "dA")
            {
                Console.WriteLine("introduceti numele de familie: ");
                string NUME = Console.ReadLine();
                Console.WriteLine("introduceti prenumele: ");
                string PRENUME = Console.ReadLine();
                Console.WriteLine("introduceti email: ");
                string EMAIL = Console.ReadLine();
                Console.WriteLine("introduceti numarul de telefon: ");
                string TELEFON = Console.ReadLine();
                Console.WriteLine("introduceti adresa de domiciliu: ");
                string ADRESA = Console.ReadLine();
                bool tip = false;
                Client c = new Client(NUME, PRENUME, EMAIL, TELEFON, ADRESA, tip);
                CreeareCONT creeaza = new CreeareCONT();
                creeaza.CreareCont(c);
            }
        }
    }
}
