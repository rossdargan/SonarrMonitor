using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorFolders
{
    using MonitorFolders.Series;

    using Topshelf;

    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                
            {
                x.Service<Monitor>(s =>                       
                {
                    s.ConstructUsing(name => new Monitor(new SonarrService()));  
                    s.WhenStarted(tc => tc.Start());            
                    s.WhenStopped(tc => tc.Stop());             
                });
                x.RunAsLocalSystem();                           

                x.SetDescription("Monitors a path for files added, then asks sonarr to re-scan the path.");       
                x.SetDisplayName("Sonarr Monitor");                      
                x.SetServiceName("SonarrMonitor");                      
            });
        }
    }                                                                   
}
