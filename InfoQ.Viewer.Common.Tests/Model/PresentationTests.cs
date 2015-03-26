using InfoQ.Viewer.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoQ.Viewer.Common.Tests.Model
{
    [TestFixture]
    public class PresentationTests
    {
        [Test]
        public void CanCreateInstanceOfPresentation()
        {
            Presentation instance = new Presentation();

            Assert.IsNotNull(instance, "instance not created");
            Assert.AreEqual(StorageType.Internal, instance.StorageLocation, "default should be internal");
            Assert.AreEqual(0, instance.TotalTime, "should be zero by default");
            Assert.AreEqual(0, instance.ViewedTime, "should be zero by default");
        }

        [Test]
        public void CanGetPercentageOfZeroForUnkownTotalTime()
        {
            Presentation instance = new Presentation();

            Assert.AreEqual(0, instance.Percentage, "should be zero");
        }

        [Test]
        public void CanGetPercentageMaxedOutAt100()
        {
            Presentation instance = new Presentation();
            instance.ViewedTime = 1000;
            instance.TotalTime = 500;

            Assert.AreEqual(100, instance.Percentage, "should be 100");
        }

        [Test]
        public void CanGetPercentageAsInteger()
        {
            Presentation instance = new Presentation();
            instance.ViewedTime = 500;
            instance.TotalTime = 500;

            Assert.AreEqual(100, instance.Percentage, "should be 100");

            instance.ViewedTime = 250;
            Assert.AreEqual(50, instance.Percentage, "should be 50%");

            instance.ViewedTime = 249;
            Assert.AreEqual(49, instance.Percentage, "should be 49% - round down");
        }
    }
}
