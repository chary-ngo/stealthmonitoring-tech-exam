using ImageDownloader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImageDownloader.Providers
{
    public class MarsRoverQueryProvider
    {
        private const string BASE_URL = "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?api_key=";
        private const string API_KEY = "6KgpD6g7umaVBu3VWL4dgGe4FntG0mSUlR8RbNrS";

        private readonly HttpClient httpClient;

        public MarsRoverQueryProvider() {
            httpClient = new HttpClient();
        }

        public async Task<List<Photo>> GetImageSrcByDate(DateTime date)
        {
            try
            {
                var response = await httpClient.GetAsync($"{BASE_URL}{API_KEY}&earth_date={date.ToString("yyyy-MM-dd")}");

                MarsRoverPhotoResult photoResult = JsonConvert.DeserializeObject<MarsRoverPhotoResult>(await response.Content.ReadAsStringAsync());
                return photoResult.Photos;
            }
            catch(Exception e)
            {
                //add log here
            }
            return null;
        }
    }
}
