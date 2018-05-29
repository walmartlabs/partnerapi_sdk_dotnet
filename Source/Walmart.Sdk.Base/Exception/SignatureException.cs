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



namespace Walmart.Sdk.Base.Exception
{
    public class SignatureException : Primitive.BaseException
    { 
        /// <summary>
        /// Constructor for exception, triggered if sdk is unable to sign request 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SignatureException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Factory method to assemble correct error message from provided info
        /// </summary>
        /// <param name="consumerId"></param>
        /// <param name="fullUrl">API URL submitted for signing</param>
        /// <param name="httpMethod"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        public static SignatureException Factory(string consumerId, string fullUrl, string httpMethod, System.Exception innerException)
        {
            var exceptionMessage = string.Format("Unable to generate signature for [{0};{2};{1}]", consumerId, fullUrl, httpMethod);
            return new SignatureException(exceptionMessage, innerException);
        }
    }
}
