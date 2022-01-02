using System;
using System.Drawing;
using System.IO.Ports;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.IO.Pipes;

namespace WVMC_Service
{
    public class Observer
    {
        private NotifyIcon _notifyIcon;

        private readonly byte[] _header;
        private SerialPort _port;
        private IntPtr _hookHandle;
        private AnonymousPipeServerStream _anonymusPipeServerStream;

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
            /*using (var currentProcess = Process.GetCurrentProcess())
            using (var currentModule = currentProcess.MainModule)
            {
                var id = WinSDKImports.GetCurrentThreadId();
                var type = (int) WinSDKImports.HookType.WH_KEYBOARD_LL;
                
                _hookHandle = WinSDKImports.SetWindowsHookEx(
                    type, 
                    _proc, 
                    WinSDKImports.GetModuleHandle(currentModule?.ModuleName), 
                    id);

                if (_hookHandle == IntPtr.Zero)
                {
                    Console.WriteLine("Error: " + Marshal.GetLastWin32Error());
                }
            }*/
            Process process = new Process();
            process.StartInfo.FileName = "..\\WVMC-LowLevelKeyboardHook.exe";
            process.StartInfo.UseShellExecute = false;
            process.Start();

            //_anonymusPipeServerStream = new AnonymousPipeServerStream(PipeDirection.Out, System.IO.HandleInheritability.Inheritable);
            //_anonymusPipeServerStream.


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

            //WinSDKImports.UnhookWindowsHookEx(_hookHandle);
            
            _notifyIcon.Dispose();
            
            //_port.Close();
        }
    }
}