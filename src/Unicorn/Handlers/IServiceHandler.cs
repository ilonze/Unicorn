using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicorn.Handlers
{
    public interface IServiceHandler
    {
        IEnumerable<Service> GetServices(string serviceName);
        void AddService(Service service);
        void SetService(Service service);
        void DeleteService(string serviceId);
    }
}
