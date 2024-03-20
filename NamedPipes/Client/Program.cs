using PipesUtil;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.CompilerServices;

internal class Program
{
    static PipesHelper? pipeService1;
    static PipesHelper? pipeService2;
    const string PipeName1 = "pipe2";
    const string PipeName2 = "pipe1";

    static void Main(string[] args)
    {
        Task t1 = Task.Factory.StartNew(() => { CreateServer(); });
        char input = 'r';
        Console.WriteLine("Write M to send a message to client \r\n");
        Console.WriteLine("Write Q to EXIT");

        do
        {
            input = Console.ReadKey().KeyChar;
            if (input == 'M' || input == 'm')
            {
                CreateClient();
            }
            if (input == 'q' || input == 'Q')
            {
                Environment.Exit(0);
            }
        } while (input != 'Q');




    }
    static void CreateServer()
    {


        while (true)
        {
            try
            {
                if (pipeService1 == null)
                    pipeService1 = new PipesHelper(PipeName1, pipeType.server);

                using (var pipe = pipeService1._namedPipeServerStream)
                {
                    Console.WriteLine("waiting for connections ...");
                    pipe?.WaitForConnection();

                    Console.WriteLine("One client is connected!");
                    using (var reader = pipeService1.sr)
                    {

                        string temp = reader.ReadLine();
                        if (temp != null)
                        {
                            Console.WriteLine(temp);

                        }
                    }

                    Console.WriteLine(pipe?.IsConnected);

                }
                pipeService1.Dispose();
                pipeService1 = null;
            }
            catch (System.IO.IOException ioExc)
            {
                Console.WriteLine(ioExc.ToString());
            }
            catch (System.Exception Exc)
            {
                Console.WriteLine(Exc.ToString());
            }
        }

    }
    static void CreateClient()
    {
        try
        {
            if (pipeService2 == null)
            {
                pipeService2 = new PipesHelper(PipeName2, pipeType.client);

                Console.Write("Attempting to connect to test pipe {0} press Q to exit...", PipeName1);
                pipeService2?._namedPipeClientStream?.Connect(TimeSpan.FromSeconds(3));
                Console.WriteLine("Client connected to pipe!, with {0} Instances.", pipeService2?._namedPipeClientStream?.NumberOfServerInstances);
            }

            Console.WriteLine("\n Write a message to the server:");
            var message = Console.ReadLine();
            if (message != null)
            {
                pipeService2?.sw?.Write(message);
                pipeService2?.Dispose();
                pipeService2 = null;
                Console.WriteLine("\n Message:{0} sent to the server ", message);
                

            }


        }
        catch (System.TimeoutException te)
        {
            pipeService2 = null;
            Console.WriteLine("server is not available...");

        }

    }



}