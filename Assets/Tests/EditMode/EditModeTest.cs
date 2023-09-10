using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.Editmode
{
    public class EditModeTest
    {
        [TestCase(1, 1)]
        [TestCase(0, 0)]
        [TestCase(-1,-1)]
        [TestCase(-0.5f ,-1)]
        [TestCase(0.5f ,1)]
        public void Test1(float value1, int actual) {
            Assert.That(actual, Is.EqualTo(PlayerBehavior.Round(value1)));
        }
    }
}

