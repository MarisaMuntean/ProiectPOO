using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using ECommerceApp.Utilizatori;

namespace ECommerceApp
{
    internal class MagazinClient
    {
        internal void MagazinC()
        {
            Console.WriteLine(" Buna Ziua! Doriti sa va creati cont ?");
            string raspuns = Console.ReadLine();
            Inregistrare clientNou = new Inregistrare();
            clientNou.Inscriere(raspuns);
            Console.WriteLine(" Aveti deja un cont? ");
            raspuns = Console.ReadLine();
            CautareClient cautareClient = new CautareClient();
            int persona =Convert.ToInt32( cautareClient.CautareUtilizator(raspuns));
            if (persona>=0)
            {
                Console.WriteLine("Bine ati venit in magazin!");
                Console.WriteLine("Apasati: ");
                Console.WriteLine("0.Daca doriti sa iesiti din magazin.");
                Console.WriteLine("1.Pentru a vedea produsele.");
                Console.WriteLine("2. Pentru a adauga un produs in cos.");
                Console.WriteLine("3.Pentru a edita cantitatea unui produs din cos.");
                Console.WriteLine("4.Pentru a sterge un produs din cos");
                Console.WriteLine("5.Pentru afisarea produselor din cos.");
                Console.WriteLine("6.Pentru plasarea comenzii ");
                Console.WriteLine("7.Pentru adaugarea unui produs in Wishlist");
                Console.WriteLine("8.Pentru a sterge produsul din Wishlist.");
                Console.WriteLine("9.Pentru vizualizarea Wishlist-ului.");
                Console.WriteLine("10.Pentru mutarea unui produs din Wishlist in cos.");
                
                int nr = Convert.ToInt32(Console.ReadLine());//citire optiune
                while (nr != 0)
                {
                    if (nr == 1)//afisare produse
                    {
                        NavigareProduse navigare = new NavigareProduse();
                        navigare.VizualizeazaProduse();
                    }
                    if (nr == 2)//adaugare produse in cos
                    {
                        NavigareProduse navigare = new NavigareProduse();
                        navigare.VizualizeazaProduse();
                        Console.WriteLine("Uitati-va in lista de produse si completati urmatoarele: ");
                        Console.WriteLine("ID produs:");
                        int ID_produs = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Cantitatea dorita:");
                        int Cantitate = Convert.ToInt32(Console.ReadLine());
                        MetodeCOS invocMetoda = new MetodeCOS();
                        invocMetoda.AdaugaInCos(persona, ID_produs, Cantitate);
                    }
                    if (nr == 3)//actualizarea cantitatii
                    {
                        NavigareProduse navigare = new NavigareProduse();
                        navigare.VizualizeazaProduse();
                        Console.WriteLine("Uitati-va in lista de produse si completati urmatoarele: ");
                        Console.WriteLine("ID produs:");
                        int ID_produs = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Cantitatea dorita:");
                        int Cantitate = Convert.ToInt32(Console.ReadLine());
                        MetodeCOS invocMetoda = new MetodeCOS();
                        invocMetoda.ModificaCantitate(persona, ID_produs, Cantitate);
                    }
                    if (nr == 4)//eliminare produs din cos 
                    {
                        MetodeCOS invocMetoda = new MetodeCOS();
                        invocMetoda.AfiseazaCos(persona);
                        Console.WriteLine("Uitati-va in lista de produse si completati urmatoarele: ");
                        Console.WriteLine("ID produs:");
                        int ID_produs = Convert.ToInt32(Console.ReadLine());
                        invocMetoda.EliminaDinCos(persona, ID_produs);
                    }
                    if (nr == 5)//afisez produsele mele din cos
                    {
                        MetodeCOS invocMetoda = new MetodeCOS();
                        invocMetoda.AfiseazaCos(persona);
                    }
                    if (nr == 6)// creează o comandă pentru clientul găsit
                    {
                        Comanda comanda = new Comanda();
                        Console.WriteLine("Completati metoda de plata: (Card la livrare/Cas/Online cu cardul)");
                        string metodaPlata = Console.ReadLine();
                        Console.WriteLine("Completati adresa de livrare: ");
                        string adresaLivrare = Console.ReadLine();
                        comanda.FisaDeComanda(persona, metodaPlata,adresaLivrare);
                        break;
                    }
                    if(nr == 7)
                    {
                        NavigareProduse navigare = new NavigareProduse();
                        navigare.VizualizeazaProduse();
                        Console.WriteLine("Introduceti ID-ul produsului pe care doriti sa il adaugati in Wishlist: ");
                        int ID_Produs = Convert.ToInt32(Console.ReadLine());
                        MetodeWISHLIST invocMetoda = new MetodeWISHLIST();
                        invocMetoda.AdaugaInWishlist(persona, ID_Produs);
                    }
                    if(nr == 8)
                    {
                        MetodeWISHLIST invocMetoda = new MetodeWISHLIST();
                        invocMetoda.VizualizeazaWishlist(persona);
                        Console.WriteLine("Introduceti ID-ul produsului pe care doriti sa il stergeti din Wishlist: ");
                        int ID_Produs = Convert.ToInt32(Console.ReadLine());
                        invocMetoda.StergeProdusDinWishlist(persona, ID_Produs);
                    }
                    if (nr==9)
                    {
                        MetodeWISHLIST invocMetoda = new MetodeWISHLIST();
                        invocMetoda.VizualizeazaWishlist(persona);
                    }
                    if (nr==10)
                    {
                        MetodeWISHLIST invocMetoda = new MetodeWISHLIST();
                        invocMetoda.VizualizeazaWishlist(persona);
                        Console.WriteLine("Introduceti ID-ul produsului pe care doriti sa il achizitionati din Wishlist: ");
                        int ID_Produs = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Introduceti cantitatea dorita: ");
                        int Cantitate = Convert.ToInt32(Console.ReadLine());
                        invocMetoda.MutaDinWishlistInCos(persona, ID_Produs, Cantitate);
                    }

                    Console.WriteLine("Bine ati venit in magazin!");
                    Console.WriteLine("Apasati: ");
                    Console.WriteLine("0.Daca doriti sa iesiti din magazin.");
                    Console.WriteLine("1.Pentru a vedea produsele.");
                    Console.WriteLine("2. Pentru a adauga un produs in cos.");
                    Console.WriteLine("3.Pentru a edita cantitatea unui produs din cos.");
                    Console.WriteLine("4.Pentru a sterge un produs din cos");
                    Console.WriteLine("5.Pentru afisarea produselor din cos.");
                    Console.WriteLine("6.Pentru plasarea comenzii ");
                    Console.WriteLine("7.Pentru adaugarea unui produs in Wishlist");
                    Console.WriteLine("8.Pentru a sterge produsul din Wishlist.");
                    Console.WriteLine("9.Pentru vizualizarea Wishlist-ului.");
                    Console.WriteLine("10.Pentru mutarea unui produs din Wishlist in cos.");
                    nr = Convert.ToInt32(Console.ReadLine());
                }
            }

            Console.WriteLine("Aveti o comanda pe care ati vrea sa o urmariti ?");
            raspuns = Console.ReadLine();
            if(raspuns=="da"||raspuns=="DA"|| raspuns=="Da"||raspuns=="dA")
            {
                Console.WriteLine("Introdu numele de familie: ");
                string nume = Console.ReadLine();
                Console.WriteLine("Introdu prenumele: ");
                string prenume = Console.ReadLine();
                MetodeURMARIRE invocMetode = new MetodeURMARIRE();
                bool rasp=invocMetode.AfiseazaComenziClient(nume, prenume);
                if(rasp)
                {
                    Console.WriteLine("Introdu ID-ul comenzii pe care vrei sa o urmaresti: ");
                    int ID_urmarit = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Ce doriti sa faceti? ");
                    Console.WriteLine("0.Parasiti meniul.");
                    Console.WriteLine("1.Urmariti comanda.");
                    Console.WriteLine("2.Acordati 0 evaluare produselor achizitionate.");
                    Console.WriteLine("3.Anulati comanda.");
                    int nr = Convert.ToInt32(Console.ReadLine());
                    while(nr!=0)
                    {
                        if(nr==1)
                        {
                            MetodeURMARIRE invocMetoda = new MetodeURMARIRE();
                            string statusComanda = invocMetoda.StatusComanda(ID_urmarit); 
                            Console.WriteLine($"Statusul comenzii cu ID-ul {ID_urmarit} este: {statusComanda}");
                        }
                        if(nr==2)
                        {
                            MetodeURMARIRE invocMetoda = new MetodeURMARIRE();
                            invocMetoda.AcordaEvaluariProduse(ID_urmarit);
                        }
                        if(nr==3)
                        {
                            MetodeURMARIRE invocMetoda = new MetodeURMARIRE();
                            invocMetoda.AnuleazaComanda(ID_urmarit);
                        }
                        Console.WriteLine("Ce doriti sa faceti? ");
                        Console.WriteLine("0.Parasiti meniul.");
                        Console.WriteLine("1.Urmariti comanda.");
                        Console.WriteLine("2.Acordati 0 evaluare produselor achizitionate.");
                        Console.WriteLine("3.Anulati comanda.");
                        nr = Convert.ToInt32(Console.ReadLine());
                    }
                }
            }
        }


        

    }
}