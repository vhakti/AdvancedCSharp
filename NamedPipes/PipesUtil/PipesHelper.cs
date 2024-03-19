using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using System.Runtime.CompilerServices;

namespace PipesUtil
{
    public  enum pipeType
    {
        server,
        client
    }
    public class PipesHelper : IDisposable
    {
        public NamedPipeServerStream? _namedPipeServerStream { get; set; }
        public NamedPipeClientStream? _namedPipeClientStream { get; set; }
        public StreamReader? sr { get; private set; } 

        public StreamWriter? sw { get;private set; }

        public PipesHelper(string namePipe, pipeType type)
        {
            switch (type)
            {
                case pipeType.server:
                    _namedPipeServerStream = new NamedPipeServerStream(namePipe, PipeDirection.InOut);
                    sr = new StreamReader(_namedPipeServerStream);
                    break;
                case pipeType.client:
                    _namedPipeClientStream = new NamedPipeClientStream(".",namePipe, PipeDirection.InOut);
                    sw = new StreamWriter(_namedPipeClientStream);
                    break;
            };
        }
        
        public void Dispose()
        {
            _namedPipeServerStream?.Dispose();
            sr?.Dispose();
            sw?.Dispose();
        }
    }
}
