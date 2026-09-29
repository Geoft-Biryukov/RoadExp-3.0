using RoadExp.Common;
using System.Runtime.InteropServices;

namespace RoadExp.Measurements.Data.GpsMeasuringSystem.Serializable
{
    /// <summary>
    /// Структура заголовка файла gps
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TGPSDataHeader
    {
        #region Структура общей части заголовка файла измерения с размерами

        #region Общая часть заголовка
        /// <summary>
        /// Id файла
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.FILE_ID_MAX_SIZE)]
        public char[] FileId;

        /// <summary>
        /// Версия файла
        /// </summary>
        public ushort Version;
        #endregion

        /// <summary>
        /// Размер заголовка 
        /// </summary>
        public uint HeaderSize;

        /// <summary>
        /// Размер каждого набора данных
        /// </summary>
        public uint DataSize;
        #endregion

        #region Структура информации о лаборатории

        /// <summary>
        /// Ф.И.О. оператора проводившего измерения
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.OPERATOR_NAME_MAX_SIZE)]
        public char[] OperatorName;

        /// <summary>
        /// Заводской номер лаборатории
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.LABORATORY_SERIAL_NUMBER_MAX_SIZE)]
        public char[] LaboratorySerialNumber;

        /// <summary>
        /// Уникальный ID ключа HASP
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.HASP_KEY_ID_MAX_SIZE)]
        public char[] HaspKeyID;

        /// <summary>
        /// Название организации
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.ORGANIZATION_MAX_SIZE)]
        public char[] Organization;

        /// <summary>
        /// Гос.номер лаборатории (автомобиля)
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.NUMBER_PDL_MAX_SIZE)]
        public char[] NumberPDL;
        #endregion

        /// <summary>
        /// Не используется
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 155)]
        public char[] NotUsed;

        /// <summary>
        /// Id дороги
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.ROAD_ID_MAX_SIZE)]
        public char[] RoadID;


        #region Структура информации о дороге

        /// <summary>
        /// Код дороги
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.ROAD_CODE_MAX_SIZE)]
        public char[] RoadCode;

        /// <summary>
        /// Наименование дороги
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.ROAD_NAME_MAX_SIZE)]
        public char[] RoadName;
        #endregion

        #region Структура информации о дате и времени начала измерений

        /// <summary>
        /// Структура информации о дате
        /// </summary>
        public TDateIzmer DateBegin;

        /// <summary>
        /// Структура информации о времени
        /// </summary>
        public TTimeIzmer TimeBegin;
        #endregion

        #region Структура информации о дате и времени окончания измерений

        /// <summary>
        /// Структура информации о дате
        /// </summary>
        public TDateIzmer DateEnd;

        /// <summary>
        /// Структура информации о времени
        /// </summary>
        public TTimeIzmer TimeEnd;
        #endregion

        #region Структура информации об участке

        /// <summary>
        /// Адрес начала участка измерения, см
        /// </summary>
        public int BeginPoint;

        /// <summary>
        /// Адрес конца участка измерения, см
        /// </summary>
        public int EndPoint;

        /// <summary>
        /// Направление (0 - по титулу, 1 - против титула)
        /// </summary>
        public byte WayDirect;

        /// <summary>
        /// Номер полосы
        /// </summary>
        public byte LineNumber;

        /// <summary>
        /// Категория дороги
        /// </summary>
        public byte RoadCategory;
        #endregion

        /// <summary>
        /// (НЕ ИСПОЛЬЗУЕТСЯ) Азимут начала участка измерения, град
        /// </summary>
        public double BegPlanAngle;

        /// <summary>
        /// (НЕ ИСПОЛЬЗУЕТСЯ) Азимут конца участка измерения, град
        /// </summary>
        public double EndPlanAngle;

        /// <summary>
        /// Тип датчика пройденного пути: 1 - основной, 2 - дополнительный, 128 - эмуляция,(Значение не задано = 0)
        /// </summary>
        public byte WayCntType;

        /// <summary>
        /// Идентификатор измерительного оборудования (Значение не задано = 0)
        /// </summary>
        public ushort EquipmentID;

        /// <summary>
        /// ИД измерения
        /// </summary>
        public uint MeasurementID;

        /// <summary>
        /// ID-шник в титуле
        /// </summary>
        public int C_ROADInTitul;

        /// <summary>
        /// Зарезервированные на будущее
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public uint[] Reserved;
    }
}
