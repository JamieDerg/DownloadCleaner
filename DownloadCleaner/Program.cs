using Topshelf;

namespace DownloadCleaner
{
    class Program
    {
        
        static void Main(string[] args)
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
                x.SetServiceName("DovnloadCleaner");
                x.OnException(exception =>
                {
                    Logger.getInstance().Fatal("{message} \n {stacktrace}",exception.Message,exception.StackTrace);
                });
            });


        }
        
        

    }
    
   
    
}