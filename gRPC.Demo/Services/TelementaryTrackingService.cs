using Google.Protobuf.WellKnownTypes;
using gRPC.Demo.protos;
using Grpc.Core;

namespace gRPC.Demo.Services
{
    public class TelementaryTrackingService : TrackingService.TrackingServiceBase
    {
        private readonly ILogger<TelementaryTrackingService> logger;

        public TelementaryTrackingService(ILogger<TelementaryTrackingService> logger)
        {
            this.logger = logger;
        }
        // c# service, same as endpoint in rest api
        public override Task<TrackingResponse> SendMessage(TrackingMessage request, ServerCallContext context)
        {
            logger.LogInformation($"New Message Received: {request.DeviceId}  Location {request.Location.Latitude} {request.Location.Longitude} sensors {request.Sensors.Count} ");
            return Task.FromResult(new TrackingResponse
            {
                Success = true
            });
        }

        public override async Task<Empty> KeepAlive(IAsyncStreamReader<PulseMessage> requestStream, ServerCallContext context)
        {
            await Task.Run(async () =>
           {
               // foreach will wait for every item untill the client send new message
               // iterate over expected messages from the client because it is a stream
               await foreach (var message in requestStream.ReadAllAsync())
               {
                   logger.LogInformation($"Pulse received from Device: {message.DeviceId}  With Status {message.Status.ToString()} at {message.Stamp.ToDateTime()}");
               }
           });
            return new Empty();
        }

        public override async Task<SumResponse> GetSum(IAsyncStreamReader<NumberRequest> requestStream, ServerCallContext context)
        {
            int sum = 0;

            // will finish when client finsihes sending messages [stream.RequestStream.CompleteAsync();]
            await foreach (var number in requestStream.ReadAllAsync())
            {
                Console.WriteLine($"Received: {number.Number}");
                sum += number.Number;
            }

            // when the client finishes sending numbers, the server return only one response
            Console.WriteLine("Client finished sending numbers.");
            return new SumResponse { Sum = sum };
        }
    }
}
