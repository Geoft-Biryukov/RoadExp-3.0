namespace RoadExp.GpsMeasuringSystem.Tests
{
    public class GpsDataConverterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ToByteArrayFromByteArray_CorrectData_ShouldEqualsStrings()
        {
            // Arrange
            string[] data = new string[2];

            data[0] = "$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47";
            data[1] = "$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A";

            //Act
            var serializer = new GpsDataConverter();

            var serialized = serializer.ToByteArray(new GpsRawData(data));
            var deserialized = serializer.FromByteArray(serialized);

            //Assert
            Assert.That(deserialized.Nmea, Is.EqualTo(data));
        }
    }
}