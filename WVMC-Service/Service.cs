using System.Diagnostics;
using System.IO.Pipes;
using StreamReader = System.IO.StreamReader;

namespace WVMC_Service;

public class Service
{
    protected Observer _observer;

    private readonly AnonymousPipeServerStream _syncPipe;
    private readonly StreamReader _syncPipeReader;
    private readonly AnonymousPipeServerStream _stopPipe;
    private readonly StreamWriter _stopWriter;
    
    private readonly Process _uiProcess;

    private Task _uiListener;
    
    public Service()
    {
        //_observer = new Observer();
        
        _syncPipe = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);
        _syncPipeReader = new StreamReader(_syncPipe);
        _stopPipe = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
        _stopWriter = new StreamWriter(_stopPipe);
        
        _uiProcess = new Process();
    }

    public void Start()
    {
        //_observer.Start();

        var uiArguments = _syncPipe.GetClientHandleAsString() + " " + _stopPipe.GetClientHandleAsString();
        
        _uiProcess.StartInfo.FileName = "WVMC-UserInterface.exe";
        _uiProcess.StartInfo.Arguments = uiArguments;
        _uiProcess.StartInfo.UseShellExecute = false;
        
        _uiProcess.Start();

        //_uiListener = Task.Run(ListenToUI);
    }

    private void ListenToUI()
    {
        using var reader = new StreamReader(_stopPipe);
        
        while (true)
        {
            var input = reader.ReadLine();
                
            if (input == null)
                continue;

            if (input == "continue")
            {
            } // TODO(FK): add logic for rebinding shortcuts

            if (input == "pause")
            {
                //TODO(FK): add logic for pausing Observer
            }
        }
    }

    private void StopUserInterface()
    {
        _stopWriter.WriteLine("stop");
        _stopWriter.Flush();
    }

    public async Task Stop()
    {
        //await _observer.Stop();

        StopUserInterface();
        
        Console.WriteLine("Now Waiting...");
        
        await _uiProcess.WaitForExitAsync();
        _uiProcess.Dispose();

        Console.WriteLine("Stopped UI...");
        
        await _stopWriter.DisposeAsync();
        _syncPipeReader.Dispose();
        
        _stopPipe.Close();
        await _stopPipe.DisposeAsync();
        
        _syncPipe.Close();
        await _syncPipe.DisposeAsync();

        Console.WriteLine("Stop has ended");
    }
}