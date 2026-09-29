using RoadExp.Measurements.Core;

namespace RoadExp.Application
{
    public class RoadExpApplication
    {       
        private readonly Dictionary<string, IMeasuringSystem> _measuringSystems = new();        

        /// <summary>
        /// Закрытый конструктор. Инстанцировать только билдером
        /// </summary>
        private RoadExpApplication()
        {            
        }
        
        public static RoadExpApplicationBuilder CreateBuilder()
        {
            return new RoadExpApplicationBuilder();
        }

        public void Run()
        {
            
        }

        public sealed class RoadExpApplicationBuilder
        {                        

            public RoadExpApplication Build()
            {                                 
                var app = new RoadExpApplication();                                                

                return app;
            }
        }
    }
}
