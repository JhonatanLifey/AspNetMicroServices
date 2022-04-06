using System;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {

        public IntegrationBaseEvent() //esto es un contructor se ejecuta al inicio, sirva para inicializar variables
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public IntegrationBaseEvent(Guid id, DateTime createDate) 
        {
            Id = id;
            CreationDate = createDate;
        }

        public Guid Id { get; private set; }
        public DateTime CreationDate{ get; private set; }

    }
}
