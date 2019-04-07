using EventCatalogAPI.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogAPI.Data
{
    public class EventSeed
    {
        public static void Seed(EventContext context)
        {
            context.Database.Migrate();

            if (!context.EventCategories.Any())
            {
                context.EventCategories.AddRange(GetPreconfiguredEventCatagories());
                context.SaveChanges();
            }

            if (!context.EventMetroAreas.Any())
            {
                context.EventMetroAreas.AddRange(GetPreconfiguredEventMetroAreas());
                context.SaveChanges();
            }

            if (!context.Events.Any())
            {
                context.Events.AddRange(GetPreconfiguredEvents());
                context.SaveChanges();
            }
        }


        private static IEnumerable<EventCategory> GetPreconfiguredEventCatagories()
        {
            return new List<EventCategory>()
            {
                new EventCategory(){ Category="Music"},
                new EventCategory(){ Category="Arts"},
                new EventCategory(){ Category="Sports&Fitness"},
                new EventCategory(){ Category="Family&Education"}

            };
        }

        private static IEnumerable<EventMetroArea> GetPreconfiguredEventMetroAreas()
        {
            return new List<EventMetroArea>()
            {
                new EventMetroArea(){ MetroArea="Seattle", State="WA"},
                new EventMetroArea(){ MetroArea="New York", State="NY"},
                new EventMetroArea(){ MetroArea="Los Angeles", State="CA"},
                new EventMetroArea(){ MetroArea="San Francisco", State="CA"},
                new EventMetroArea(){ MetroArea="Chicago", State="IL"},
                new EventMetroArea(){ MetroArea="Miami", State="FL"}

            };
        }

        private static IEnumerable<Event> GetPreconfiguredEvents()
        {
            return new List<Event>()
            {
                new Event() { EventCategoryId=3,EventMetroAreaId=1,
                    Description = "Prepare yourself for 100+ draft beers from regional breweries, 30+ creative bacon dishes from local chefs, the Hormel bacon eating contest, lawn games, music, and so much more!",
                    Title = "Seattle Bacon and Beer Classic 2019",
                    Date = DateTime.ParseExact("07/10/2019 13:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "T-Mobile Park",
                    Address2 = "1250 1st Avenue South", City = "Seattle",
                    State = "WA", ZipCode = 98134, FreeEvent = false,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/1" },

                new Event() { EventCategoryId=4,EventMetroAreaId=2,
                    Description = "Find out all the information to study abroad!",
                    Title = "2019 New York State Education Abroad Expo",
                    Date = DateTime.ParseExact("04/10/2019 12:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "New York State Fairgrounds",
                    Address2 = "581 State Fair Blvd", City = "Syracuse",
                    State = "NY", ZipCode = 13209, FreeEvent = true ,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/2" },

                new Event() { EventCategoryId=1,EventMetroAreaId=3,
                    Description = "LA's biggest annual music event returns",
                    Title = "Los Angeles International Music Festival",
                    Date = DateTime.ParseExact("05/12/2019 10:20", "MM/dd/yyyy HH:mm", null),
                    Address1 = "Walt Disney Concert Hall",
                    Address2 = "111 S Grand Ave", City = "Los Angeles",
                    State = "CA", ZipCode = 90012, FreeEvent = false ,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/3" },

                new Event() { EventCategoryId=1,EventMetroAreaId=4,
                    Description = "Untameable and driven by passion, Carmen scorches every heart in her path in Georges Bizet's explosive opera, returning to San Francisco Opera in a new production this season. ",
                    Title = "San Francisco Opera - Carmen",
                    Date = DateTime.ParseExact("04/29/2019 19:00", "MM/dd/yyyy HH:mm", null),
                    Address1 = "War Memorial Opera House",
                    Address2 = "301 Van Ness Ave", City = "San Francisco",
                    State = "CA", ZipCode = 94102, FreeEvent = false,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/4" },

                new Event() { EventCategoryId=3,EventMetroAreaId=5,
                    Description = "United Center is a multi-purpose arena located in the Near West Side neighborhood of Chicago, Illinois",
                    Title = "Chicago Bulls V. Philadelphia 76ers",
                    Date = DateTime.ParseExact("06/12/2019 12:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "United Center",
                    Address2 = "1901 W Madison St", City = "Chicago",
                    State = "IL", ZipCode = 60612, FreeEvent = false,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/5" },

                new Event() { EventCategoryId=2,EventMetroAreaId=6,
                    Description = "Guests are invited for a studio visit following the performance and artist talk.", Title = "Performance + Artist Talk",
                    Date = DateTime.ParseExact("09/22/2019 07:00", "MM/dd/yyyy HH:mm", null),
                    Address1 = "Bakehouse Art Complex",
                    Address2 = "561 Northwest 32nd Street", City = "Miami",
                    State = "FL", ZipCode = 33127, FreeEvent = true,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/6" },

                new Event() { EventCategoryId=2,EventMetroAreaId=1,
                    Description = "Pacific Science Center’s Laser Dome has a packed catalog of shows from Laser Cardi B to Laser ODESZA that feature stunning laser imagery and powerful sound. ",
                    Title = "Laser Dome at Pacific Science Center",
                    Date = DateTime.ParseExact("06/06/2019 20:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "Pacific Science Center",
                    Address2 = "200 Second Ave. North", City = "Seattle",
                    State = "WA", ZipCode = 98109, FreeEvent = false,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/7"  },

                new Event() { EventCategoryId=4,EventMetroAreaId=2,
                    Description = "Face Painting By Katie, Bounce House Inflatables, Balloons Twisters, Petting Zoo, Dancing,CRP...",
                    Title = "Family Fun Day & Expo",
                    Date = DateTime.ParseExact("05/10/2019 09:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "Aviator Sports & Events Center",
                    Address2 = "3159 Flatbush Ave", City = "Brooklyn",
                    State = "NY", ZipCode = 11234, FreeEvent = true,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/8" },

                new Event() { EventCategoryId=4,EventMetroAreaId=3,
                    Description = "It's an explosion of cultural fusion  representing our rich Multi-cultural society and celebrating diversity as One World & One People.",
                    Title = "Carnival Culture Village",
                    Date = DateTime.ParseExact("06/20/2019 10:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "6800 Hollywood Blvd.",
                    Address2 = "Hollywood", City = "Los Angeles",
                    State = "CA", ZipCode = 90028, FreeEvent = true ,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/9" },

                new Event() { EventCategoryId=4,EventMetroAreaId=4,
                    Description = "Comparative & International Education Society",
                    Title = "2019 Education Conference",
                    Date = DateTime.ParseExact("07/11/2019 08:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "Hyatt Regency San Francisco",
                    Address2 = "5 Embarcadero Center", City = "San Francisco",
                    State = "CA", ZipCode = 94111, FreeEvent = true,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/10" },

                new Event() { EventCategoryId=3,EventMetroAreaId=5,
                    Description = "Take your strength, stamina, and agility to the next level. ",
                    Title = "Women’s Fitness Boot Camp",
                    Date = DateTime.ParseExact("04/30/2019 09:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "STUDIO FIT CHICAGO",
                    Address2 = "1011 W. Armigate Ave.", City = "Chicago",
                    State = "IL", ZipCode = 60614, FreeEvent = false,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/11" },

                new Event() { EventCategoryId=3,EventMetroAreaId=6,
                    Description = "People coming together. People running together. A test of endurance, strength and will.",
                    Title = "Miami Marathon and Half Marathon",
                    Date = DateTime.ParseExact("10/10/2019 07:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "AmericanAirlines Arena",
                    Address2 = "601 Biscayne Blvd", City = "Miami",
                    State = "FL", ZipCode = 33132, FreeEvent = false,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/12" },

                new Event() { EventCategoryId=1,EventMetroAreaId=1,
                    Description = "We welcome international jazz artists to Ballard at our Mainstage Concert at the Nordic Heritage Museum.",
                    Title = "Ballard Jazz Festival",
                    Date = DateTime.ParseExact("08/15/2019 18:30", "MM/dd/yyyy HH:mm", null),
                    Address1 = "Nordic Museum",
                    Address2 = "2655 NW Market St", City = "Seattle",
                    State = "WA", ZipCode = 98107, FreeEvent = false,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/13" },

                new Event() { EventCategoryId=2,EventMetroAreaId=2,
                    Description = "Come and express yourself from the heart at New York City's #1 art school for all ages and levels",
                    Title = "1 NIGHT PAINT AND SIP CLASS PARTY",
                    Date = DateTime.ParseExact("07/01/2019 18:00", "MM/dd/yyyy HH:mm", null),
                    Address1 = "The Art Studio NY",
                    Address2 = "145 W 96th St ", City = "New York",
                    State = "NY", ZipCode = 10025, FreeEvent = false,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/14" },

                new Event() { EventCategoryId=4,EventMetroAreaId=4,
                    Description = "Summer Zoo Camp is the perfect place for your animal-loving child!",
                    Title = "Summer Zoo Camp",
                    Date = DateTime.ParseExact("06/30/2019 10:00", "MM/dd/yyyy HH:mm", null),
                    Address1 = "San Francisco Zoo",
                    Address2 = "1 Zoo Road", City = "San Francisco",
                    State = "CA", ZipCode = 94132, FreeEvent = false,
                    PictureUrl = "http://externalcatalogbaseurltobereplaced/api/pic/15" }
            };
        }

    }
}
