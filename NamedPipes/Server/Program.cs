using PipesUtil;
using System.IO;
using System.IO.Pipes;

internal class Program
{
    static PipesHelper? pipeService;
 
    static void Main(string[] args)
    {
        try
        {
            const string pipeName = "testpipe";
            while (true)
            {
                pipeService = new PipesHelper(pipeName, pipeType.server);
                using (var pipe = pipeService._namedPipeServerStream)
                {
                    Console.WriteLine("waiting for connections ...");
                    pipe?.WaitForConnection();

                    Console.WriteLine("One client is connected!");
                    using (var reader = pipeService.sr)
                    {

                        string temp = reader.ReadLine();
                        if (temp != null)
                        {
                            Console.WriteLine(temp);

                        }
                    }

                    Console.WriteLine(pipe?.IsConnected);
                   
                }
                pipeService.Dispose();
            }
        }catch(System.IO.IOException ioExc)
        {
            Console.WriteLine(ioExc.ToString());
        }
        catch(System.Exception Exc)
        {
            Console.WriteLine(Exc.ToString());
        }
    }

    static void NoMain(string[] args)
    {
        using (NamedPipeServerStream pipeServer =
             new NamedPipeServerStream("testpipe", PipeDirection.InOut))
        {
            Console.WriteLine("NamedPipeServerStream object created.");

            // Wait for a client to connect
            Console.Write("Waiting for client connection...");
            pipeServer.WaitForConnection();

            Console.WriteLine("\n Client connected.");
            char option;

            using (StreamReader sr = new StreamReader(pipeServer))
            {
                do
                {
                try
                {

                        // Display the read text to the console
                        string temp = sr.ReadLine();
                        if(temp != null)
                        {

                            Console.WriteLine("Received from client: {0}", temp);
                        }

                        // Read user input and send that to the client process.
                        //using (StreamWriter sw = new StreamWriter(pipeServer))
                        //{
                        //    sw.AutoFlush = true;
                        //    Console.Write("Enter text: ");
                        //    sw.WriteLine(Console.ReadLine());
                        //}
                    

                   
                }
                // Catch the IOException that is raised if the pipe is broken
                // or disconnected.
                catch (IOException e)
                {
                    Console.WriteLine("ERROR: {0}", e.Message);
                    break;
                }

                Console.WriteLine("Enter Q to quit.");
                option = Console.ReadKey().KeyChar;
                    if(option =='Q')
                        break;

            } while (true);
          }
        }
    }
}