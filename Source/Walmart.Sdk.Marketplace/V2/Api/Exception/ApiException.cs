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

using System;
using Walmart.Sdk.Base.Http;
using Walmart.Sdk.Base.Primitive;

namespace Walmart.Sdk.Marketplace.V2.Api.Exception
{
    public class ApiException : BaseException
    {
        public IErrorsPayload Details { get; private set; }
        public IResponse Response { get; private set; }

        protected ApiException(string message) : base(message)
        { }

        public static ApiException Factory(IErrorsPayload errorDetails, IResponse errorResponse)
        {
            var httpResponse = errorResponse.RawResponse;
            var exceptionMessage = string.Format("API Error Occured [{0} {1}]", ((int)httpResponse.StatusCode).ToString(), httpResponse.ReasonPhrase);
            exceptionMessage += errorDetails.RenderErrors();
            var exception = new ApiException(exceptionMessage)
            {
                Details = errorDetails,
                Response = errorResponse
            };
            
            return exception;
        }
    }
}
