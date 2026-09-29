using RoadExp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RoadExp.Measurements.Data.GeometryMeasuringSystem.Serializable
{
    /// <summary>
    /// Структура данных файла геометрии
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TGeometryData
    {
        /// <summary>
        /// Индекс 
        /// </summary>
        public long Index;

        /// <summary>
        /// Местоположение, cм
        /// </summary>
        public int smPosition;

        /// <summary>
        /// Угол с курсового гироскопа, град
        /// </summary>
        public double grCourse;

        /// <summary>
        /// Угол продольного крена, град
        /// </summary>
        public double grLongHeel;

        /// <summary>
        /// Угол поперечного крена, град
        /// </summary>
        public double grCrossHeel;

        /// <summary>
        /// Cкорость км/ч
        /// </summary>
        public ushort Speed;

        /// <summary>
        /// Поперечные уклоны по всем объединенным полосам
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.GEOMETRY_LONG_HEEL_LINES_COUNT)]
        public float[] grLongHeelLines;

        /// <summary>
        /// Количество прошедших миллисекунд начиная с 01.01.1970(Нужна для синхронизации разных модулей) 
        /// </summary>
        public ulong SystemTime;
    }
}
