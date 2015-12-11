namespace MonitorFolders
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;

    using MonitorFolders.Series;

    /// <summary>
    /// The monitor service
    /// </summary>
    public class Monitor
    {
        /// <summary>
        /// The sonarr service.
        /// </summary>
        private readonly ISonarrService _sonarrService;


        /// <summary>
        /// The file system watcher
        /// </summary>
        private FileSystemWatcher _watcher;

        public Monitor(ISonarrService sonarrService)
        {
            _sonarrService = sonarrService;
        }

        public void Start()
        {
            _watcher = new FileSystemWatcher();
            _watcher.Path = ConfigurationManager.AppSettings["Path"];
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            _watcher.IncludeSubdirectories = true;
            _watcher.Created += Watcher_Created;                
            _watcher.EnableRaisingEvents = true;
            Console.WriteLine($"Started monitoring path {_watcher.Path}");
        }
        /// <summary>
        /// Fires when a new file is found
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Watcher_Created(object sender, FileSystemEventArgs e)
        {            
            FileInfo fileInfo = new FileInfo(e.FullPath);

            string directory = fileInfo.Directory.FullName;

            Entities.Series series = await FindSeriesByPath(directory);
            if (series != null)
            {
                _sonarrService.RescanSeries(series);
            }
            Console.WriteLine(directory);
        }

        /// <summary>
        /// See if the directory we have found is matched a series in sonarr.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private async Task<Entities.Series> FindSeriesByPath(string directory)
        {
            foreach (var series in await _sonarrService.GetSeries())
            {
                if (directory.ToLower().Contains(series.Path.ToLower()))
                {
                    return series;
                }
            }

            return null;
        }

        /// <summary>
        /// Stop monitoring the folder.
        /// </summary>
        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }

    }
}
