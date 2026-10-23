namespace RoadExp.VideoRecordingSystem.DeviceComponents
{
    public interface IVideoFrameProvider
    {
        int Width { get; }
        int Height { get; }
        int BytesPerPixel { get; }

        /// <summary>
        /// Возвращает массив байт кадра (формат BGR или BGRA) по триггеру
        /// </summary>
        byte[] GetVideoFrame();
    }
}
