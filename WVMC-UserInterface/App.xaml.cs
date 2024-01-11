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
        private Forms.ToolStripItem _observerSwitchItem;
        private Bitmap _disableIcon;
        private Bitmap _enableIcon;
        
        private AnonymousPipeClientStream _stopPipe;
        private StreamReader _stopReader;
        private AnonymousPipeClientStream _syncPipe;
        private StreamWriter _syncWriter;

        private bool _isObserverEnabled;


        public App()
        {
            _isObserverEnabled = true;
        }
        
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
            _notifyIcon.Icon = new Icon("Resources\\Icon.ico", 64, 64);
            _notifyIcon.Visible = true;
            
            var menuStrip = new Forms.ContextMenuStrip();
            menuStrip.Items.Add("Options", new Bitmap("Resources\\Settings.ico"), OpenOptions);

            _disableIcon = new Bitmap("Resources\\Cross.ico");
            _enableIcon = new Bitmap("Resources\\Check.ico");
            _observerSwitchItem = menuStrip.Items.Add("Disable Scanner", _disableIcon, DisableScanner);
            
            menuStrip.Items.Add(new Forms.ToolStripSeparator());
            menuStrip.Items.Add("Shutdown", new Bitmap("Resources\\Power.ico"), ManuallyShutdown);
            _notifyIcon.ContextMenuStrip = menuStrip;
            

            Task.Run(ListenToService);
            
            base.OnStartup(e);
        }
        
        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            
            base.OnExit(e);
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

        private void DisableScanner(object? o, EventArgs e)
        {
            if (_isObserverEnabled)
            {
                _observerSwitchItem.Image = _enableIcon;
                _observerSwitchItem.Text = "Enable Scanner";
            }
            else
            {
                _observerSwitchItem.Image = _disableIcon;
                _observerSwitchItem.Text = "Disable Scanner";
            }

            _isObserverEnabled = !_isObserverEnabled;
        }

        private void OpenOptions(object? o, EventArgs e)
        {
            
        }

        private void ManuallyShutdown(object? o, EventArgs e)
        {
            const string message = "Are you sure you want to shutdown?\nThis will cause the Host daemon to shutdown as well!";
            var result = MessageBox.Show(message, "Are you sure?", MessageBoxButton.YesNo);
            
            if (result == MessageBoxResult.Yes)
                Shutdown();
        }
    }
}