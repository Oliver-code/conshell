using Microsoft.VisualBasic;
using System.Dynamic;
using System.Runtime.CompilerServices;

class Program
{
    
    static void ChangeSetting(string setting, int value)
    {

     
       if (setting == "openonboot")
       {


            File.Delete("Reg\\Settings\\openonboot\\openonboot.bin");
            File.WriteAllText("Reg\\Settings\\openonboot\\openonboot.bin", $"{value}");


            
       }

       if (setting == "saveonexit")
       {


            File.Delete("Reg\\Settings\\saveonexit\\saveonexit.bin");
            File.WriteAllText("Reg\\Settings\\saveonexit\\saveonexit.bin", $"{value}");


            
       }

       


        
    }

    

    static void ApplySettings()
    {
        try
        {

            string open = File.ReadAllText("Reg\\Settings\\openonboot\\openonboot.bin");
            string save = File.ReadAllText("Reg\\Settings\\saveonexit\\saveonexit.bin");
            string deb = File.ReadAllText("Reg\\Debug\\debug.bin");
            string acti = File.ReadAllText("Reg\\Activation\\activated\\activated.bin");

            if (open == "1")
            {

                openonboot = true;

            }

            if (save == "1")
            {

                saveonexit = true;

            }

            if (deb == "1")
            { 

                if (debfirstboot)
                {
                    Console.WriteLine("Debug Mode is enabled and setting states will be shown");
                    debfirstboot = false; 
                }
                debug = true;
            }


            if (Directory.Exists("Reg\\ForceActivate"))
            {

                if (File.Exists("Reg\\ForceActivate\\Activate.act"))
                {

                    acti = "1";


                }

            }

            if (acti == "1")
            {
                activated = true;
            }
        }

        catch
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"There was a error loading the settings, the registry could be damaged or corrupted");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
     
    static string Splitfirst(string input)
    {
        int index = input.IndexOf(' ');

        return input.Substring(0, index);
    }

    static string Splitsecond(string input)
    {
        int index = input.IndexOf(' ');
        return input.Substring(index + 1);
    }

    

    static string syspath = "C:\\";
    static string oldpath;
    static bool openonboot = false;
    static bool debfirstboot = true;
    static bool debug = false;
    static bool saveonexit = false;
    static bool activated = false;
    static bool firstboot = true;

static void Main()
    {


        Console.Title = "Conshell";
        
        if (!Directory.Exists("Reg"))
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Critical Error: Registry does not exist");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey();
            Environment.Exit(0);
        }

        if (debug)
        {
            Console.WriteLine($"saveonexit: {saveonexit}, openonboot: {openonboot}, activated: {activated}, debug: {debug}");
        }
        
        if (firstboot)
        {
            

            if (openonboot)
            {

                if (!File.Exists("Reg\\pathsave\\path.pathsave"))
                {

                    goto error;

                }

                syspath = File.ReadAllText("Reg\\pathsave\\path.pathsave");

            }
            error:
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to Conshell, type 'help' for all commands. Top tip: The file system is the same to cmd.");
            
            firstboot = false;

        }
        ApplySettings();


        string path = Path.Combine(syspath, ">");
        
        Console.Write(path);
        string input = Console.ReadLine();
        string coma = input.ToLower();

        if (input == "")
        {
            Main();
        }

        if (coma == "cls")
        {

            Console.Clear();
            Main();

        }

        if (coma == "help")
        {

            Console.WriteLine("echo <text> \nexit \nabout \ndir \nread <file> \nmkdir <folder> \nmkfile <file> \nregedit \npathsave <path or 'dir' for existing folder> \ndebug <enable / disable / show> \npathopen \nrmfile <file> \nwrite <file> \nchangset <setting> <value> \nvalikey <product key> \nrmfile <file> \ncd <folder> \nrmdir <folder> \nactivate <product key>");
            Main();

        }

        if (coma == "refresh")
        {
            Console.Clear();
            firstboot = true;
            Main();
              
        }

        if (coma == "pathopen")
        {

            if (!File.Exists("Reg\\pathsave\\usrpath.pathsave"))
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No pathsave data found for user");
                Console.ForegroundColor = ConsoleColor.White;
                Main();
            }

            string usrpath = File.ReadAllText("Reg\\pathsave\\usrpath.pathsave");
            syspath = usrpath;
            Console.WriteLine("Opened and applyed saved path");
            Main();

        }

        if (coma == "exit")
        {


            if (syspath.StartsWith("Reg"))
            {
                syspath = oldpath;
                Main();


            }

            if (saveonexit)
            {
                if (!Directory.Exists(syspath))
                {
                    Environment.Exit(0);

                }

                if (File.Exists("Reg\\pathsave\\path.pathsave"))
                {

                    try
                    {
                        File.Delete("Reg\\pathsave\\path.pathsave");
                        File.AppendAllText("Reg\\pathsave\\path.pathsave", syspath);
                        Environment.Exit(0);
                    }

                    catch
                    {

                        Environment.Exit(0);

                    }

                }

                if (!File.Exists("Reg\\pathsave\\path.pathsave"))
                {
                    File.AppendAllText("Reg\\pathsave\\path.pathsave", syspath);
                    Environment.Exit(0);
                }
            }
            else
            {

                Environment.Exit(0);
                
            }
        }

        if (coma == "about")
        {

            Console.WriteLine($"Conshell version: 1.5 \nCreator: Oliver \nabout: A terminal desined for creating, editing and deleting files and folders. With auto save on exit and auto open saved paths on startup \nActivated: {activated}");
            Main();
        }

        if (coma == "regedit")
        {
            if (!activated)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You need to activate conshell to use the registry editer");
                Console.ForegroundColor = ConsoleColor.White;
                Main();

            }

            oldpath = syspath;

            syspath = "Reg";

            Main();


        }

        if (coma == "dir")
        {

            if (!Directory.Exists(syspath))
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There has been a error and the path is not valid. enter the new path below");

                Console.ForegroundColor = ConsoleColor.White;
                syspath = Console.ReadLine();
                if (!syspath.EndsWith('\\'))
                {
                    syspath = $"{syspath}\\";
                }


                Main();

            }

            DirectoryInfo[] folders = new DirectoryInfo(syspath).GetDirectories();
            FileInfo[] files = new DirectoryInfo(syspath).GetFiles();

            foreach (DirectoryInfo folder in folders)
            {



                Console.WriteLine($"Directory: {folder.Name}");

            }

            foreach (FileInfo file in files)
            {

                int filekb = Convert.ToInt32(file.Length / 1024);

                if (file.Length > 1024 && filekb > 1000)
                {
                    Console.WriteLine($"File: {file.Name}, Size: {filekb} KB");
                    goto end;
                }

                if (file.Length / 1024 > 1000)
                {

                    long filemb = file.Length / 1024 / 1000;

                    Console.WriteLine($"File: {file.Name}, Size: {filemb} MB");
                    goto end;
                }

                else
                {
                    Console.WriteLine($"File: {file.Name}, Size: {file.Length} Bytes");
                    goto end;
                }

            
            end:;
                

            }

            Main();
            
        }

        if (input.Contains(' '))
        {
            string com = Splitfirst(input).ToLower();
            string exp = Splitsecond(input);

            if (com == "echo")
            {
                Console.WriteLine(exp + "\n");
                Main();
            }

            if (com == "valikey")
            {

                string[] keys = { "c6aa-d82A-conshell-Ofa0", "d18C-O679-conshell-d155", "39FO-013c-conshell-cOCF", "88OD-95dD-conshell-o7CD", "1f31-2FC7-conshell-13c9", "a91D-19FO-conshell-4F76", "O7Ad-D2DC-conshell-7dcO", "AcDD-oO9A-conshell-05fD", "3A5a-oc5o-conshell-63AD", "6055-5aA8-conshell-0oDc", "o0f6-ao53-conshell-Aa19", "6ocO-9o8c-conshell-oDAf", "3658-5COO-conshell-82A1", "3163-141O-conshell-08A3", "4653-CC0a-conshell-484O", "O571-O8A9-conshell-F7C7", "4D6d-583c-conshell-263D", "10c3-df4C-conshell-3oa7", "daOf-foFF-conshell-OcAo", "51cf-7729-conshell-A800", "0Co2-Fc5F-conshell-AccD", "F888-a2cc-conshell-08DF", "98dc-363c-conshell-50A4", "2c53-cdf4-conshell-Aa3o", "Ao79-15D4-conshell-59Do" };

                foreach (string key in keys)
                {


                    if (key == exp)
                    {


                        Console.WriteLine("Key is valid");
                        Main();

                    }
                }

                Console.WriteLine("The key is not valid");
                Main();

            }

            if (com == "rmdir")
            {
                Console.Write($"WARNING: ALL DATA IN '{exp}' WILL BE DELETED! Continue?[Y\\N]: ");
                string yn = Console.ReadLine();

                if (yn == "Y")
                {
                    Directory.Delete($"{syspath}\\{exp}", true);
                    Console.WriteLine($"Folder {exp} was deleted");
                    Main();
                }
                
                if (yn == "N")
                {

                    Console.WriteLine($"Folder {exp} was not deleted");
                    Main();

                }

            }

            if (com == "mkfile")
            {

                File.Create($"{syspath}{exp}");
                Console.WriteLine($"Created file: {exp}");
                Main();

            }

            if (com == "debug")
            {
                if (exp == "enable")
                {
                    File.Delete("Reg\\Debug\\debug.bin");
                    File.WriteAllText("Reg\\Debug\\debug.bin", "1");
                    debug = true;
                    Console.WriteLine("Debug mode enabled");
                    Main();
                }
                if (exp == "disable")
                {
                    File.Delete("Reg\\Debug\\debug.bin");
                    File.WriteAllText("Reg\\Debug\\debug.bin", "0");
                    debug = false;
                    Console.WriteLine("Debug mode disabled");
                    Main();
                }
                if (exp == "show")
                {
                    
                    Console.WriteLine($"Debug mode: {debug}");
                    Main();
                }
            }

            if (com == "write")
            {
                string file = $"{syspath}{exp.Split(exp)[0]}";

                try
                {
                    if (!exp.Contains("/n"))
                    {


                        File.AppendAllText(file, exp);
                        Console.WriteLine("Written text to the file");
                        Main();

                    }

                    


                    string[] lines = exp.Split("/n");

                    File.AppendAllLines(file, lines);
                    Console.WriteLine("Written text to the file");
                    Main();
                }

                catch
                {

                    Console.WriteLine("There was a error writing to the file");
                    Main();
                }

            }

            if (com == "pathsave")
            {

                if (exp == "dir" && !Directory.Exists(exp))
                {

                    File.WriteAllText("Reg\\pathsave\\usrpath.pathsave", syspath);
                    Console.WriteLine("Saved path");
                    Main();
                }

                if (!Directory.Exists(exp))
                {


                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Path to save does not exist");
                    Console.ForegroundColor = ConsoleColor.White;
                    Main();
                }


                File.WriteAllText("Reg\\pathsave\\usrpath.pathsave", exp);
                Console.WriteLine("Saved path");
                Main();

            }

            if (com == "activate")
            {

                if (exp == "show")
                {

                    if (!File.Exists("Reg\\Activation\\key.key"))
                    {

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No activation key data found, try another key for activation. If this does not work wait for another release ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Main();
                    }

                    string key = File.ReadAllText("Reg\\Activation\\key.key");
                    Console.WriteLine(key);
                    Main();
                }


                

                string[] keys = { "c6aa-d82A-conshell-Ofa0", "d18C-O679-conshell-d155", "39FO-013c-conshell-cOCF", "88OD-95dD-conshell-o7CD", "1f31-2FC7-conshell-13c9", "a91D-19FO-conshell-4F76", "O7Ad-D2DC-conshell-7dcO", "AcDD-oO9A-conshell-05fD", "3A5a-oc5o-conshell-63AD", "6055-5aA8-conshell-0oDc", "o0f6-ao53-conshell-Aa19", "6ocO-9o8c-conshell-oDAf", "3658-5COO-conshell-82A1", "3163-141O-conshell-08A3", "4653-CC0a-conshell-484O", "O571-O8A9-conshell-F7C7", "4D6d-583c-conshell-263D", "10c3-df4C-conshell-3oa7", "daOf-foFF-conshell-OcAo", "51cf-7729-conshell-A800", "0Co2-Fc5F-conshell-AccD", "F888-a2cc-conshell-08DF", "98dc-363c-conshell-50A4", "2c53-cdf4-conshell-Aa3o", "Ao79-15D4-conshell-59Do" };

                foreach (string key in keys)
                {

                    if (key == exp)
                    {

                        File.Delete("Reg\\Activation\\activated\\activated.bin");
                        File.WriteAllText("Reg\\Activation\\activated\\activated.bin", "1");
                        File.WriteAllText("Reg\\Activation\\key.key", exp);
                        Console.WriteLine($"Activated with product key '{exp}'");
                        Main();
                    }

                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid product key, please obtain an valid key");
                Console.ForegroundColor = ConsoleColor.White;
                Main();
            }

            if (com == "mkdir")
            {

                Directory.CreateDirectory($"{syspath}\\{exp}");
                Console.WriteLine($"Created folder: {exp}");
                Main();

            }

            if (com == "cd")
            {


                if (exp.Contains(':'))
                {

                    DriveInfo[] drives = DriveInfo.GetDrives();

                    foreach (DriveInfo drive in drives)
                    {
                        if (drive.Name.Equals(exp, StringComparison.OrdinalIgnoreCase))
                        {
                            syspath = drive.Name;
                            Main();
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The drive does not exist");
                    Console.ForegroundColor = ConsoleColor.White;
                    Main();

                }

                if (!Directory.Exists($"{syspath}\\{exp}"))
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"The directory '{exp}' does not exist");
                    Console.ForegroundColor = ConsoleColor.White;
                    Main();
                }


                if (exp == "..")
                {
                    string dir = Path.Combine(syspath, "..");

                    string[] parts = dir.Split('\\');
                    

                    try
                    {


                        if (parts[1] == "..")
                        {

                            Main();

                        }



                        
                        Array.Resize(ref parts, parts.Length - 2);
                        
                        syspath = string.Join("\\", parts);

                        Main();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: " + ex.Message);
                        Main();
                    }
                }

              


                if (exp.Contains(' '))
                {

                    syspath = $"{syspath}{exp}\\";
                    Main();

                }

                if (!syspath.EndsWith('\\'))
                {
                    syspath = $"{syspath}\\";
                }

                if (!exp.EndsWith('\\'))
                {
                    syspath = $"{syspath}{exp}\\";
                    Main();
                }

                if (exp.EndsWith('\\'))
                {

                    syspath = $"{syspath}{exp}";

                }
                
                if (!exp.EndsWith('\\'))
                {

                    syspath = $"{syspath}\\{exp}";

                }



                Main();
            }

            if (com == "rmfile")
            {

                if (!File.Exists($"{syspath}{exp}"))
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file does not exist");
                    Console.ForegroundColor = ConsoleColor.White;
                    Main();

                }

                File.Delete($"{syspath}{exp}");
                Console.WriteLine($"Deleted file: {exp}");
                Main();

            }

            if (com == "read")
            {
                string filepath = $"{syspath}{exp}";

                if (!File.Exists(filepath))
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The file does not exist");
                    Console.ForegroundColor = ConsoleColor.White;
                    Main();

                }

                string contents = File.ReadAllText(filepath);

                Console.WriteLine(contents);
                Main();

            }

            
            if (com == "changset")
            {


                if (!activated)
                {

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You need to activate conshell to change the settings, activate now with the 'activate' command with the key");
                    Console.ForegroundColor = ConsoleColor.White;
                    Main();

                }

                string[] parts = exp.Split(' ');
                int value = int.Parse(parts[1]);

                ChangeSetting(parts[0], value);

                Console.WriteLine($"Changed setting: {parts[0]} to: {parts[1]}. The app now will close to apply settings.");
                Console.Write("Press any key to Exit . . . ");
                Console.ReadKey();
                Environment.Exit(0);

            }


            else
            {



                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Command '{input}' does not exist");
                Console.ForegroundColor = ConsoleColor.White;
                Main();

            }


        }


        else
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Command '{input}' does not exist");
            Console.ForegroundColor = ConsoleColor.White;
            Main();

        }



    }

}