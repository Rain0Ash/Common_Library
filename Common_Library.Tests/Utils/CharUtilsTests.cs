// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using Common_Library.Utils;
using NUnit.Framework;

namespace Tests.Utils
{
    [TestFixture]
    public class CharUtilsTests
    {
        [Test]
        public void IsControl()
        {
            Assert.IsTrue(CharUtils.IsControl('\0'));
            
            Assert.IsFalse(CharUtils.IsControl('	'));
            
            Assert.IsTrue(CharUtils.IsControl('\n'));
            
            Assert.IsFalse(CharUtils.IsControl('d'));
            
            Assert.IsFalse(CharUtils.IsControl(' '));
        }
    }
}