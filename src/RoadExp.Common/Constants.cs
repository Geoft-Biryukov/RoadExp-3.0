using System.Collections.ObjectModel;

namespace RoadExp.Common
{
    /// <summary>
    /// Класс содержащий общие константы для всего решения
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Размер пакета данных для модулей ГКВ
        /// </summary>
        public static int GKV_PacketLenght = 60;

        public static char BACKSLASH = '\\';

        public static char FORWARD_SLASH = '/';

        public static char UNDERSCORE = '_';

        public static char SPACE = ' ';

        public static char NULL_CHARACTER = '\0';

        public const char DOT = '.';

        /// <summary>
        /// Точность сравнения чисел с плавающей точкой
        /// </summary>
        public const double FLOATING_NUMBERS_COMPARE_EPS = 0.00001;

        /// <summary>
        /// Символы запрещенные для ввода в названия
        /// </summary>
        public static List<char> ForbiddenSymbols = new List<char>() { BACKSLASH, FORWARD_SLASH, ':', '*', '?', '\"', '<', '>', '|' };

        /// <summary>
        /// Список вариантов для второй части идентификатора дороги
        /// </summary>
        public static List<string> RoadIdPart2List = new List<string>() { "ОП", "НП" };

        /// <summary>
        /// Список вариантов для третьей части идентификатора дороги
        /// </summary>
        public static List<string> RoadIdPart3List = new List<string>() { "ФЗ", "РЗ", "МЗ", "МП", "МР", "МГ", "ЧС" };

        /// <summary>
        /// Значение по умолчанию для первой части идентификатора дороги
        /// </summary>
        public static string RoadIdPart1DefaultValue = "99";

        /// <summary>
        /// Значение по умолчанию для второй части идентификатора дороги
        /// </summary>
        public static string RoadIdPart2DefaultValue = RoadIdPart2List[1];

        /// <summary>
        /// Значение по умолчанию для третьей части идентификатора дороги
        /// </summary>
        public static string RoadIdPart3DefaultValue = RoadIdPart3List[6];

        /// <summary>
        /// Значение по умолчанию для четвертой части идентификатора дороги
        /// </summary>
        public static string RoadIdPart4DefaultValue = "99999";

        /// <summary>
        /// Название варианта «без группировки» при выборе группы дорог
        /// </summary>
        public const string ROAD_GROUP_NO_GROUPING = "без группировки";

        /// <summary>
        /// Имя файла базы данных GDB по умолчанию
        /// </summary>
        public const string GDB_DATABASE_DEFAULT_FILE_NAME = "rl";
        /// <summary>
        /// Расширение файла базы данных GDB
        /// </summary>
        public const string GDB_DATABASE_FILE_EXT = ".gdb";

        /// <summary>
        /// Сообщение о начале измерений
        /// </summary>
        public const string MEASUREMENT_STARTED_MESSAGE = "Измерения начаты";

        /// <summary>
        /// Сообщение о конце измерений
        /// </summary>
        public const string MEASUREMENT_FINISHED_MESSAGE = "Измерения закончены";

        public const string COM_SUFFIX = "COM";
        /// <summary>
        /// Список скоростей для COM порта
        /// </summary>
        public static List<int> BaudRateList = new List<int>() { 4800, 9600, 19200, 57600, 115200, 230400, 460800, 921600 };
        public static readonly string BRANDNAME_MOXA = "MOXA";
        public static readonly string VENDORID_MOXA = "VID_110A";

        public const string SYSTEM_WAY_NAME = "DPP";
        public const string SYSTEM_GPS_NAME = "GPS";
        public const string SYSTEM_GEOM_NAME = "Geom";
        public const string SYSTEM_VIDEO_DEFECT_NAME = "VideoDefect";
        public const string SYSTEM_IRI_NAME = "IRI";
        public const string SYSTEM_TRANVERSE_PROFILE_NAME = "TPC";
        public const string SYSTEM_FAST_TRANVERSE_PROFILE_NAME = "FTPC";
        public const string SYSTEM_SKPK_NAME = "SKPK";
        public const string SYSTEM_GEO_SCAN_NAME = "GeoScan";
        public const string SYSTEM_VIDEO_NAME = "Video";
        public const string SYSTEM_SCEP_NAME = "Scep";
        public const string SYSTEM_LIGHTING_NAME = "Lighting";
        public const string SYSTEM_FIX_OBJECTS_NAME = "FixObjects";
        public const string SYSTEM_DINA_NAME = "Dina";

        public const string SYSTEM_WAY_DESCR = "Модуль датчика пути";
        public const string SYSTEM_GPS_DESCR = "Модуль GPS приемника";
        public const string SYSTEM_GEOM_DESCR = "Модуль 'Геометрия'";
        public const string SYSTEM_VIDEO_DEFECT_DESCR = "Модуль видео дефектовки";
        public const string SYSTEM_IRI_DESCR = "Модуль IRI";
        public const string SYSTEM_TRANVERSE_PROFILE_DESCR = "Модуль 'Колейность'";
        public const string SYSTEM_SKPK_DESCR = "Модуль SKPK";
        public const string SYSTEM_GEO_SCAN_DESCR = "Модуль гео-радара";
        public const string SYSTEM_VIDEO_DESCR = "Модуль видео съемки";
        public const string SYSTEM_SCEP_DESCR = "Модуль сцепления";
        public const string SYSTEM_LIGHTING_DESCR = "Модуль освещенность";
        public const string SYSTEM_BUS_CONTROL_DESCR = "Модуль управления шиной";
        public const string FIX_OBJECTS_SYSTEM_DESCR = "Модуль фиксации объектов";
        public const string SYSTEM_CONTROL_MODULE_DESCR = "Модуль USB контроллера";
        public const string SYSTEM_ADCINIT_MODULE_DESCR = "Модуль АЦП";
        public const string SYSTEM_UNKNOWN_MODULE_DESCR = "Модуль не определен";
        public const string GLOBAL_MODULE_DESCR = "Система";


        /// <summary>
        /// Отсутствует в комплектации (USED_SYSTEM = false)
        /// </summary>
        //public const int SYSTEM_STATE_MISSING = 0;
        /// <summary>
        /// Отключена
        /// </summary>
        public const int SYSTEM_STATE_DISCONNECTED = 1;
        /// <summary>
        /// Включена с некритической ошибкой
        /// </summary>
        public const int SYSTEM_STATE_WARNING = 2;
        /// <summary>
        /// Включена с критической ошибкой
        /// </summary>
        public const int SYSTEM_STATE_ERROR = 3;
        /// <summary>
        /// Включена без ошибок
        /// </summary>
        public const int SYSTEM_STATE_CONNECTED = 4;

        public const string SYSTEM_CONNECTED = "Система готова";
        public const string SYSTEM_CALIBRATING = "Система калибруется...";
        public const string SYSTEM_GPS_WHITE_GOODNESS = "Ожидание спутников...";
        public const string SYSTEM_GPS_SYNK = "Синхронизация времени...";
        public const string SYSTEM_SEARCH_CAMERAS = "Поиск камер...";
        public const string SYSTEM_CONNECTING = "Инициализация...";
        public const string SYSTEM_DISCONNECTED = "Система отключена";
        public const string SYSTEM_DISCONNECTING = "Отключение системы...";

        /// <summary>
        /// Путь к cmd.exe. Должен лежать в корне.
        /// </summary>
        public const string CMD_EXE_PATH = "cmd.exe";
        /// <summary>
        /// Команда для запуска менеджера устройств
        /// </summary>
        public const string CMD_EXE_RUN_DEVICE_MANAGER_COMMAND = "/MIN /c devmgmt.msc";

        public const string GPS_MAP_MAIN_ICON_RELATIVE_PATH = "resource/gps_main.png";
        public const string GPS_MAP_ADDITIONAL_ICON_RELATIVE_PATH = "resource/gps_additional.png";
        public const string GPS_TRACK_BEGIN_ICON_RELATIVE_PATH = "resource/gps_track_begin.png";
        public const string GPS_TRACK_END_ICON_RELATIVE_PATH = "resource/gps_track_end.png";

        /// <summary>
        /// Расширение для файлов измерения GPS
        /// </summary>
        public const string GPS_FILE_EXT = "gps";

        /// <summary>
        /// Расширение для файлов измерения Геометрии(после фильтрации, вычитания показаний скпк и прореживание с шагом 1 метр)
        /// </summary>
        public const string GEOM_FILE_EXT = "gm";

        /// <summary>
        /// Расширение для исходных файлов измерения Геометрии(до фильтрации, вычитания показаний скпк и прореживание с шагом 1 метр)
        /// </summary>
        public const string PREGEOM_FILE_EXT = "pregm";

        /// <summary>
        /// Расширение для файлов измерения СКПК
        /// </summary>
        public const string SKPK_FILE_EXT = "skpk";

        /// <summary>
        /// Расширение для файлов Видеосъемки
        /// </summary>
        public const string VIDEO_FILMING_FILE_EXT = "wrk2";

        /// <summary>
        /// Расширение для файлов измерения поперечной ровности(колейности)
        /// </summary>
        public const string TRANSVERSE_PROFILE_FILE_EXT = "tvp";

        /// <summary>
        /// Расширение для файлов измерения поперечной ровности(колейности) в формате RoadPro
        /// </summary>
        public const string TRANSVERSE_PROFILE_ROADPRO_FILE_EXT = "crf";

        /// <summary>
        /// Расширение для файлов Видеодефектовки
        /// </summary>
        public const string VIDEO_DEFECT_FILE_EXT = "vdef2";

        /// <summary>
        /// Расширение для файлов Видеодефектовки
        /// </summary>
        public const string VIDEO_DEFECT_ROADPRO_FILE_EXT = "vdef";

        /// <summary>
        /// Расширение для файлов измерения коэф.сцепления
        /// </summary>
        public const string SCEPLENIE_FILE_EXT = "scep";

        /// <summary>
        /// Расширение для файлов Георадарной съемки
        /// </summary>
        public const string GEOSCAN_FILE_EXT = "gpr";

        /// <summary>
        /// Расширение для файлов данными пройденного пути
        /// </summary>
        public const string WAY_FILE_EXT = "way";

        /// <summary>
        /// Расширение для файлов с кадрами от камеры ladybug
        /// </summary>
        public const string LADYBUG_FILE_EXT = "pgr";

        /// <summary>
        /// Расширение для файлов измерения освещенности
        /// </summary>
        public const string LIGHTING_FILE_EXT = "light";

        /// <summary>
        /// Расширение для файлов осцилограммы освещенности
        /// </summary>
        public const string OSC_LIGHTING_FILE_EXT = "lightOsc";

        /// <summary>
        /// Расширение для файлов профилометров
        /// </summary>
        public const string PROFILOMETER_FST_FILE_EXT = "fst";

        /// <summary>
        /// Расширение для файлов камеры видеодефектовки
        /// </summary>
        public const string VIDEODEFECT_CCF_FILE_EXT = "ccf";

        /// <summary>
        /// Расширения файлов, которые требуют обработки
        /// </summary>
        public static readonly IList<string> NEED_PROCESS_EXT = new ReadOnlyCollection<string>(new List<string> { PREIRI_FILE_EXT, PREGEOM_FILE_EXT, TRANSVERSE_PROFILE_FILE_EXT, VIDEO_DEFECT_FILE_EXT, OSC_LIGHTING_FILE_EXT });

        /// <summary>
        /// Расширение для текстового документа
        /// </summary>
        public const string TXT_FILE_EXT = "txt";

        /// <summary>
        /// Путь к диску С
        /// </summary>
        public const string C_PATH = "C:\\";

        /// <summary>
        /// Путь к папке Program Files (x86)
        /// </summary>
        public const string PROGRAMM_FILES_X86_PATH = "Program Files (x86)\\";

        /// <summary>
        /// Путь к папке Firebird
        /// </summary>
        public const string FIREBIRD_PATH = "Firebird\\";
        /// <summary>
        /// Путь к папке Firebird_2_5
        /// </summary>
        public const string FIREBIRD_VERSION_PATH = "Firebird_2_5\\";
        /// <summary>
        /// Имя файла с алиасами Firebird
        /// </summary>
        public const string FIREBIRD_ALIASES_CONF = "aliases.conf";
        /// <summary>
        /// Сообщение Firebird о том, что служба остановлена на русском
        /// </summary>
        public const string FIREBIRD_RUS_MESSAGE_SUCCESS_STOP = "Служба \"Firebird Server - DefaultInstance\" успешно остановлена.";
        /// <summary>
        /// Сообщение Firebird о том, что служба уже остановлена на русском
        /// </summary>
        public const string FIREBIRD_RUS_MESSAGE_ALREADY_STOP = "Служба \"Firebird Server - DefaultInstance\" не запущена.";
        /// <summary>
        /// Сообщение Firebird о том, что служба запущена на русском
        /// </summary>
        public const string FIREBIRD_RUS_MESSAGE_SUCCESS_RUN = "Служба \"Firebird Server - DefaultInstance\" успешно запущена.";
        /// <summary>
        /// Сообщение Firebird о том, что служба уже запущена на русском
        /// </summary>
        public const string FIREBIRD_RUS_MESSAGE_ALREADY_RUN = "Затребованная служба уже запущена.";

        /// <summary>
        /// Путь к папке с результатами измерений
        /// </summary>
        public const string RESULT_PATH = "Result\\";

        /// <summary>
        /// Название файла с пользовательскими настройками
        /// </summary>
        public const string USER_SETTINGS_FILE_NAME = "RoadExp(user).cfg";

        /// </summary>
        /// Название файла с пользовательскими настройками
        /// </summary>
        public const string VEHICLE_CONFIG_FILE_NAME = "Vehicle.cfg";

        /// <summary>
        /// Название файла с последней инфомацие об участке измерения
        /// </summary>
        public const string LAST_ROAD_SECTOR_INFO_FILE_NAME = "LastRoadSector.info";

        /// <summary>
        /// Путь к папке с логами
        /// </summary>
        public const string LOGS_PATH = "Logs\\";

        /// <summary>
        /// Путь к папке с дампами при краше приложения (переменная окружения %TEMP%).
        /// </summary>
        public const string WER_CRASH_DUMPS_FOLDER = "%TEMP%\\RoadExpCrashDumps";

        /// <summary>
        /// Расширение файла логов
        /// </summary>
        public const string LOGS_EXT = ".log";

        /// <summary>
        /// Время жизни папки с логами, секунды (по умолчанию 3 дня)
        /// </summary>
        public const ulong LOG_TIME_TO_LIVE = 259200;

        /// <summary>
        /// Путь к папке с сохраненными тайлами
        /// </summary>
        public const string TILES_PATH = "Tiles\\";
        /// <summary>
        /// Путь к папке с сохраненными тайлами OSM
        /// </summary>
        public const string TILES_OSM_PATH = "OSM\\";

        /// <summary>
        /// Путь к папке с вспомогательными данными
        /// </summary>
        public const string DATA_PATH = "Data\\";

        /// <summary>
        /// Путь к папке с пустой базой данных
        /// </summary>
        public const string SOURCE_DB_PATH = "SourceDB\\";

        /// <summary>
        /// Путь к папке с коэффициентами для фильтров
        /// </summary>
        public const string FILTER_COEF_PATH = DATA_PATH + "FilterCoef\\";

        /// <summary>
        /// Путь к папке с файлами эмуляции
        /// </summary>
        public const string EMULATIONS_PATH = DATA_PATH + "Emulations\\";

        /// <summary>
        /// Путь к папке с картинками
        /// </summary>
        public const string IMAGES_PATH = DATA_PATH + "Images\\";

        /// <summary>
        /// Путь к папке с файлами снятыми при различных поверках
        /// </summary>
        public const string CALIBRATION_FILES_PATH = "CalibrationFiles\\";

        /// <summary>
        /// Путь к папке с файлами метрологических параметров
        /// </summary>
        public const string METROLOGY_PARAMS_PATH = "MetrologyData\\";

        /// <summary>
        /// Путь к папке где установлен видеобанк
        /// </summary>
        public const string PANORAMA_VIDEOBANK_PATH = "VideoBankX\\";

        /// <summary>
        /// имя исполняемого файла видеобанка
        /// </summary>
        public const string PANORAMA_VIDEOBANK_FILE_NAME = "VideoBankX.exe";

        /// <summary>
        /// Расширение файла параметров для видеобанка
        /// </summary>
        public const string PANORAMA_ENBLEND_PARAMS_FILE_EXT = "params";

        /// <summary>
        /// Название файла с GIF'ок закгрузки
        /// </summary>
        public const string LOADING_GIF_FILE_NAME = "loading.gif";

        /// <summary>
        /// Название файла с картинкой примера ID дороги
        /// </summary>
        public const string EXAMPLE_ROAD_ID_FILE_NAME = "id_example.bmp";

        /// <summary>
        /// Формат даты
        /// </summary>
        public const string DATE_FORMAT = "dd.MM.yyyy";

        /// <summary>
        /// Флаг успешного выполнения любой функции, которая возвращает код ошибки
        /// в основном используется для функций из библиотек
        /// </summary>
        public const int ROAD_EXP_NO_ERROR = 0;

        /// <summary>
        /// Флаг ошибки соединения с устройством
        /// в основном используется для функций из библиотек
        /// </summary>
        public const int ROAD_EXP_CONNECTION_ERROR = 100;

        /// <summary>
        /// Неизвестная ошибка, в основном должна быть помещена в секцию catch 
        /// </summary>
        public const int ROAD_EXP_UNKNOWN_ERROR = -1;

        /// <summary>
        /// Количество датчиков СКПК
        /// </summary>
        public const byte SKPK_SENSOR_COUNT = 4;

        /// <summary>
        /// Максмальное количество камер видеосъемки
        /// </summary>
        public const byte MAX_VIDEO_CAMERA_COUNT = 20;

        /// <summary>
        /// Максмальная длина чексуммы видеосъемки
        /// </summary>
        public const byte MAX_VIDEO_CHECKSUM_LENGTH = 32;

        /// <summary>
        /// Максмальное количество профилометров
        /// </summary>
        public const byte MAX_PROFILOMETR_COUNT = 4;

        /// <summary>
        /// Угол наклона профилометра по умолчанию
        /// </summary>
        public const double DEFAULT_PROFILOMETR_ANGEL = 0;

        /// <summary>
        /// Значение границ общего профиля по умолчанию
        /// </summary>
        public const double DEFAULT_PROFILOMETR_BORDER_VALUE = 0;

        /// <summary>
        /// Максмальное кол-во данных с профилометра
        /// </summary>
        public const int MAX_PROFILOMETR_DATA_COUNT = 10000;

        /// <summary>
        /// Максмальное кол-во данных тензометрического датчика ПКРС
        /// </summary>
        public const int SCEPLENIE_RESULT_DATA_MAX_SIZE = 1000;

        /// <summary>
        /// Максимальное кол-во каналов георадара
        /// </summary>
        public const int GEO_SCAN_MAX_CHANNELS_COUNT = 2;

        /// <summary>
        /// Временная задержка между началом/концом замера и торможением, мс
        /// </summary>
        public const int DELAY_MEASUREMENT_AND_BRAKING = 3000;

        /// <summary>
        /// Таймаут ожидания инициализации оборудования в измерениях, мс
        /// </summary>
        public const int MEASUREMENT_INITIALITION_TIMEOUT = 20000;

        public const int FILE_ID_MAX_SIZE = 5;
        public const int OPERATOR_NAME_MAX_SIZE = 50;
        public const int LABORATORY_SERIAL_NUMBER_MAX_SIZE = 20;
        public const int HASP_KEY_ID_MAX_SIZE = 15;
        public const int ORGANIZATION_MAX_SIZE = 150;
        public const int NUMBER_PDL_MAX_SIZE = 20;
        public const int ROAD_ID_MAX_SIZE = 100;
        public const int ROAD_CODE_MAX_SIZE = 7;
        public const int ROAD_NAME_MAX_SIZE = 255;
        public const int MAC_ADDRESS_MAX_SIZE = 6;
        public const int OPTICAL_DATA_MAX_SIZE = 15;
        public const int OPTICAL_CAMERA_MATRIX_MAX_COLS = 3;
        public const int OPTICAL_CAMERA_MATRIX_MAX_ROWS = 3;
        public const int OPTICAL_DIST_COEFFS_MAX_COUNT = 12;
        public const string OPTICAL_PARAMS_FILE_EXT = "op2";
        public const string OLD_OPTICAL_PARAMS_FILE_EXT = "op";

        public const int MAX_PARAM_ACCESS_STRING_LENGTH = 5;
        public const int MAX_PARAM_DESCRIPTION_STRING_LENGTH = 255;
        public const int MAX_PARAM_TYPE_STRING_LENGTH = 20;
        public const int MAX_PARAM_NAME_LENGTH = 100;
        public const int MAX_PARAM_ARRAY_STRING_LENGTH = 5000;

        public const int FOLDER_NAME_MAX_LENGTH = 200;

        /// <summary>
        /// Максмальное количество полос на дороге
        /// </summary>
        public const byte MAX_POLOS_NUMBER_COUNT = 20;

        /// <summary>
        /// Пароль администратора
        /// </summary>
        public const string ADMIN_PASSWORD = ",hsckfgfy";

        /// <summary>
        /// Пароль метролога
        /// </summary>
        public const string METROLOG_PASSWORD = "gjdthrf";

        #region Путь

        /// <summary>
        /// Идентификатор формата файла пути
        /// </summary>
        public const string FORMAT_WAY_DATA_FILE = "WAYDF";

        /// <summary>
        /// Номер версии формата пути
        /// </summary>
        public const int VERSION_WAY_DATA_FILE = 101;

        /// <summary>
        /// Тип датчика пути не задан/неизвестен
        /// </summary>
        public const int WAY_SENSOR_TYPE_UNKNOWN = 0;

        /// <summary>
        /// Тип датчика пути эмуляция
        /// </summary>
        public const int WAY_SENSOR_TYPE_EMULATION = 128;

        #endregion

        #region Gps

        /// <summary>
        /// Идентификатор формата GPS Data File
        /// </summary>
        public const string FORMAT_GPS_DATA_FILE = "GPSDF";

        /// <summary>
        /// Номер версии формата (На сентябрь 2020 1.04 = 104)
        /// </summary>
        public const ushort VERSION_GPS_DATA_FILE = 104;

        /// <summary>
        /// Тип оборудования gps эмуляция
        /// </summary>
        public const int GPS_EQUIPMENT_TYPE_EMULATION = 128;

        /// <summary>
        /// Показатель качества навигационных данных: навигационные данные не достоверны
        /// </summary>
        public const int GPS_RECEIVER_STATUS_NO_FIX = 0;

        /// <summary>
        /// Показатель качества навигационных данных: навигационные данные достоверны (автономный режим)
        /// </summary>
        public const int GPS_RECEIVER_STATUS_3D = 1;

        /// <summary>
        /// Показатель качества навигационных данных: навигационные данные достоверны (дифференциальный режим)
        /// </summary>
        public const int GPS_RECEIVER_STATUS_3D_DIFF = 2;

        /// <summary>
        /// Показатель качества навигационных данных: навигационные данные достоверны (RTK Fix)
        /// </summary>
        public const int GPS_RECEIVER_STATUS_RTK_FIX = 4;

        /// <summary>
        /// Показатель качества навигационных данных: навигационные данные достоверны (RTK Float)
        /// </summary>
        public const int GPS_RECEIVER_STATUS_RTK_FLOAT = 5;

        /// <summary>
        /// Значение координаты разрыва трека
        /// </summary>
        public const double GPS_TRACK_SEPARATE_COORDINATE = -1000.0;

        /// <summary>
        /// Максимальный угол отклонения gps трека на карте для разрежения
        /// </summary>
        public const double GPS_MAXIMUM_TRACK_TURNING_ANGLE = 5.0;

        /// <summary>
        /// Значение зума, при котором будет рисоваться весь эталонный трек
        /// </summary>
        public const double GPS_ZOOM_VALUE_TO_SHOW_FUUL_REFERENCE_TRACK = 0.05;

        #endregion

        #region Видео

        /// <summary>
        /// Идентификатор формата wrk2
        /// </summary>
        public const string FORMAT_WRK2_DATA_FILE = "WRK20";

        /// <summary>
        /// Номер версии формата wrk2
        /// </summary>
        public const ushort VERSION_WRK2_DATA_FILE = 208;

        #endregion

        #region Видеодефектовка

        /// <summary>
        /// Идентификатор формата vdef2
        /// </summary>
        public const string FORMAT_VDEF2_DATA_FILE = "VDEF2";

        /// <summary>
        /// Номер версии формата vdef2
        /// </summary>
        public const ushort VERSION_VDEF2_DATA_FILE = 201;

        #endregion

        #region Iri

        /// <summary>
        /// Идентификатор формата продольной ровности
        /// </summary>
        public const string FORMAT_IRI_DATA_FILE = "IRIDF";

        /// <summary>
        /// Расширение для файлов измерения IRI(не обработанных фильтрами)
        /// </summary>
        public const string PREIRI_FILE_EXT = "preiri";

        /// <summary>
        /// Расширение для файлов измерения IRI
        /// </summary>
        public const string IRI_FILE_EXT = "iri";

        /// <summary>
        /// Расширение для файлов исключения секторов IRI
        /// </summary>
        public const string EXCLUDE_IRI_SECTOR_FILE_EXT = "rislf";

        /// <summary>
        /// Идентификатор формата файла исключения секторов IRI
        /// </summary>
        public const string FORMAT_IRI_SECTOR_FILE = "IRISL";

        /// <summary>
        /// Версия формата файла исключения секторов IRI
        /// </summary>
        public const int VERSION_IRI_SECTOR_FILE = 100;

        /// <summary>
        /// Имя файла для хранения информации о полигонах IRI
        /// </summary>
        public const string CALIBRATION_IRI_STANDARTS_FILE_NAME = "полигоны_iri.json";

        /// <summary>
        /// Кол-во данных приходящее для модуля IRI (показания лазера и акселерометра, слева и справа)
        /// </summary>
        public const int IRI_DATA_COUNT = 128;

        /// <summary>
        /// Временной интервал между исходными данными IRI, зависит от частоты опроса АЦП
        /// </summary>
        public const uint IRI_DATA_TIME_INTERVAL = 200;

        /// <summary>
        /// Временной интервал, через который нужно добавлять показания ДПП при расчете IRI во время измерений
        /// </summary>
        public const int IRI_CALCULATION_ADD_WAY_TIMEOUT = 500;

        /// <summary>
        /// Максимальное количество точек, отобажаемое на графиках показаний лазера и акселерометра слева и справа
        /// </summary>
        public const uint IRI_CHART_MAX_COUNT = 250;

        /// <summary>
        /// Число точек, которое необходимо получить, чтобы обновить график
        /// </summary>
        public const int IRI_POINT_COUNT_TO_UPDATE = 25;

        #endregion

        #region Скпк

        /// <summary>
        /// Идентификатор формата скпк
        /// </summary>
        public const string FORMAT_SKPK_DATA_FILE = "SKPKD";

        #endregion

        #region Геометрия

        /// <summary>
        /// Идентификатор формата = 'GEODF' Geometry Data File
        /// </summary>
        public const string FORMAT_GEOMETRY_DATA_FILE = "GEODF";

        /// <summary>
        /// Номер версии формата (На сентябрь 2020 2.01 = 201)
        /// </summary>
        public const ushort VERSION_GEOMETRY_DATA_FILE = 201;

        /// <summary>
        /// Номер версии формата (На январь 2020 1.06 = 106) для RoadPRO
        /// </summary>
        public const ushort VERSION_GEOMETRY_DATA_FILE_PRO = 106;

        /// <summary>
        /// Тип оборудования геометрии эмуляция
        /// </summary>
        public const int GEOMETRY_EQUIPMENT_TYPE_EMULATION = 128;

        /// <summary>
        /// Размер массива полос
        /// </summary>
        public const int GEOMETRY_LONG_HEEL_LINES_COUNT = 8;

        #endregion

        #region Колейность

        /// <summary>
        /// Идентификатор формата колейности
        /// </summary>
        public const string FORMAT_TRANSVERSE_PROFILE_DATA_FILE = "TRPRF";

        /// <summary>
        /// Максмальное количество профилометров для колейности sensorE
        /// </summary>
        public const byte MAX_SENSOR_E_PROFILOMETR_COUNT = 3;

        #endregion

        #region Сцепление

        /// <summary>
        /// Идентификатор формата scpdf
        /// </summary>
        public const string FORMAT_SCEP_DATA_FILE = "SCPDF";

        /// <summary>
        /// Номер версии формата scpdf
        /// </summary>
        public const ushort VERSION_SCEP_DATA_FILE = 106;

        #endregion

        public const string GLOBAL_PARAM_VALUE_TYPE_INT32 = "INT32";
        public const string GLOBAL_PARAM_VALUE_TYPE_UINT32 = "UINT32";
        public const string GLOBAL_PARAM_VALUE_TYPE_INT16 = "INT16";
        public const string GLOBAL_PARAM_VALUE_TYPE_UINT16 = "UINT16";
        public const string GLOBAL_PARAM_VALUE_TYPE_INT8 = "INT8";
        public const string GLOBAL_PARAM_VALUE_TYPE_UINT8 = "UINT8";
        public const string GLOBAL_PARAM_VALUE_TYPE_DOUBLE = "DOUBLE";
        public const string GLOBAL_PARAM_VALUE_TYPE_STRING = "STRING";
        public const string GLOBAL_PARAM_VALUE_TYPE_BOOL = "BOOL";
        public const string GLOBAL_PARAM_VALUE_TYPE_DATE = "DATE";
        public const string GLOBAL_PARAM_VALUE_TYPE_INT_ARRAY = "INT_ARRAY";
        public const string GLOBAL_PARAM_VALUE_TYPE_DOUBLE_ARRAY = "DOUBLE_ARRAY";
        public const string GLOBAL_PARAM_VALUE_TYPE_STRING_ARRAY = "STRING_ARRAY";
        public const string GLOBAL_PARAM_VALUE_TYPE_BOOL_ARRAY = "BOOL_ARRAY";
        public const string GLOBAL_PARAM_VALUE_TYPE_DATE_ARRAY = "DATE_ARRAY";
        public const string GLOBAL_PARAM_VALUE_TYPE_CONTAINER = "CONTAINER";

        /// !!! ВАЖНО !!!
        /// коды объектов переведены из System.Windows.Input.Key
        /// в линуксе коды возможно будут отличаться
        #region Фиксация объектов

        /// <summary>
        /// Путь к файлу, хранящему значения по умолчанию для объектов, фиксируемых с клавиатуры
        /// </summary>
        public const string FIX_OBJECT_DEFAULT_VALUES_PATH = "RoadExp(default values).txt";
        /// <summary>
        /// Сообщение о неверном типе базы данных при использовании фиксации объектов
        /// </summary>
        public const string FIX_OBJECT_USE_GDB_ONLY_MESSAGE = "Фиксация объектов возможна только при использовании базы данных типа GDB.";
        /// <summary>
        /// Сообщение о неверном типе базы данных при использовании фиксации объектов
        /// </summary>
        public const string FIX_OBJECT_FIXING_ERROR_MESSAGE = "Произошла ошибка при фиксации объекта: {0}.";

        #region Объекты

        #region Протяженные объекты
        /// <summary>
        /// Мост
        /// </summary>
        public const int FIX_OBJECT_BRIDGE = 56; // int.M;
        /// <summary>
        /// Ремонт
        /// </summary>
        public const int FIX_OBJECT_REPAIR = 34; // int.D0;
        /// <summary>
        /// Переходно-скоростная полоса (ПСП)
        /// </summary>
        public const int FIX_OBJECT_PSP = 140; // int.Oem1;
        /// <summary>
        /// Освещение
        /// </summary>
        public const int FIX_OBJECT_LIGHTING = 61; // int.R;
        /// <summary>
        /// Ограждение
        /// </summary>
        public const int FIX_OBJECT_FENCING = 48; // int.E;
        /// <summary>
        /// Нежилая застройка
        /// </summary>
        public const int FIX_OBJECT_NONRESIDENTAL_DEVELOPMENT = 142; // int.OemComma;
        /// <summary>
        /// Жилая застройка
        /// </summary>
        public const int FIX_OBJECT_RESIDENTAL_DEVELOPMENT = 144; // int.OemPeriod;
        /// <summary>
        /// Кустарник
        /// </summary>
        public const int FIX_OBJECT_BUSH = 145; // int.OemQuestion;
        /// <summary>
        /// Выгон
        /// </summary>
        public const int FIX_OBJECT_PASTURE = 150; // int.Oem5;
        /// <summary>
        /// Сигнальные столбики
        /// </summary>
        public const int FIX_OBJECT_SIGNAL_BOLLARDS = 62; // int.S;
        /// <summary>
        /// Пашня
        /// </summary>
        public const int FIX_OBJECT_ARABLE_LAND = 84; // int.Multiply;
        /// <summary>
        /// Лес
        /// </summary>
        public const int FIX_OBJECT_FOREST = 55; // int.L;

        /// <summary>
        /// Населенный пункт
        /// </summary>
        public const int FIX_OBJECT_LOCALITY = 57; // int.N;

        /// <summary>
        /// Площадка отдыха
        /// </summary>
        public const int FIX_OBJECT_RECOGNITION_AREA = 49; // int.F;
        /// <summary>
        /// Коллекция протяженных объектов
        /// </summary>
        public static readonly IList<int> EXTENDED_FIX_OBJECTS = new ReadOnlyCollection<int>
            (new List<int> {   FIX_OBJECT_BRIDGE, FIX_OBJECT_REPAIR, FIX_OBJECT_PSP, FIX_OBJECT_LIGHTING, FIX_OBJECT_FENCING, FIX_OBJECT_NONRESIDENTAL_DEVELOPMENT,
                                                    FIX_OBJECT_RESIDENTAL_DEVELOPMENT, FIX_OBJECT_BUSH, FIX_OBJECT_PASTURE, FIX_OBJECT_SIGNAL_BOLLARDS, FIX_OBJECT_ARABLE_LAND,
                                                    FIX_OBJECT_FOREST, FIX_OBJECT_LOCALITY, FIX_OBJECT_RECOGNITION_AREA});
        #endregion

        #region Точечные объекты
        /// <summary>
        /// Знак "Начало населенного пункта"
        /// </summary>
        public const int FIX_OBJECT_SIGN_BEGIN_TOWN = 66; // int.W;
        /// <summary>
        /// Знак "Конец населенного пункта"
        /// </summary>
        public const int FIX_OBJECT_SIGN_END_TOWN = 68; // int.Y;
        /// <summary>
        /// Линия связи
        /// </summary>
        public const int FIX_OBJECT_COMMUNICATION_LINE = 67; // int.X;
        /// <summary>
        /// Светофор
        /// </summary>
        public const int FIX_OBJECT_TRAFFIC_LIGHT = 146; // int.Oem3;
        /// <summary>
        /// Железнодорожный переезд
        /// </summary>
        public const int FIX_OBJECT_RAILROAD_CROSSING = 53; // int.J;
        /// <summary>
        /// Знак "Уступи дорогу"
        /// </summary>
        public const int FIX_OBJECT_SIGN_GIVE_WAY = 54; // int.K;
        /// <summary>
        /// Линия электропередач
        /// </summary>
        public const int FIX_OBJECT_POWER_LINE = 46; // int.C;
        /// <summary>
        /// Высоковольтная линия электропередач
        /// </summary>
        public const int FIX_OBJECT_HIGH_VOLTAGE_POWER_LINE = 65; // int.V;
        /// <summary>
        /// Пешеходный переход 
        /// </summary>
        public const int FIX_OBJECT_CROSSWALK = 141; // int.OemPlus;
        /// <summary>
        /// Пересечение/примыкание
        /// </summary>
        public const int FIX_OBJECT_INTERSECTION = 58; // int.O;
        /// <summary>
        /// Дорожно-патрульная служба
        /// </summary>
        public const int FIX_OBJECT_HIGHWAY_PATROL_SERVICE = 50; // int.G;
        /// <summary>
        /// Условная точка
        /// </summary>
        public const int FIX_OBJECT_CONDITIONAL_POINT = 44; // int.A;
        /// <summary>
        /// Пункт сервиса
        /// </summary>
        public const int FIX_OBJECT_SERVICVE_POINT = 51; // int.H;
        /// <summary>
        /// Автобусная остановка
        /// </summary>
        public const int FIX_OBJECT_BUS_STOP = 45; // int.B;
        /// <summary>
        /// Реклама
        /// </summary>
        public const int FIX_OBJECT_ADVERTISING = 89; // int.Divide;
        /// <summary>
        /// Труба
        /// </summary>
        public const int FIX_OBJECT_TUBE = 63; // int.T;
        /// <summary>
        /// Знак "Главная дорога"
        /// </summary>
        public const int FIX_OBJECT_SIGN_MAIN_ROAD = 69; // int.Z;
        /// <summary>
        /// Уширение проезжей части
        /// </summary>
        public const int FIX_OBJECT_ROADWAY_WIEDENING = 64; // int.U;
        /// <summary>
        /// Километровый столб
        /// </summary>
        public const int FIX_OBJECT_KILOMETER_POST = 60; // int.Q;
        /// <summary>
        /// Съезд
        /// </summary>
        public const int FIX_OBJECT_SIDE_WAY = 59; // int.P;
        /// <summary>
        /// Автозаправочная станция
        /// </summary>
        public const int FIX_OBJECT_GAS_STATION = 47; // int.D;
        /// <summary>
        /// Коллекция точечных объектов
        /// </summary>
        public static readonly IList<int> POINT_FIX_OBJECTS = new ReadOnlyCollection<int>
            (new List<int> {   FIX_OBJECT_SIGN_BEGIN_TOWN, FIX_OBJECT_SIGN_END_TOWN, FIX_OBJECT_COMMUNICATION_LINE, FIX_OBJECT_TRAFFIC_LIGHT, FIX_OBJECT_RAILROAD_CROSSING, 
                                                    /*FIX_OBJECT_SERVICE_STATION,*/ FIX_OBJECT_SIGN_GIVE_WAY, FIX_OBJECT_POWER_LINE, FIX_OBJECT_HIGH_VOLTAGE_POWER_LINE, FIX_OBJECT_CROSSWALK,
                                                    FIX_OBJECT_INTERSECTION, FIX_OBJECT_HIGHWAY_PATROL_SERVICE, FIX_OBJECT_CONDITIONAL_POINT, FIX_OBJECT_SERVICVE_POINT,
                                                    FIX_OBJECT_BUS_STOP, FIX_OBJECT_ADVERTISING, FIX_OBJECT_TUBE, FIX_OBJECT_SIGN_MAIN_ROAD, FIX_OBJECT_ROADWAY_WIEDENING,
                                                    FIX_OBJECT_KILOMETER_POST, FIX_OBJECT_SIDE_WAY, FIX_OBJECT_GAS_STATION});
        #endregion

        /// <summary>
        /// Код кнопкки - название объекта
        /// </summary>
        public static readonly IDictionary<int, string> FIX_OBJECT_KEY_TO_FIX_OBJECT_NAME = new ReadOnlyDictionary<int, string>
            (new Dictionary<int, string>() {   { FIX_OBJECT_BRIDGE, "Мост" }, { FIX_OBJECT_REPAIR, "Ремонт" }, { FIX_OBJECT_PSP, "ПСП" },
                                                                    { FIX_OBJECT_LIGHTING, "Освещение" }, { FIX_OBJECT_FENCING, "Ограждение" }, { FIX_OBJECT_NONRESIDENTAL_DEVELOPMENT, "Нежилая застройка" },
                                                                    { FIX_OBJECT_RESIDENTAL_DEVELOPMENT, "Жилая застройка" }, { FIX_OBJECT_BUSH, "Кустарник" }, { FIX_OBJECT_PASTURE, "Выгон" },
                                                                    { FIX_OBJECT_SIGNAL_BOLLARDS, "Сигнальные столбики" }, { FIX_OBJECT_ARABLE_LAND, "Пашня" }, { FIX_OBJECT_FOREST, "Лес" }, { FIX_OBJECT_LOCALITY, "Населенный пункт" },
                                                                    { FIX_OBJECT_SIGN_GIVE_WAY, "Знак \"Уступи дорогу\"" }, { FIX_OBJECT_SIGN_BEGIN_TOWN, "Знак \"Начало населенного пункта\"" },{ FIX_OBJECT_SIGN_END_TOWN, "Знак \"Конец населенного пункта\"" },{ FIX_OBJECT_COMMUNICATION_LINE, "Линия связи" }, { FIX_OBJECT_TRAFFIC_LIGHT, "Светофор" },
                                                                    { FIX_OBJECT_RAILROAD_CROSSING, "Железнодорожный переезд" }, { FIX_OBJECT_RECOGNITION_AREA, "Площадка отдыха" }, /*{ FIX_OBJECT_SERVICE_STATION, "СТО" },*/
                                                                    { FIX_OBJECT_POWER_LINE, "ЛЭП" }, { FIX_OBJECT_HIGH_VOLTAGE_POWER_LINE, "ВЛЭП" }, { FIX_OBJECT_CROSSWALK, "Пешеходный переход" },
                                                                    { FIX_OBJECT_INTERSECTION, "Пересечение/Примыкание" }, { FIX_OBJECT_HIGHWAY_PATROL_SERVICE, "ДПС" }, { FIX_OBJECT_CONDITIONAL_POINT, "Условная точка" },
                                                                    { FIX_OBJECT_SERVICVE_POINT, "Пункт сервиса" }, { FIX_OBJECT_BUS_STOP, "Автобусная остановка" }, { FIX_OBJECT_ADVERTISING, "Реклама" },
                                                                    { FIX_OBJECT_TUBE, "Труба" }, { FIX_OBJECT_SIGN_MAIN_ROAD, "Знак \"Главная дорога\"" }, { FIX_OBJECT_ROADWAY_WIEDENING, "Уширение проезжей части" },
                                                                    { FIX_OBJECT_KILOMETER_POST, "Киломеровый столб" }, { FIX_OBJECT_SIDE_WAY, "Съезд" }, { FIX_OBJECT_GAS_STATION, "АЗС" }});
        /// <summary>
        /// Название типа объекта в значениях по умолчанию - тип связанных объектов
        /// </summary>
        public static readonly IDictionary<string, List<int>> FIX_OBJECT_DEFAULT_OBJECT_TYPE_NAME_TO_FIX_OBJECT_TYPE = new ReadOnlyDictionary<string, List<int>>
            (new Dictionary<string, List<int>>() { { "Sign", new List<int>() { FIX_OBJECT_SIGN_MAIN_ROAD, FIX_OBJECT_SIGN_GIVE_WAY, FIX_OBJECT_SIGN_BEGIN_TOWN, FIX_OBJECT_SIGN_END_TOWN, FIX_OBJECT_KILOMETER_POST } },
                                                                        { "Lights", new List<int>() { FIX_OBJECT_TRAFFIC_LIGHT } },
                                                                        { "Tube", new List<int>() { FIX_OBJECT_TUBE } },
                                                                        { "Engin", new List<int>() { FIX_OBJECT_SERVICVE_POINT, FIX_OBJECT_HIGHWAY_PATROL_SERVICE, FIX_OBJECT_GAS_STATION } },
                                                                        { "LineOtvod", new List<int>() { FIX_OBJECT_BUSH, FIX_OBJECT_PASTURE, FIX_OBJECT_ARABLE_LAND, FIX_OBJECT_FOREST } },
                                                                        { "Commun", new List<int>() { FIX_OBJECT_POWER_LINE, FIX_OBJECT_HIGH_VOLTAGE_POWER_LINE, FIX_OBJECT_COMMUNICATION_LINE } },
                                                                        { "Busstop", new List<int>() { FIX_OBJECT_BUS_STOP } },
                                                                        { "CrossRailway", new List<int>() { FIX_OBJECT_RAILROAD_CROSSING } },
                                                                        { "Bridge", new List<int>() { FIX_OBJECT_BRIDGE } },
                                                                        { "HumanCross", new List<int>() { FIX_OBJECT_CROSSWALK } },
                                                                        { "Living", new List<int>() { FIX_OBJECT_RESIDENTAL_DEVELOPMENT, FIX_OBJECT_NONRESIDENTAL_DEVELOPMENT } },
                                                                        { "Cross", new List<int>() { FIX_OBJECT_INTERSECTION, FIX_OBJECT_SIDE_WAY } },
                                                                        { "Ograd", new List<int>() { FIX_OBJECT_FENCING } },
                                                                        { "Stolb", new List<int>() { FIX_OBJECT_SIGNAL_BOLLARDS } },
                                                                        { "TerritoryTown", new List<int>() { FIX_OBJECT_LOCALITY } },
                                                                        { "Work", new List<int>() { FIX_OBJECT_REPAIR } },
                                                                        { "PSP", new List<int>() { FIX_OBJECT_PSP } },
                                                                        { "Lighting", new List<int>() { FIX_OBJECT_LIGHTING } },
                                                                        { "ConditionalPoint", new List<int>() { FIX_OBJECT_CONDITIONAL_POINT } },
                                                                        { "Advertisement", new List<int>() { FIX_OBJECT_ADVERTISING } },
                                                                        { "Platform", new List<int>() { FIX_OBJECT_RECOGNITION_AREA } } });
        #region Коммуникации
        /// <summary>
        /// Тип коммуникации - номер коммуникации для получения значений по умолчанию 
        /// (0 - водопровод, 1 - высоковольтные лэп, 2 - воздушные линии связи, 3 - газопровод, 4 - кабель, 5 - канализация, 6 - лэп, 7 - нефтепровод, 8 подземный трубопровод)
        /// </summary>
        public static readonly IDictionary<int, int> FIX_OBJECT_COMMUNICATIONS_TO_DEFAULT_VALUE_SUBCALSS_NUM = new ReadOnlyDictionary<int, int>
            (new Dictionary<int, int>() { { (int)FIX_OBJECT_HIGH_VOLTAGE_POWER_LINE, 1 }, { (int)FIX_OBJECT_COMMUNICATION_LINE, 2 }, { (int)FIX_OBJECT_POWER_LINE, 6 } });
        /// <summary>
        /// Тип коммуникации - номер подкласса коммуникации из ЭПЖ (6 - высоковольтные лэп, 4 - воздушные линии связи, 5 - лэп)
        /// </summary>
        public static readonly IDictionary<int, int> FIX_OBJECT_COMMUNICATIONS_TO_SUBCALSS_NUM = new ReadOnlyDictionary<int, int>
            (new Dictionary<int, int>() { { (int)FIX_OBJECT_HIGH_VOLTAGE_POWER_LINE, 6 }, { (int)FIX_OBJECT_COMMUNICATION_LINE, 4 }, { (int)FIX_OBJECT_POWER_LINE, 5 } });
        #endregion

        #region Пересечения
        /// <summary>
        /// Тип пересечения - номер подкласса пересечения из ЭПЖ (2 - пересечение/примыкание, 3 - съезд)
        /// </summary>
        public static readonly IDictionary<int, int> FIX_OBJECT_INTERSECTIONS_TO_SUBCALSS_NUM = new ReadOnlyDictionary<int, int>
            (new Dictionary<int, int>() { { (int)FIX_OBJECT_INTERSECTION, 2 }, { (int)FIX_OBJECT_SIDE_WAY, 3 } });
        #endregion

        #region Пункты сервиса
        /// <summary>
        /// Тип пункта сервиса - номер пункта сервиса для получения значений по умолчанию 
        /// (0 - АЗС, 1 - вокзал, 2 - газовая АЗС, 3 - магазин, 4 - мойка, 5 - ДПС, 6 - СТО, 7 - стоянка)
        /// </summary>
        public static readonly IDictionary<int, int> FIX_OBJECT_SERVICVE_POINT_TO_DEFAULT_VALUE_SUBCALSS_NUM = new ReadOnlyDictionary<int, int>
            (new Dictionary<int, int>() { { (int)FIX_OBJECT_GAS_STATION, 0 }, { (int)FIX_OBJECT_HIGHWAY_PATROL_SERVICE, 5 }, { (int)FIX_OBJECT_SERVICVE_POINT, 6 } });
        /// <summary>
        /// Тип коммуникации - номер подкласса пункта сервиса из ЭПЖ (23 - АЗС, 24 - ДПС, 22 - СТО)
        /// </summary>
        public static readonly IDictionary<int, int> FIX_OBJECT_SERVICVE_POINT_TO_SUBCALSS_NUM = new ReadOnlyDictionary<int, int>
            (new Dictionary<int, int>() { { (int)FIX_OBJECT_GAS_STATION, 23 }, { (int)FIX_OBJECT_HIGHWAY_PATROL_SERVICE, 24 }, { (int)FIX_OBJECT_SERVICVE_POINT, 22 } });
        #endregion

        #region Линия отвода
        /// <summary>
        /// Тип объекта линии отвода - номер объекта линии отвода для получения значений по умолчанию 
        /// (0 - болото, 1 - выгон, 2 - газон, 3 - кладбище, 4 - кустарник, 5 - *пропущено*, 7 - поросль(лес?), 8 - сад, 9 - сель хоз застройка, 10 - пашня)
        /// </summary>
        public static readonly IDictionary<int, int> FIX_OBJECT_LINE_OTVOD_TO_DEFAULT_VALUE_SUBCALSS_NUM = new ReadOnlyDictionary<int, int>
            (new Dictionary<int, int>() { { (int)FIX_OBJECT_PASTURE, 1 }, { (int)FIX_OBJECT_BUSH, 4 }, { (int)FIX_OBJECT_FOREST, 7 }, { (int)FIX_OBJECT_ARABLE_LAND, 10 } });
        /// <summary>
        /// Тип объекта линии отвода - номер подкласса объекта линии отвода из ЭПЖ (17 - выгон, 4 - кустарник, 45 - лес, 6 - пашня)
        /// </summary>
        public static readonly IDictionary<int, int> FIX_OBJECT_LINE_OTVOD_TO_SUBCALSS_NUM = new ReadOnlyDictionary<int, int>
            (new Dictionary<int, int>() { { (int)FIX_OBJECT_PASTURE, 17 }, { (int)FIX_OBJECT_BUSH, 4 }, { (int)FIX_OBJECT_FOREST, 45 }, { (int)FIX_OBJECT_ARABLE_LAND, 6 } });
        #endregion

        #region Знаки
        /// <summary>
        /// Тип объекта знака - номер типа знака из ЭПЖ (306 - главная дорога, 315 - уступи дорогу, 408 - начало населенного пункта, 410 - конец населенного пункта, 
        ///                                              452 - километровый столб, )
        /// </summary>
        public static readonly IDictionary<int, int> FIX_OBJECT_SIGN_TO_TYPE_SIGN = new ReadOnlyDictionary<int, int>
            (new Dictionary<int, int>() {   { (int)FIX_OBJECT_SIGN_MAIN_ROAD, 306 }, { (int)FIX_OBJECT_SIGN_GIVE_WAY, 315 }, { (int)FIX_OBJECT_SIGN_BEGIN_TOWN, 408 },
                                            { (int)FIX_OBJECT_SIGN_END_TOWN, 410 }, { (int)FIX_OBJECT_KILOMETER_POST, 452 } });
        #endregion

        #region Застройка
        /// <summary>
        /// Тип объекта линии отвода - номер подкласса объекта линии отвода из ЭПЖ (12 - жилая застройка, 19 - нежилая застрйока)
        /// </summary>
        public static readonly IDictionary<int, int> FIX_OBJECT_LIVING_TO_SUBCALSS_NUM = new ReadOnlyDictionary<int, int>
            (new Dictionary<int, int>() { { (int)FIX_OBJECT_RESIDENTAL_DEVELOPMENT, 12 }, { (int)FIX_OBJECT_NONRESIDENTAL_DEVELOPMENT, 19 } });
        #endregion

        #region Объекты фиксируемые поперек дороги
        public static readonly List<int> FIX_OBJECT_ACROSS_ROAD_OBJECTS = new List<int>()
            { FIX_OBJECT_BRIDGE, FIX_OBJECT_COMMUNICATION_LINE, FIX_OBJECT_RAILROAD_CROSSING, FIX_OBJECT_POWER_LINE, FIX_OBJECT_HIGH_VOLTAGE_POWER_LINE, FIX_OBJECT_CROSSWALK,
              FIX_OBJECT_TUBE };
        #endregion

        #endregion

        #region Сторона
        /// <summary>
        /// Сторона заглушка
        /// </summary>
        public const int FIX_OBJECT_NOT_SIDE_VALUE = -1;
        /// <summary>
        /// Левая сторона
        /// </summary>
        public const int FIX_OBJECT_LEFT_SIDE = 23; // int.Left;
        /// <summary>
        /// Значение левой стороны
        /// </summary>
        public const int FIX_OBJECT_LEFT_SIDE_VALUE = 1;
        /// <summary>
        /// Правая сторона
        /// </summary>
        public const int FIX_OBJECT_RIGHT_SIDE = 25; // int.Right;
        /// <summary>
        /// Значение правой стороны
        /// </summary>
        public const int FIX_OBJECT_RIGHT_SIDE_VALUE = 0;
        /// <summary>
        /// По центру
        /// </summary>
        public const int FIX_OBJECT_CENTER_SIDE = 79; // int.NumPad5;
        /// <summary>
        /// Значение центральной стороны
        /// </summary>
        public const int FIX_OBJECT_CENTER_SIDE_VALUE = 2;
        /// <summary>
        /// Коллекция значений всех сторон
        /// </summary>
        public static readonly IList<int> FIX_OBJECT_SIDES = new ReadOnlyCollection<int>
            (new List<int> { FIX_OBJECT_LEFT_SIDE, FIX_OBJECT_RIGHT_SIDE, FIX_OBJECT_CENTER_SIDE });
        /// <summary>
        /// Код кнопкки - название стороны
        /// </summary>
        public static readonly IDictionary<int, string> FIX_OBJECT_SIDE_KEY_TO_FIX_SIDE_NAME = new ReadOnlyDictionary<int, string>
            (new Dictionary<int, string>() { { FIX_OBJECT_LEFT_SIDE, "Левая" }, { FIX_OBJECT_RIGHT_SIDE, "Правая" }, { FIX_OBJECT_CENTER_SIDE, "Центр" } });
        /// <summary>
        /// Значение стороны - название стороны 
        /// </summary>
        public static readonly IDictionary<int, string> FIX_OBJECT_SIDE_VALUE_TO_FIX_SIDE_NAME = new ReadOnlyDictionary<int, string>
            (new Dictionary<int, string>() { { FIX_OBJECT_LEFT_SIDE_VALUE, "Левая" }, { FIX_OBJECT_RIGHT_SIDE_VALUE, "Правая" }, { FIX_OBJECT_CENTER_SIDE_VALUE, "Центр" } });
        #endregion

        #region Цифры
        /// <summary>
        /// 0
        /// </summary>
        public const int FIX_OBJECT_NUM_0 = 74; // int.NumPad0;
        /// <summary>
        /// 1
        /// </summary>
        public const int FIX_OBJECT_NUM_1 = 35; // int.D1;
        /// <summary>
        /// 2
        /// </summary>
        public const int FIX_OBJECT_NUM_2 = 36; // int.D2;
        /// <summary>
        /// 3
        /// </summary>
        public const int FIX_OBJECT_NUM_3 = 37; // int.D3;
        /// <summary>
        /// 4
        /// </summary>
        public const int FIX_OBJECT_NUM_4 = 38; // int.D4;
        /// <summary>
        /// 5
        /// </summary>
        public const int FIX_OBJECT_NUM_5 = 39; // int.D5;
        /// <summary>
        /// 6
        /// </summary>
        public const int FIX_OBJECT_NUM_6 = 40; // int.D6;
        /// <summary>
        /// 7
        /// </summary>
        public const int FIX_OBJECT_NUM_7 = 41; // int.D7;
        /// <summary>
        /// 8
        /// </summary>
        public const int FIX_OBJECT_NUM_8 = 42; // int.D8;
        /// <summary>
        /// 9
        /// </summary>
        public const int FIX_OBJECT_NUM_9 = 43; // int.D9;
        #endregion

        #region Фиксация
        /// <summary>
        /// Фиксация объекта 
        /// </summary>
        public const int FIX_OBJECT = 18; // int.Space;
        #endregion

        #endregion

        /// <summary>
        /// Путь в реестре, где хранятся данные об установленных приложениях
        /// </summary>
        public const string REGISTRY_SOFT_PATH = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
        /// <summary>
        /// Путь в реестре, где хранятся данные об установленных приложениях x64
        /// </summary>
        public const string REGISTRY_SOFT_PATH_64 = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
        /// <summary>
        /// Ключевое слово для поиска Microsoft Visual C++ Redistributable
        /// </summary>
        public const string SEARCH_KEYWORD_VCREDIST = "Microsoft Visual C++";

        public const string LOADING_STATUS_TEXT = "Загружено {0} из {1}";
        public const string LOADING_TEXT = "Загрузка...";
    }
}
