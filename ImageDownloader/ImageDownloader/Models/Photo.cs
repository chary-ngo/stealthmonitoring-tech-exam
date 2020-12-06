
using System.Text.Json.Serialization;

namespace ImageDownloader.Models
{
    public class Photo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sol")]
        public int Sol { get; set; }

        [JsonPropertyName("img_src")]
        public string Img_Src { get; set; }

        [JsonPropertyName("earth_date")]
        public string Earth_Date { get; set; }

        [JsonPropertyName("camera")]
        public Camera Camera { get; set; }

        [JsonPropertyName("rover")]
        public Rover Rover { get; set; }
    }
}
