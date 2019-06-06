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
    using Xunit;
    using Moq;
    using Walmart.Sdk.Base.Http.Fetcher;
    using System.Threading.Tasks;

    public class HandlerTests
    {
        [Fact]
        public void SimulationModeSwitchFetchers()
        {
            var config = new Primitive.BaseConfig("test", "test", "test");
            config.BaseUrl = "http://www.test.com";
            config.RequestTimeoutMs = 1000;

            var handlerFactory = new Base.Http.HttpFactory();
            var handler = handlerFactory.GetHttpHandler(config);

            Assert.IsType<Base.Http.Fetcher.HttpFetcher>(handler.Fetcher);

            handler.SimulationEnabled = true;

            Assert.IsType<Base.Http.Fetcher.LocalFetcher>(handler.Fetcher);
        }

        [Fact]
        public async Task GetRequestGoThroughRetryPolicy()
        {
            var response = new Mock<Base.Http.IResponse>();
            var request = new Mock<Base.Http.IRequest>();
            var policy = new Mock<Base.Http.Retry.IRetryPolicy>();
            policy.Setup(t => t.GetResponse(It.IsAny<IFetcher>(), It.IsAny<Base.Http.IRequest>()))
                .ReturnsAsync(response.Object);
            var config = new Mock<Base.Primitive.Config.IHttpConfig>();
            config.Setup(t => t.RequestTimeoutMs).Returns(1000);
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");

            var handler = new Base.Http.Handler(config.Object);
            handler.RetryPolicy = policy.Object;
            var result = await handler.GetAsync(request.Object);
            Assert.Equal(response.Object, result);
        }

        [Fact]
        public async Task PostRequestGoThroughRetryPolicy()
        {
            var response = new Mock<Base.Http.IResponse>();
            var request = new Mock<Base.Http.IRequest>();
            var policy = new Mock<Base.Http.Retry.IRetryPolicy>();
            policy.Setup(t => t.GetResponse(It.IsAny<IFetcher>(), It.IsAny<Base.Http.IRequest>()))
                .ReturnsAsync(response.Object);
            var config = new Mock<Base.Primitive.Config.IHttpConfig>();
            config.Setup(t => t.RequestTimeoutMs).Returns(1000);
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");

            var handler = new Base.Http.Handler(config.Object);
            handler.RetryPolicy = policy.Object;
            var result = await handler.PostAsync(request.Object);
            Assert.Equal(response.Object, result);
        }

        [Fact]
        public async Task PutRequestGoThroughRetryPolicy()
        {
            var response = new Mock<Base.Http.IResponse>();
            var request = new Mock<Base.Http.IRequest>();
            var policy = new Mock<Base.Http.Retry.IRetryPolicy>();
            policy.Setup(t => t.GetResponse(It.IsAny<IFetcher>(), It.IsAny<Base.Http.IRequest>()))
                .ReturnsAsync(response.Object);
            var config = new Mock<Base.Primitive.Config.IHttpConfig>();
            config.Setup(t => t.RequestTimeoutMs).Returns(1000);
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");

            var handler = new Base.Http.Handler(config.Object);
            handler.RetryPolicy = policy.Object;
            var result = await handler.PutAsync(request.Object);
            Assert.Equal(response.Object, result);
        }
    }
}
