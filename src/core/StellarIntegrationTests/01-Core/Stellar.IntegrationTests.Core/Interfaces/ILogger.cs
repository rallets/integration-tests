using System;
using System.Runtime.CompilerServices;

namespace Stellar.IntegrationTests.Core.Interfaces
{
    public interface ILogger
    {
        void Trace(string text);
        void Write(string text, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
        void Write(Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
    }
}
