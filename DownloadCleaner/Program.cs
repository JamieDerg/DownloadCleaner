using Topshelf;

namespace DownloadCleaner
{
    internal static class Program
    {
        
        public static void Main(string[] args)
        {

            HostFactory.Run(x =>
            {
                x.Service<Startup>(s =>
                {
                    s.ConstructUsing(name => new Startup());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tx => tx.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Service that cleans your download folder");
                x.SetDisplayName("Download Cleaner Service");
                x.SetServiceName("DownloadCleaner");
                x.OnException(exception =>
                {
                    Logger.GetInstance().Fatal("{message} \n {stacktrace}",exception.Message,exception.StackTrace);
                });
            });


        }
        
        

    }
    
   
    
}