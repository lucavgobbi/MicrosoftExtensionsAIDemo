namespace AIDemo.Models
{
    public class BookModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateTime ReadAt { get; set; }
        public int UserId { get; set; }
    }

    public class GoogleBooksResponse
    {
        public string Kind { get; set; }
        public int TotalItems { get; set; }
        public List<GoogleBookItem> Items { get; set; }
    }

    public class GoogleBookItem
    {
        public string Kind { get; set; }
        public string Id { get; set; }
        public string Etag { get; set; }
        public string SelfLink { get; set; }
        public GoogleVolumeInfo VolumeInfo { get; set; }
    }

    public class GoogleVolumeInfo
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string PublishedDate { get; set; }
        public string Description { get; set; }
        public List<GoogleIndustryIdentifier> IndustryIdentifiers { get; set; }
        public int PageCount { get; set; }
        public List<string> Categories { get; set; }
        public double AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public string MaturityRating { get; set; }
        public GoogleImageLinks ImageLinks { get; set; }
        public string Language { get; set; }
        public string PreviewLink { get; set; }
        public string InfoLink { get; set; }
        public string CanonicalVolumeLink { get; set; }
    }

    public class GoogleIndustryIdentifier
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
    }

    public class GoogleImageLinks
    {
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
    }
}

