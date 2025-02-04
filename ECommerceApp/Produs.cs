﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp
{
    internal class Produs
    {
        internal string Nume { get; set; }
        internal string Descriere { get; set; }
        internal string Categorie { get; set; }
        internal int Pret { get; set; }
        internal int Stoc { get; set; }
        internal int Evaluare { get; set; }
        internal string Reducere { get; set; } = "-";

        public Produs(string nume, string descriere, string categorie, int pret, int stoc, int evaluare)
        {
            Nume = nume;
            Descriere = descriere;
            Categorie = categorie;
            Pret = pret;
            Stoc = stoc;
            Evaluare = evaluare;
        }
    }
}
