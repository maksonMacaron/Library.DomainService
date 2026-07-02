using System;

namespace Library.DomainService.Adds
{
    public static class Logger
    {
        public static void Info(string text)
        {
            Console.WriteLine("[INFO] " + text);
        }

        public static void Success(string text)
        {
            Console.WriteLine("[OK] " + text);
        }

        public static void Error(string text)
        {
            Console.WriteLine("[ERROR] " + text);
        }
    }
}