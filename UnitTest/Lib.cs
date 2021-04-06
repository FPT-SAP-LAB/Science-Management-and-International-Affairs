//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections;

namespace UnitTest
{
    [TestFixture]
    public class Lib
    {
        [Test]
        public void TestMethod1()
        {
            Assert.AreEqual(1,1);
        }

        [Test]
        public void ClassicModel()
        {
            //Doc: https://docs.nunit.org/articles/nunit/writing-tests/assertions/assertion-models/classic.html
            
            //For example:
            Assert.AreEqual(1, 1, "Message error 1");
        }
        [Test]
        public void ConstraintModel()
        {
            //Doc: https://docs.nunit.org/articles/nunit/writing-tests/assertions/assertion-models/constraint.html

            //For example:
            int[] array = new int[] { 1, 2, 3 };
            //check array has exactly one value = 3
            Assert.That(array, Has.Exactly(1).EqualTo(3));
        }
        [Test]
        public void MultiAssert()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(5.2, 5.2, "Message error 1");
                Assert.AreEqual(3.9, 3.9, "Message error 2");
            });
        }

        [TestCase(1, 4, 9)]
        [TestCase(10, 7, 17)]
        public void TestPreparedTCs(int a,int b,int c)
        {
            Assert.AreEqual(a+b,c,"Message error");
        }
    }
}
