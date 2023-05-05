using jsGrid2Mvc.Model;

namespace jsGrid2Mvc.Tests
{
    public class GridTests
    {
        Grid _TestGrid;

        [SetUp]
        public void Setup()
        {
            this._TestGrid = new Grid("testGrid");
        }

        [Test]
        public void Constructor_ThrowEx_IfIdIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Grid(""));
            Assert.Pass();
        }

        [Test]
        public void UseStaticData_ThrowEx_IfjsonArrayIsNull()
        {
            //********* GIVEN
            string jsonArray = "";

            //********* WHEN
            void testCode() => this._TestGrid.UseStaticData(jsonArray);

            //********* ASSERT
            Assert.Throws<ArgumentNullException>(testCode);
            Assert.Pass();
        }
    }
}