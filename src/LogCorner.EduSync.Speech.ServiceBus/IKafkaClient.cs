﻿using System.Threading;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public interface IKafkaClient
    {
        Task SendAsync(string topic, EventStore @event);

        Task ReceiveAsync(string topic, CancellationToken stoppingToken,bool runAlawys = true);
    }
}