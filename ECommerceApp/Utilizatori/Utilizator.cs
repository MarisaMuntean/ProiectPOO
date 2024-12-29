using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Utilizatori
{
    internal abstract class Utilizator
    {
        public string ID { get; protected set; }
        public string Nume { get; protected set; }
        public string Prenume { get; protected set; }
        public string Email { get; protected set; }
        public string Telefon { get; protected set; }
        public string Adresa { get; protected set; }
        public string Tip { get; protected set; }


        public Utilizator(string id, string nume, string prenume, string email, string telefon, string adresa, string tip)
        {
            this.ID = id;
            this.Nume = nume;
            this.Prenume = prenume;
            this.Email = email;
            this.Telefon = telefon;
            this.Adresa = adresa;
            this.Tip = tip;
        }
    }
}
