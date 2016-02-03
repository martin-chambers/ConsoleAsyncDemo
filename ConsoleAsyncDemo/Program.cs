using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsyncDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            Task t = MainAsync(args);
            t.Wait();
            Console.WriteLine("Finished. Press any key to continue ...");
            Console.ReadKey();
        }
        static async Task MainAsync(string[] args)
        {
            try
            {
                bool error = 
                    args.Length == 0 || 
                    args.Length > 1 || 
                    !(new string[] { "TRUE", "FALSE" }.Contains(args[0].ToUpper()));
                if (error)
                {
                    Console.WriteLine("");
                    Console.WriteLine("ConsoleAsyncDemo");
                    Console.WriteLine("________________");
                    Console.WriteLine("");
                    Console.WriteLine("USAGE: 'ConsoleAsyncDemo [True | False], " +
                        "where true means run in async mode and false means run in sync mode");
                    return;
                }
                bool Sync = (args[0].ToUpper() == "TRUE") ? true : false;
                // using the same instance of the client for each http request
                HttpClient client = new HttpClient();
                client.Timeout = new TimeSpan(0, 1, 0);
                DateTime start = DateTime.Now;
                TimeSpan span;
                if(Sync)
                {
                    // ASYCHRONOUS 
                    Console.WriteLine("");
                    Console.WriteLine("ASYNCHRONOUS OPERATIONS STARTING ...");
                    Console.WriteLine("");
                    Task<string> a = WaitForSiteStringAsync(client, "http://toyota.co.uk");
                    Task<string> b = WaitForSiteStringAsync(client, "http://bmw.co.uk");
                    Task<string> c = WaitForSiteStringAsync(client, "http://nissan.co.uk");
                    Task<string> d = WaitForSiteStringAsync(client, "http://ford.co.uk");
                    Task<string> e = WaitForSiteStringAsync(client, "http://jaguar.co.uk");
                    Console.WriteLine("");
                    Console.WriteLine("Awaiting ...");
                    Console.WriteLine("");
                    await a;
                    await b;
                    await c;
                    await d;
                    await e;
                    span = DateTime.Now - start;
                    Console.WriteLine("");
                    Console.WriteLine("All data is available after " + span.TotalMilliseconds.ToString() + " ms");
                    Console.WriteLine("");
                    Show("A - TOYOTA: ", a.Result);
                    Show("B - BMW: ", b.Result);
                    Show("C - NISSAN: ", c.Result);
                    Show("D - FORD: ", d.Result);
                    Show("E - JAGUAR: ", e.Result);
                    Console.WriteLine("");
                    Console.WriteLine("ASYNCHRONOUS OPERATIONS COMPLETE ...");
                    Console.WriteLine("");
                }
                else
                {
                    // SYCHRONOUS 
                    Console.WriteLine("");
                    Console.WriteLine("SYNCHRONOUS OPERATIONS STARTING ...");
                    Console.WriteLine("");
                    string a = WaitForSiteString(client, "http://toyota.co.uk");
                    string b = WaitForSiteString(client, "http://bmw.co.uk");
                    string c = WaitForSiteString(client, "http://nissan.co.uk");
                    string d = WaitForSiteString(client, "http://ford.co.uk");
                    string e = WaitForSiteString(client, "http://jaguar.co.uk");
                    span = DateTime.Now - start;
                    Console.WriteLine("");
                    Console.WriteLine("All data is available after " + span.TotalMilliseconds.ToString() + " ms");
                    Console.WriteLine("");
                    Show("A - TOYOTA: ", a);
                    Show("B - BMW: ", b);
                    Show("C - NISSAN: ", c);
                    Show("D - FORD: ", d);
                    Show("E - JAGUAR: ", e);
                    Console.WriteLine("");
                    Console.WriteLine("SYNCHRONOUS OPERATIONS COMPLETE ...");
                    Console.WriteLine("");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        static void Show(string s1, string s2)
        {
            Console.WriteLine(s1 + s2.Length.ToString() + " text length");
        }

        static async Task<string> WaitForSiteStringAsync(HttpClient client, string url)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("Starting load data from " + url);
            Task<string> ts = client.GetStringAsync(url);
            await ts;
            TimeSpan span = DateTime.Now - start;
            Console.WriteLine("Finished load data from " + url + " (" + span.TotalMilliseconds.ToString() + " ms duration)");
            return ts.Result;
        }
        static string WaitForSiteString(HttpClient client, string url)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("Starting load data from " + url);
            string s = client.GetStringAsync(url).Result;
            TimeSpan span = DateTime.Now - start;
            Console.WriteLine("Finished load data from " + url + " (" + span.TotalMilliseconds.ToString() + " ms duration)");
            return s;
        }
    }
}
