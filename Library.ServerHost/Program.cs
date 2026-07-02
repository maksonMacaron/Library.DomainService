using System;
using System.ServiceModel;
using Library.DomainService.Contracts;
using Library.DomainService.Impl;

namespace Library.ServerHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string serviceAddress = "127.0.0.1:8080";
            var binding = new NetTcpBinding();

            Console.WriteLine($"Starting host on net.tcp://{serviceAddress}");

            // AuthService
            var authServiceHost = new ServiceHost(
                typeof(AuthService),
                new Uri($"net.tcp://{serviceAddress}/authService"));

            authServiceHost.AddServiceEndpoint(
                typeof(IAuthService),
                binding,
                "");

            // BookService
            var bookServiceHost = new ServiceHost(
                typeof(BookService),
                new Uri($"net.tcp://{serviceAddress}/bookService"));

            bookServiceHost.AddServiceEndpoint(
                typeof(IBookService),
                binding,
                "");

            // ReaderService
            var readerServiceHost = new ServiceHost(
                typeof(ReaderService),
                new Uri($"net.tcp://{serviceAddress}/readerService"));

            readerServiceHost.AddServiceEndpoint(
                typeof(IReaderService),
                binding,
                "");

            // LoanService
            var loanServiceHost = new ServiceHost(
                typeof(LoanService),
                new Uri($"net.tcp://{serviceAddress}/loanService"));

            loanServiceHost.AddServiceEndpoint(
                typeof(ILoanService),
                binding,
                "");

            // DatabaseService
            var databaseServiceHost = new ServiceHost(
                typeof(DatabaseService),
                new Uri($"net.tcp://{serviceAddress}/databaseService"));

            databaseServiceHost.AddServiceEndpoint(
                typeof(IDatabaseService),
                binding,
                "");

            try
            {
                authServiceHost.Open();
                Console.WriteLine("AuthService started.");

                bookServiceHost.Open();
                Console.WriteLine("BookService started.");

                readerServiceHost.Open();
                Console.WriteLine("ReaderService started.");

                loanServiceHost.Open();
                Console.WriteLine("LoanService started.");

                databaseServiceHost.Open();
                Console.WriteLine("DatabaseService started.");

                Console.WriteLine();
                Console.WriteLine("=================================");
                Console.WriteLine(" All WCF services are running.");
                Console.WriteLine("=================================");
                Console.WriteLine();

                Console.WriteLine("Press ENTER to stop server...");
                Console.ReadLine();
            }
            finally
            {
                CloseHost(authServiceHost);
                CloseHost(bookServiceHost);
                CloseHost(readerServiceHost);
                CloseHost(loanServiceHost);
                CloseHost(databaseServiceHost);

                Console.WriteLine("All services stopped.");
            }
        }

        private static void CloseHost(ServiceHost host)
        {
            if (host == null)
                return;

            try
            {
                if (host.State == CommunicationState.Opened)
                    host.Close();
                else
                    host.Abort();
            }
            catch
            {
                host.Abort();
            }
        }
    }
}