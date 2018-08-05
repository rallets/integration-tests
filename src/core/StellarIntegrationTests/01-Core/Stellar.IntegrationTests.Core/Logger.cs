using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Stellar.IntegrationTests.Core.Interfaces;

namespace Stellar.IntegrationTests.Core
{
    public class Logger : ILogger
    {
        public void Trace(string text)
        {
            Write(text);
        }

        public void Write(string text, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                text = $"{DateTime.Now}\t[{memberName}]\t{text}\t[{sourceFilePath}::{sourceLineNumber}]";
                //Console.WriteLine(text);
//#if DEBUG
                Debug.WriteLine(text);
//#endif
            }
            catch { }
        }

        public void Write(Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                string text = ex.ToString();
                Write(text, memberName, sourceFilePath, sourceLineNumber);
            }
            catch { }
        }
    }
}
