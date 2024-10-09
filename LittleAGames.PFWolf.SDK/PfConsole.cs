using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleAGames.PFWolf.SDK;

/// <summary>
/// The console that keeps a log of all of the messages
/// </summary>
public class PfConsole
{
    //List<string> messages = new List<string>();

    public static void Log(string message)
    {
        //messages.Add(message);
        Console.WriteLine(message);
    }
}
