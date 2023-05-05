using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text.Json;
using System.Web;

namespace jsGrid2Mvc.Samples.Controllers
{
    public enum Country
    {
        UnitedStates = 1,
        Canada = 2,
        UnitedKingdom = 3,
        France = 4,
        Brazil = 5,
        China = 6,
        Russia = 7
    }


    public class Client
    {

        public int ID { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public Country? Country { get; set; }
        public string? Address { get; set; }
        public bool Married { get; set; }

    }

    public class ClientFilter
    {

        public string? Name { get; set; }
        public string? Address { get; set; }
        public Country? Country { get; set; }
        public bool? Married { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        public Client Post([FromBody] Client client)
        {
            var clients = ReadCLients().ToList();
            Client? existing = clients.SingleOrDefault(x => x.ID == client.ID);
            if (existing != null)
                return null;
            client.ID = clients.Count + 1;
            clients.Add(client);
            SaveAll(clients);
            return client;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<object> Get()
        {
            var clients = ReadCLients();

            return ApplyFilter(clients);
        }

        [HttpPost]
        [Route("{id}")]
        public void Put(int id, [FromBody] Client editedClient)
        {
            var clients = ReadCLients().ToList();
            Client? client = clients.SingleOrDefault(x => x.ID == id);
            if (client == null)
                return;

            client.Name = editedClient.Name;
            client.Age = editedClient.Age;
            client.Country = editedClient.Country;
            client.Address = editedClient.Address;
            client.Married = editedClient.Married;

            SaveAll(clients);
        }

        [HttpDelete]
        [Route("{id}")]
        public void Delete(int id)
        {
            var clients = ReadCLients().ToList();
            Client? client = clients.SingleOrDefault(x => x.ID == id);

            if (client == null)
                return;

            var index = clients.IndexOf(client);
            clients.RemoveAt(index);
            SaveAll(clients);
        }

        static IEnumerable<Client> ReadCLients()
        {
            var clients = new List<Client>();
            var filePath = @"C:\Temp\Data.json";
            if (!System.IO.File.Exists(filePath))
            {
                clients.Add(new Client()
                {
                    ID = 1,
                    Name = "Marco Rossi",
                    Address = "Via ROma, 17, 20017, RHO (MI)",
                    Age = 30,
                    Country = Country.UnitedKingdom,
                    Married = false
                });
                clients.Add(new Client()
                {
                    ID = 2,
                    Name = "Sara Bianchi",
                    Address = "Via Milano, 33, 20017, RHO (MI)",
                    Age = 37,
                    Country = Country.UnitedKingdom,
                    Married = true
                });
                SaveAll(clients, filePath);
            }
            var jsonString = System.IO.File.ReadAllText(filePath);
            clients = JsonSerializer.Deserialize<List<Client>>(jsonString) ?? new List<Client>();
            return clients;
        }

        static void SaveAll(List<Client> clients, string filePath = @"C:\Temp\Data.json")
        {
            string json = JsonSerializer.Serialize(clients, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            System.IO.File.WriteAllText(filePath, json);
        }

        ClientFilter GetFilter()
        {
            NameValueCollection filter = HttpUtility.ParseQueryString(Request.QueryString.Value);
            if (filter.Count == 0)
                return new ClientFilter();

            var name = filter.AllKeys.Contains("name") ? filter["name"] : "";
            var address = filter.AllKeys.Contains("address") ? filter["address"] : "";
            var country = filter.AllKeys.Contains("country") && filter["country"] != "0" ? (Country)int.Parse(filter["country"]) : (Country?)null;
            var isMarried = String.IsNullOrEmpty(filter["married"]) ? (bool?)null : bool.Parse(filter["married"]);

            return new ClientFilter
            {
                Name = name,
                Address = address,
                Country = country,
                Married = isMarried
            };
        }

        IEnumerable<object> ApplyFilter(IEnumerable<Client> listofItems)
        {
            ClientFilter filter = GetFilter();

            var result = listofItems.Where(c =>
                (String.IsNullOrEmpty(filter.Name) || c.Name.Contains(filter.Name)) &&
                (String.IsNullOrEmpty(filter.Address) || c.Address.Contains(filter.Address)) &&
                (!filter.Married.HasValue || c.Married == filter.Married) &&
                (!filter.Country.HasValue || c.Country == filter.Country)
            );

            return result.ToArray();
        }


    }

}
