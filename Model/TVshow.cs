namespace TVshows.Model
{
    public class TVshow
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageURL { get; set; }
        public string? Rating { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
    }

    public class ShowResponse
    {
        public Show Show { get; set; }
    }

    public class Show
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public Image Image { get; set; }
        public Rating Rating { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class Image
    {
        public string Medium { get; set; }
    }

    public class Rating
    {
        public double? Average { get; set; }
    }
}
