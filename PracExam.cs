using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace voters
{

    public class program
    {

        static void Main()
        {
            while (true)
            {
                Console.Write("[1] Add Information\n[2] Summary of Informtion\n[3] Delete Indormation\n[4] Exit\nChoice: ");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.Write("Enter your Voter's Id: ");
                        string Id = Console.ReadLine();
                        Console.Write("Enter your name: ");
                        string Name = Console.ReadLine();
                        Console.Write("Enter your gender: ");
                        string Gender = Console.ReadLine();
                        Console.Write("Enter your age: ");
                        int Age = int.Parse(Console.ReadLine());
                        Console.Write("Enter your Barangay Code: ");
                        string Bcode = Console.ReadLine().ToUpper();
                        Console.Write("Enter your Barangay: ");
                        string B = Console.ReadLine();
                        Console.Write("Enter your Municipality Code: ");
                        string Mcode = Console.ReadLine();
                        Console.Write("Enter your Municipality: ");
                        string M = Console.ReadLine();
                        VoterInfo voterInfo = new VoterInfo(Id, Name, Gender, Age, Bcode, B, Mcode, M);
                        break;
                    case 2:
                        FileHandling file = new FileHandling();
                        file.show_summary();
                        break;
                    case 3:
                        Console.Write("Are you sure? Y/N: ");
                        string Choice = Console.ReadLine().ToUpper();
                        if (Choice == "Y")
                        {
                            FileHandling file1 = new FileHandling();
                            file1.clear_file();
                        }
                        else
                        {
                            continue;
                        }
                        break;
                    case 4:
                        //Print Exitinggggg
                        return;
                    default:
                        Console.WriteLine("Please enter a valid choice.");
                        break;

                }
            }
        }
    }
    public class VoterInfo
    {
        private string Id;
        private string Name;
        private string Gender;
        private int Age;
        private string Code;
        private string Barangay;
        private string Muni_Code;
        private string Muni;

        public string id { get { return Id; } }
        public string name { get { return Name; } }
        public string gender { get { return Gender; } }
        public int age { get { return Age; } }
        public string bcode { get { return Code; } }
        public string barangay { get { return Barangay; } }
        public string mcode { get { return Muni_Code; } }
        public string muni { get { return Muni; } }

        public VoterInfo(string voters_Id, string voters_Name, string voters_Gender, int voters_Age, string Bcode, string B, string Mcode, string M)
        {
            Id = voters_Id;
            Name = voters_Name;
            Gender = voters_Gender;
            Age = voters_Age;
            Code = Bcode;
            Barangay = B;
            Muni_Code = Mcode;
            Muni = M;

            string logs = $"{Id} | {Name} | {Gender} | {Age} | {Code} | {Barangay} | {Muni_Code} | {Muni}";
            FileHandling file = new FileHandling();
            file.create_write(logs, Code);
        }
    }

    public class FileHandling
    {
        public string filepath = @"C:\Users\IT111L-CIS201-13\source\repos\Par_PracExam\Par_PracExam\File.txt";

        public void create_write(string logs, string bCode)
        {
            if (string.IsNullOrEmpty(logs)) return;

            if (!File.Exists(filepath))
            {
                File.WriteAllText(filepath, "");
            }

            List<string> lines = new List<string>(File.ReadAllLines(filepath));
            string sectionHeader = $"=== Barangay Code {bCode.ToUpper()} ===";

            if (!lines.Contains(sectionHeader))
            {
                bool inserted = false;
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].StartsWith("=== Barangay Code "))
                    {
                        string existing = lines[i].Substring(12, 1);
                        if (string.Compare(existing, bCode, StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            lines.Insert(i, "");
                            lines.Insert(i, sectionHeader);
                            inserted = true;
                            break;
                        }
                    }
                }

                if (!inserted)
                {
                    if (lines.Count > 0) lines.Add("");
                    lines.Add(sectionHeader);
                }
            }

            int index = -1;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i] == sectionHeader)
                {
                    index = i + 1;
                    while (index < lines.Count && !lines[index].StartsWith("=== Barangay Code "))
                        index++;
                    break;
                }
            }

            if (index == -1)
                lines.Add(logs);
            else
                lines.Insert(index, logs);

            File.WriteAllLines(filepath, lines);
        }

        public void show_summary()
        {
            if (!File.Exists(filepath))
            {
                Console.WriteLine("No records found.");
                return;
            }

            string[] lines = File.ReadAllLines(filepath);
            Dictionary<string, List<int>> sectionAges = new Dictionary<string, List<int>>();
            List<int> allAges = new List<int>();

            string[] allSections = { "A", "B" };
            foreach (string s in allSections)
                sectionAges[s] = new List<int>();

            string currentSection = "";

            foreach (string line in lines)
            {
                if (line.StartsWith("=== Barangay Code "))
                {
                    currentSection = line.Replace("=", "").Replace("Barangay Code", "").Trim();
                    if (!sectionAges.ContainsKey(currentSection))
                        sectionAges[currentSection] = new List<int>();
                }
                else if (!string.IsNullOrWhiteSpace(line))
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 8)
                    {
                        int age = int.Parse(parts[3].Trim());
                        string sec = parts[2].Trim().ToUpper();
                        allAges.Add(age);

                        if (!sectionAges.ContainsKey(sec))
                            sectionAges[sec] = new List<int>();
                        sectionAges[sec].Add(age);
                    }
                }
            }

            int totalStudents = allAges.Count;

            Console.WriteLine("\n=== View Summary ===");
            Console.WriteLine($"Number of residents per barangay: {totalStudents}");

            foreach (var sec in allSections)
            {
                int count = sectionAges.ContainsKey(sec) ? sectionAges[sec].Count : 0;
                Console.WriteLine($"Number of residents per municipality {sec}: {count}");
            }

             
            Console.WriteLine("");
        }

        public void clear_file()
        {
            File.WriteAllText(filepath, "");
            Console.WriteLine("All records cleared.");
            Console.WriteLine("");
        }
    }
}
