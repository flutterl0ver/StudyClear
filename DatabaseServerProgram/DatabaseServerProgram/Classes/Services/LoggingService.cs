namespace DatabaseServerProgram.Classes.Services;

public static class LoggingService {
    private static void EnsureLoggingFileExists() { }

    public static void Log(string message, ConsoleColor color = ConsoleColor.Gray) {
        EnsureLoggingFileExists();
        DateTime now = DateTime.Now;

        Console.Write($"[{now}] - ");

        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}