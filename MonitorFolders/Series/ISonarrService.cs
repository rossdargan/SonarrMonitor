namespace MonitorFolders.Series
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MonitorFolders.Entities;

    /// <summary>
    /// The Sonarr Service interface.
    /// </summary>
    public interface ISonarrService
    {
        /// <summary>
        /// Returns all series monitored by Sonarr
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        Task<IEnumerable<Series>> GetSeries();

        /// <summary>
        /// Causes Sonarr to rescan the directory to find the newly updated episode
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        Task RescanSeries(Series series);
    }
}
