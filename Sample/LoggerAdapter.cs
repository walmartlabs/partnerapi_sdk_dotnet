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

using Walmart.Sdk.Base.Primitive;

namespace Walmart.Sdk.Marketplace.Sample
{
    class LoggerAdapter: ILoggerAdapter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ApiClient));

        public void Info(string message) => log.Info(message);

        public void Debug(string message)
        {
            ConsoleWriter.WriteLine(message);
        }
        public void Warning(string message) => log.Warn(message);
        public void Error(string message) => log.Error(message);
        public void Fatal(string message) => log.Fatal(message);

        public bool IsLevelEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.INFO:
                    return log.IsInfoEnabled;
                case LogLevel.DEBUG:
                    return log.IsDebugEnabled;
                case LogLevel.WARNING:
                    return log.IsWarnEnabled;
                case LogLevel.ERROR:
                    return log.IsErrorEnabled;
                case LogLevel.FATAL:
                    return log.IsFatalEnabled;
                default:
                    return false;
            }
        }
    }
}
