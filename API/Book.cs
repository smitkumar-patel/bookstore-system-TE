using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API
{
    public class Book
    {
        private DataSet dsBooks = new DataSet("Books");
        private DataTable categoryTable = new DataTable();
        private DataTable bookTable = new DataTable();
      
        public DataSet getDataset()
        {
            return dsBooks;
        }
        public void getAllData()
        {
            getBookData();
            GetCategoryData();
            setRelation();
        }
        public void setRelation()
        {
            categoryTable.TableName = "Category";
            bookTable.TableName = "Books";

            dsBooks.Tables.Add(categoryTable);
            dsBooks.Tables.Add(bookTable);

            DataRelation drCat_Book = new DataRelation("drCat_Book",
                dsBooks.Tables["Category"].Columns["CategoryID"],
                dsBooks.Tables["Books"].Columns["CategoryID"], false);
            dsBooks.Relations.Add(drCat_Book);
        }
        public async void getBookData()
        {
            
            DataColumn ISBN = new DataColumn();
            ISBN.DataType = System.Type.GetType("System.String");
            ISBN.ColumnName = "ISBN";
            bookTable.Columns.Add(ISBN);

            
            DataColumn CategoryID = new DataColumn();
            CategoryID.DataType = System.Type.GetType("System.Int32");
            CategoryID.ColumnName = "CategoryID";
            bookTable.Columns.Add(CategoryID);

            DataColumn Title = new DataColumn();
            Title.DataType = System.Type.GetType("System.String");
            Title.ColumnName = "Title";
            bookTable.Columns.Add(Title);

            DataColumn Author = new DataColumn();
            Author.DataType = System.Type.GetType("System.String");
            Author.ColumnName = "Author";
            bookTable.Columns.Add(Author);

            
            DataColumn Price = new DataColumn();
            Price.DataType = System.Type.GetType("System.Double");
            Price.ColumnName = "Price";
            bookTable.Columns.Add(Price);

            DataColumn Year = new DataColumn();
            Year.DataType = System.Type.GetType("System.Int32");
            Year.ColumnName = "Year";
            bookTable.Columns.Add(Year);

            DataColumn Edition = new DataColumn();
            Edition.DataType = System.Type.GetType("System.Int32");
            Edition.ColumnName = "Edition";
            bookTable.Columns.Add(Edition);

            DataColumn Publisher = new DataColumn();
            Publisher.DataType = System.Type.GetType("System.String");
            Publisher.ColumnName = "Publisher";
            bookTable.Columns.Add(Publisher);


            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = "http://localhost:3000/getbookdata";
                //  var parameters = new Dictionary<string, string> { { "name", "smit" }, { "name2", "skp2" } };
                // var encodedContent = new FormUrlEncodedContent(parameters);

                HttpResponseMessage response = await client.GetAsync(url);
                var message = await response.Content.ReadAsStringAsync();
                JArray data = JArray.Parse(message);
                foreach (JObject obj in data.Children<JObject>())
                {
                    DataRow row = bookTable.NewRow();
                    foreach (JProperty sin in obj.Properties())
                    {
                        if(bookTable.Columns.Contains(sin.Name))
                        {
                            row[sin.Name] = sin.Value.ToString();
                        }
                        
                    }
                    bookTable.Rows.Add(row);
                }
            }
        }

        
        public async void GetCategoryData()
        {
            DataColumn CategoryID = new DataColumn();
            CategoryID.DataType = System.Type.GetType("System.Int32");
            CategoryID.ColumnName = "CategoryID";
            categoryTable.Columns.Add(CategoryID);

            DataColumn Name = new DataColumn();
            Name.DataType = System.Type.GetType("System.String");
            Name.ColumnName = "Name";
            categoryTable.Columns.Add(Name);

            DataColumn Description = new DataColumn();
            Description.DataType = System.Type.GetType("System.String");
            Description.ColumnName = "Description";
            categoryTable.Columns.Add(Description);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10; Win64; x64; rv:60.0) Gecko/20100101 Firefox/60.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = "http://localhost:3000/getcategorydata";
                //  var parameters = new Dictionary<string, string> { { "name", "smit" }, { "name2", "skp2" } };
                // var encodedContent = new FormUrlEncodedContent(parameters);

                HttpResponseMessage response = await client.GetAsync(url);
                var message = await response.Content.ReadAsStringAsync();
              //  categoryTable = 
               // categoryTable = (DataTable)JsonConvert.DeserializeObject(message,(typeof(DataTable)));
                JArray data = JArray.Parse(message);
                foreach (JObject obj in data.Children<JObject>())
                {
                    DataRow row = categoryTable.NewRow();
                    foreach (JProperty sin in obj.Properties())
                    {
                        row[sin.Name] = sin.Value.ToString();
                    }
                    categoryTable.Rows.Add(row);
                }
            }
        }

      
    }
}
