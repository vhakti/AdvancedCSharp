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

   
}