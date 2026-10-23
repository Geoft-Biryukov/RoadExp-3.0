namespace RoadExp.VideoRecordingSystem.DeviceComponents.Emulation
{
    public class EmulatedFrameProviderFactory : IEmulatedFrameProviderFactory
    {
        #region private fields
        private const int _width = 640;
        private const int _height = 480; 
        private readonly EmulatedObjects[] _emulatedObjects = Enum.GetValues<EmulatedObjects>();
        private int _counter = 0;
        #endregion

        #region public methods        
        public IVideoFrameProvider CreateNext()
        {
            var currentObject = _emulatedObjects[_counter++];

            if(_counter >= _emulatedObjects.Length) 
                _counter = 0;

            return new EmulatedFrameProvider(_width, _height, currentObject);
        }
        #endregion
    }
}
