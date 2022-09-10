using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IStorage
    {
        public void Add(Client client);
        public void Remove();
        public void Editing();

    }
}
