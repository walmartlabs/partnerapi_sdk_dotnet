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

namespace Walmart.Sdk.Marketplace.V3.Payload
{
    using System;
    using Walmart.Sdk.Marketplace.V3.Api.Exception;
    using Walmart.Sdk.Base.Http;
    using Walmart.Sdk.Base.Primitive;

    public class PayloadFactory: Base.Primitive.BasePayloadFactory
    {
        public override Exception CreateApiException(ApiFormat format, string content, IResponse response)
        {
            try
            {
                var errors = GetSerializer(format).Deserialize<Feed.Errors>(content);
                return ApiException.Factory(errors, response);
            }
            catch (Exception firstAttemptEx)
            {
                try
                {
                    var errors = GetSerializer(format).Deserialize<Feed.ErrorsWithoutNS>(content);
                    return ApiException.Factory(errors, response);
                }
                catch (Exception secondAttempEx)
                {
                    var exceptionList = new Exception[] { firstAttemptEx, secondAttempEx };
                    var aggrEx = new AggregateException("Unable to parse error response >" + content + "<", exceptionList);
                    throw aggrEx;
                }
            }
        }

    }
}
