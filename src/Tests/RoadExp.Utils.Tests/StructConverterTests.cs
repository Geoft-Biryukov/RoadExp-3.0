using System.Runtime.InteropServices;

namespace RoadExp.Utils.Tests
{
    [TestFixture]
    public class StructConverterTests
    {
        [Test]
        public void StructToByteArray_ByteArrayToStruct_ShouldBeTheSame()
        {
            // Arrange
            var s = new TestStruct
            {
                Index = 2128506,
                DoubleItem = 3.1415926,
                IntItem = 20260805
            };

            // Act
            var array = StructConverter.StructToByteArray(s);

            Assert.IsNotNull(array);

            var converted = StructConverter.ByteArrayToStruct<TestStruct>(array);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(converted.Index, Is.EqualTo(s.Index));
                Assert.That(converted.DoubleItem, Is.EqualTo(s.DoubleItem));
                Assert.That(converted.IntItem, Is.EqualTo(s.IntItem));
            });
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct TestStruct
        {
            public long Index;

            public double DoubleItem;

            public int IntItem;
        }

    }
}
