using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace StarbucksTest
{
    public class Testing
    {
        private readonly ITestOutputHelper testOutputHelper;

        public Testing(ITestOutputHelper _testOutputHelper)
        {
            testOutputHelper = _testOutputHelper;
        }
        [Fact]
        public void ReplaceCharacters()
        {
            string message = "Oatmeal &amp; Yogurt";

            string newMessage = message.Replace("&amp;", "&");

            testOutputHelper.WriteLine("Old Message: " + message);
            testOutputHelper.WriteLine("New Message: " + newMessage);
        }
    }
}
