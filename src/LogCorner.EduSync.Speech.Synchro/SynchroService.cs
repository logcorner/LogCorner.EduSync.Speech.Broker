using LogCorner.EduSync.Speech.ServiceBus;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using LogCorner.EduSync.Speech.Synchro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Synchro
{
    public class SynchroService : ISynchroService
    {
        private readonly IKafkaProducer _serviceBus;
        private readonly LogcornerEdusyncSpeechDataContext _db;

        private List<EventStore> _eventStoreItems;

        public SynchroService(IKafkaProducer serviceBus, LogcornerEdusyncSpeechDataContext db)
        {
            _serviceBus = serviceBus;
            _db = db;
        }

        public async Task StartAsync()
        {
            _eventStoreItems = await _db.EventStore.ToListAsync();
        }

        public async Task StopAsync()
        {
            await _db.DisposeAsync();
        }

        public async Task DoWorkAsync()
        {
            foreach (var eventStoreItem in _eventStoreItems.OrderBy(e => e.OccurredOn))
            {
                Console.WriteLine($"**SynchroService::DoWorkAsync - topic : {Topics.Synchro},output : {eventStoreItem} ");
                await _serviceBus.SendAsync(Topics.Synchro, eventStoreItem);
            }
        }
    }
}