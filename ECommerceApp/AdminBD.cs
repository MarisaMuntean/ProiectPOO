using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data.OleDb;
using System.Collections;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;
using ECommerceApp.Reduceri;


namespace ECommerceApp.MetodeAdmin
{
    internal class AdminBD
    {
        private readonly string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\ECommerceApp.accdb;Persist Security Info=True";

               
        //Metode pentru administratori in cadrul magazinului
        public void AdaugareProduse()
        {
            Console.WriteLine("Introduceti numele produsului: ");
            string nume = Console.ReadLine();
            Console.WriteLine("Introduceti descrierea produsului: ");
            string descriere = Console.ReadLine();
            Console.WriteLine("Introduceti categoria produsului: ");
            string categorie = Console.ReadLine();

            int pret;
            Console.WriteLine("Introduceti pretul produsului: ");
            string input = Console.ReadLine();
            bool pretValid = int.TryParse(input, out pret);

            int stoc;
            Console.WriteLine("Introduceti stocul produsului: ");
            input = Console.ReadLine();
            bool stocValid = int.TryParse(input, out stoc);

            int evaluare;
            Console.WriteLine("Introduceti evaluarea produsului: ");
            input = Console.ReadLine();
            bool evaluareValid = int.TryParse(input, out evaluare);

            if (pretValid && evaluareValid && stocValid)
            {
                Produs produs = new Produs(nume, descriere, categorie, pret, stoc, evaluare);

                using (OleDbConnection connection = new OleDbConnection(connString))
                {
                    string query = "INSERT INTO PRODUS (NumeProdus, Descriere, Pret,Stoc, Evaluare, Categorie, Reducere) VALUES (@NumeProdus, @Descriere, @Pret, @Stoc, @Evaluare, @Categorie, @Reducere)";
                    OleDbCommand command = new OleDbCommand(query, connection);
                    command.Parameters.AddWithValue("@NumeProdus", produs.Nume);
                    command.Parameters.AddWithValue("@Descriere", produs.Descriere);
                    command.Parameters.AddWithValue("@Pret", produs.Pret);
                    command.Parameters.AddWithValue("@Stoc", produs.Stoc);
                    command.Parameters.AddWithValue("@Evaluare", produs.Evaluare);
                    command.Parameters.AddWithValue("@Categorie", produs.Categorie);
                    command.Parameters.AddWithValue("@Reducere", produs.Reducere);

                    try
                    {
                        connection.Open();  // Deschide conexiunea
                        command.ExecuteNonQuery();  // Execută interogarea (inserarea în baza de date)
                        Console.WriteLine("Produsul a fost adaugat in baza de date!");
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Eroare: " + ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Nu ati introdus o valoare valida pentru pretul, stocul sau evaluarea produsului.");
            }

        }

        public void ModificareProdus()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                int idCautat = 0;

                Console.WriteLine("Introduceti ID-ul produsului cautat: ");
                string input = Console.ReadLine();

                bool succes = int.TryParse(input, out idCautat);
                if (succes)
                {

                    bool deModificat = true;

                    while (deModificat)
                    {
                        Console.WriteLine("Ce doresti sa modifici?\n" +
                       "Nume\n" +
                       "Descriere\n" +
                       "Pret\n" +
                       "Stoc\n" +
                       "Evaluare\n" +
                       "Categorie\n" +
                       "Nimic");
                        input = Console.ReadLine().ToLower();

                        string query = "";
                        OleDbCommand cmd = new OleDbCommand(query, connection);

                        switch (input)
                        {
                            case "nume":
                                query = "UPDATE PRODUS SET NumeProdus = ? WHERE ID = ?";
                                Console.WriteLine("Introduceti noul nume: ");
                                cmd.Parameters.AddWithValue("@NumeProdus", Console.ReadLine());
                                cmd.Parameters.AddWithValue("@ID", idCautat);
                                break;
                            case "descriere":
                                query = "UPDATE PRODUS SET Descriere = ? WHERE ID = ?";
                                Console.WriteLine("Introduceți noua descriere: ");
                                cmd.Parameters.AddWithValue("@Descriere", Console.ReadLine());
                                cmd.Parameters.AddWithValue("@ID", idCautat);
                                break;
                            case "pret":
                                query = "UPDATE PRODUS SET Pret = ? WHERE ID = ?";
                                int pret;
                                Console.WriteLine("Introduceti noul preț: ");
                                input = Console.ReadLine();
                                bool inputValid = int.TryParse(input, out pret);
                                if (inputValid)
                                {
                                    cmd.Parameters.AddWithValue("@Pret", pret);
                                    cmd.Parameters.AddWithValue("@ID", idCautat);
                                }
                                else
                                {
                                    Console.WriteLine("Pret invalid.");
                                }
                                break;
                            case "stoc":
                                query = "UPDATE PRODUS SET Stoc = ? WHERE ID = ?";
                                int stoc;
                                Console.WriteLine("Introduceti noua cantitate: ");
                                input = Console.ReadLine();
                                inputValid = int.TryParse(input, out stoc);
                                if (inputValid)
                                {
                                    cmd.Parameters.AddWithValue("@Stoc", stoc);
                                    cmd.Parameters.AddWithValue("@ID", idCautat);
                                }
                                else
                                {
                                    Console.WriteLine("Cantitate invalida.");
                                }
                                break;
                            case "evaluare":
                                query = "UPDATE PRODUS SET Evaluare = ? WHERE ID = ?";
                                int evaluare;
                                Console.WriteLine("Introduceti noua evaluare: ");
                                input = Console.ReadLine();
                                inputValid = int.TryParse(input, out evaluare);
                                if (inputValid)
                                {
                                    cmd.Parameters.AddWithValue("@Evaluare", evaluare);
                                    cmd.Parameters.AddWithValue("@ID", idCautat);
                                }
                                else
                                {
                                    Console.WriteLine("Evaluare invalida.");
                                }
                                break;
                            case "categorie":
                                query = "UPDATE PRODUS SET Categorie = ? WHERE ID = ?";
                                Console.WriteLine("Introduceti noua categorie: ");
                                cmd.Parameters.AddWithValue("@Categorie", Console.ReadLine());
                                cmd.Parameters.AddWithValue("@ID", idCautat);
                                break;
                            case "nimic":
                                query = string.Empty;
                                deModificat = false;
                                Console.WriteLine("Modificările s-au încheiat.");
                                break;
                            default:
                                Console.WriteLine("Nu exista caracteristica.");
                                continue;
                        }

                        //daca este ceva de modificat

                        try
                        {
                            connection.Open();
                            if (!string.IsNullOrEmpty(query))
                            {
                                cmd.CommandText = query;
                                cmd.ExecuteNonQuery();
                                Console.WriteLine("Modificarea a fost realizata!");
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Eroare: " + ex.Message);
                        }
                        finally
                        {
                            if (connection.State == System.Data.ConnectionState.Open)
                                connection.Close();
                        }


                    }

                }
                else
                {
                    Console.WriteLine("Id-ul cautat nu exista!");
                }
            }

        }

        public string StergereProdus()
        {
            int idProdusDeSters;
            Console.WriteLine("Introduceti id-ul produsului de sters: ");
            string input = Console.ReadLine();
            bool succes = int.TryParse(input, out idProdusDeSters);

            //daca este un numar din baza de date
            if (succes == true && idProdusDeSters <= UltimulID() && idProdusDeSters > 0)
            {
                using (OleDbConnection connection = new OleDbConnection(connString))
                {
                    string stergereProdusQuery = "DELETE FROM PRODUS WHERE ID = ?";
                    OleDbCommand stergereProdusCmd = new OleDbCommand(stergereProdusQuery, connection);
                    stergereProdusCmd.Parameters.AddWithValue("@ID", idProdusDeSters);

                    try
                    {
                        connection.Open();
                        int produseAfectate = stergereProdusCmd.ExecuteNonQuery();
                        if (produseAfectate > 0)
                        {
                            return "Produsul a fost sters!";
                        }
                        else
                        {
                            return "Produsul nu exista.";
                        }
                    }
                    catch (Exception ex)
                    {
                        return "Produsul nu a fost sters. Eroare: " + ex.Message;
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                    }
                }

            }
            else
            {
                return "Produsul nu exista.";
            }
        }

        private int UltimulID()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                //gaseste ultimul id
                string ultimIdQuery = "SELECT TOP 1 ID FROM PRODUS ORDER BY ID DESC";
                OleDbCommand ultimIdCmd = new OleDbCommand(ultimIdQuery, connection);
                try
                {
                    connection.Open();
                    return (int)ultimIdCmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Eroare: " + ex.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
                return -1;
            }
        }


        public void MonitorizareStocPerProdus()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                int idProdus;
                Console.WriteLine("Introduceti ID-ul produsului: ");
                string input = Console.ReadLine();
                bool succes = int.TryParse(input, out idProdus);
                if (succes)
                {
                    string monitorizareStocQuery = "SELECT Stoc FROM PRODUS WHERE ID = ?";
                    OleDbCommand monitorizareStocCmd = new OleDbCommand(monitorizareStocQuery, connection);
                    monitorizareStocCmd.Parameters.AddWithValue("@ID", idProdus);

                    try
                    {
                        connection.Open();
                        object valoareStoc = monitorizareStocCmd.ExecuteScalar();
                        if (valoareStoc != null)
                        {
                            if ((int)valoareStoc < 5)
                                Console.WriteLine("Atentie! Mai sunt doar " + valoareStoc + " produse.");
                            else
                                Console.WriteLine("Stoc disponibil.\n");

                            Console.WriteLine("Doresti sa modifici cantitatea produsului? Da/Nu");
                            input = Console.ReadLine().ToLower();

                            if (input.Equals("da"))
                            {
                                int cantitate;
                                Console.WriteLine("Introduceti noua cantitate: ");
                                input = Console.ReadLine();
                                bool inputValid = int.TryParse(input, out cantitate);

                                if (inputValid)
                                {
                                    string modificareCantitateQuery = "UPDATE PRODUS SET Stoc = ? WHERE ID = ?";
                                    OleDbCommand modificareCantitateCmd = new OleDbCommand(modificareCantitateQuery, connection);
                                    modificareCantitateCmd.Parameters.AddWithValue("@Stoc", cantitate);
                                    modificareCantitateCmd.Parameters.AddWithValue("@ID", idProdus);
                                    modificareCantitateCmd.ExecuteNonQuery();
                                    Console.WriteLine("Cantitatea a fost modificata.");
                                }
                                else
                                {
                                    Console.WriteLine("Cantitate invalida.");
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("Produsul nu mai este in stoc.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Eroare: " + ex.Message);
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Produsul nu exista.");
                }
            }
        }

        public void AplicareReducereProdus()
        {

            Console.WriteLine("Ce tip de reducere?\n" +
                           "2+1\n" +
                           "discount procent\n" +
                           "discount fix");
            string tip = Console.ReadLine().ToLower();
            switch (tip)
            {
                case "2+1":
                    DoiPlusUnu doiPlusUnu = new DoiPlusUnu();
                    doiPlusUnu.AplicareReducereProdus();
                    break;
                case "discount procent":
                    DiscountProcent discountProcent = new DiscountProcent();
                    discountProcent.AplicareReducereProdus();
                    break;
                case "discount fix":
                    DiscountFix discountFix = new DiscountFix();
                    discountFix.AplicareReducereProdus();
                    break;
                default:
                    Console.WriteLine("Nu exista acest tip de reducere.");
                    break;
            }
        }

        public void AplicareReducereCategorie()
        {

            Console.WriteLine("Ce tip de reducere?\n" +
                           "2+1\n" +
                           "discount procent\n" +
                           "discount fix");
            string tip = Console.ReadLine().ToLower();
            switch (tip)
            {
                case "2+1":
                    DoiPlusUnu doiPlusUnu = new DoiPlusUnu();
                    doiPlusUnu.AplicareReducereCategorie();
                    break;
                case "discount procent":
                    DiscountProcent discountProcent = new DiscountProcent();
                    discountProcent.AplicareReducereCategorie();
                    break;
                case "discount fix":
                    DiscountFix discountFix = new DiscountFix();
                    discountFix.AplicareReducereCategorie();
                    break;
                default:
                    Console.WriteLine("Nu exista acest tip de reducere.");
                    break;
            }
        }



    }

}

