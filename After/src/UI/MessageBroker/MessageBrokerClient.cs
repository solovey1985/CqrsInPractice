using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace UI.MessageBroker
{
   public class MessageBrokerClient: IDisposable
    {
        private const string host = "localhost";
        private const string user = "guest";
        private const string pass = "guest";
        private const string queueName = "cqrs.queue";
        private const string exchangeName = "";
        private const string virtualHost = "";
        private int port = 0;

        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel model;
        public MessageBrokerClient()
        {
            connectionFactory = new ConnectionFactory {
                HostName = host,
                UserName = user,
                Password = pass
            };
            if (!string.IsNullOrWhiteSpace(virtualHost))
            {
                connectionFactory.VirtualHost = virtualHost;
            }
            if (port>0)
            {
                connectionFactory.Port = port;
            }
            connection = connectionFactory.CreateConnection();
            model = connection.CreateModel();
        }

        public void Send(string message)
        {
            var properties = model.CreateBasicProperties();
            properties.Persistent= true;

            byte[] messageBuffer = Encoding.Default.GetBytes(message);
            model.BasicPublish(exchangeName, queueName, properties, messageBuffer);

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MessageBrokerClient() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
