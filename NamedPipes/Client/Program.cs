using PipesUtil;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.CompilerServices;

internal class Program
{
    static PipesHelper? pipeService;
    const string PipeName = "testpipe";
    static void Main(string[] args)
    {

        while(true) {
            if (pipeService == null)
            {
                pipeService = new PipesHelper(PipeName, pipeType.client);

                Console.Write("Attempting to connect to test pipe...");
                pipeService?._namedPipeClientStream?.Connect();
            }
            Console.WriteLine("Client connected to pipe!, with {0} Instances.",pipeService?._namedPipeClientStream?.NumberOfServerInstances);

            Console.WriteLine("Enter M and press Enter to send a message.");
            Console.WriteLine("Enter Q to exit.");
            char input = Console.ReadKey().KeyChar;
            
            if(input == 'Q')
            {
                pipeService?.Dispose();
                break;
                
            }
            if (input == 'M')
            {
                Console.WriteLine("\n Write a message to the server:");
                var message = Console.ReadLine();
                if (message != null)
                {
                    pipeService?.sw?.Write(message);
                    pipeService?.Dispose();
                    pipeService = null;
                    Console.WriteLine("\n Message:{0} sent to the server ", message);

                }
            }
            else
            {
                Console.WriteLine("\n Invalid option.");
            }


        }
        System.Environment.Exit(1);
    }
}