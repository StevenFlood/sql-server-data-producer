
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.IntGenerators;

namespace SQLDataProducer.Tests.ValueGenerators
{
    [TestFixture]
    [MSTest.TestClass]
    public class RandomWeibullNumbersIntGeneratorTest
    {
        public RandomWeibullNumbersIntGeneratorTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateValue()
        {
            var gen = new RandomWeibullNumbersIntGenerator(new ColumnDataTypeDefinition("TinyInt", false));
            var firstValue = gen.GenerateValue(1);
            var secondValue = gen.GenerateValue(1);
            Assert.That(firstValue, Is.Not.Null);
            Assert.That(firstValue, Is.Not.EqualTo(secondValue));
        }

    }
}