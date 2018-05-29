/**
Copyright (c) 2018-present, Walmart Inc.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

namespace Walmart.Sdk.Base.Test.Http
{
    using System.Threading.Tasks;
    using Xunit;
    using Moq;
    using Walmart.Sdk.Base.Http;
    using Walmart.Sdk.Base.Http.Fetcher;
    using System.Net;
    using System.Net.Sockets;

    public class HttpFetcherTests
    { 

        [Fact]
        public async Task EmptyBaseUriTriggersException()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var config = factory.Create<Primitive.Config.IHttpConfig>();
            config.Setup(t => t.RequestTimeoutMs).Returns(1000);
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");
            var client = factory.Create<IHttpClient>();
            var request = factory.Create<Base.Http.IRequest>();
            request.Setup(t => t.EndpointUri).Returns("");

            var instance = new Base.Http.Fetcher.HttpFetcher(config.Object, client.Object);

            // should trigger InitException
            await Assert.ThrowsAsync<Base.Exception.InvalidValueException>(
                () => instance.ExecuteAsync(request.Object)
            );

            factory.Verify();
        }

        [Fact]
        public async Task VerifyThrownExceptionFor503Response()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var config = factory.Create<Primitive.Config.IHttpConfig>();
            config.Setup(t => t.RequestTimeoutMs).Returns(1000);
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");
            var client = factory.Create<IHttpClient>();
            var request = factory.Create<Base.Http.IRequest>();
            request.Setup(t => t.EndpointUri).Returns("/api/uri");
            var response = factory.Create<IResponse>();
            response.Setup(t => t.StatusCode).Returns(HttpStatusCode.ServiceUnavailable);

            var instance = new Base.Http.Fetcher.HttpFetcher(config.Object, client.Object);
            client.Setup(t => t.SendAsync(It.IsAny<IRequest>())).ReturnsAsync(response.Object);

            await Assert.ThrowsAsync<Base.Http.Exception.GatewayException>(
                () => instance.ExecuteAsync(request.Object)
            );
        }

        [Fact]
        public async Task VerifyBehaviorForApiThrottling()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            
            var config = factory.Create<Primitive.Config.IHttpConfig>();
            config.Setup(t => t.RequestTimeoutMs).Returns(1000);
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");
            var client = factory.Create<IHttpClient>();
            var request = factory.Create<Base.Http.IRequest>();
            request.Setup(t => t.EndpointUri).Returns("/api/uri");
            var response = factory.Create<IResponse>();
            response.Setup(t => t.StatusCode).Returns((System.Net.HttpStatusCode)429);

            var instance = new Base.Http.Fetcher.HttpFetcher(config.Object, client.Object);
            client.Setup(t => t.SendAsync(It.IsAny<IRequest>())).ReturnsAsync(response.Object);

            await Assert.ThrowsAsync<Base.Http.Exception.ThrottleException>(
                () => instance.ExecuteAsync(request.Object)
            );
        }

        [Fact]
        public async Task VerifyTimeoutBehavior()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var config = factory.Create<Primitive.Config.IHttpConfig>();
            config.Setup(t => t.RequestTimeoutMs).Returns(1000);
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");
            var client = factory.Create<IHttpClient>();
            var request = factory.Create<Base.Http.IRequest>();
            request.Setup(t => t.EndpointUri).Returns("/api/uri");
            var response = factory.Create<IResponse>();
            response.Setup(t => t.StatusCode).Returns(HttpStatusCode.ServiceUnavailable);

            var instance = new Base.Http.Fetcher.HttpFetcher(config.Object, client.Object);
            client.Setup(t => t.SendAsync(It.IsAny<IRequest>())).ThrowsAsync(new TaskCanceledException());

            await Assert.ThrowsAsync<Base.Http.Exception.ConnectionException>(
                () => instance.ExecuteAsync(request.Object)
            );
        }

        [Fact]
        public async Task VerifySocketErrorBehavior()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var config = factory.Create<Primitive.Config.IHttpConfig>();
            config.Setup(t => t.RequestTimeoutMs).Returns(1000);
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");
            var client = factory.Create<IHttpClient>();
            var request = factory.Create<Base.Http.IRequest>();
            request.Setup(t => t.EndpointUri).Returns("/api/uri");
            var response = factory.Create<IResponse>();
            response.Setup(t => t.StatusCode).Returns(HttpStatusCode.ServiceUnavailable);

            var instance = new Base.Http.Fetcher.HttpFetcher(config.Object, client.Object);
            client.Setup(t => t.SendAsync(It.IsAny<IRequest>())).ThrowsAsync(new System.Exception("test", new SocketException()));

            await Assert.ThrowsAsync<Base.Http.Exception.ConnectionException>(
                () => instance.ExecuteAsync(request.Object)
            );
        }

        [Fact]
        public void ValidRequestFlowWorks()
        {
            var factory = new MockRepository(MockBehavior.Loose);
            var config = factory.Create<Primitive.Config.IHttpConfig>();
            config.Setup(t => t.RequestTimeoutMs).Returns(1000);
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");
            var client = factory.Create<IHttpClient>();
            var request = factory.Create<Base.Http.IRequest>();
            request.Setup(t => t.EndpointUri).Returns("/api/uri");
            var response = factory.Create<IResponse>();

            var instance = new Base.Http.Fetcher.HttpFetcher(config.Object, client.Object);
            client.Setup(t => t.SendAsync(It.IsAny<IRequest>())).ReturnsAsync(response.Object);

            var task = instance.ExecuteAsync(request.Object);
            task.Wait();

            client.Verify(t => t.SendAsync(It.IsAny<IRequest>()), Times.Once);
            request.Verify(t => t.EndpointUri, Times.Once);
            factory.VerifyAll();
        }
    }
}
