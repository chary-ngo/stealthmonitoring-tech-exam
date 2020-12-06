using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace ImageDownloaded.Test
{
    public class UnitTest
    {
        [Fact]
        public void DownloadImange_Success()
        {
            // change date.txt content
            var dates = new List<string>() { "Jul-13-2016", "June 2, 2018" };

            var filePath = $"{System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\'))}\\dates.txt";
            System.IO.File.WriteAllText(filePath, $"{string.Join("\r\n",dates)}");

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("https://localhost:44381/marsrover/DownloadImage");
            }

            // asset date folder separated
            var folders = Directory.GetDirectories($"C:\\Downloads\\MarsRover");
            Assert.NotEmpty(folders);
            Assert.Equal(folders.Length, dates.Count());

            // files downloaded in folder

            if (folders != null)
            {
                foreach (var folder in folders)
                {
                    var imgs = Directory.GetFiles($"C:\\Downloads\\MarsRover\\{folder}");

                    Assert.NotEmpty(imgs);
                }
            }
        }

        [Fact]
        public void DownloadImage_InvalidInputDate()
        {
            // change date.txt content to invalid dates
            var dates = new List<string>() { "April-31-2016", "Jane 2, 2018" };

            var filePath = $"{System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(0, System.Reflection.Assembly.GetExecutingAssembly().Location.LastIndexOf('\\'))}\\dates.txt";
            System.IO.File.WriteAllText(filePath, $"{string.Join("\r\n", dates)}");

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("https://localhost:44381/marsrover/DownloadImage");
            }

            // invalid txt exist
            filePath = $"C:\\Downloads\\MarsRover\\invalid.txt";
            Assert.True(System.IO.File.Exists(filePath));

            var invalidDates = System.IO.File.ReadAllText(filePath).Split("\n").ToList();
            invalidDates.RemoveAt(0);
            Assert.Equal(invalidDates.Count, dates.Count);
        }


    }
}
