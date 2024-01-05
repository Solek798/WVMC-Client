using System.IO.Ports;
using System.Diagnostics;
using System.Text;
using System.IO.Pipes;
using System.Windows.Input;

namespace WVMC_Service
{
    public class Observer
    {
        private NotifyIcon _notifyIcon;

        private readonly byte[] _header;
        private SerialPort _port;

        private Process _hook;
        
        private AnonymousPipeServerStream _inPipe;

        private Task _listenTask;

        public Observer()
        {
            _header = Encoding.UTF8.GetBytes("vmk:");
            _port = new SerialPort("COM2");
        }

        private void Write(byte message)
        {
            _port.Write(_header, 0, 1);
            _port.Write(_header, 1, 1);
            _port.Write(_header, 2, 1);
            _port.Write(_header, 3, 1);
            _port.Write(new[] {message} , 0, 1);
            
            Console.WriteLine(message);
        }


        public void Start()
        {
            //_port.Open();
            
            _inPipe = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);
            
            
            Console.WriteLine(_inPipe.IsAsync);
            
            _hook = new Process();
            _hook.StartInfo.FileName = "..\\WVMC-LowLevelKeyboardHook.exe";
            _hook.StartInfo.Arguments = _inPipe.GetClientHandleAsString();
            _hook.StartInfo.UseShellExecute = false;
            _hook.Start();

            _inPipe.DisposeLocalCopyOfClientHandle();
            
            _listenTask = Task.Run(Listen);

            
        }

        private void Listen()
        {
            var buffer = new byte[4];
            
            while (true)
            {
                var count = _inPipe.Read(buffer);

                // Check if Quit confirmation is received...
                if (count == 1 && buffer[0] == 0)
                    // ...if it is, end the Task
                    break;
                    
                if (count != 4)
                    continue;

                var message = BitConverter.ToInt32(buffer);

                Console.WriteLine("Message: " + message);
            }
            
            Console.WriteLine("Listener Stops...");
        }

        public async Task Stop()
        {
            /*var buffer = new byte[8];
            var header = Encoding.UTF8.GetBytes("vmk:");
            var message = BitConverter.GetBytes(-1);
            
            Buffer.BlockCopy(header, 0, buffer, 0, header.Length);
            Buffer.BlockCopy(message, 0, buffer, header.Length, message.Length);

            _port.Write(buffer, 0, buffer.Length);*/
            Console.WriteLine("Stop");

            // Stop LowLevelKeyboardHook ...
            SendExitMessageToHook();
            await _hook.WaitForExitAsync();
            _hook.Dispose();
            
            // ... which ultimately stops Listener Task
            await _listenTask.WaitAsync(CancellationToken.None);
            
            // Close Pipe
            _inPipe.Close();
            await _inPipe.DisposeAsync();
            
            // Shutdown NotifyIcon
            _notifyIcon.Dispose();
            
            
            //_port.Close();
        }

        private void SendExitMessageToHook()
        {
            DllImports.PostThreadMessage(
                (uint) _hook.Threads[0].Id, 
                DllImports.WM_QUIT, 
                UIntPtr.Zero, 
                IntPtr.Zero);
        }
    }
}