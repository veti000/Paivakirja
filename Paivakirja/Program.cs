using System;
using System.Collections.Generic;
using System.IO;

namespace PaivakirjaOhjelma
{
    class Program
    {
        // Vakio ohjelman versiolle
        const string OHJELMAN_VERSIO = ": Beta 0.2";

        // Tiedoston nimi ja sijainti
        const string TIEDOSTON_NIMI = @"C:\Kansio\paivakirja.txt";

        // Päiväkirjamerkintöjen lista
        static List<string> paivakirjaMerkinnat = new List<string>();

        static void Main(string[] args)
        {
            bool jatka = true;

            // Luodaan kansio, jos sitä ei ole olemassa.
            LuoKansioJosTarvitaan();

            // Ladataan olemassa olevat merkinnät tiedostosta ohjelman käynnistyessä.
            LataaMerkinnat();

            while (jatka)
            {
                Console.Clear(); // Tyhjennetään konsoli
                NaytaOtsikko();
                NaytaValikko();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Valitse toiminto (1-4): ");
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

            // Tallennetaan merkinnät tiedostoon ohjelman lopettaessa
            TallennaMerkinnat();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Kiitos käytöstä! Ohjelma päättyy.");
            Console.ResetColor();
        }

        // Metodi näyttää ohjelman otsikon
        static void NaytaOtsikko()
        {
            string paivakirjaversio = "     PÄIVÄKIRJA - VERSIO " + OHJELMAN_VERSIO;
            int linelength = paivakirjaversio.Length + 5;
            string line = new string('=', linelength);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(line);
            Console.WriteLine(paivakirjaversio);
            Console.WriteLine(line + "\n");
            Console.ResetColor();
        }

        // Metodi näyttää valikon
        static void NaytaValikko()
        {
            string valinta1 = " 1. Lisää merkintä";
            string valinta2 = " 2. Näytä merkinnät";
            string valinta3 = " 3. Poista merkintä";
            string valinta4 = " 4. Lopeta ohjelma";

            int len1 = valinta1.Length;
            int len2 = valinta2.Length;
            int len3 = valinta3.Length;
            int len4 = valinta4.Length;

            int lineLen12 = Math.Max(len1, len2);
            int lineLen34 = Math.Max(len3, len4);
            int lineLenMax = Math.Max(lineLen12, lineLen34) + 1;

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

        // Metodi lisää uuden päiväkirjamerkinnän
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

                // Tallennetaan merkinnät tiedostoon heti lisäyksen jälkeen
                TallennaMerkinnat();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nTyhjä merkintä ei ole sallittu.");
            }
            Console.ResetColor();
        }

        // Metodi näyttää kaikki päiväkirjamerkinnät
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

        // Metodi poistaa merkinnän annetulla indeksillä
        static void PoistaMerkinta()
        {
            NaytaMerkinnat();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nAnna poistettavan merkinnän numero: ");
            Console.ResetColor();

            if (int.TryParse(Console.ReadLine(), out int indeksi))
            {
                if (indeksi >= 1 && indeksi <= paivakirjaMerkinnat.Count)
                {
                    paivakirjaMerkinnat.RemoveAt(indeksi - 1);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nMerkintä poistettu!");

                    // Tallennetaan merkinnät tiedostoon poiston jälkeen
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

        // Metodi tallentaa merkinnät tiedostoon
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

        // Metodi lataa merkinnät tiedostosta ohjelman käynnistyessä
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

        // Metodi luo tarvittavan kansion, jos sitä ei ole olemassa
        static void LuoKansioJosTarvitaan()
        {
            try
            {
                // Haetaan kansio tiedostopolusta
                string kansioPolku = Path.GetDirectoryName(TIEDOSTON_NIMI);

                // Luodaan kansio, jos sitä ei ole
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
