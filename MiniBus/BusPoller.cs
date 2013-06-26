using System;
using System.Threading;
using MiniBus.Interfaces;

namespace MiniBus
{
    public class BusPoller
    {
        private readonly ITransport transport;
        private readonly IMessageProcessor processor;
        private readonly string address;
        private readonly Thread[] workers;
        private readonly AutoResetEvent[] waitHandles;
        private readonly CancellationTokenSource cancellationTokenSource;

        public BusPoller(ITransport transport, IMessageProcessor processor, uint countOfWorkers, string address)
        {
            this.transport = transport;
            this.processor = processor;
            this.address = address;
            if (countOfWorkers == 0)
            {
                throw new ArgumentException("Count of workers must be greater than zero", "countOfWorkers");
            }

            workers = new Thread[countOfWorkers];
            waitHandles = new AutoResetEvent[countOfWorkers];
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            for (int i = 0; i < workers.Length; i++)
            {
                Func<TransportMessage> getMessage = () => this.transport.Receive(this.address);

                var waitHandle = new AutoResetEvent(false);
                var cancelToken = cancellationTokenSource.Token;
                var worker = new Thread(() => Consume(getMessage, waitHandle, cancelToken));

                workers[i] = worker;
                waitHandles[i] = waitHandle;

                worker.Name = "Worker " + i;
                worker.Start();
            }

        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            WaitHandle.WaitAll(waitHandles);
        }

        private void Consume(Func<TransportMessage> getMessage , AutoResetEvent waitHandle, CancellationToken cancelToken)
        {
            while (true)
            {
                if (cancelToken.IsCancellationRequested)
                {
                    waitHandle.Set();
                    return;
                }

                var message = getMessage();
                processor.Process(message);

                if (cancelToken.IsCancellationRequested)
                {
                    waitHandle.Set();
                    return;
                }
            }
        }
    }
}