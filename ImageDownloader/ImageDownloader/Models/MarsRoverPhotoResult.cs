
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ImageDownloader.Models
{
    public class MarsRoverPhotoResult
    {
        [JsonPropertyName("photos")]
        public List<Photo> Photos { get; set; }
    }
}
