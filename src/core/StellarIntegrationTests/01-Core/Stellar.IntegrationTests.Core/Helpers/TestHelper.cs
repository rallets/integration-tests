using System;
using System.Diagnostics;
using Stellar.IntegrationTests.Core.Interfaces;

namespace Stellar.IntegrationTests.Core.Helpers
{
    public class TestHelper
    {
        private ILogger _logger;

        public TestHelper(ILogger logger)
        {
            _logger = logger;
        }

        public bool WaitForEvent(int timeoutMs, Func<bool> action, int maxIterations = int.MaxValue)
		{
			var result = false;
            int iterations = 0;

			Stopwatch s = new Stopwatch();
			s.Start();
			do
			{
				result = action();
			    iterations++;
			}
			while (iterations < maxIterations && s.Elapsed < TimeSpan.FromMilliseconds(timeoutMs) && !result);

			s.Stop();

		    if (result)
		    {
		        _logger.Write($"Event completed in {s.ElapsedMilliseconds} ms, iterations: {iterations}");
		    }
		    else
		    {
		        _logger.Write($"Event timed out after {s.ElapsedMilliseconds} ms, iterations: {iterations}");
            }

		    return result;
		}
		
	}
}
