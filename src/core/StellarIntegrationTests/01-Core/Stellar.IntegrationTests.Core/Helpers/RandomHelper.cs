using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stellar.IntegrationTests.Core.Helpers
{
    public class RandomHelper
    {
        public decimal NextDecimal(int min = 0, int max = int.MaxValue)
        {
            var rnd = new Random();
            var result = (decimal)(rnd.Next(0, int.MaxValue) * rnd.NextDouble());
            return result;
        }
    }
}
