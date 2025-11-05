using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static int select;
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("=== Mini Student Registry ===");
            Console.WriteLine("[1] Add Student");
            Console.WriteLine("[2] View Summary");
            Console.WriteLine("[3] Clear File");
            Console.WriteLine("[4] Exit");
            Console.Write("Select: ");
            select = int.Parse(Console.ReadLine());
            Console.WriteLine("");

            switch (select)
            {
                case 1:
                    Console.Write("Enter ID: ");
                    string id = Console.ReadLine();
                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter age: ");
                    int age = int.Parse(Console.ReadLine());
                    Console.Write("Enter Section (A/B/C): ");
                    string section = Console.ReadLine().ToUpper();
                    Console.WriteLine("");

                    AddStudent.combine(id, name, age, section);
                    break;
                case 2:
                    FileHandling.show_summary();
                    break;
                case 3:
                    Console.Write("Are you sure? Y/N: ");
                    string choice = Console.ReadLine().ToUpper();
                    if(choice == "Y")
                    {
                        FileHandling.clear_file();
                    }
                    else
                    {
                        continue;
                    }
                        break;
                case 4:
                    Console.WriteLine("Exiting...");
                    return;

            }
        }
    }
}
public class AddStudent
{

    private string _id;
    private string _name;
    private int _age;
    private string _section;

    public string ID { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public int Age { get { return _age; } set { _age = value; } }
    public string Section { get { return _section; } set { _section = value; } }

    public AddStudent(string id, string name, int age, string section)
    {
        _id = id;
        _name = name;
        _age = age;
        _section = section;
    }

    public static void combine(string id, string name, int age, string section)
    {
        string single_Str = $"{id} | {name} | {age} | {section}";
        FileHandling.create_write(single_Str, section);
    }

}
public class FileHandling
{
    public static string filepath = @"C:\Users\Dean Par\source\repos\MiniStudReg\log.txt";

    public static void create_write(string logs, string section)
    {
        if (string.IsNullOrEmpty(logs)) return;

        if (!File.Exists(filepath))
        {
            File.WriteAllText(filepath, "");
        }

        List<string> lines = new List<string>(File.ReadAllLines(filepath));
        string sectionHeader = $"=== Section {section.ToUpper()} ===";

        if (!lines.Contains(sectionHeader))
        {
            bool inserted = false;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("=== Section "))
                {
                    string existing = lines[i].Substring(12, 1);
                    if (string.Compare(existing, section, StringComparison.OrdinalIgnoreCase) > 0)
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
                while (index < lines.Count && !lines[index].StartsWith("=== Section "))
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

    public static void show_summary()
    {
        if (!File.Exists(filepath))
        {
            Console.WriteLine("No records found.");
            return;
        }

        string[] lines = File.ReadAllLines(filepath);
        Dictionary<string, List<int>> sectionAges = new Dictionary<string, List<int>>();
        List<int> allAges = new List<int>();

        string[] allSections = { "A", "B", "C" };
        foreach (string s in allSections)
            sectionAges[s] = new List<int>();

        string currentSection = "";

        foreach (string line in lines)
        {
            if (line.StartsWith("=== Section "))
            {
                currentSection = line.Replace("=", "").Replace("Section", "").Trim();
                if (!sectionAges.ContainsKey(currentSection))
                    sectionAges[currentSection] = new List<int>();
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                string[] parts = line.Split('|');
                if (parts.Length >= 4)
                {
                    int age = int.Parse(parts[2].Trim());
                    string sec = parts[3].Trim().ToUpper();
                    allAges.Add(age);

                    if (!sectionAges.ContainsKey(sec))
                        sectionAges[sec] = new List<int>();
                    sectionAges[sec].Add(age);
                }
            }
        }

        int totalStudents = allAges.Count;
        double averageAge = totalStudents > 0 ? allAges.Average() : 0;

        Console.WriteLine("\n=== View Summary ===");
        Console.WriteLine($"Total Students: {totalStudents}");

        foreach (var sec in allSections)
        {
            int count = sectionAges.ContainsKey(sec) ? sectionAges[sec].Count : 0;
            Console.WriteLine($"Section {sec}: {count}");
        }

        Console.WriteLine($"Average Age: {averageAge:F1}");
        Console.WriteLine("");
    }

    public static void clear_file()
    {
        File.WriteAllText(filepath, "");
        Console.WriteLine("All records cleared.");
        Console.WriteLine("");
    }
}
