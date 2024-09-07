using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class RabbitMqPublishModel<T>
    {
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public T Message { get; set; }
    }
}
