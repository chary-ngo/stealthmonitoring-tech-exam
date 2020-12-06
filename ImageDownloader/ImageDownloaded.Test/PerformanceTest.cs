using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace ImageDownloaded.Test
{
    public class UnitTest1
    {
        [Fact]
        public void RequestTime_LessThan20Images()
        {
            DateTime startDttm, endDttm;

            // TODO: change date.txt content (look for a data that has < 20 images

            using(var client = new HttpClient())
            {
                startDttm = DateTime.Now;
                var response = client.GetAsync("https://localhost:44381/marsrover/DownloadImage");
                endDttm = DateTime.Now;
            }

            var expected = 1;
            var actual = (int)(endDttm - startDttm).TotalMilliseconds;
            Assert.True(actual <= expected, $"Actual response of {actual} is <= the expected {expected}");
        }

        [Fact]
        public void RequestTime_1000Images()
        {
            DateTime startDttm, endDttm;

            using (var client = new HttpClient())
            {
                startDttm = DateTime.Now;
                var response = client.GetAsync("https://localhost:44381/marsrover/DownloadImage");
                endDttm = DateTime.Now;
            }

            var expected = 1;
            var actual = (int)(endDttm - startDttm).TotalMilliseconds;
            Assert.True(actual <= expected, $"Actual response of {actual} is <= the expected {expected}");
        }

        [Fact]
        public void AverageResponseTime_100Requests()
        {
            var allResponseTime = new List<(DateTime Start, DateTime End)>();

            for (var i = 0; i < 100; i++)
            {
                using (var client = new HttpClient())
                {
                    var start = DateTime.Now;
                    var response = client.GetAsync("https://localhost:44381/marsrover/DownloadImage");
                    var end = DateTime.Now;

                    allResponseTime.Add((start, end));
                }
            }

            var expected = 1;
            var actual = (int)allResponseTime.Select(r => (r.End - r.Start).TotalMilliseconds).Average();
            Assert.True(actual <= expected, $"Average actual response of {actual} is <= the average expected {expected}");
        }


    }
}
