using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    public static string filepath = @"C:\Users\Dean Par\source\repos\IT111_LabExer3\log.txt";
    public static string folderpath = @"C:\Users\Dean Par\source\repos\IT111_LabExer3";

    static void Main()
    {
        Console.WriteLine("=== Help Desk Logbook ===");
        Console.WriteLine($"Data folder: {folderpath}");
        Console.WriteLine($"Log file:    {filepath}");
        Console.WriteLine("\n[Create] New log header written.\n");
        Console.WriteLine("Type log lines. Press Enter on an empty line to finish.\n");

        try
        {
            logs_datetime();

            string logs;
            while (true)
            {
                Console.Write("> ");
                logs = Console.ReadLine();
                create_write(logs);
                if (string.IsNullOrEmpty(logs)) break;
            }

            Console.WriteLine("\n[Read] File Contents:\n");
            read_logs();

            Console.WriteLine("\n[Stats]");
            stats();
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Oops! I don’t have permission to access this file or folder.");
            Console.WriteLine("Try running as Administrator or saving to a different location.");
        }
        catch (IOException ex)
        {
            Console.WriteLine("File access error: " + ex.Message);
            Console.WriteLine("Check if the file is open in another program or if your disk is full.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Something unexpected happened: " + ex.Message);
        }
    }

    public static void stats()
    {
        try
        {
            string fileContent = File.ReadAllText(filepath);
            string[] words = Regex.Split(fileContent, @"\W+", RegexOptions.IgnorePatternWhitespace);
            FileInfo fileBytes = new FileInfo(filepath);

            int lineCount = File.ReadLines(filepath).Count();
            int wordCount = words.Count(w => !string.IsNullOrEmpty(w));
            int characterCount = fileContent.Length;
            long fileSizeBytes = fileBytes.Length;
            

            Console.WriteLine($"Lines: {lineCount}\tWords: {wordCount}\tCharacters: {characterCount}");
            Console.WriteLine($"Size (bytes): {fileSizeBytes}");
        }
        catch (IOException)
        {
            Console.WriteLine("Sorry, I couldn’t read the file to calculate stats.");
            Console.WriteLine("Please make sure the file isn’t being used by another program.");
        }
    }

    public static void read_logs()
    {
        try
        {
            if (!File.Exists(filepath))
            {
                Console.WriteLine("Log file not found! Nothing to read yet.");
                return;
            }

            string[] lines = File.ReadAllLines(filepath);
            foreach (string line in lines)
            {
                Console.WriteLine(line ?? "");
            }
        }
        catch (IOException)
        {
            Console.WriteLine("I couldn’t read the log file.");
            Console.WriteLine("Maybe it’s being used by another program or temporarily unavailable.");
        }
    }

    public static void logs_datetime()
    {
        try
        {
            DateTime curDateTime = DateTime.Now;
            string formatDateTime = curDateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
            using (StreamWriter writer = File.AppendText(filepath))
            {
                writer.WriteLine("=== Help Desk Logbook ===");
                writer.WriteLine($"Created {formatDateTime}");
                writer.WriteLine("Format: free-text entries per line");
                writer.WriteLine("----------------------------------------");
            }
        }
        catch (IOException)
        {
            Console.WriteLine("Couldn’t write the log header.");
            Console.WriteLine("Please make sure the file isn’t open somewhere else.");
        }
    }

    public static void create_write(string logs)
    {
        if (string.IsNullOrEmpty(logs)) return;

        try
        {
            using (StreamWriter writer = File.AppendText(filepath))
            {
                writer.WriteLine(logs ?? "");
            }
        }
        catch (IOException)
        {
            Console.WriteLine("Couldn’t write to the log file.");
            Console.WriteLine("Try closing any other apps that might be using the file, then try again.");
        }
    }
}
