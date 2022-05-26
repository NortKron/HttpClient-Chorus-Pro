using System;
using System.Threading;

namespace TestChorusPro
{
    class Program
    {
        private static string SMB_DELIMITER = "|";

        static void Main(string[] args)
        {
            string pathFileXML = "C:\\Users\\Admin\\source\\repos\\TestChorusPro\\Sample Invoices\\out.Admin_02.xml"

            Console.WriteLine("0. Start");

            ChrorusProClient client = new ChrorusProClient();
            var token  = client.GetToken();

            Console.WriteLine("1. Token : " + token.AccessToken);

            FlowDeposit flow = client.SubmitUBL(token, pathFileXML);

            Console.WriteLine($"2. Invoice sent. Flow Deposit number : {flow.NumeroFluxDepot}");

            Console.WriteLine("Sleep on 30 seconds");
            Thread.Sleep(30000);

            FlowDetail detail = client.CheckUBL(flow, token);

            while (detail.CodeRetour != 0)
            {
                Console.WriteLine("Stream not found. Retry after 30 seconds");
                Thread.Sleep(30000);

                detail = client.CheckUBL(flow, token);
            }

            Console.WriteLine($"3. Check result\nFlow Deposit number : {flow.NumeroFluxDepot}"
                   + $"\nStatus code : {detail.EtatCourantDepotFlux}");

            if (detail.ListeErreurDP != null)
            {
                Console.WriteLine("\nErrors from ListeErreurDP");

                for (int i=0; i<detail.ListeErreurDP.Count; i++)
                {
                    if (detail.ListeErreurDP[i].LibelleErreurDP.Contains(SMB_DELIMITER))
                    {
                        Console.WriteLine($"{i + 1}. Details:");
                        string[] words = detail.ListeErreurDP[i].LibelleErreurDP.Split(SMB_DELIMITER);

                        foreach (var word in words)
                        {
                            Console.WriteLine($" - {word}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{i + 1}. Details: {detail.ListeErreurDP[i].LibelleErreurDP}");
                    }
                }
            }

            if (detail.ListeErreurTechnique != null)
            {
                Console.WriteLine("\nErrors from ListeErreurTechnique");

                for (int i = 0; i < detail.ListeErreurTechnique.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Details: {detail.ListeErreurTechnique[0].LibelleErreur}");
                }
            }
        }
    }
}
