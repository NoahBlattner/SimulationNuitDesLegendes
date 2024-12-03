using System.Text;

namespace SimulationNuitDesLegendes;

public static class Logger
{
    public static StringBuilder LogSb = new StringBuilder();

    public static void Log(string logString)
    {
        LogSb.AppendLine(logString);
    }
    
    public static void ClearLog()
    {
        LogSb.Clear();
    }
    
    public static void SplitLog(int spliterSize = 1)
    {
        for (int i = 0; i < spliterSize; i++)
        {
            LogSb.AppendLine("--------------------------------------------------");
        }
    }

    public static string GetLog()
    {
        return LogSb.ToString();
    }

    public static void SaveLogFile(string name)
    {
        var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        
        // Create .txt file
        File.CreateText(projectDirectory + "/Logs/" + name + ".txt").Write(LogSb);
    }
}