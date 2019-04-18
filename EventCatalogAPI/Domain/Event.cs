using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.Domain
{
    public class Event
    {           
        public int Id { get; set; }

        public double Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int ZipCode { get; set; }

        public bool FreeEvent { get; set; }

        public int EventCategoryId { get; set; }

        public int EventMetroAreaId { get; set; }

        public string PictureUrl { get; set; }

        public virtual EventCategory EventCategory { get; set; }

        public virtual EventMetroArea EventMetroArea { get; set; }

    }
}
