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
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Walmart.Sdk.Base;
using Walmart.Sdk.Base.Primitive;

namespace Walmart.Sdk.Base.Util
{
    /// <summary>
    /// Util class for calculating the Digital Authentication Signature
    /// </summary>
    public static class DigitalSignature
    {
        public static string GetCurrentTimestamp()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            return ((long)t.TotalMilliseconds).ToString();
        }

        public static string GetCorrelationId() => Guid.NewGuid().ToString();

        internal static string SignData(string stringToBeSigned, string privateKey)
        {
            byte[] encodedKeyBytes = Convert.FromBase64String(privateKey);
            byte[] data = Encoding.UTF8.GetBytes(stringToBeSigned);

            AsymmetricKeyParameter asymmetricKeyParameter = PrivateKeyFactory.CreateKey(encodedKeyBytes);
            RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
            ISigner signer = SignerUtilities.GetSigner("SHA-256withRSA");
            signer.Init(true, rsaKeyParameters);
            var stringToSignInBytes = Encoding.UTF8.GetBytes(stringToBeSigned);
            signer.BlockUpdate(stringToSignInBytes, 0, stringToSignInBytes.Length);
            var signature = signer.GenerateSignature();
            return Convert.ToBase64String(signature);
        }
    }
}
