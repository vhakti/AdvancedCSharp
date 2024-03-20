using PipesUtil;

internal class Program
{
    static PipesHelper? pipeService1;
    static PipesHelper? pipeService2;
    const string PipeName1 = "pipe1";
    const string PipeName2 = "pipe2";

    static void Main(string[] args)
    {
        Task t1 = Task.Factory.StartNew(() => { CreateServer(); });
        char input = 'r';
       

        do
        {
            Console.WriteLine("1-Write M to send a message to client \r\n" +
               "2-Write Q to EXIT \r\n" +
               "********************");

            input = Console.ReadKey().KeyChar;
            if (input == 'M' || input == 'm')
            {
                CreateClient();
            }
            if (input == 'q' || input == 'Q')
            {
                Console.Write("\n Good bye!");
                Task.Delay(2);
                Environment.Exit(0);
            }
            else{
                Console.WriteLine("Invalid option");
            }
        } while (input != 'Q');
       
    }
    static void CreateServer()
    {
        try
        {
            while (true)
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
                    Console.WriteLine("Done!");
                }
            
            }
        }
        catch (System.IO.IOException ioExc)
        {
            Console.WriteLine(ioExc.ToString());
        }
        catch (System.Exception Exc)
        {
            Console.WriteLine(Exc.ToString());
        }
        finally
        {
            pipeService1?.Dispose();
            pipeService1 = null;
        }
    }
    static void CreateClient()
    {
            try
            {
                    if (pipeService2 == null)
                    {
                        pipeService2 = new PipesHelper(PipeName2, pipeType.client);

                        Console.Write("\r\n Attempting to connect to test pipe {0}...", PipeName1);
                        pipeService2?._namedPipeClientStream?.Connect(TimeSpan.FromSeconds(3));
                        Console.WriteLine("Client connected to pipe!, with {0} Instances.", pipeService2?._namedPipeClientStream?.NumberOfServerInstances);
                    }

                    Console.WriteLine("\n Write a message to the server:");
                    var message = Console.ReadLine();
                    if (message != null)
                    {
                        pipeService2?.sw?.Write(message);
                     
                        Console.WriteLine("\n Message:{0} sent to the server ", message);
                        pipeService2?.Dispose();
                        pipeService2 = null;

                    }
            }
        catch (System.TimeoutException te)
        {
            pipeService2 = null;
            Console.WriteLine("server is not available...");

        }
        catch (Exception exc)
        {
            pipeService2 = null;
            Console.WriteLine("General error:{0}", exc.Message);
        }

    }
}