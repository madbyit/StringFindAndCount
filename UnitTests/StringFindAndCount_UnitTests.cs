using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using StringFindAndCount;

namespace UnitTests
{
    [TestClass]
    public class StringFindAndCount_UnitTest
    {
        private string filePath = Environment.CurrentDirectory + "/Test_App_Data/";
        
        [TestMethod]
        public void TestMethod_BattleCount()
        {
            string[] files = Directory.GetFiles(@filePath);
            StringFindAndCountProgram strfind = new(files);
            var res = strfind.GetKeyValue("Battle");
            Assert.IsTrue(res == 5);
        }

        [TestMethod]
        public void TestMethod_NoCount()
        {
            string[] files = Directory.GetFiles(@filePath);
            StringFindAndCountProgram strfind = new(files);
            var res = strfind.GetKeyValue("StarWars");
            Assert.IsTrue(res == 0);
        }

    }
}
