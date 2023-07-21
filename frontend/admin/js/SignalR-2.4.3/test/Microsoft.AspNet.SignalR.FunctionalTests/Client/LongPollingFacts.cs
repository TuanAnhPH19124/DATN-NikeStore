// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Infrastructure;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.AspNet.SignalR.Infrastructure;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Microsoft.AspNet.SignalR.Tests
{
    public class LongPollingFacts
    {
        [Fact]
        public async Task LongPollingDoesNotPollAfterClose()
        {
            var disconnectCts = new CancellationTokenSource();

            var mockConnection = new Mock<Client.IConnection>();
            mockConnection.SetupGet(c => c.JsonSerializer).Returns(JsonSerializer.CreateDefault());
            mockConnection.Setup(c => c.TotalTransportConnectTimeout).Returns(TimeSpan.FromSeconds(5));
            mockConnection.SetupProperty(c => c.MessageId);

            var pollTaskCompletionSource = new TaskCompletionSource<IResponse>();
            var pollingWh = new TaskCompletionSource<object>();

            var mockHttpClient = CreateFakeHttpClient(
                (url, request, postData, isLongRunning) =>
                {
                    var responseMessage = string.Empty;
                    if (url.Contains("connect?"))
                    {
                        responseMessage = "{\"C\":\"d-C6243495-A,0|B,0|C,1|D,0\",\"S\":1,\"M\":[]}";
                    }
                    else if (url.Contains("poll?"))
                    {
                        pollingWh.TrySetResult(null);
                        return pollTaskCompletionSource.Task;
                    }

                    return Task.FromResult(CreateResponse(responseMessage));
                });

            var longPollingTransport = new LongPollingTransport(mockHttpClient.Object);

            await
                longPollingTransport.Start(mockConnection.Object, string.Empty, disconnectCts.Token)
                    .OrTimeout(TimeSpan.FromSeconds(15));

            // wait for the first polling request
            await pollingWh.Task.OrTimeout();

            // stop polling loop
            disconnectCts.Cancel();

            // finish polling request
            pollTaskCompletionSource.TrySetResult(CreateResponse(string.Empty));

            // give it some time to make sure a new poll was not setup after verification
            await Task.Delay(1000);

            mockHttpClient
                .Verify(c => c.Post(It.Is<string>(url => url.StartsWith("poll?")), It.IsAny<Action<Client.Http.IRequest>>(),
                    It.IsAny<IDictionary<string, string>>(), It.IsAny<bool>()), Times.Once());
        }

        [Fact]
        public async Task LongPollingDoesNotPollAfterTransportIsBeingStoppedMidRequest()
        {
            var disconnectCts = new CancellationTokenSource();

            var mockConnection = new Mock<Client.IConnection>();
            mockConnection.SetupGet(c => c.JsonSerializer).Returns(JsonSerializer.CreateDefault());
            mockConnection.Setup(c => c.TotalTransportConnectTimeout).Returns(TimeSpan.FromSeconds(10));
            mockConnection.SetupProperty(c => c.MessageId);

            var pollingTcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            var mockHttpClient = CreateFakeHttpClient(
                (url, request, postData, isLongRunning) =>
                {
                    var responseMessage = string.Empty;
                    if (url.Contains("connect?"))
                    {
                        responseMessage = "{\"C\":\"d-C6243495-A,0|B,0|C,1|D,0\",\"S\":1,\"M\":[]}";
                    }
                    else if (url.Contains("poll?"))
                    {
                        pollingTcs.TrySetResult(null);

                        // stop polling loop
                        disconnectCts.Cancel();
                    }

                    return Task.FromResult(CreateResponse(responseMessage));
                });

            var longPollingTransport = new LongPollingTransport(mockHttpClient.Object);

            try
            {
                await longPollingTransport.Start(mockConnection.Object, string.Empty, disconnectCts.Token)
                    .OrTimeout(TimeSpan.FromSeconds(15));
            }
            catch (StartException stex) when (stex.InnerException is OperationCanceledException)
            {
                // An OCE is expected sometimes, depending on a race condition.
            }

            await pollingTcs.Task.OrTimeout(TimeSpan.FromSeconds(2));

            // give it some time to make sure a new poll was not setup after verification
            await Task.Delay(1000);

            mockHttpClient
                .Verify(c => c.Post(It.Is<string>(url => url.StartsWith("poll?")), It.IsAny<Action<Client.Http.IRequest>>(),
                    It.IsAny<IDictionary<string, string>>(), It.IsAny<bool>()), Times.Once());
        }

        [Fact]
        public async Task InitDoesNotHaveToBeFirstMessage()
        {
            var disconnectCts = new CancellationTokenSource();

            var mockConnection = new Mock<Client.IConnection>();
            mockConnection.SetupGet(c => c.JsonSerializer).Returns(JsonSerializer.CreateDefault());
            mockConnection.Setup(c => c.TotalTransportConnectTimeout).Returns(TimeSpan.FromSeconds(500));
            mockConnection.SetupProperty(c => c.MessageId);

            var mockHttpClient = CreateFakeHttpClient((url, request, postData, isLongRunning) =>
                Task.FromResult(CreateResponse(url.StartsWith("poll?")
                    ? "{\"C\":\"d-C6243495-A,0|B,0|C,1|D,0\",\"S\":1,\"M\":[]}"
                    : "{\"C\":\"d-C6243495-A,0|B,0|C,1|D,0\",\"M\":[]}")));

            var longPollingTransport = new LongPollingTransport(mockHttpClient.Object);

            await longPollingTransport.Start(mockConnection.Object, string.Empty, disconnectCts.Token).OrTimeout(TimeSpan.FromSeconds(15));

            // stop polling loop
            disconnectCts.Cancel();
        }

        [Fact]
        public async Task PollingLoopNotRestartedIfStartFails()
        {
            var disconnectCts = new CancellationTokenSource();

            var mockConnection = new Mock<Client.IConnection>();
            mockConnection.SetupGet(c => c.JsonSerializer).Returns(JsonSerializer.CreateDefault());
            mockConnection.Setup(c => c.TotalTransportConnectTimeout).Returns(TimeSpan.FromSeconds(500));
            mockConnection.SetupProperty(c => c.MessageId);

            var mockHttpClient = CreateFakeHttpClient((url, request, postData, isLongRunning) =>
                Task.FromException<IResponse>(new InvalidOperationException("Request rejected")));

            mockHttpClient.Setup(
                m => m.Get(It.IsAny<string>(), It.IsAny<Action<Client.Http.IRequest>>(), It.IsAny<bool>()))
                .Returns<string, Action<Client.Http.IRequest>, bool>(
                    (url, request, isLongRunning) => Task.FromResult(CreateResponse("{ \"Response\" : \"started\"}")));

            var longPollingTransport = new LongPollingTransport(mockHttpClient.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                longPollingTransport.Start(mockConnection.Object, string.Empty, disconnectCts.Token))
                    .OrTimeout(TimeSpan.FromSeconds(15));

            // give it some time to settle
            await Task.Delay(1000);

            mockHttpClient
                .Verify(c => c.Post(It.Is<string>(url => url.StartsWith("poll?")), It.IsAny<Action<Client.Http.IRequest>>(),
                    It.IsAny<IDictionary<string, string>>(), It.IsAny<bool>()), Times.Never());
        }

        private static Mock<IHttpClient> CreateFakeHttpClient(Func<string, Action<Client.Http.IRequest>, IDictionary<string, string>, bool, Task<Client.Http.IResponse>> postFunc)
        {
            var mockHttpClient = new Mock<IHttpClient>();
            mockHttpClient.Setup(m => m.Post(It.IsAny<string>(),
                It.IsAny<Action<Client.Http.IRequest>>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<bool>()))
                .Returns(postFunc);

            mockHttpClient.Setup(
                m => m.Get(It.IsAny<string>(), It.IsAny<Action<Client.Http.IRequest>>(), It.IsAny<bool>()))
                .Returns<string, Action<Client.Http.IRequest>, bool>(
                    (url, request, isLongRunning) => Task.FromResult(CreateResponse("{ \"Response\" : \"started\"}")));

            return mockHttpClient;
        }

        private static IResponse CreateResponse(string contents)
        {
            var mockResponse = new Mock<IResponse>();
            mockResponse.Setup(r => r.GetStream())
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(contents)));

            return mockResponse.Object;
        }
    }
}
