# WVMC-Client

> ***<mark>Warning</mark>:*** *Feature incomplete and work in progress! Use at your own risk!*

### <u>What is the WVMC?</u>

The Windows-Virtual-Machine-Communicator *(short WVMC)* is a Toolset for sending pre-defined commands from a qemu-based
Windows-VM to a Linux Host. It consists of a Guest-side Client and a Host-side Server which communicate via a virtual Serial-port.

Link to the [Server Repository](https://github.com/Solek798/WVMC-Server).

### <u>How will the WVMC-Client operate?</u>

> *As mentioned above this is still work in progress. So I will describe how I plan it to work when it's done.*

The Client is responsible for scanning the guest vm's input and sending the equivalent command when a shortcut is 
pressed. The shortcuts are scanned for by a low-level-keyboard-hook to ensure even the windows-button can be used and 
no other program can block it. They are defined and saved on the guest via the Clients UI. On Startup the Client is 
Started as Background process and connects to a pre-defined serial-port.

> *The name of the port will also be saved in a config file*

> *Note that the WVMC as a whole needs a virtual serial-port defined in the vm.xml to operate.*

The Client is divided in three parts:

- The service which always runs in the background
- The low-level-keyboard-hook that listens to system input
- The UI with which the shortcut are configured

The Service starts the LLKH and the UI as separate processes. These communicate with the Service via anonymous-pipes.

> *LLKH being a separate program is due to Windows-LLKH's only work with valid function-pointers (therefore C# won't
> do).    
> The UI being separate is more of a design decision. I wasn't comfortable starting a WPF-App (or a WinFrom-App) from 
> inside a Windows-background-process.*

The UI is started as windows-less WPF-App with a NotifyIcon. Via this Icon the user can open the settings where the 
shortcuts are configured.

> *The user should be able to deactivate at least the input scanning. Whether or not he should beable to deactivate the 
> whole client is something that needs to be tested.*

### <u>Missing Features</u>

- [ ] configuring shortcuts via UI
- [ ] saving shortcuts to .conf file
- [ ] ability to disable client
- [ ] communicating with the server
- [ ] sending commands to the server


