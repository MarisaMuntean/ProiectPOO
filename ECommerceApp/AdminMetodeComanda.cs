using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections;


namespace ECommerceApp
{
    internal class AdminMetodeComanda
    {
        private readonly string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\ECommerceApp.accdb";

        public void VizualizareComenzi()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                string vizualizareComenziQuery = "SELECT * FROM [COMENZI]";

                try
                {
                    connection.Open();

                    using (OleDbCommand command = new OleDbCommand(vizualizareComenziQuery, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            // Verificăm dacă există rânduri în rezultat
                            if (reader.HasRows)
                            {
                                Console.WriteLine("Comenzile din tabel sunt:");

                                // Iterăm prin fiecare rând și afișăm datele
                                while (reader.Read())
                                {
                                    Console.WriteLine($"ID: {reader["ID"]}, " +
                                                      $"Nume Client: {reader["Nume_Client"]}, " +
                                                      $"Prenume Client: {reader["Prenume_Client"]}, " +
                                                      $"Status: {reader["Status"]}, " +
                                                      $"Adresa Livrare: {reader["Adresa_livrare"]}, " +
                                                      $"Data Plasare: {reader["Data_Plasare"]}, " +
                                                      $"Data Livrare: {reader["Data_livrarare"]}, " +
                                                      $"Valoarea Comenzii: {reader["Valoarea_Comenzii"]}, " +
                                                      $"Metoda Plata: {reader["MetodaPlata"]}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nu există comenzi momentan.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"A apărut o eroare: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
        }

        public void ModificareStatus()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                int id;
                Console.WriteLine("Introduceti id-ul comenzii: ");
                string input = Console.ReadLine();
                bool succes = int.TryParse(input, out id);

                Console.WriteLine("Statusul poate fi: in procesare, expediata, livrata, anulata.\n");
                Console.WriteLine("Introduceti noul status al comenzii: ");
                string status = Console.ReadLine().ToLower();

                bool statusValid = false;
                if (status == "in procesare" || status == "expediata" || status == "livrata" || status == "anulata")
                    statusValid = true;

                if (succes && statusValid)
                {
                    string modificareStatusQuery = "UPDATE COMENZI Set Status = ? WHERE ID = ?";
                    OleDbCommand modificareStatusCmd = new OleDbCommand(modificareStatusQuery, connection);
                    modificareStatusCmd.Parameters.AddWithValue("@Status", status);
                    modificareStatusCmd.Parameters.AddWithValue("@ID", id);

                    try
                    {
                        connection.Open();
                        int comandaModificata = (int)modificareStatusCmd.ExecuteNonQuery();
                        if (comandaModificata > 0)
                            Console.WriteLine("Statusul a fost modificat!");
                        else
                            Console.WriteLine("Statusul nu a fost modificat.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"A apărut o eroare: {ex.Message}");
                    }
                    finally
                    {
                        if (connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Id-ul comenzii este un numar.");
                }
            }
        }


        public void EmitereFacturiPDF()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                int id;
                Console.WriteLine("Introduceti id-ul comenzii: ");
                string input = Console.ReadLine();
                bool succes = int.TryParse(input, out id);


                if (succes)
                {
                    string selectareComandaQuery = "SELECT * FROM COMENZI WHERE ID = ?";
                    OleDbCommand selectareComandaCmd = new OleDbCommand(selectareComandaQuery, connection);
                    selectareComandaCmd.Parameters.AddWithValue("@ID", id);

                    try
                    {
                        connection.Open();
                        OleDbDataReader reader = selectareComandaCmd.ExecuteReader();
                        
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //Creare document pdf
                                Document pdfDoc = new Document();

                                // Setare cale completă pentru salvarea fișierului PDF
                                string folderPath = @"E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\Facturi";

                                // Verificare dacă folderul există și îl creăm dacă este necesar
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }

                                // Construire calea completă a fișierului PDF
                                string fileName = Path.Combine(folderPath, $"Factura_{reader["ID"]}.pdf");

                                // Creare un writer care scrie în fișierul PDF
                                PdfWriter.GetInstance(pdfDoc, new FileStream(fileName, FileMode.Create));
                                pdfDoc.Open();

                                // Adăugare detaliilor pentru factura
                                pdfDoc.Add(new Paragraph("Factura"));
                                pdfDoc.Add(new Paragraph($"ID Comandă: {reader["ID"]}"));
                                pdfDoc.Add(new Paragraph($"Nume Client: {reader["Nume_Client"]} {reader["Prenume_Client"]}"));
                                pdfDoc.Add(new Paragraph($"Status: {reader["Status"]}"));
                                pdfDoc.Add(new Paragraph($"Adresa Livrare: {reader["Adresa_livrare"]}"));
                                pdfDoc.Add(new Paragraph($"Data Plasare: {reader["Data_Plasare"]}"));
                                pdfDoc.Add(new Paragraph($"Data Livrare: {reader["Data_livrarare"]}"));
                                pdfDoc.Add(new Paragraph($"Valoarea Comenzii: {reader["Valoarea_Comenzii"]}"));
                                pdfDoc.Add(new Paragraph($"Metoda de Plata: {reader["MetodaPlata"]}"));


                                // Închidere document PDF
                                pdfDoc.Close();

                                Console.WriteLine($"Factura PDF pentru comanda cu ID {reader["ID"]} a fost generată.");

                            }

                        }
                        else
                            Console.WriteLine("Nu există nicio comandă cu ID-ul specificat.");
                        reader.Close();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("A aparut o eroare: " + ex.Message);
                    }
                    finally
                    {
                        if(connection.State == System.Data.ConnectionState.Open)
                            connection.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Id-ul comenzii este un numar.");
                }

            }
        }

        public void CitireRaportVanzariPDF()
        {
            // Interogare SQL pentru generarea unui raport de vânzări:
            // - Se selectează ID-ul comenzii, ID-ul produsului, cantitatea, și valoarea totală (cantitate * preț).
            // - Se combină tabelele COMENZI, DETALII_COMENZI și PRODUS folosind INNER JOIN pentru a corela comenzile cu produsele și detaliile lor.
            string CitireRaportVanzariQuery = @"
            SELECT 
                COMENZI.ID AS ComandaID,
                DETALII_COMENZI.ID_Produs,
                DETALII_COMENZI.Cantitate,
                (DETALII_COMENZI.Cantitate * PRODUS.Pret) AS ValoareTotala
            FROM 
                (COMENZI
                INNER JOIN DETALII_COMENZI ON COMENZI.ID = DETALII_COMENZI.ID_Comanda)
                INNER JOIN PRODUS ON DETALII_COMENZI.ID_Produs = PRODUS.ID";

            // Setare cale completă pentru salvarea fișierului PDF
            string folderPath = @"E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\Facturi";

            // Verificare dacă folderul există și îl creăm dacă este necesar
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Construire calea completă a fișierului PDF
            string fileName = Path.Combine(folderPath, "RaportVanzari.pdf");

            // Crearea documentului PDF
            Document document = new Document();
            try
            {
                // Inițializare writer pentru PDF
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
                document.Open();

                // Adăugare titlu în PDF
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var textFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                Paragraph title = new Paragraph("Raport de Vanzari\n\n", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Crearea tabelului în PDF
                PdfPTable table = new PdfPTable(4); // 4 coloane
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2, 2, 2, 2 }); // Proporția lățimilor coloanelor

                // Adăugare antet tabel
                table.AddCell(new PdfPCell(new Phrase("ID Comandă", textFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("ID Produs", textFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Cantitate", textFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Valoare Totală", textFont)) { BackgroundColor = BaseColor.LIGHT_GRAY });

                // Conectare la baza de date și preluare date
                using (OleDbConnection connection = new OleDbConnection(connString))
                {
                    connection.Open();
                    using (OleDbCommand citireRaportVanzariCmd = new OleDbCommand(CitireRaportVanzariQuery, connection))
                    {
                        using (OleDbDataReader reader = citireRaportVanzariCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                table.AddCell(new Phrase(reader["ComandaID"].ToString(), textFont));
                                table.AddCell(new Phrase(reader["ID_Produs"].ToString(), textFont));
                                table.AddCell(new Phrase(reader["Cantitate"].ToString(), textFont));
                                table.AddCell(new Phrase(reader["ValoareTotala"].ToString(), textFont));
                            }
                        }
                    }
                }

                // Adăugare tabel în document
                document.Add(table);

                // Finalizare PDF
                Console.WriteLine($"Raportul de vânzări a fost generat.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A apărut o eroare la generarea PDF-ului: {ex.Message}");
            }
            finally
            {
                document.Close();
            }
        }



        public void CelMaiVandutProdus()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                //Selectează ID_Produs și suma totală a cantităților vândute (TotalCantitate), dar doar pentru produsul cu cele mai mari vânzări.
                //Specifică tabelul DETALII_COMENZI ca sursa datelor.
                //Gruparea rezultatelor după ID_Produs pentru a calcula suma cantităților pentru fiecare produs.
                //Sortează produsele descrescător după suma totală a cantităților vândute, astfel încât cel mai vândut produs să fie primul.
                //SUM returneaza by default double
                string prodCelMaiVandutQuery = @"SELECT TOP 1 [ID_Produs], SUM([Cantitate]) AS TotalCantitate
                FROM [DETALII_COMENZI]
                GROUP BY [ID_Produs]
                ORDER BY SUM([Cantitate]) DESC;
                ";


                try
                {
                    connection.Open();

                    using (OleDbCommand prodCelMaiVandutCmd = new OleDbCommand(prodCelMaiVandutQuery, connection))
                    {
                        using (OleDbDataReader reader = prodCelMaiVandutCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int idProdus = reader.GetInt32(0); // ID_Produs
                                double totalCantitateDouble = reader.GetDouble(1); // TotalCantitate

                                int totalCantitate = (int)totalCantitateDouble;

                                Console.WriteLine($"Cel mai vandut produs este ID: {idProdus} cu {totalCantitate} bucati vandute.");
                            }
                            else
                            {
                                Console.WriteLine("Nu s-au găsit produse în tabel.");
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"A apărut o eroare: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }


            }
        }

        public void VenitulGenerat()
        {
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                // Interogarea SQL venitGeneratQuery folosește o funcție de agregare (SUM) pentru a calcula venitul total (VenitTotal),
                // înmulțind cantitatea fiecărui produs (DETALII_COMENZI.Cantitate) cu prețul acestuia (PRODUS.Pret).
                //INNER JOIN combină datele din tabelele DETALII_COMENZI și PRODUS, bazându-se pe coloana comună ID_Produs.
                string venitGeneratQuery = @"
                    SELECT SUM(DETALII_COMENZI.Cantitate * PRODUS.Pret) AS VenitTotal
                    FROM DETALII_COMENZI
                    INNER JOIN 
                    PRODUS
                    ON 
                    DETALII_COMENZI.ID_Produs = PRODUS.ID";

                try
                {
                    connection.Open();
                    
                    using (OleDbCommand venitGeneratCmd = new OleDbCommand(venitGeneratQuery, connection))
                    {
                        object venitObiect = venitGeneratCmd.ExecuteScalar();
                        if (venitObiect != DBNull.Value)
                        {
                            int venitTotal = Convert.ToInt32(venitObiect);
                            Console.WriteLine($"Venitul total generat este: {venitTotal}");
                        }
                        else
                        {
                            Console.WriteLine("Nu s-a generat niciun venit.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"A apărut o eroare: {ex.Message}");
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                }
            }
        }

    }
}

