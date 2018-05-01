using System.Collections.Generic;
using System.Text;

namespace Programmer.Test.Framework
{
    public class QEventListener
    {
        private static ICollection<string> _qEvents;
        public static IEnumerable<string> QEvents => _qEvents ?? (_qEvents = new List<string>());

        public static void Flush()
        {
            _qEvents?.Clear();
        }

        public static void Handle(byte[] body)
        {
            var message = Encoding.UTF8.GetString(body);
            (QEvents as ICollection<string>)?.Add(message);
        }
    }

}
