using Newtonsoft.Json;

namespace CGP.Feature.SEO.Models.StructuredData
{
    public class ArticleSchema
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; set; } = "https://schema.org";

        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "Article";
        public MainEntityOfPageSchema MainEntityOfPage { get; set; }
        public string Headline { get; set; }
        public string Image { get; set; }
        public string DatePublished { get; set; }
        public AuthorSchema Author { get; set; }
        public PublisherSchema publisher { get; set; }
        public string Description { get; set; }
    }
    public class MainEntityOfPageSchema
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "WebPage";

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
    public class AuthorSchema
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "Person";
       
        public string Name { get; set; }
    }
    public class ImageObjectSchema
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "ImageObject";

        public string Url { get; set; }
    }
    public class PublisherSchema
    {
        [JsonProperty(PropertyName = "@type")]
        public string Type { get; set; } = "Organization";
      
        public string Name { get; set; }

        public ImageObjectSchema Logo { get; set; }
    }
}