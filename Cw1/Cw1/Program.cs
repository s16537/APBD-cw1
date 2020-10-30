using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();

            if (args.Length == 0)
            {
                throw new ArgumentNullException();
            }
            else if (!Uri.IsWellFormedUriString(args[0], UriKind.Absolute))
            {
                throw new ArgumentException("Niepoprawny adres podany jako argument.");
            }

            var response = await httpClient.GetAsync(args[0]);

            httpClient.Dispose();
            List<string> addresses = new List<string>();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contents = await response.Content.ReadAsStringAsync();

                Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
                MatchCollection emailMatches = emailRegex.Matches(contents);

                foreach (Match email in emailMatches)
                {
                    //Console.WriteLine(email.Value);
                    var mailData = email.Value;
                    if (!addresses.Contains(mailData))
                    {
                        addresses.Add(email.Value);
                    }
                }

                if(addresses.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono adresow email.");
                }
                else
                {
                    foreach(string address in addresses)
                    {
                        Console.WriteLine(address);
                    }
                }
            }
            else
            {
                Console.WriteLine("Blad w czasie pobierania strony.");
            }
        }
    }
}
