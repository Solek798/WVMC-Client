using System.IO.Ports;
using System.Diagnostics;
using System.Text;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace WVMC_Service
{
    
    
    public class Observer
    {
        private NotifyIcon _notifyIcon;

        private readonly byte[] _header;
        private SerialPort _port;
        private IntPtr _hookHandle;

        private Process _hook;
        private AnonymousPipeServerStream _inPipe;
        private AnonymousPipeServerStream _outPipe;

        private StreamReader _listener;
        private Task _listenTask;
        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(uint threadId, uint msg, UIntPtr wParam, IntPtr lParam);
        
        private const uint WM_QUIT = 0x0012;

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
            //_outPipe = new AnonymousPipeServerStream(PipeDirection.O, HandleInheritability.Inheritable);
            
            _hook = new Process();
            _hook.StartInfo.FileName = "..\\WVMC-LowLevelKeyboardHook.exe";
            _hook.StartInfo.Arguments = _inPipe.GetClientHandleAsString();
            _hook.StartInfo.UseShellExecute = false;
            _hook.Start();
            

            //Console.WriteLine(_hook.MainWindowHandle);
            
            _inPipe.DisposeLocalCopyOfClientHandle();
            //_anonymousPipeServerStream.Write(Encoding.UTF8.GetBytes("test"));
            //_anonymousPipeServerStream.ReadMode

            _listener = new StreamReader(_inPipe);
            _listenTask = Task.Run(Listen);


            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "Test Icon!";
            _notifyIcon.Icon = new Icon(SystemIcons.Exclamation, 40, 40);
            _notifyIcon.Visible = true;
        }

        private void Listen()
        {
            /*while (true)
            {
                var message = _listener.Read();
                Console.WriteLine("Message: " + message);
            }*/
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

            _listenTask.Dispose();
            _listener.Dispose();
            _inPipe.Dispose();
            
            PostThreadMessage((uint) _hook.Threads[0].Id, WM_QUIT, UIntPtr.Zero, IntPtr.Zero);
            await _hook.WaitForExitAsync();
            //_hook.Close();
            //_hook.Kill();
            //_hook.CloseMainWindow();
            _notifyIcon.Dispose();
            
            
            //_port.Close();
        }

        private void SendExitMessageToHook()
        {
            
        }
    }
}