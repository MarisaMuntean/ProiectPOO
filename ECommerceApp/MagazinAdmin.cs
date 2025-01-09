using ECommerceApp.MetodeAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp
{
    internal class MagazinAdmin
    {
        public void Magazin()
        {
            while (true)
            {
                Console.WriteLine("Buna ziua! Doriti sa va creati un cont?");
                string raspuns = Console.ReadLine();
                bool flag = false;

                if (raspuns.ToLower() == "da")
                {
                    Inregistrare adminNou = new Inregistrare();
                    adminNou.InscriereAdmin();
                    flag = true;
                }
                else
                {
                    Console.WriteLine("Apasati Enter pentru a continua...");
                    while (Console.ReadKey().Key != ConsoleKey.Enter)
                    {
                    }
                }
                
                Console.WriteLine("Aveti deja un cont?");
                raspuns = Console.ReadLine();
                if (raspuns.ToLower() == "da")
                {
                    Console.WriteLine("Introduceti numele: ");
                    string nume = Console.ReadLine();
                    Console.WriteLine("Introduceti prenumele: ");
                    string prenume = Console.ReadLine();
                    CautareAdmin admin = new CautareAdmin();
                    if (nume != null && prenume != null)
                        flag = admin.Cautare(nume, prenume);

                }
                else
                {
                    Console.WriteLine("Apasati Enter pentru a continua...");
                    while (Console.ReadKey().Key != ConsoleKey.Enter)
                    {
                    }
                }

                if (flag)
                {
                    Console.WriteLine("Bine ati venit in magazin!");
                    Console.WriteLine("Apasati: ");
                    Console.WriteLine("0. Iesire din magazin.");
                    Console.WriteLine("1. Adaugare produse");
                    Console.WriteLine("2. Editare produse");
                    Console.WriteLine("3. Stergere produse");
                    Console.WriteLine("4. Monitorizare stoc pentru fiecare produs");                    
                    Console.WriteLine("5. Vizualizare comenzi ");
                    Console.WriteLine("6. Vizualizuare raport de vanzari");
                    Console.WriteLine("7. Emitere facturi");
                    Console.WriteLine("8. Cel mai vandut produs");
                    Console.WriteLine("9. Venitul generat");
                    Console.WriteLine("10. Aplicare reducere pentru un produs");
                    Console.WriteLine("11. Aplicare reducere pentru o categorie");

                    AdminBD adminBD = new AdminBD();
                    AdminMetodeComanda metodaComanda = new AdminMetodeComanda();

                    while(true)
                    {
                        Console.WriteLine("Ce doriti sa faceti?");
                        Console.WriteLine("Apasati: ");
                        Console.WriteLine("0. Iesire din magazin.");
                        Console.WriteLine("1. Adaugare produse");
                        Console.WriteLine("2. Editare produse");
                        Console.WriteLine("3. Stergere produse");
                        Console.WriteLine("4. Monitorizare stoc pentru fiecare produs");
                        Console.WriteLine("5. Vizualizare comenzi ");
                        Console.WriteLine("6. Vizualizuare raport de vanzari");
                        Console.WriteLine("7. Emitere facturi");
                        Console.WriteLine("8. Cel mai vandut produs");
                        Console.WriteLine("9. Venitul generat");
                        Console.WriteLine("10. Aplicare reducere pentru un produs");
                        Console.WriteLine("11. Aplicare reducere pentru o categorie");
                        string optiune = Console.ReadLine();

                        switch (optiune)
                        {
                            case "0":
                                Console.WriteLine("Ati iesit din magazin. ");
                                return;
                            case "1":
                                adminBD.AdaugareProduse();
                                break;
                            case "2":
                                adminBD.ModificareProdus();
                                break;
                            case "3":
                                adminBD.StergereProdus();
                                break;
                            case "4":
                                adminBD.MonitorizareStocPerProdus();
                                break;
                            case "5":
                                metodaComanda.VizualizareComenzi();
                                break;
                            case "6":
                                metodaComanda.CitireRaportVanzariPDF();
                                break;
                            case "7":
                                metodaComanda.EmitereFacturiPDF();
                                break;
                            case "9":
                                metodaComanda.CelMaiVandutProdus();
                                break;
                            case "10":
                                adminBD.AplicareReducereProdus();
                                break;
                            case "11":
                                adminBD.AplicareReducereCategorie();
                                break;
                            default: Console.WriteLine("Optiune invalida");
                                break;

                        }

                    }

                }
                else
                {
                    Console.WriteLine("Inregisatrarea/Logarea a esuat. Incercati din nou.");
                }
            }
            Console.WriteLine("La revedere!");

        }
    }
}
