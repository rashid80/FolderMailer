using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class ConsoleLogger
{
    public static void Error(string message)
    {
        ConsoleWriteLineWithColor(message, ConsoleColor.Red);
    }

    public static void Success(string message)
    {
        ConsoleWriteLineWithColor(message, ConsoleColor.Green);
    }

    public static void Debug(string message)
    {
        Console.ResetColor();
        Console.WriteLine(message);
    }

    private static void ConsoleWriteLineWithColor(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
