using Application.BackgroundServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IProcessEventExampleService
    {
        Task ProcessEvent(RabbitMqExampleModel model);
    }
}
