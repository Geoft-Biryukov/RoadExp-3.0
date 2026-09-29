namespace RoadExp.Measurements.Core.SystemsConnection
{
    /// <summary>
    /// Выполняет действие по команде 
    /// </summary>
    public interface ITriggered
    {
        /// <summary>
        /// Выполнить команду
        /// </summary>
        void Trigger();
    }
}
