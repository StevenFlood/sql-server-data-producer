
using NUnit.Framework;
using MSTest = Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.Generators.StringGenerators;

namespace SQLDataProducer.Tests.ValueGenerators
{
    [TestFixture]
    [MSTest.TestClass]
    public class NameStringGeneratorTest
    {
        public NameStringGeneratorTest()
        {
        }

        [Test]
        [MSTest.TestMethod]
        public void ShouldGenerateValue()
        {
            var gen = new NameStringGenerator(new ColumnDataTypeDefinition("VarChar(123)", false));
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