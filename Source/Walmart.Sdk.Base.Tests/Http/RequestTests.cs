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
    using System.Collections.Generic;
    using Xunit;
    using Moq;
    using Walmart.Sdk.Base.Primitive;
    using System.Threading.Tasks;

    public class RequestTests
    {
        // this is random key generate for this test only
        private const string PRIVATE_KEY_EXAMPLE =
            "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAKvx1Q4XAKH2pSP7nIa3+ZisAm4IVb0xLVnM6r8QqpU77YdXMRyKxqIMKlo/Ysm9efL8afkxL+lJWwKYRIEiQ2yyTIIA2E/8/1MC/5NI8jihSGlchZSTp4S0xsE9GuAvEu/xvSn0zimIB1r5baq+Gq1W3COR1hCTz8njv7zJqwd5AgMBAAECgYBRvznh5wG//vlocBls207pTO4izgelWRoMlaYNjKjtJn65V7TbswbYyBviqDwZWeH+qg7gEIiMyvlf9HCtTFLtNSDCu9PwmhkyFJP6vPl4JlWZE1JSfVLiJkitws5vE+IY0jzGYyhA8R259vMaytkOTYWmq1G4saZwxmo1byO3GQJBANHi/oQKVrt2IHVQMDzn5GzXeeQ35QMXh7ZenZPzS5+/BIzCskl8bnFw2OIHf8KeER86rV/OT6LagDaOwpLjQUsCQQDRuM0M6TcKmqnjIgqNBDC+0f1p2REYANVuc85QspD5mk9dxJSfpaI7MZrAN2+z9Tbj+U6/7+TNRi5PQXoL+yPLAkBICrEv41iX6dkES2zzSulWDrQRCLp70DTN/TX7VZRMlcVbB20o2iQSnhhjpQ1OYPEhlgNykh81l+hQUboprwV3AkEAzQvuD42FbyzApX4G6tEKB8ewCOleOSW3h4JaWbP84s3ogzlMRrre3xkkwIJzCHPb8xEx2Z9nCPxxErRl64jj+QJATAuVPnQ/Hk2IOjNPh3e2evaB49lcmTFdbUOYMZFzj2x/KeAu3r+6a/FnZBPICokg9XtIZxbmaZJ3+MG4IW7HbQ==";

        private readonly string[] walmartHeaders =
        {
            "WM_SEC.AUTH_SIGNATURE", "WM_SEC.TIMESTAMP", "WM_CONSUMER.CHANNEL.TYPE",
            "WM_CONSUMER.ID", "WM_SVC.NAME", "WM_QOS.CORRELATION_ID"
        };

        [Fact]
        public void AddWalmartSpecificHeadersOnFinalize()
        {
            var config = new Mock<Base.Primitive.Config.IRequestConfig>();
            config.Setup(t => t.UserAgent).Returns("test user-agent");
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");
            config.Setup(t => t.Credentials).Returns(new Credentials("test", PRIVATE_KEY_EXAMPLE));
            var request = new Base.Http.Request(config.Object);
            request.FinalizePreparation();

            foreach (var name in walmartHeaders)
            {
                Assert.True(request.HttpRequest.Headers.Contains(name));
                IEnumerable<string> values = new List<string>();
                Assert.True(request.HttpRequest.Headers.TryGetValues(name, out values));
                Assert.NotEmpty(values);
            }
        }

        [Fact]
        public void InvalidCredentialsResultsInException()
        {
            var config = new Mock<Base.Primitive.Config.IRequestConfig>();
            config.Setup(t => t.UserAgent).Returns("test user-agent");
            config.Setup(t => t.BaseUrl).Returns("http://www.test.com");
            config.Setup(t => t.Credentials).Returns(new Credentials("test", "invalid-key"));
            var request = new Base.Http.Request(config.Object);

            Assert.Throws<Base.Exception.SignatureException>(
                () => request.FinalizePreparation());
        }
    }
}
