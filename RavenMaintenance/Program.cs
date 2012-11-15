using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Hosting.Self;
using Nancy;
using RavenLib;

namespace RavenMaintenance
{
    class Program
    {
        static void Main(string[] args)
        {
            var queryDocModule = new QueryDocsModule();

            var host = new NancyHost(new Uri("http://localhost:2112"));
            host.Start();
            Console.WriteLine("Server running ...");

            Console.ReadKey();
            Console.WriteLine("Server shutting down ...");
            host.Stop();

        }
    }
}
