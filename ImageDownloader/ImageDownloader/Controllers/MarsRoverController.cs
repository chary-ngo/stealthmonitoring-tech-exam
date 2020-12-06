using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ImageDownloader.Models;
using ImageDownloader.Providers;
using Microsoft.AspNetCore.Mvc;

namespace ImageDownloader.Controllers
{
    [ApiController]
    [Route("marsrover")]
    public class MarsRoverController : ControllerBase
    {
        private readonly MarsRoverQueryProvider _queryProvider;
        private const string DATE_FORMAT = "yyy-MM-dd";
        private const string BASE_PATH = "C:\\Downloads\\MarsRover";
        private const string DATES_FILENAME = "dates.txt";
        private const string INVALID_DATE_FILENAME = "invalid.txt";

        public MarsRoverController(MarsRoverQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        [HttpGet("DownloadImage")]
        public void DownloadImage()
        {
            try
            {
                var invalidDates = "";

                if (!Directory.Exists(BASE_PATH))
                    Directory.CreateDirectory(BASE_PATH);

                // get all date files from directory
                var earthDates = GetDates();
                if (earthDates != null)
                {
                    

                    foreach (var earthDate in earthDates)
                    {
                        DateTime dttm = new DateTime();
                        var result = DateTime.TryParse(earthDate, out dttm);

                        if (result)
                        {
                            if (!Directory.Exists($"{BASE_PATH}\\{dttm.ToString(DATE_FORMAT)}"))
                                Directory.CreateDirectory($"{BASE_PATH}\\{dttm.ToString(DATE_FORMAT)}");

                            var images = _queryProvider.GetImageSrcByDate(dttm).Result;

                            DownloadMultipleFilesAsync(images, $"{BASE_PATH}\\{dttm.ToString(DATE_FORMAT)}").Wait();
                        }
                        else
                        {

                            // write to file thos dates where input date is invaid
                            invalidDates += $"{earthDate}\n";
                        }
                    }

                    if(!String.IsNullOrEmpty(invalidDates))
                    {
                        var filePath = $"{BASE_PATH}\\{INVALID_DATE_FILENAME}";
                        System.IO.File.WriteAllText(filePath, $"Invalid inputs. Cannot retrieve images for the following:\n{invalidDates}");
                    }
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                
            }
        }

        private async Task DownloadMultipleFilesAsync(List<Photo> photos, string path)
        {
            await Task.WhenAll(photos.Select(photo => DownloadImageAsync(photo, path)));
        }

        private async Task DownloadImageAsync(Photo photo, string path)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFileAsync(new Uri(photo.Img_Src), $"{path}\\{photo.Id}.jpg");
                }
            }
            catch (Exception e)
            {
                // log
            }
        }

        private List<string> GetDates()
        {
            try
            {
                var dateFilePath = $"{System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\'))}\\{DATES_FILENAME}";
                if (System.IO.File.Exists(dateFilePath))
                {
                    var dates = System.IO.File.ReadAllText(dateFilePath).Split("\r\n");
                    return dates.ToList();
                }
            }
            catch (Exception e)
            {
                // log
            }
            return null;
        }
    }
}
