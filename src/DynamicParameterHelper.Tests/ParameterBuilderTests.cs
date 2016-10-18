using System.Data;
using System.Linq;
using NUnit.Framework;
using DynamicParameterHelper.Attributes;
using DynamicParameterHelper.Enums;
using Moq;

namespace DynamicParameterHelper.Tests
{
    [TestFixture]
    public class ParameterBuilderTests
    {
        private TestClass _test;
        [SetUp]
        public void Setup()
        {
            _test = Mock.Of<TestClass>();
        }

        [TearDown]
        public void TearDown()
        {
            _test = null;
        }

        [Test]
        public void DoDynamicParametersPopulateCorrectlyForCreate()
        {
            var dynamicParameters = _test.ToDynamicParameters(CrudType.Create);

            Assert.AreEqual(dynamicParameters.ParameterNames.Count(), 1);
        }

        [Test]
        public void DoDynamicParametersPopulateCorrectlyUpdate()
        {
            var dynamicParameters = _test.ToDynamicParameters(CrudType.Update);
            Assert.AreEqual(dynamicParameters.ParameterNames.Count(), 2);
        }
    }

    public class TestClass
    {
        [DynamicParameter(true, "@id", DbType.Int32)]
        public int Id { get; set; }
        [DynamicParameter(false, "@description", DbType.AnsiString, Scalar = 10)]
        public string Description { get; set; }
        public string NotIncluded { get; set; }
    }

}