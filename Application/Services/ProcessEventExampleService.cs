using Application.BackgroundServices.Models;
using Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProcessEventExampleService : IProcessEventExampleService
    {
        public async Task ProcessEvent(RabbitMqExampleModel model)
        {
            Console.WriteLine("Received Message", model);
        }
    }
}
