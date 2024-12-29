using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Utilizatori
{
    internal class Admin : Utilizator
    {
        public Admin(string nume, string prenume, string email, string telefon, string adresa, bool tip) :
            base(nume, prenume, email, telefon, adresa, tip)
        { }
    }
}
