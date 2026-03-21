using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utils;

namespace Tests
{
    public class DatabaseTest
    {
        [Fact]
        public void PatternTest()
        {
            Database db = new Database();

            int id = db.GetPatternId("glider");
            Assert.Equal(id, 1);
        }
    }
}
