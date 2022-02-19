using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Application.Queries;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace API.Config
{
    public interface IFileUpdateSender
    {
        Task SendFile(CreateUpload file);
    }
}
