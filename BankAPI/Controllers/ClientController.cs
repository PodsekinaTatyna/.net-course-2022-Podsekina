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
            return _clientService.GetClient(id);
        }

        [HttpPost]
        public void AddClient(Client client)
        {
            _clientService.AddNewClient(client);
        }

        [HttpDelete]
        public void DeleteClient(Client client)
        {
            _clientService.DeleteClient(client);
        }

        [HttpPut]
        public void UpdateClient(Client client)
        {
            _clientService.UpdateClient(client);
        }

}
}
