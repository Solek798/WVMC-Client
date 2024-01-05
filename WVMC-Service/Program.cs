using Topshelf;
using WVMC_OptionsMenu;
using WVMC_UserInterface;


namespace WVMC_Service
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var service = new Service();
            
            service.Start();

            Console.ReadLine();
            
            await service.Stop();

            
            /*var errorCode = HostFactory.Run(host =>
            {
                host.Service<Observer>(service =>
            {
                    service.ConstructUsing(observer => new Observer());
                    service.WhenStarted(observer => observer.Start());
                    service.WhenStopped(observer => observer.Stop());
                });

                host.RunAsLocalSystem();
                
                host.SetServiceName("VMK-Client");
                host.SetDisplayName("VMK-Client");
                host.SetDescription("Client of the VK Communicator");
            });*/

            
            Console.WriteLine("Hello World");
        }
    }
}