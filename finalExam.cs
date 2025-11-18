using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.IO.Pipes;
using System.Xml.Linq;

namespace Assessment3Exer
{
    class Voter
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Gender { get; private set; }
        public int Age { get; private set; }
        public string BaranggayCode { get; private set; }
        public string MunicipalityCode { get; private set; }


        public Voter(string id, string name, string gender, int age, string baranggaycode, string municipalitycode)
        {
            ID = id;
            Name = name;
            Gender = gender;
            Age = age;
            BaranggayCode = baranggaycode;
            MunicipalityCode = municipalitycode;
        }

        public string ToString()
        {
            return $"{ID} | {Name} | {Gender}  | {Age} | {BaranggayCode} | {MunicipalityCode}\n";
        }

    }


    class VotersRegistry
    {
        private string filePath = Path.Combine(AppContext.BaseDirectory, "voters.txt");

        public void AddVoter()
        {
            Console.WriteLine("\n===ADD NEW VOTER===");
            string id = GetString("ID");
            string name = GetString("Name"); 
            string gender = GetListFrom("Gender", new List<string> { "M", "F", "m", "f" });
            int age = GetAge();
            string municipalitycode = GetListFrom("Municipality", new List<string> { "1", "2" });
            string baranggaycode = GetBaranggay(municipalitycode);
            Voter v = new Voter(id, name, gender, age, baranggaycode, municipalitycode); 
            File.AppendAllText(filePath, v.ToString());
            Console.WriteLine("Voter saved!");
        }

        public void ViewSummary()
        {
            if (!File.Exists(filePath) || File.ReadAllLines(filePath).Length == 0)
            {
                Console.WriteLine(">> No voter data found.");
                return;
            }

            var lines = File.ReadAllLines(filePath);

            int total = 0;
            int cabuyao = 0;
            int lipa = 0;
            int male = 0;
            int female = 0;
            int teenage = 0;

            foreach (string line in lines)
            {
                string[] p = line.Split('|');
                if (p.Length < 6) continue;

                total++;

                string gender = p[2];
                int age = int.Parse(p[3]);
                string barangaystring = p[4];
                char barangay = barangaystring[0];
                string municipalitystring = p[5];
                char municipality = municipalitystring[1];


                if (gender == "M") male++;
                if (gender == "F") female++;

                if (age >= 18 && age <= 20) teenage++;

                if (municipality == '1') { cabuyao += 1; }
                ;
                if (municipality == '2') { lipa += 1; }
                ;
            }

            Console.WriteLine("\n=== Voter Summary ===");
            Console.WriteLine($"Total Voters: {total}");
            Console.WriteLine($"Cabuyao Voters: {cabuyao}");
            Console.WriteLine($"Lipa Voters: {lipa}");
            Console.WriteLine($"number of Male Voters: {male}");
            Console.WriteLine($"Number of Female voters: {female}");
            Console.WriteLine($"Number of teenage Voters: {teenage}");

            Console.WriteLine("\n=== Voter Information ===");
            foreach (string line in lines)
            { Console.WriteLine(line); }
        }


        public void ClearFile()
        {
            Console.Write("Are you sure you want to clear the file? (Y/N): ");
            string c = Console.ReadLine()?.Trim().ToUpper();

            if (c == "Y")
            {
                File.WriteAllText(filePath, string.Empty);
                Console.WriteLine(">> Cleared");
            }
            else
            {
                Console.WriteLine(">> Cancelled.");
            }

        }


        private string GetString(string str)
        {
            while (true)
            {
                Console.Write($"Enter {str}: ");
                string input = Console.ReadLine().Trim();
                if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                Console.WriteLine($">> {str} cannot be empty.");
            }
        }



        private int GetAge()
        {
            while (true)
            {
                Console.Write("Enter Age: ");
                if (int.TryParse(Console.ReadLine(), out int age) && age > 0)
                {
                    return age;
                }
                else
                {
                    Console.WriteLine(">> Invalid age");
                }
            }
        }

        private string GetBaranggay(String municipality)
        {
            while (true)
            {
                Console.Write($"Enter Baranggay Code: ");
                string g = Console.ReadLine().ToUpper();
                char m = municipality[0];
                if (new List<string> { "A", "B" }.Contains(g.ToUpper()))
                {
                    if (m.Equals('1') && g == "A")
                    {
                        return "A : Pulo";
                    }
                    if (m.Equals('1') && g == "B")
                    {
                        return "B : Mamatid";
                    }
                    if (m.Equals('2') && g == "A")
                    {
                        return "A : Balete";
                    }
                    if (m.Equals('2') && g == "B")
                    {
                        return "B : Lagyo";
                    }
                }
                else
                {
                    Console.WriteLine($">> Invalid Baranggay Code!");
                }

            }
        }
        private string GetListFrom(String statement, List<string> valids)
        {
            while (true)
            {
                Console.Write($"Enter {statement}: ");
                string g = Console.ReadLine();
                if (valids.Contains(g))
                {
                    if (g.Equals("M") || g.Equals("m") || g.Equals("F") || g.Equals("f"))
                    {
                        return g.ToUpper();
                    }
                    if (g.Equals("1"))
                    {
                        return "1 : Cabuyao";
                    }
                    if (g.Equals("2"))
                    {
                        return "2 : Lipa";
                    }
                    return g;
                }
                else
                {
                    Console.WriteLine($">> Invalid {statement}!");
                }

            }
        }

    }
    internal class Program
    {
        static void Main()
        {

            VotersRegistry VotersRegistry = new VotersRegistry();

            while (true)
            {
                Console.WriteLine("\n=== Voter's Information ===");
                Console.WriteLine("[1] Add Voter's Information");
                Console.WriteLine("[2] Summary of Informations");
                Console.WriteLine("[3] Clear File");
                Console.WriteLine("[4] Exit");
                Console.Write("Select: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine(">> Invalid input");
                    continue;
                }

                switch (choice)
                {
                    case 1: VotersRegistry.AddVoter(); break; //oop approach here too
                    case 2: VotersRegistry.ViewSummary(); break;
                    case 3: VotersRegistry.ClearFile(); break;
                    case 4: Console.WriteLine(">> Program Exited"); return;
                    default: Console.WriteLine(">> Invalid choice."); break;
                }
            }
        }
    }
}
