using System;
using System.Management; 
using System.Runtime.InteropServices; 

public class NeofetchCommand : ICommand
{

    public void Execute(string args)
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.White;
            PrintAsciiArt();
           

            Console.ResetColor();
            PrintSystemInfo();
            PrintSeparator();


        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
    private void PrintAsciiArt()
    {
        string[] asciiArt =
        {"--  *****************************************************",
        "--  *********************=---:--=+***********************",
        "--  *******************-::::......-**********************",
        "--  ******************-::-++-:.....-*********************",
        "--  *****************-:..:*#+:..::::*********************",
        "--  ****************+.::...:.:::==++*######**************",
        "--  ****************-.::.......-=-==*#######*************",
        "--  ****************-:::::::.:-=---==*######*************",
        "--  **************#*-::::-:::+++===++=+######************",
        "--  ************###*-::-===--:=+++======+*###************",
        "--  **********#####*=::--=+=---=+**+===--=+*##**********#",
        "--  ********#*######+:::::-===+####***++++***#**********#",
        "--  ***#*******#####*-:::.::-=*###*####*****#***********#",
        "--  ***###**########*-::::..::+######*##**###**#*****####",
        "--  #######*########*-::::::::-######****####***#****####",
        "--  ################*-..:::::::*#####**#########****#####",
        "--  ################*=..:::::::=#############*******#####",
        "--  #################-:.:::::::-*################**######",
        "--  #################*=-::::::::-##########****#**#######",
        "--  ################*+*++-:::-==+*###########**##*#######",
        "--  ################***++*+=++++++############*##########",
        "--  #############*+:+*****+**+===+*##########*###*#######",
        "--  ###########***+:=****+===++++=***########*###########",
        "--  #########*++**+:-**=:::::--+*-****###################",
        "--  #######*++*****-.::::::::-:::-******#################",
        "--  ######*+*******=:::::::::-::-=*******################",
        "--  ####***********=::::::::--::-=**+*****###############",
        "--  ##*************+::::::::::::-+*+++*+*****############",
        "--  #***************-:::::..::::=*********#**############"
        };
        foreach (var line in asciiArt)
        {
            Console.WriteLine(line);
        }
    }
    private void PrintSystemInfo()
    {
        // Systema Info 
        Console.WriteLine($"\n{"ОС:",-15} {Environment.OSVersion}");
        Console.WriteLine($"{"Name Pc:",-15} {Environment.MachineName}");
        Console.WriteLine($"{"Processors:",-15} {Environment.ProcessorCount} cores");
        Console.WriteLine($"{"Memory:",-15} {GetTotalMemoryInGB()} GB");
        Console.WriteLine($"{"UserName:",-15} {Environment.UserName}");
        Console.WriteLine($"{"SystemUptime:",-15} {GetSystemUptime()}");

        //Info GPU(Windows)
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Console.WriteLine($"{"Video-card:",-15} {GetGPUInfo()}");
        }
    }
    private void PrintSeparator()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n" + new string('═', 50));
        Console.ResetColor();
    }
    private void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {message}");
        Console.ResetColor();
    }


    private string GetTotalMemoryInGB()
    {
        return (GC.GetTotalMemory(false) / (1024 * 1024 * 1024.0)).ToString("0.00");
    }

    private string GetGPUInfo()
    {
        try
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return "N/A (Windows)";

            using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (var obj in searcher.Get())
            {
                return obj["Name"]?.ToString() ?? "Unknown";
            }
            return "Not detected";
        }
        catch
        {
            return "Data retrieval error";
        }
    }

    private string GetSystemUptime()
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetWindowsUptime();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return GetLinuxUptime();
            }
            else
            {
                return "N/A";
            }
        }
        catch
        {
            return "Data retrieval error";
        }
    }

    private string GetWindowsUptime()
    {
        using var uptime = new ManagementObject("Win32_OperatingSystem=@");
        uptime.Get();
        var lastBoot = ManagementDateTimeConverter.ToDateTime(uptime["LastBootUpTime"].ToString());
        return (DateTime.Now - lastBoot).ToString("dd'd 'hh'h 'mm'm'");
    }

    private string GetLinuxUptime()
    {
        var uptime = File.ReadAllText("/proc/uptime").Split(' ')[0];
        var seconds = double.Parse(uptime);
        return TimeSpan.FromSeconds(seconds).ToString("dd'd 'hh'h 'mm'm'");
    }
}