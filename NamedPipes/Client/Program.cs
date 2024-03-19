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

        Console.Read();
    }


    static void Main2(string[] args)
    {
        Task.Delay(4000);
        using (NamedPipeClientStream pipeClient =
             new NamedPipeClientStream(".", "testpipe", PipeDirection.InOut))
        {

            // Connect to the pipe or wait until the pipe is available.
            Console.Write("Attempting to connect to pipe...");
            pipeClient.Connect();

            Console.WriteLine("Connected to pipe.");
            Console.WriteLine("There are currently {0} pipe server instances open.",
               pipeClient.NumberOfServerInstances);


                char option = 'x';
                do
                {
                    if (pipeClient.IsConnected)
                     {
                        StreamWriter wr = new StreamWriter(pipeClient);
                       // wr.AutoFlush = true;
                        Console.WriteLine("Send a message to the server:");


                        var message = Console.ReadLine();
                        if (message != null)
                        {
                       
                                wr.Write(message);
                                wr.Dispose();
                                Console.WriteLine("Message:{0} sent to the server", message);
                             
                                //using (StreamReader sr = new StreamReader(pipeClient))
                                //{
                                //    // Display the read text to the console
                                //    string temp;
                                //    while ((temp = sr.ReadLine()) != null)
                                //    {
                                //        Console.WriteLine("Received from server: {0}", temp);
                                //    }
                                //}

                        }

                }
                {
                    // pipeClient.Connect(option);
                    Console.WriteLine("Disconnected from server ...");

                }
                 Console.WriteLine("Enter Q to quit.\n");
                 option = Console.ReadKey().KeyChar;

            } while (option != 'Q');
            

        }
        Console.Write("Press Enter to continue...\n");
        Console.ReadLine();
    }
}