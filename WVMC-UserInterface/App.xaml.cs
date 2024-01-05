using System;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Forms = System.Windows.Forms;


namespace WVMC_UserInterface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Forms.NotifyIcon _notifyIcon;
        
        private AnonymousPipeClientStream _stopPipe;
        private StreamReader _stopReader;
        private AnonymousPipeClientStream _syncPipe;
        private StreamWriter _syncWriter;
        
        
        protected override void OnStartup(StartupEventArgs e)
        {
            var syncHandle = e.Args[0];
            var stopHandle = e.Args[1];
            
            _syncPipe = new AnonymousPipeClientStream(PipeDirection.Out, syncHandle);
            _syncWriter = new StreamWriter(_syncPipe);
            _stopPipe = new AnonymousPipeClientStream(PipeDirection.In, stopHandle);
            _stopReader = new StreamReader(_stopPipe);
            
            _notifyIcon = new Forms.NotifyIcon();
            _notifyIcon.Text = "Handle 1: " + syncHandle + " | Handle 2: " + stopHandle;
            _notifyIcon.Icon = new Icon(SystemIcons.Exclamation, 40, 40);
            _notifyIcon.Click += Magic;
            _notifyIcon.Visible = true;
            

            Task.Run(ListenToService);
            
            base.OnStartup(e);
        }

        private void ListenToService()
        {
            while (true)
            {
                var input = _stopReader.ReadLine();

                if (input == "stop")
                    break;
            }
            
            Dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
        }

        // Demo Method to manually Shutdown App if necessary
        public void Magic(object? o, EventArgs e)
        {
            Shutdown();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            
            base.OnExit(e);
        }
        
    }
}