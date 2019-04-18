namespace WebMvc.Models
{
    public class Event
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public System.DateTime Date { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int ZipCode { get; set; }

        public bool FreeEvent { get; set; }

        public decimal Price { get; set; }

        public int EventCategoryId { get; set; }

        public int EventMetroAreaId { get; set; }

        public string PictureUrl { get; set; }

        public string EventCategory { get; set; }

        public string EventMetroArea { get; set; }
    }
}