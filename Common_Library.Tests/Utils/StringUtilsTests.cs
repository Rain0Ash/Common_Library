// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Linq;
using Common_Library.Utils;
using NUnit.Framework;

namespace Tests.Utils
{
    [TestFixture]
    public class StringUtilsTests
    {
        [Test]
        public void GetFormatVariables()
        {
            Assert.Throws<ArgumentNullException>(() => StringUtils.GetFormatVariables(null));
            
            Assert.IsTrue(StringUtils.GetFormatVariables("").SequenceEqual(new String[0]));
            
            Assert.IsTrue(StringUtils.GetFormatVariables("{").SequenceEqual(new String[0]));
            
            Assert.IsTrue(StringUtils.GetFormatVariables("}").SequenceEqual(new String[0]));
            
            Assert.IsTrue(StringUtils.GetFormatVariables("{}").SequenceEqual(new String[0]));
            
            Assert.IsTrue(StringUtils.GetFormatVariables("{ }").SequenceEqual(new []{" "}));
            
            Assert.IsTrue(StringUtils.GetFormatVariables("Question {question} is {result}").SequenceEqual(new []{"question", "result"}));
        }

        [Test]
        public void IsBracketsWellFormed()
        {
            Assert.False(StringUtils.IsBracketsWellFormed(null));

            Assert.True(StringUtils.IsBracketsWellFormed(String.Empty));
            
            Assert.True(StringUtils.IsBracketsWellFormed("Test"));
            
            Assert.True(StringUtils.IsBracketsWellFormed("{Test}"));
            
            Assert.False(StringUtils.IsBracketsWellFormed("[{Test]}"));
        }

        [Test]
        public void SplitByUpperCase()
        {
            
        }
    }
}