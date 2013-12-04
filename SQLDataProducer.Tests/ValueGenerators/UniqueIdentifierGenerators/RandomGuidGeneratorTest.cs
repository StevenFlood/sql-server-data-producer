
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.UniqueIdentifierGenerators;

namespace SQLDataProducer.Tests.ValueGenerators
{
    [TestFixture]
    [MSTest.TestClass]
    public class RandomGuidGeneratorTest
    {
        public RandomGuidGeneratorTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateValue()
        {
            var gen = new RandomGuidGenerator(new ColumnDataTypeDefinition("UniqueIdentifier", false));
            var firstValue = gen.GenerateValue(1);
            Assert.That(firstValue, Is.Not.Null);
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldTestStep()
        {
            
        }
        [Test]
        [MSTest.TestMethod]
        public void ShouldTestStartValue()
        {
            
        }
        [Test]
        [MSTest.TestMethod]
        public void ShouldTestOverFlow()
        {
            
        }
    }
}