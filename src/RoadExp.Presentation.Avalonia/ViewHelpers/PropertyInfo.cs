namespace RoadExp.Presentation.Avalonia.ViewHelpers
{
    public class PropertyInfo
    {       
        public PropertyInfo() { }

        public PropertyInfo(ushort index, string displayName)
        {
            Index = index;
            DisplayName = displayName;
        }
        
        public ushort Index { get; set; }
        public string DisplayName { get; set; }
    }
}
