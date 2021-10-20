using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
      

        private readonly List<Task> tasks;

        private readonly Serilog.Core.Logger logger;
        private bool shouldStop = false;
        private BackgroundWorker bg;
        private readonly ManualResetEvent resetEvent;
        public TaskRunner()
        {
            bg = new BackgroundWorker();
            logger = Logger.GetInstance();
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
                   RunTasks();
                   resetEvent.WaitOne(COOLDOWN);
               }
              
           });
           bg.RunWorkerCompleted += (sender, args) =>
           {
               if (args.Error != null)
               {
                   logger.Fatal("An exception occurded: {message} \n {stacktrace}",args.Error.Message,args.Error.StackTrace);
                   Process.GetCurrentProcess().Kill();
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
                
                logger.Information("running Task: [{taskName}]", task.GetTaskName());
                task.RunTask();
                
                watch.Stop();
                var elapsedSeconds = watch.ElapsedMilliseconds / 1000f;
                logger.Information("Task took: {time} seconds", elapsedSeconds );
                if(shouldStop) return;
            }

      
        }
        public void addTask(Task task)
        {
            logger.Information("added Task: [{taskName}]",task.GetTaskName());
            tasks.Add(task);
        }

        public void Stop()
        { 
            logger.Information("Shutting down");
            shouldStop = true;
            resetEvent.Set();
        }
        
    }
}