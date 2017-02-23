using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnimmtGame;

namespace SnimmtTest
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void TestEquals()
        {
            var card1 = new Card(55);
            var card2 = new Card(55);

            var card3 = new Card(104);

            Assert.AreEqual(card1, card2);
            Assert.AreNotEqual(card1, card3);
        }
    }
}
