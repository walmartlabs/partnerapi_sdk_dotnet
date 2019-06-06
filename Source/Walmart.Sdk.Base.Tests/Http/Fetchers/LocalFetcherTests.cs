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

using Xunit;
using Moq;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Walmart.Sdk.Base.Http;
using Walmart.Sdk.Base.Primitive;
using System.Net.Http;

namespace Walmart.Sdk.Base.Test.Http
{
    public class LocalFetcherTests
    {
        public const string MOCK_FOLDER = "/Http/Fetchers/LocalFetcherMocks/";
        public Primitive.Config.IHttpConfig config;
        public LocalFetcherTests()
        {
            config = new Primitive.BaseConfig("test", "test-key", "Test");
        }

        [Fact]
        public void DefaultMappingLoadingFineFromAssembly()
        {
            var fakeFetcher = new Base.Http.Fetcher.LocalFetcher(config);
            Assert.False(fakeFetcher is null);
        }

        [Fact]
        public async Task CanSetCustomFolderAndJSONOutputForMocks()
        {
            var fullDir = Directory.GetCurrentDirectory() + MOCK_FOLDER;
            config.ApiFormat = ApiFormat.JSON; 

            var request = new Mock<Base.Http.IRequest>();
            request.Setup(t => t.Method).Returns(HttpMethod.Get);
            request.Setup(t => t.EndpointUri).Returns("/feeds");


            var fakeFetcher = new Base.Http.Fetcher.LocalFetcher(config);
            fakeFetcher.SetCustomMockFolder(fullDir);

            var result = await fakeFetcher.ExecuteAsync(request.Object);

            Assert.IsType<Response>(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(
                "{\"custom\": \"folder\"}",
                await result.GetPayloadAsString()
            );
        }

        [Fact]
        public async Task CanSetCustomFolderAndXMLOutputForMocks()
        {
            var fullDir = Directory.GetCurrentDirectory() + MOCK_FOLDER;

            var request = new Mock<Base.Http.IRequest>();
            request.Setup(t => t.Method).Returns(HttpMethod.Get);
            request.Setup(t => t.EndpointUri).Returns("/feeds");


            var fakeFetcher = new Base.Http.Fetcher.LocalFetcher(config);
            fakeFetcher.SetCustomMockFolder(fullDir);

            var result = await fakeFetcher.ExecuteAsync(request.Object);

            Assert.IsType<Response>(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(
                "<xml>custom mock folder</xml>",
                await result.GetPayloadAsString()
            );
        }

        [Fact]
        public void UsingWrongFolderIsNoGood()
        {
            var fakeFetcher = new Base.Http.Fetcher.LocalFetcher(config);
            Assert.Throws<Base.Exception.InitException>( 
                () => fakeFetcher.SetCustomMockFolder("WRONG-FOLDER")
            );
        }

        [Fact]
        public void UsingValidFolderWithoutMappingFileIsStillNoGood()
        {
           

            var fakeFetcher = new Base.Http.Fetcher.LocalFetcher(config);
            Assert.Throws<Base.Exception.InitException>(
                () => fakeFetcher.SetCustomMockFolder(Directory.GetCurrentDirectory())
            );
        }

        [Fact]
        public void RequestingCustomPayloadMissingInMappingNoGood()
        {
            var fullDir = Directory.GetCurrentDirectory() + MOCK_FOLDER;

            var request = new Mock<Base.Http.IRequest>();
            request.Setup(t => t.Method).Returns(HttpMethod.Get);
            request.Setup(t => t.EndpointUri).Returns("/invalid-get-request");

            var fakeFetcher = new Base.Http.Fetcher.LocalFetcher(config);
            fakeFetcher.SetCustomMockFolder(fullDir);

            Assert.ThrowsAsync<Base.Http.Exception.GatewayException>(
                () => fakeFetcher.ExecuteAsync(request.Object)
            );
        }

        [Fact]
        public void RequestingDefaultPayloadMissingInMappingNoGood()
        {
            var fullDir = Directory.GetCurrentDirectory() + MOCK_FOLDER;

            var request = new Mock<Base.Http.IRequest>();
            request.Setup(t => t.Method).Returns(HttpMethod.Get);
            request.Setup(t => t.EndpointUri).Returns("/invalid-get-request");

            var fakeFetcher = new Base.Http.Fetcher.LocalFetcher(config);

            Assert.ThrowsAsync<Base.Http.Exception.GatewayException>(
                () => fakeFetcher.ExecuteAsync(request.Object)
            );
        }

    }
}
