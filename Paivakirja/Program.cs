using System;
using System.Collections.Generic;
using System.IO;

namespace PaivakirjaOhjelma
{
    class Program
    {
        const string OHJELMAN_VERSIO = ": Beta 0.5";
        const string TIEDOSTON_NIMI = @"C:\Kansio\paivakirja.txt";
        static List<string> paivakirjaMerkinnat = new List<string>();

        static void Main(string[] args)
        {
            bool jatka = true;
            LuoKansioJosTarvitaan();
            LataaMerkinnat();

            while (jatka)
            {
                Console.Clear();
                NaytaOtsikko();
                NaytaValikko();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Valitse toiminto (1-4): ");  // Päivitetty valinta.
                Console.ResetColor();

                string valinta = Console.ReadLine();

                switch (valinta)
                {
                    case "1":
                        LisaaMerkinta();
                        break;
                    case "2":
                        NaytaMerkinnat();
                        break;
                    case "3":
                        PoistaMerkinta();
                        break;
                    case "4":
                        jatka = false;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nVirheellinen valinta, yritä uudelleen.");
                        Console.ResetColor();
                        break;
                }

                if (jatka)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\nPaina mitä tahansa näppäintä jatkaaksesi...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

            TallennaMerkinnat();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Kiitos käytöstä! Ohjelma päättyy.");
            Console.ResetColor();
        }

        static void NaytaOtsikko()
        {

            string decorativeText = @"
                                                                                                              
__________  _ _     __         _ _     __     __              __         
\______   \_____    __ ___  _______   |  | __ __ _______      __ _____   
 |     ___/\__  \  |  |\  \/ /\__  \  |  |/ /|  |\_  __ \    |  |\__  \  
 |    |     / __ \_|  | \   /  / __ \_|    < |  | |  | \/    |  | / __ \_
 |____|    (____  /|__|  \_/  (____  /|__|_ \|__| |__|   /\__|  |(____  /
                \/                 \/      \/            \______|     \/ ";

            // Display the decorative text
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(decorativeText);
            Console.ResetColor();


            string paivakirjaversio = "     VERSIO " + OHJELMAN_VERSIO;
            int linelength = paivakirjaversio.Length + 5;
            string line = new string('=', linelength);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(line);
            Console.WriteLine(paivakirjaversio);
            Console.WriteLine(line + "\n");
            Console.ResetColor();
        }

        static void NaytaValikko()
        {
            string valinta1 = " 1. Lisää merkintä";
            string valinta2 = " 2. Näytä merkinnät";
            string valinta3 = " 3. Poista merkintä";
            string valinta4 = " 4. Lopeta ohjelma";  // Päivitetty valinta

            int len1 = valinta1.Length;
            int len2 = valinta2.Length;
            int len3 = valinta3.Length;
            int len4 = valinta4.Length;

            int lineLen123 = Math.Max(Math.Max(len1, len2), len3);
            int lineLen45 = len4;
            int lineLenMax = Math.Max(lineLen123, lineLen45) + 1;

            string line = new string('-', lineLenMax);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(valinta1);
            Console.WriteLine(line);
            Console.WriteLine(valinta2);
            Console.WriteLine(line);
            Console.WriteLine(valinta3);
            Console.WriteLine(line);
            Console.WriteLine(valinta4 + "\n");
            Console.ResetColor();
        }

        static void LisaaMerkinta()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nKirjoita uusi merkintä: ");
            Console.ResetColor();

            string merkinta = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(merkinta))
            {
                DateTime aika = DateTime.Now;
                paivakirjaMerkinnat.Add($"{aika}: {merkinta}");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nMerkintä lisätty!");
                TallennaMerkinnat();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nTyhjä merkintä ei ole sallittu.");
            }
            Console.ResetColor();
        }

        static void NaytaMerkinnat()
        {
            Console.Clear();
            NaytaOtsikko();

            if (paivakirjaMerkinnat.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Päiväkirjamerkinnät:\n");
                Console.ResetColor();

                for (int i = 0; i < paivakirjaMerkinnat.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"{i + 1}. {paivakirjaMerkinnat[i]}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ei merkintöjä.");
            }
            Console.ResetColor();
        }

        static void PoistaMerkinta()
        {
            NaytaMerkinnat();

            if (paivakirjaMerkinnat.Count == 0)
            {
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nAnna poistettavan merkinnän numero tai kirjoita '0' poistaaksesi kaikki merkinnät: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int indeksi))
            {
                if (indeksi == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\nOletko varma, että haluat poistaa kaikki merkinnät? (K/E): ");
                    Console.ResetColor();

                    string vastaus = Console.ReadLine().ToUpper();

                    if (vastaus == "K")
                    {
                        paivakirjaMerkinnat.Clear();
                        TallennaMerkinnat();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nKaikki merkinnät poistettu!");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nToiminto peruutettu. Merkinnät säilytetty.");
                    }
                }
                else if (indeksi >= 1 && indeksi <= paivakirjaMerkinnat.Count)
                {
                    paivakirjaMerkinnat.RemoveAt(indeksi - 1);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nMerkintä poistettu!");
                    TallennaMerkinnat();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVirheellinen indeksi.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nVirheellinen syöte, anna numero.");
            }
            Console.ResetColor();
        }

        static void TallennaMerkinnat()
        {
            try
            {
                File.WriteAllLines(TIEDOSTON_NIMI, paivakirjaMerkinnat);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Virhe tallennettaessa tiedostoon: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void LataaMerkinnat()
        {
            try
            {
                if (File.Exists(TIEDOSTON_NIMI))
                {
                    paivakirjaMerkinnat.AddRange(File.ReadAllLines(TIEDOSTON_NIMI));
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Virhe ladattaessa tiedostoa: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void LuoKansioJosTarvitaan()
        {
            try
            {
                string kansioPolku = Path.GetDirectoryName(TIEDOSTON_NIMI);

                if (!Directory.Exists(kansioPolku))
                {
                    Directory.CreateDirectory(kansioPolku);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Virhe kansion luomisessa: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
