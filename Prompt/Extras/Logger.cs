using System;
using System.IO;


//Provides the erros to the "application.log" file so you can know what happen
public static class Logger
{
    private static readonly string logFilePath = "application.log";

    public static void Log(string message)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(logFilePath, true);
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log: {ex.Message}");
        }
    }

    public static void LogError(string errorMessage)
    {
        Log($"ERRO: {errorMessage}");
    }
}