using NUnit.Framework;
using ForestFireSimulator.ViewModel;
using ForestFireSimulator.Model;

namespace Tests
{
    [TestFixture]
    public class ControllerParseData
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Invalid_File_Extention()
        {
            Controller controller = new Controller();

            string[] data = new string[] { "sth.sth", "12", "12", "12" };

            Assert.AreEqual(false, controller.ParseData(data));
        }

        [Test]
        public void Invalid_File_Path()
        {
            Controller controller = new Controller();

            string[] data = new string[] { "abc.sth.bmp", "12", "12", "12" };

            Assert.AreEqual(false, controller.ParseData(data));
        }

        [Test]
        public void Correct_File_Extention()
        {
            Controller controller = new Controller();

            string[] data = new string[] { "sth.bmp", "12", "12", "12" };

            Assert.AreEqual(true, controller.ParseData(data));
        }

        [Test]
        public void Invalid_Propability_Format()
        {
            Controller controller = new Controller();

            string[] data = new string[] { "sth.bmp", "as", "12", "va" };

            Assert.AreEqual(false, controller.ParseData(data));
        }

        [Test]
        public void Invalid_Propability_Format_Float()
        {
            Controller controller = new Controller();

            string[] data = new string[] { "sth.bmp", "12", "12.32", "42" };

            Assert.AreEqual(false, controller.ParseData(data));
        }
    }
}