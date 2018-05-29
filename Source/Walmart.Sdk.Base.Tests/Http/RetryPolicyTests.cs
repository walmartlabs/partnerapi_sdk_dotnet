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

namespace Walmart.Sdk.Base.Test.Http.RetryPolicy
{
    using System.Threading.Tasks;
    using Xunit;
    using Moq;

    public class RetryPolicyTests
    {
        [Fact]
        public void FixedCountAttemptsCountShouldBePositive()
        {
            Assert.Throws<Base.Exception.InitException>(
                () => new Base.Http.Retry.FixedCountPolicy(-1)
            );
        }

        [Fact]
        public async Task FixedCountNonHttpExceptionBubblesUp()
        {
            var fetcher = new Mock<Base.Http.Fetcher.IFetcher>();
            var request = new Mock<Base.Http.IRequest>();

            var policy = new Base.Http.Retry.FixedCountPolicy(1);

            fetcher.Setup(t => t.ExecuteAsync(It.IsAny<Base.Http.IRequest>()))
                .ThrowsAsync(new Base.Http.Exception.ClientException("test"));

            await Assert.ThrowsAsync<Base.Http.Exception.ClientException>(
                () => policy.GetResponse(fetcher.Object, request.Object)
            );
        }

        [Fact]
        public async Task FixedCountReturnResultForSuccessWithoutRetries()
        {
            var fetcher = new Mock<Base.Http.Fetcher.IFetcher>();
            var request = new Mock<Base.Http.IRequest>();
            var response = new Mock<Base.Http.IResponse>();

            fetcher.Setup(t => t.ExecuteAsync(It.IsAny<Base.Http.IRequest>())).ReturnsAsync(response.Object);

            var policy = new Base.Http.Retry.FixedCountPolicy(1);
            var result = await policy.GetResponse(fetcher.Object, request.Object);
            Assert.Equal(result, response.Object);
        }

        [Fact]
        public async Task FixedCountThrowsExceptionWhenAllRetriesSpend()
        {
            var fetcher = new Mock<Base.Http.Fetcher.IFetcher>();
            var request = new Mock<Base.Http.IRequest>();

            fetcher.Setup(t => t.ExecuteAsync(It.IsAny<Base.Http.IRequest>()))
                .ThrowsAsync(new Base.Http.Exception.HttpException("test"));

            var policy = new Base.Http.Retry.FixedCountPolicy(1);
            await Assert.ThrowsAsync<Base.Http.Exception.NoRetriesLeftException>(
                () => policy.GetResponse(fetcher.Object, request.Object)
            );
        }

        [Fact]
        public async Task FixedCountMultipleAttemptsGiveResult()
        {
            var fetcher = new Mock<Base.Http.Fetcher.IFetcher>();
            var request = new Mock<Base.Http.IRequest>();
            var response = new Mock<Base.Http.IResponse>();

            fetcher.SetupSequence(t => t.ExecuteAsync(It.IsAny<Base.Http.IRequest>()))
                .ThrowsAsync(new Base.Http.Exception.HttpException("test"))
                .ThrowsAsync(new Base.Http.Exception.HttpException("test"))
                .Returns(Task.FromResult(response.Object));

            var policy = new Base.Http.Retry.FixedCountPolicy(3);
            var result = await policy.GetResponse(fetcher.Object, request.Object);

            Assert.Equal(result, response.Object);
        }


        [Fact]
        public async Task LuckyMeNonHttpExceptionBubblesUp()
        {
            var fetcher = new Mock<Base.Http.Fetcher.IFetcher>();
            var request = new Mock<Base.Http.IRequest>();

            var policy = new Base.Http.Retry.LuckyMePolicy();

            fetcher.Setup(t => t.ExecuteAsync(It.IsAny<Base.Http.IRequest>()))
                .ThrowsAsync(new Base.Http.Exception.ClientException("test"));

            await Assert.ThrowsAsync<Base.Http.Exception.ClientException>(
                () => policy.GetResponse(fetcher.Object, request.Object)
            );
        }

        [Fact]
        public async Task LuckyMeBubbleUpHttpException()
        {
            var fetcher = new Mock<Base.Http.Fetcher.IFetcher>();
            var request = new Mock<Base.Http.IRequest>();

            fetcher.Setup(t => t.ExecuteAsync(It.IsAny<Base.Http.IRequest>()))
                .ThrowsAsync(new Base.Http.Exception.HttpException("test"));

            var policy = new Base.Http.Retry.LuckyMePolicy();
            await Assert.ThrowsAsync<Base.Http.Exception.HttpException>(
                () => policy.GetResponse(fetcher.Object, request.Object)
            );
        }

        [Fact]
        public async Task LuckyMeFetchResult()
        {
            var fetcher = new Mock<Base.Http.Fetcher.IFetcher>();
            var request = new Mock<Base.Http.IRequest>();
            var response = new Mock<Base.Http.IResponse>();

            fetcher.Setup(t => t.ExecuteAsync(It.IsAny<Base.Http.IRequest>()))
                .Returns(Task.FromResult(response.Object));

            var policy = new Base.Http.Retry.LuckyMePolicy();
            var result = await policy.GetResponse(fetcher.Object, request.Object);
            Assert.Equal(result, response.Object);
        }
    }
}
