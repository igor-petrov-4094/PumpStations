using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.EHandling.Interface
{
    public delegate void EventCodeExceptionInterceptor(EventCodeException e);

    [Serializable]
    public class EventCodeException : Exception
    {
        public EventCodeDescriptor eventCodeDescriptor { get; set; }

        public static event EventCodeExceptionInterceptor exceptionInterceptor;

        public EventCodeException(string message, EventCodeDescriptor _eventCodeDescriptor) : 
            base($"Ошибка #{_eventCodeDescriptor.longName}: [{message}]") 
        {
            eventCodeDescriptor = _eventCodeDescriptor;

            exceptionInterceptor?.Invoke(this);
        }
        
        protected EventCodeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
