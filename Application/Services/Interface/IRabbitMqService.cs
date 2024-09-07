using Application.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IRabbitMqService
    {
        void Publish<T>(RabbitMqPublishModel<T> rabbitMqConfig);
    }

}
