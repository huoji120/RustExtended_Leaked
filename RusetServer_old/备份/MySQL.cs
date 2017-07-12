using RustExtended;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Timers;

public class MySQL
{
    public enum LogLevelType
    {
        NONE,
        ERRORS,
        ALL
    }

    public enum ClientFlags : ulong
    {
        CLIENT_LONG_PASSWORD = 1uL,
        CLIENT_FOUND_ROWS,
        CLIENT_LONG_FLAG = 4uL,
        CLIENT_CONNECT_WITH_DB = 8uL,
        CLIENT_NO_SCHEMA = 16uL,
        CLIENT_COMPRESS = 32uL,
        CLIENT_ODBC = 64uL,
        CLIENT_LOCAL_FILES = 128uL,
        CLIENT_IGNORE_SPACE = 256uL,
        CLIENT_PROTOCOL_41 = 512uL,
        CLIENT_INTERACTIVE = 1024uL,
        CLIENT_SSL = 2048uL,
        CLIENT_IGNORE_SIGPIPE = 4096uL,
        CLIENT_TRANSACTIONS = 8192uL,
        CLIENT_RESERVED = 16384uL,
        CLIENT_SECURE_CONNECTION = 32768uL,
        CLIENT_MULTI_STATEMENTS = 65536uL,
        CLIENT_MULTI_RESULTS = 131072uL,
        CLIENT_PS_MULTI_RESULTS = 262144uL
    }

    public enum MySqlOption
    {
        MYSQL_OPT_CONNECT_TIMEOUT,
        MYSQL_OPT_COMPRESS,
        MYSQL_OPT_NAMED_PIPE,
        MYSQL_INIT_COMMAND,
        MYSQL_READ_DEFAULT_FILE,
        MYSQL_READ_DEFAULT_GROUP,
        MYSQL_SET_CHARSET_DIR,
        MYSQL_SET_CHARSET_NAME,
        MYSQL_OPT_LOCAL_INFILE,
        MYSQL_OPT_PROTOCOL,
        MYSQL_SHARED_MEMORY_BASE_NAME,
        MYSQL_OPT_READ_TIMEOUT,
        MYSQL_OPT_WRITE_TIMEOUT,
        MYSQL_OPT_USE_RESULT,
        MYSQL_OPT_USE_REMOTE_CONNECTION,
        MYSQL_OPT_USE_EMBEDDED_CONNECTION,
        MYSQL_OPT_GUESS_CONNECTION,
        MYSQL_SET_CLIENT_IP,
        MYSQL_SECURE_AUTH,
        MYSQL_REPORT_DATA_TRUNCATION,
        MYSQL_OPT_RECONNECT,
        MYSQL_OPT_SSL_VERIFY_SERVER_CERT
    }

    [Flags]
    public enum FieldFlags
    {
        NOT_NULL_FLAG = 1,
        PRI_KEY_FLAG = 2,
        UNIQUE_KEY_FLAG = 4,
        MULTIPLE_KEY_FLAG = 8,
        BLOB_FLAG = 16,
        UNSIGNED_FLAG = 32,
        ZEROFILL_FLAG = 64,
        BINARY_FLAG = 128,
        ENUM_FLAG = 256,
        AUTO_INCREMENT_FLAG = 512,
        TIMESTAMP_FLAG = 1024,
        SET_FLAG = 2048,
        NUM_FLAG = 32768,
        PART_KEY_FLAG = 16384,
        GROUP_FLAG = 32768,
        UNIQUE_FLAG = 65536
    }

    public enum FieldTypes
    {
        MYSQL_TYPE_DECIMAL,
        MYSQL_TYPE_TINY,
        MYSQL_TYPE_SHORT,
        MYSQL_TYPE_LONG,
        MYSQL_TYPE_FLOAT,
        MYSQL_TYPE_DOUBLE,
        MYSQL_TYPE_NULL,
        MYSQL_TYPE_TIMESTAMP,
        MYSQL_TYPE_LONGLONG,
        MYSQL_TYPE_INT24,
        MYSQL_TYPE_DATE,
        MYSQL_TYPE_TIME,
        MYSQL_TYPE_DATETIME,
        MYSQL_TYPE_YEAR,
        MYSQL_TYPE_NEWDATE,
        MYSQL_TYPE_VARCHAR,
        MYSQL_TYPE_BIT,
        MYSQL_TYPE_TIMESTAMP2,
        MYSQL_TYPE_DATETIME2,
        MYSQL_TYPE_TIME2,
        MYSQL_TYPE_NEWDECIMAL = 246,
        MYSQL_TYPE_ENUM,
        MYSQL_TYPE_SET,
        MYSQL_TYPE_TINY_BLOB,
        MYSQL_TYPE_MEDIUM_BLOB,
        MYSQL_TYPE_LONG_BLOB,
        MYSQL_TYPE_BLOB,
        MYSQL_TYPE_VAR_STRING,
        MYSQL_TYPE_STRING,
        MYSQL_TYPE_GEOMETRY,
        FIELD_TYPE_DECIMAL = 0,
        FIELD_TYPE_NEWDECIMAL = 246,
        FIELD_TYPE_TINY = 1,
        FIELD_TYPE_SHORT,
        FIELD_TYPE_LONG,
        FIELD_TYPE_FLOAT,
        FIELD_TYPE_DOUBLE,
        FIELD_TYPE_NULL,
        FIELD_TYPE_TIMESTAMP,
        FIELD_TYPE_LONGLONG,
        FIELD_TYPE_INT24,
        FIELD_TYPE_DATE,
        FIELD_TYPE_TIME,
        FIELD_TYPE_DATETIME,
        FIELD_TYPE_YEAR,
        FIELD_TYPE_NEWDATE,
        FIELD_TYPE_ENUM = 247,
        FIELD_TYPE_SET,
        FIELD_TYPE_TINY_BLOB,
        FIELD_TYPE_MEDIUM_BLOB,
        FIELD_TYPE_LONG_BLOB,
        FIELD_TYPE_BLOB,
        FIELD_TYPE_VAR_STRING,
        FIELD_TYPE_STRING,
        FIELD_TYPE_CHAR = 1,
        FIELD_TYPE_INTERVAL = 247,
        FIELD_TYPE_GEOMETRY = 255,
        FIELD_TYPE_BIT = 16
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class SQLQuery
    {
        public string Query;

        public MySQL.Result Result;

        public SQLQuery(MySQL.Result result, string query)
        {
            this.Query = query;
            this.Result = result;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class Field
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        [MarshalAs(UnmanagedType.LPStr)]
        public string OrgName;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Table;

        [MarshalAs(UnmanagedType.LPStr)]
        public string OrgTable;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Database;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Catalog;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Default;

        public uint Length;

        public uint MaxLength;

        public uint NameLength;

        public uint OrgNameLength;

        public uint TableLength;

        public uint OrgTableLength;

        public uint DatabaseLength;

        public uint CatalogLength;

        public uint DefaultLength;

        public MySQL.FieldFlags Flags;

        public uint Decimals;

        public uint Charset;

        public MySQL.FieldTypes Type;

        public bool IsAutoIncrement
        {
            get
            {
                return !(this.Flags & MySQL.FieldFlags.AUTO_INCREMENT_FLAG).Equals(0);
            }
        }

        public bool IsPrimaryKey
        {
            get
            {
                return !(this.Flags & MySQL.FieldFlags.PRI_KEY_FLAG).Equals(0);
            }
        }

        public bool IsNotNull
        {
            get
            {
                return !(this.Flags & MySQL.FieldFlags.NOT_NULL_FLAG).Equals(0);
            }
        }

        public bool IsNumber
        {
            get
            {
                return !(this.Flags & MySQL.FieldFlags.NUM_FLAG).Equals(0);
            }
        }

        public bool IsBinary
        {
            get
            {
                return !(this.Flags & MySQL.FieldFlags.BINARY_FLAG).Equals(0);
            }
        }

        public bool IsBlob
        {
            get
            {
                return !(this.Flags & MySQL.FieldFlags.BLOB_FLAG).Equals(0);
            }
        }

        public bool IsEnum
        {
            get
            {
                return !(this.Flags & MySQL.FieldFlags.ENUM_FLAG).Equals(0);
            }
        }

        public bool IsSet
        {
            get
            {
                return !(this.Flags & MySQL.FieldFlags.SET_FLAG).Equals(0);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class Record
    {
        private string Data;

        private MySQL.Field _Field;

        public MySQL.Field Field
        {
            get
            {
                return this._Field;
            }
        }

        public bool IsNull
        {
            get
            {
                return string.IsNullOrEmpty(this.Data);
            }
        }

        public string AsString
        {
            get
            {
                return this.Data;
            }
        }

        public bool AsBool
        {
            get
            {
                return this.Data.Equals("YES") || this.Data.Equals("Y") || this.Data.Equals("ON");
            }
        }

        public int AsInt
        {
            get
            {
                int result;
                if (!int.TryParse(this.Data, out result))
                {
                    result = 0;
                }
                return result;
            }
        }

        public long AsInt64
        {
            get
            {
                long result;
                if (!long.TryParse(this.Data, out result))
                {
                    result = 0L;
                }
                return result;
            }
        }

        public uint AsUInt
        {
            get
            {
                uint result;
                if (!uint.TryParse(this.Data, out result))
                {
                    result = 0u;
                }
                return result;
            }
        }

        public ulong AsUInt64
        {
            get
            {
                ulong result;
                if (!ulong.TryParse(this.Data, out result))
                {
                    result = 0uL;
                }
                return result;
            }
        }

        public float AsFloat
        {
            get
            {
                float result;
                if (!float.TryParse(this.Data, out result))
                {
                    result = 0f;
                }
                return result;
            }
        }

        public double AsDouble
        {
            get
            {
                double result;
                if (!double.TryParse(this.Data, out result))
                {
                    result = 0.0;
                }
                return result;
            }
        }

        public TimeSpan AsTimeSpan
        {
            get
            {
                TimeSpan result;
                if (!TimeSpan.TryParse(this.Data, out result))
                {
                    result = default(TimeSpan);
                }
                return result;
            }
        }

        public DateTime AsDateTime
        {
            get
            {
                DateTime result;
                if (!DateTime.TryParse(this.Data, out result))
                {
                    result = default(DateTime);
                }
                return result;
            }
        }

        public Record(MySQL.Field field, string value)
        {
            this._Field = field;
            this.Data = value;
        }

        public T AsEnum<T>()
        {
            if (string.IsNullOrEmpty(this.Data) || !this._Field.IsEnum)
            {
                this.Data = "0";
            }
            return (T)((object)Enum.Parse(typeof(T), this.Data, true));
        }

        public override string ToString()
        {
            return string.Format("Value={0}; Field(name=\"{1}\", Length=\"{2}\", Type=\"{3}\")", new object[]
            {
                this.Data,
                this._Field.Name,
                this._Field.Length,
                this._Field.Type
            });
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class Row
    {
        private Dictionary<string, MySQL.Record> Record;

        private MySQL.Result Result;

        public Row(MySQL.Result store, IntPtr value)
        {
            this.Record = new Dictionary<string, MySQL.Record>();
            for (int i = 0; i < store.Field.Count; i++)
            {
                this.Record.Add(store.Field[i].Name, new MySQL.Record(store.Field[i], this.GetRecord(value, i)));
            }
        }

        public List<MySQL.Record> Get()
        {
            return new List<MySQL.Record>(this.Record.Values);
        }

        public MySQL.Record Get(string field_name)
        {
            MySQL.Record result;
            if (this.Record.ContainsKey(field_name))
            {
                result = this.Record[field_name];
            }
            else
            {
                result = null;
            }
            return result;
        }

        private string GetRecord(IntPtr res, int index)
        {
            IntPtr intPtr = Marshal.ReadIntPtr(res, index * IntPtr.Size);
            string result;
            if (intPtr != IntPtr.Zero)
            {
                result = Marshal.PtrToStringAnsi(intPtr);
            }
            else
            {
                result = null;
            }
            return result;
        }
    }

    public class Result
    {
        public List<MySQL.Field> Field = new List<MySQL.Field>();

        public List<MySQL.Row> Row = new List<MySQL.Row>();

        public uint Fields;

        public ulong Rows;

        public void Clear()
        {
            if (this.Row != null)
            {
                this.Row.Clear();
            }
            this.Rows = 0uL;
            if (this.Field != null)
            {
                this.Field.Clear();
            }
            this.Fields = 0u;
        }
    }

    private static MySQL Instance = new MySQL();

    private static bool Initialized = false;

    private static string HostName = null;

    private static uint HostPort = 3306u;

    private static string Username = null;

    private static string Password = null;

    private static string Database = null;

    private static string UnixSocket = null;

    private static int ConnectFlags = 0;

    private static IntPtr SQL = IntPtr.Zero;

    public static List<string> Queue = new List<string>();

    public static readonly uint MinVersion = 40100u;

    public static readonly uint MaxVersion = 60103u;

    public static string Charset = "latin1";

    public static bool Reconnect = true;

    public static int ConnectTimeout = 300;

    public static Timer QueueThread = null;

    public static MySQL.LogLevelType LogLevel = MySQL.LogLevelType.ERRORS;

    public static MySQL Singleton
    {
        get
        {
            return MySQL.Instance;
        }
    }

    public static uint VersionId
    {
        get;
        private set;
    }

    public static Version Version
    {
        get;
        private set;
    }

    public static Version ServerVersion
    {
        get;
        private set;
    }

    public static uint Protocol
    {
        get;
        private set;
    }

    public static string Host
    {
        get;
        private set;
    }

    public static uint Port
    {
        get;
        private set;
    }

    public static bool IsReady
    {
        get;
        private set;
    }

    public static bool IsConnected
    {
        get;
        private set;
    }

    public static bool Connected
    {
        get
        {
            return MySQL.IsConnected = (MySQL.SQL != IntPtr.Zero && MySQL.mysql_ping(MySQL.SQL) == 0);
        }
    }

    public static bool Queued
    {
        get
        {
            return MySQL.SQL != IntPtr.Zero && MySQL.Queue.Count > 0;
        }
    }

    public static uint ErrorCode
    {
        get
        {
            uint result;
            if (MySQL.SQL != IntPtr.Zero)
            {
                result = MySQL.mysql_errno(MySQL.SQL);
            }
            else
            {
                result = 0u;
            }
            return result;
        }
    }

    public static IntPtr Handle
    {
        get
        {
            return MySQL.SQL;
        }
    }

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_init(IntPtr nulled);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern int mysql_options(IntPtr mysql, MySQL.MySqlOption option, IntPtr arg);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern int mysql_ping(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern void mysql_close(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_real_connect(IntPtr mysql, string host, string user, string passwd, string db_name, uint port, string unix_socket, int client_flag);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_get_client_info();

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern uint mysql_get_client_version();

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_get_server_info(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern uint mysql_get_server_version(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_get_host_info(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_character_set_name(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_list_dbs(IntPtr mysql, string wild);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern int mysql_select_db(IntPtr mysql, string db_name);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern int mysql_query(IntPtr mysql, string query);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern int mysql_real_query(IntPtr mysql, string query, int length);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_store_result(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_use_result(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern void mysql_free_result(IntPtr result);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern uint mysql_errno(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_error(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_list_tables(IntPtr mysql, string wild);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_list_fields(IntPtr mysql, string table, string wild);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern uint mysql_field_count(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern uint mysql_num_fields(IntPtr result);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern ulong mysql_num_rows(IntPtr result);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern ulong mysql_affected_rows(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern uint mysql_fetch_lengths(IntPtr result);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_fetch_field(IntPtr result);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_fetch_row(IntPtr result);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern void mysql_data_seek(IntPtr result, ulong offset);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern ulong mysql_insert_id(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern int mysql_set_character_set(IntPtr mysql, IntPtr charset);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern int mysql_real_escape_string(IntPtr mysql, string to, string from, int length);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_stat(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern uint mysql_get_proto_info(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern IntPtr mysql_list_processes(IntPtr mysql);

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern void mysql_thread_end();

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern uint mysql_thread_init();

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern uint mysql_thread_safe();

    [DllImport("libmysql.dll", SetLastError = true)]
    internal static extern ulong mysql_thread_id(IntPtr mysql);

    public MySQL()
    {
        MySQL.Charset = "latin1";
        MySQL.Host = "127.0.0.1";
        MySQL.Port = 3306u;
    }

    public static bool Initialize()
    {
        MySQL.SQL = MySQL.mysql_init(IntPtr.Zero);
        bool result;
        if (MySQL.Initialized = (MySQL.SQL != IntPtr.Zero))
        {
            MySQL.VersionId = MySQL.mysql_get_client_version();
            MySQL.Version = new Version(MySQL.mysql_get_client_version().ToString("#-##-##").Replace("-", "."));
            if (MySQL.VersionId < MySQL.MinVersion || MySQL.VersionId > MySQL.MaxVersion)
            {
                result = (MySQL.Initialized = false);
                return result;
            }
        }
        result = MySQL.Initialized;
        return result;
    }

    public static bool Connect(string host, string username, string password, string db_name, uint port, string unix_socket = null, MySQL.ClientFlags flags = (MySQL.ClientFlags)0uL)
    {
        if (!MySQL.Initialized)
        {
            MySQL.Initialize();
        }
        MySQL.HostName = host;
        MySQL.HostPort = port;
        MySQL.Username = username;
        MySQL.Password = password;
        MySQL.Database = db_name;
        MySQL.UnixSocket = unix_socket;
        MySQL.ConnectFlags = (int)(flags | MySQL.ClientFlags.CLIENT_MULTI_STATEMENTS);
        MySQL.mysql_options(MySQL.SQL, MySQL.MySqlOption.MYSQL_OPT_CONNECT_TIMEOUT, Marshal.StringToCoTaskMemAnsi(MySQL.ConnectTimeout.ToString()));
        MySQL.mysql_options(MySQL.SQL, MySQL.MySqlOption.MYSQL_OPT_RECONNECT, Marshal.StringToCoTaskMemAnsi(MySQL.Reconnect ? "1" : "0"));
        if (MySQL.Charset.Equals("UTF8", StringComparison.OrdinalIgnoreCase))
        {
            MySQL.mysql_options(MySQL.SQL, MySQL.MySqlOption.MYSQL_SET_CHARSET_NAME, Marshal.StringToCoTaskMemAnsi(MySQL.Charset));
            MySQL.mysql_options(MySQL.SQL, MySQL.MySqlOption.MYSQL_INIT_COMMAND, Marshal.StringToCoTaskMemAnsi("SET NAMES utf8"));
        }
        return MySQL.Connect();
    }

    public static bool Connect()
    {
        bool result;
        if (!MySQL.Initialized || MySQL.SQL == IntPtr.Zero)
        {
            result = false;
        }
        else if (MySQL.mysql_real_connect(MySQL.SQL, MySQL.HostName, MySQL.Username, MySQL.Password, MySQL.Database, MySQL.HostPort, MySQL.UnixSocket, MySQL.ConnectFlags) == IntPtr.Zero)
        {
            result = false;
        }
        else
        {
            MySQL.ServerVersion = new Version(MySQL.mysql_get_server_version(MySQL.SQL).ToString("#-##-##").Replace("-", "."));
            result = (MySQL.IsReady = true);
        }
        return result;
    }

    public static void Disconnect()
    {
        if (!(MySQL.SQL == IntPtr.Zero))
        {
            MySQL.QueueThread = null;
            MySQL.mysql_close(MySQL.SQL);
            MySQL.IsConnected = false;
        }
    }

    public static void Close()
    {
        if (MySQL.SQL != IntPtr.Zero)
        {
            MySQL.mysql_close(MySQL.SQL);
        }
        MySQL.IsConnected = false;
    }

    public static string GetCharacterSet()
    {
        string result;
        if (MySQL.SQL == IntPtr.Zero)
        {
            result = null;
        }
        else
        {
            result = Marshal.PtrToStringAnsi(MySQL.mysql_character_set_name(MySQL.SQL));
        }
        return result;
    }

    public static bool SelectDB(string db_name)
    {
        return MySQL.Initialized && !(MySQL.SQL == IntPtr.Zero) && MySQL.mysql_select_db(MySQL.SQL, db_name) == 0;
    }

    public static string Error()
    {
        string result;
        if (MySQL.SQL == IntPtr.Zero)
        {
            result = null;
        }
        else
        {
            result = Marshal.PtrToStringAnsi(MySQL.mysql_error(MySQL.SQL));
        }
        return result;
    }

    public static string EscapeString(string input)
    {
        string result;
        if (string.IsNullOrEmpty(input))
        {
            result = "NULL";
        }
        else
        {
            input = input.Trim(new char[]
            {
                '\''
            });
            input = Regex.Replace(input, "[\\x00'\"\\b\\n\\r\\t\\cZ\\\\%_]", delegate (Match match)
            {
                string value = match.Value;
                string text = value;
                string result2;
                if (text != null)
                {
                    if (text == "\b")
                    {
                        result2 = "\\b";
                        return result2;
                    }
                    if (text == "\n")
                    {
                        result2 = "\\n";
                        return result2;
                    }
                    if (text == "\r")
                    {
                        result2 = "\\r";
                        return result2;
                    }
                    if (text == "\t")
                    {
                        result2 = "\\t";
                        return result2;
                    }
                    if (text == "\0")
                    {
                        result2 = "\\0";
                        return result2;
                    }
                    if (text == "\u001a")
                    {
                        result2 = "\\Z";
                        return result2;
                    }
                }
                result2 = "\\" + value;
                return result2;
            });
            result = "'" + input + "'";
        }
        return result;
    }

    public static string QuoteString(string input)
    {
        string result;
        if (string.IsNullOrEmpty(input))
        {
            result = "NULL";
        }
        else
        {
            input = input.Trim(new char[]
            {
                '\''
            });
            input = Regex.Replace(input, "[\\x00'\"\\b\\n\\r\\t\\cZ]", delegate (Match match)
            {
                string value = match.Value;
                string a;
                string result2;
                if ((a = value) != null)
                {
                    if (a == "\b")
                    {
                        result2 = "\\b";
                        return result2;
                    }
                    if (a == "\n")
                    {
                        result2 = "\\n";
                        return result2;
                    }
                    if (a == "\r")
                    {
                        result2 = "\\r";
                        return result2;
                    }
                    if (a == "\t")
                    {
                        result2 = "\\t";
                        return result2;
                    }
                    if (a == "\0")
                    {
                        result2 = "\\0";
                        return result2;
                    }
                }
                result2 = "\\" + value;
                return result2;
            });
            result = "'" + input + "'";
        }
        return result;
    }

    public static List<string> Databases(string wild = null)
    {
        IntPtr intPtr = MySQL.mysql_list_dbs(MySQL.SQL, wild);
        List<string> result;
        if (intPtr == IntPtr.Zero)
        {
            result = null;
        }
        else
        {
            List<string> list = new List<string>();
            for (ulong num = 0uL; num < MySQL.mysql_num_rows(intPtr); num += 1uL)
            {
                IntPtr ptr = MySQL.mysql_fetch_row(intPtr);
                list.Add(Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(ptr)));
            }
            result = list;
        }
        return result;
    }

    public static void Update(string query)
    {
        if (MySQL.IsReady)
        {
            try
            {
                MySQL.IsReady = false;
                int num;
                if (Core.MySQL_UTF8)
                {
                    num = MySQL.mysql_query(MySQL.SQL, query);
                }
                else
                {
                    num = MySQL.mysql_real_query(MySQL.SQL, query, query.Length);
                }
                if (MySQL.LogLevel > MySQL.LogLevelType.ERRORS)
                {
                    Helper.LogSQL(query, false);
                }
                if (num != 0 || MySQL.ErrorCode != 0u)
                {
                    if (MySQL.LogLevel > MySQL.LogLevelType.NONE)
                    {
                        Helper.LogSQLError(MySQL.Error(), true);
                    }
                    MySQL.mysql_ping(MySQL.SQL);
                    MySQL.IsReady = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                Helper.LogSQLError(query, false);
                Helper.LogSQLError(ex.Message.ToString(), true);
            }
            MySQL.IsReady = true;
        }
    }

    public static MySQL.Result Query(string query, bool skip = false)
    {
        MySQL.Result result = null;
        MySQL.Result result3;
        try
        {
            if (!MySQL.IsReady && skip)
            {
                MySQL.Result result2 = result;
                result3 = result2;
                return result3;
            }
            MySQL.IsReady = false;
            int num;
            if (Core.MySQL_UTF8)
            {
                num = MySQL.mysql_query(MySQL.SQL, query);
            }
            else
            {
                num = MySQL.mysql_real_query(MySQL.SQL, query, query.Length);
            }
            if (num != 0 || MySQL.ErrorCode != 0u)
            {
                if (MySQL.LogLevel > MySQL.LogLevelType.NONE)
                {
                    Helper.LogSQLError(MySQL.Error(), true);
                }
                MySQL.mysql_ping(MySQL.SQL);
                MySQL.IsReady = true;
                MySQL.Result result2 = null;
                result3 = result2;
                return result3;
            }
            if (MySQL.LogLevel > MySQL.LogLevelType.ERRORS)
            {
                Helper.LogSQL(query, false);
            }
            IntPtr intPtr = MySQL.mysql_store_result(MySQL.SQL);
            if (MySQL.ErrorCode == 0u && !(intPtr == IntPtr.Zero))
            {
                result = new MySQL.Result();
                result.Fields = MySQL.mysql_num_fields(intPtr);
                result.Rows = MySQL.mysql_num_rows(intPtr);
                int num2 = 0;
                while ((long)num2 < (long)((ulong)result.Fields))
                {
                    result.Field.Add((MySQL.Field)Marshal.PtrToStructure(MySQL.mysql_fetch_field(intPtr), typeof(MySQL.Field)));
                    num2++;
                }
                IntPtr value;
                while ((value = MySQL.mysql_fetch_row(intPtr)) != IntPtr.Zero)
                {
                    result.Row.Add(new MySQL.Row(result, value));
                }
                MySQL.mysql_free_result(intPtr);
            }
            else
            {
                result = null;
            }
        }
        catch (Exception ex)
        {
            result = null;
            Helper.LogSQLError(query, false);
            Helper.LogSQLError(ex.Message.ToString(), true);
        }
        MySQL.IsReady = true;
        result3 = result;
        return result3;
    }

    private static void ProcessQueue()
    {
        if (MySQL.QueueThread != null)
        {
            if (MySQL.Queue.Count > 0 && MySQL.IsReady && MySQL.Query(MySQL.Queue[0], true) != null)
            {
                MySQL.Queue.RemoveAt(0);
            }
        }
    }
}
