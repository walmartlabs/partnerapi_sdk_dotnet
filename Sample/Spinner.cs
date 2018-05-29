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
using System.Threading;

namespace Walmart.Sdk.Marketplace.Sample
{
    public class Spinner : IDisposable
    {
        private readonly string Message = "Please wait ";
        private const string Sequence = @"/-\|";
        private int Counter = 0;
        private readonly int Left;
        private readonly int Top;
        private readonly int Delay;
        private readonly Thread Thread;
        private readonly ConsoleColor OriginalColor;
        private bool Alive = false;

        public Spinner(string message = null, int delay = 100)
        {
            if (message != null)
                Message = message;
            this.OriginalColor = Console.ForegroundColor;
            Left = Console.CursorLeft;
            Top = Console.CursorTop;
            Delay = delay;
            Thread = new Thread(Spin);
        }

        public void Start()
        {
            ConsoleWriter.Buffering = true;
            Alive = true;
            if (!Thread.IsAlive)
                Thread.Start();
        }

        public void Stop()
        {
            Alive = false;
            Draw(new String(' ', Message.Length + 1));
            Console.ForegroundColor = OriginalColor;
            ConsoleWriter.Buffering = false;
        }

        private void Spin()
        {
            while (Alive)
            {
                Turn();
                Thread.Sleep(Delay);
            }
        }

        private void Draw(string message)
        {
            Console.SetCursorPosition(Left, Top);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(message);
            Console.SetCursorPosition(Left, Top);
        }

        private void Turn()
        {
            Draw(Message + Sequence[++Counter % Sequence.Length]);
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
