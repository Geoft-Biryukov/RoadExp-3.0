using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RoadExp.VideoRecordingSystem.DeviceComponents.Emulation
{
    internal class EmulatedFrameProvider : IVideoFrameProvider
    {
        #region private fields
        private int _tickCount = 0; // Счетчик для анимации движения
        #endregion

        #region constructor
        public EmulatedFrameProvider(int width = 640, int height = 480, EmulatedObjects emulated = EmulatedObjects.GreenRectangle)
        {
            Width = width;
            Height = height;
            CurrentBrush = CreateBrush(emulated);
        }     
        #endregion

        #region properties
        public int Width { get; }
        public int Height { get; }
        public int BytesPerPixel => 4; // Будем использовать BGRA (32 бита)

        public Brush CurrentBrush { get; }
        #endregion

        public byte[] GetVideoFrame()
        {
            int bufferSize = Width * Height * BytesPerPixel;
            byte[] frameBuffer = new byte[bufferSize];

            // Создаем Bitmap в памяти поверх нашего массива байт
            // GCHandle пингует массив в памяти, чтобы GC его не переместил во время рисования
            GCHandle pinnedArray = GCHandle.Alloc(frameBuffer, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();

            try
            {
                using (var bitmap = new Bitmap(Width, Height, Width * BytesPerPixel, PixelFormat.Format32bppRgb, pointer))
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    // 1. Заливаем фон (имитируем темноту или статичную сцену)
                    graphics.Clear(Color.DarkSlateGray);

                    // 2. Генерируем динамику (движущийся объект по триггеру)
                    _tickCount++;
                    int rectSize = 50;
                    int x = (_tickCount * 5) % (Width - rectSize);
                    int y = (Height / 2) - (rectSize / 2);

                    // Рисуем «тестовый объект»
                    graphics.FillRectangle(Brushes.LimeGreen, x, y, rectSize, rectSize);

                    // Рисуем текст (таймштамп триггера)
                    string infoText = $"Emulated Cam | Frame: {_tickCount} | {DateTime.Now:HH:mm:ss.fff}";
                    using (var font = new Font("Arial", 14, FontStyle.Bold))
                    {
                        graphics.DrawString(infoText, font, Brushes.White, new PointF(20, 20));
                    }
                }
            }
            finally
            {
                // Обязательно освобождаем указатель
                pinnedArray.Free();
            }

            return frameBuffer;
        }

        private static Brush CreateBrush(EmulatedObjects emulated)
        {
            switch (emulated)
            {
                case EmulatedObjects.WhiteRectangle:
                    return Brushes.WhiteSmoke;
                    
                case EmulatedObjects.GreenRectangle:
                    return Brushes.LimeGreen;

                case EmulatedObjects.BlueRectangle:
                    return Brushes.BlueViolet;

                case EmulatedObjects.RedRectangle:
                    return Brushes.OrangeRed;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
