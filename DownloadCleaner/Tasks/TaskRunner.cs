using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;


namespace DownloadCleaner.Tasks
{
    public class TaskRunner
    {
        #if DEBUG
            private const int COOLDOWN = 30 * 1000;
        #else
            private const int COOLDOWN = 3 * 60 * 60 * 1000;
        #endif
      

        private List<Task> tasks;
        private Settings settings;
        private Serilog.Core.Logger logger;
        private bool shouldStop = false;
        private BackgroundWorker bg;
        private ManualResetEvent resetEvent;
        public TaskRunner()
        {
            bg = new BackgroundWorker();
            settings = Settings.GetInstance();
            logger = Logger.getInstance();
            resetEvent = new ManualResetEvent(false);
            tasks = new List<Task>();
        }

        public void Run()
        {
           bg = new BackgroundWorker();
           bg.DoWork += ((sender, args) =>
           {
              
               while (!shouldStop)
               {
                   var waitTill = DateTime.Now.AddMilliseconds(COOLDOWN);
                   RunTasks();
                   resetEvent.WaitOne(COOLDOWN);
               }
              
           });
           bg.RunWorkerCompleted += (sender, args) =>
           {
               if (args.Error != null)
               {
                   logger.Fatal("An exception occurded: {message} \n {stacktrace}",args.Error.Message,args.Error.StackTrace);
               }
               
               logger.Information("Background Thread Stopped");
           };
           bg.RunWorkerAsync();
        }
        
        private void RunTasks()
        {
            foreach (var task in tasks)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                
                logger.Information("running Task: [{taskName}]", task.taskName);
                task.RunTask();
                
                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000f;
                logger.Information("Task took: {time} seconds", elapsedSeconds );
                if(shouldStop) return;
            }

      
        }
        public void addTask(Task task)
        {
            logger.Information("added Task: [{taskName}]",task.taskName);
            tasks.Add(task);
        }

        public void Stop()
        { logger.Information("Shutting down");
           shouldStop = true;
           resetEvent.Set();
        }
        
    }
}