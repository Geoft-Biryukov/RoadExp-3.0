using System.Timers;

namespace RoadExp.GeometryMeasuringSystem
{
    public class NavigationSimulator
    {
        private System.Timers.Timer _movementTimer;
        private List<GeoPoint> _routePoints;
        private int _currentPointIndex;
        private double _speedMps;
        private ushort _packetCounter = 0;
        private Random _random = new Random();

        public GeoPoint CurrentPosition { get; private set; }
        public bool IsNavigating { get; private set; }
        public float CurrentCourse { get; private set; }

        // Событие теперь передает сырой массив байт, готовый для вашего Helpers.ParseData
        public event Action<byte[]> RawDataGenerated;
        public event Action<string> VoiceGuidanceIssued;
        public event Action NavigationFinished;

        public NavigationSimulator(double speedKmH = 60.0)
        {
            _speedMps = speedKmH / 3.6;
            _routePoints = new List<GeoPoint>();

            _movementTimer = new System.Timers.Timer(100); // Обновление 10 раз в секунду
            _movementTimer.Elapsed += UpdateSimulationStep;
        }

        public void StartNavigation(List<GeoPoint> route)
        {
            if (route == null || route.Count < 2)
                throw new ArgumentException("Маршрут должен содержать минимум 2 точки.");

            _routePoints = route;
            _currentPointIndex = 0;
            CurrentPosition = _routePoints[0];
            CurrentCourse = (float)CalculateBearing(CurrentPosition, _routePoints[1]);
            IsNavigating = true;

            _movementTimer.Start();
            VoiceGuidanceIssued?.Invoke($"Маршрут построен. Начинаем движение.");
        }

        public void StopNavigation()
        {
            _movementTimer.Stop();
            IsNavigating = false;
        }

        private void UpdateSimulationStep(object sender, ElapsedEventArgs e)
        {
            if (_currentPointIndex >= _routePoints.Count - 1)
            {
                FinishNavigation();
                return;
            }

            GeoPoint targetPoint = _routePoints[_currentPointIndex + 1];
            double distanceToTarget = CalculateDistance(CurrentPosition, targetPoint);

            if (distanceToTarget <= _speedMps)
            {
                _currentPointIndex++;
                CurrentPosition = targetPoint;

                if (_currentPointIndex < _routePoints.Count - 1)
                {
                    // Меняем курс на следующую точку
                    CurrentCourse = (float)CalculateBearing(CurrentPosition, _routePoints[_currentPointIndex + 1]);
                    VoiceGuidanceIssued?.Invoke($"Пройдена точка маршрута. Новый курс: {CurrentCourse:F1}°");
                }
            }
            else
            {
                // Движение по вектору к цели
                double ratio = _speedMps / distanceToTarget;
                double newLat = CurrentPosition.Latitude + (targetPoint.Latitude - CurrentPosition.Latitude) * ratio;
                double newLon = CurrentPosition.Longitude + (targetPoint.Longitude - CurrentPosition.Longitude) * ratio;
                CurrentPosition = new GeoPoint(newLat, newLon);
            }

            // Формируем и отправляем пакет данных МИНС
            byte[] rawPacket = GenerateMinsPacket();
            RawDataGenerated?.Invoke(rawPacket);
        }

        /// <summary>
        /// Сериализация данных навигации в массив байт по вашей структуре
        /// </summary>
        private byte[] GenerateMinsPacket()
        {
            _packetCounter++;
            int srcOffset = 8;
            int dataCount = 12;
            int dataSize = sizeof(float);

            // Создаем массив float для данных
            float[] dataArray = new float[dataCount];

            // Заполнение массива в соответствии с индексами вашего парсера
            dataArray[0] = _packetCounter;                                      // PacketNum (ushort кастуется в float)
            dataArray[1] = (float)(_random.NextDouble() * 2 - 1);               // LongHeel (симуляция качки/дифферента)
            dataArray[2] = (float)(_random.NextDouble() * 2 - 1);               // CrossHeel (симуляция крена)
            dataArray[3] = CurrentCourse;                                       // Course (Инерциальный курс)
            dataArray[4] = (float)(DateTime.UtcNow.TimeOfDay.TotalSeconds);     // GPSTime
            dataArray[5] = (float)CurrentPosition.Latitude;                     // Latitude
            dataArray[6] = (float)CurrentPosition.Longitude;                    // Longitude
            dataArray[7] = 150.0f;                                              // HeightGPS (высота над уровнем моря)
            dataArray[8] = (float)(_speedMps * 3.6 / 1.852);                    // SpeedGPS (скорость в узлах или км/ч)
            dataArray[9] = 12f;                                                 // SatellitesCount (float, кастуемый в int)
            dataArray[10] = CurrentCourse;                                      // CourseGPS (Путевой угол)
            dataArray[11] = 0.0f;                                               // VerticalSpeedGPS

            // Итоговый размер: 8 байт заголовка + (12 элементов * 4 байта) = 56 байт
            byte[] packet = new byte[srcOffset + (dataCount * dataSize)];

            // Имитация заголовка МИНС (первые 8 байт, например, маркеры пакета или таймштамп)
            byte[] header = { 0xAA, 0xBB, 0xCC, 0xDD, 0x01, 0x02, 0x03, 0x04 };
            Buffer.BlockCopy(header, 0, packet, 0, srcOffset);

            // Копируем float-массив в byte-массив со смещением 8 байт
            Buffer.BlockCopy(dataArray, 0, packet, srcOffset, dataArray.Length * dataSize);

            return packet;
        }

        private void FinishNavigation()
        {
            _movementTimer.Stop();
            IsNavigating = false;
            NavigationFinished?.Invoke();
            VoiceGuidanceIssued?.Invoke("Вы прибыли в пункт назначения.");
        }

        // Расчет расстояния (Гаверсинус)
        private double CalculateDistance(GeoPoint p1, GeoPoint p2)
        {
            double r = 6371e3;
            double phi1 = p1.Latitude * Math.PI / 180;
            double phi2 = p2.Latitude * Math.PI / 180;
            double deltaPhi = (p2.Latitude - p1.Latitude) * Math.PI / 180;
            double deltaLambda = (p2.Longitude - p1.Longitude) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            return r * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }

        // Расчет угла направления (Азимута) между точками от 0 до 360 градусов
        private double CalculateBearing(GeoPoint p1, GeoPoint p2)
        {
            double lat1 = p1.Latitude * Math.PI / 180;
            double lat2 = p2.Latitude * Math.PI / 180;
            double dLon = (p2.Longitude - p1.Longitude) * Math.PI / 180;

            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            double bearing = Math.Atan2(y, x) * 180 / Math.PI;

            return (bearing + 360) % 360;
        }
    }

    public struct GeoPoint
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public GeoPoint(double lat, double lon) { Latitude = lat; Longitude = lon; }
    }

}
