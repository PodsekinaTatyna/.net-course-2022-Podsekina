using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace BankAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private ClientService _clientService;
        public ClientController()
        {
            _clientService = new ClientService();
        }

        [HttpGet]
        public Client GetClient(Guid id)
        {
            return _clientService.GetClientAsync(id).Result;
        }

        [HttpPost]
        public void AddClient(Client client)
        {
            _clientService.AddNewClientAsync(client);
        }

        [HttpDelete]
        public void DeleteClient(Client client)
        {
            _clientService.DeleteClientAsync(client);
        }

        [HttpPut]
        public void UpdateClient(Client client)
        {
            _clientService.UpdateClientAsync(client);
        }

    }
}
