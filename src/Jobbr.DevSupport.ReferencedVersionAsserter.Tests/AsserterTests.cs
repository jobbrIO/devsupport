﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.DevSupport.ReferencedVersionAsserter.Tests
{
    [TestClass]
    public class AsserterTests
    {
        [TestMethod]
        public void WithExactVersionRule_ExactMatch_IsSuccessful()
        {
            var asserter = new Asserter("TestFiles/ExactDependencyV5.config", "TestFiles/ExactDependencyV5.nuspec");

            var result = asserter.Validate(new ExactVersionMatchRule("ExactDependency"));

            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void WithExactVersionRule_DifferentVersions_IsSuccessful()
        {
            var asserter = new Asserter("TestFiles/ExactDependencyV2.config", "TestFiles/ExactDependencyV5.nuspec");

            var result = asserter.Validate(new ExactVersionMatchRule("ExactDependency"));

            Assert.AreEqual(1, result.Messages.Count);
            Assert.IsFalse(result.IsSuccessful);
        }
    }
}