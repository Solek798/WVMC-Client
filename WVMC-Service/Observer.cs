using System.IO.Ports;
using System.Diagnostics;
using System.Text;
using System.IO.Pipes;

namespace WVMC_Service
{
    public class Observer
    {
        private NotifyIcon _notifyIcon;

        private readonly byte[] _header;
        private SerialPort _port;
        private IntPtr _hookHandle;

        private Process _hook;
        private AnonymousPipeServerStream _anonymousPipeServerStream;

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
            
            _anonymousPipeServerStream = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
            
            _hook = new Process();
            _hook.StartInfo.FileName = "..\\WVMC-LowLevelKeyboardHook.exe";
            _hook.StartInfo.Arguments = _anonymousPipeServerStream.GetClientHandleAsString();
            _hook.StartInfo.UseShellExecute = false;
            _hook.Start();
            
            _anonymousPipeServerStream.DisposeLocalCopyOfClientHandle();
            _anonymousPipeServerStream.Write(Encoding.UTF8.GetBytes("test"));

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "Test Icon!";
            _notifyIcon.Icon = new Icon(SystemIcons.Exclamation, 40, 40);
            _notifyIcon.Visible = true;
        }

        public void Stop()
        {
            /*var buffer = new byte[8];
            var header = Encoding.UTF8.GetBytes("vmk:");
            var message = BitConverter.GetBytes(-1);
            
            Buffer.BlockCopy(header, 0, buffer, 0, header.Length);
            Buffer.BlockCopy(message, 0, buffer, header.Length, message.Length);

            _port.Write(buffer, 0, buffer.Length);*/
            Console.WriteLine("Stop");

            _hook.Close();
            _notifyIcon.Dispose();
            _anonymousPipeServerStream.Dispose();
            
            //_port.Close();
        }
    }
}