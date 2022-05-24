using Topshelf;

namespace WVMC_Service
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var observer = new Observer();
            
            observer.Start();

            Console.ReadLine();
            
            observer.Stop();

            /*
            var errorCode = HostFactory.Run(host =>
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