/* ************************************************************
 * For students to work on assignments and project.
 * Permission required material. Contact: xyuan@uwindsor.ca 
Added by Kamonashish
 * ************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace API
{
    public class BookOrder
    {
        ObservableCollection<OrderItem> orderItemList = new
            ObservableCollection<OrderItem>();
        public int OrderID { get; set; }
        public ObservableCollection<OrderItem> OrderItemList
        {
            get { return orderItemList; }
        }
        public void AddItem(OrderItem orderItem)
        {
            foreach (var item in orderItemList)
            {
                if (item.BookID == orderItem.BookID)
                {
                    item.Quantity += orderItem.Quantity;
                    return;
                }
            }
            orderItemList.Add(orderItem);
        }
        public void RemoveItem(string productID)
        {
            foreach (var item in orderItemList)
            {
                if (item.BookID == productID)
                {
                    orderItemList.Remove(item);
                    return;
                }
            }
        }



        public async Task PlaceOrder(int xUserID)
        {  
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = "http://localhost:3000/placeorder";

                var total = orderItemList.Count;

                var parameters = new Dictionary<string, string> { };
                parameters.Add("userid", xUserID.ToString());
                parameters.Add("total",total.ToString());

                var i = 1;
                var prefix = "item";
                foreach (var item in orderItemList)
                {
                    parameters.Add(prefix + i, item.BookID);
                    parameters.Add(prefix + i++ + "quantity", item.Quantity.ToString());
                }

                var encodedContent = new FormUrlEncodedContent(parameters);

                HttpResponseMessage response = await client.PostAsync(url, encodedContent);
                var message = await response.Content.ReadAsStringAsync();

                var jsonObject = JObject.Parse(message);
                var IsUpdated = jsonObject["IsUpdated"].ToString();
                if (IsUpdated == "yes")
                {
                    OrderID = Convert.ToInt32(jsonObject["OrderID"]);
                }
                else
                {
                    OrderID = 0;
                }
            }
        }

    }
}
