using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceApp
{
    internal class Admin : Utilizator
    {
       public Admin(string id, string nume, string prenume, string email, string telefon, string adresa, string tip) : 
            base(id,nume,prenume,email,telefon,adresa,tip) { }

    }
}
