namespace MonitorFolders.Series
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Threading.Tasks;

    using MonitorFolders.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// The sonarr service.
    /// </summary>
    public class SonarrService : ISonarrService
    {
        /// <summary>
        /// The Sonarr api key.
        /// </summary>
        private readonly string _apiKey;

        /// <summary>
        /// The base url for the Sonarr API service.
        /// </summary>
        private readonly string _baseUrl;

        public SonarrService()
        {
            _apiKey = ConfigurationManager.AppSettings["ApiKey"];
            _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        }

        /// <summary>
        /// The get series.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public async Task<IEnumerable<Series>> GetSeries()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
            string result = await client.GetStringAsync(_baseUrl + "Series");
            return JsonConvert.DeserializeObject<IEnumerable<Series>>(result);
        }

        public Task RescanSeries(Series series)
        {
            Console.WriteLine($"Updateing series {series.Title} with id {series.Id}");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
            string command =
                $"{{\"name\": \"RescanSeries\",\"seriesId\": {series.Id}}}";
           return client.PostAsync(_baseUrl + $"command/", new StringContent(command));


        }
    }
}