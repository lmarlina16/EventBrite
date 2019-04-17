using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoesOnContainers.Services.CartApi.Model
{
    public class  Cart
    {
        public string BuyerId { get;  set; }
        public List<CartItem> Events { get; set; } 

        public Cart(string cartId)
        {
            BuyerId = cartId;
            Events = new List<Model.CartItem>();
        }
    }
}
