namespace RoadExp.VideoRecordingSystem.DeviceComponents.Emulation
{
    public interface IEmulatedFrameProviderFactory
    {
        IVideoFrameProvider CreateNext();
    }
}
