using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RustProtect
{
	public class Method
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct Result
		{
			private static Type type_0;

			private static object object_0;

			public Type Type
			{
				get
				{
					return Method.Result.type_0;
				}
			}

			public bool IsNull
			{
				get
				{
					return object.Equals(Method.Result.object_0, null);
				}
			}

			public DateTime AsDateTime
			{
				get
				{
					return Convert.ToDateTime(Method.Result.object_0);
				}
			}

			public decimal AsDecimal
			{
				get
				{
					return Convert.ToDecimal(Method.Result.object_0);
				}
			}

			public bool AsBoolean
			{
				get
				{
					return Convert.ToBoolean(Method.Result.object_0);
				}
			}

			public string AsString
			{
				get
				{
					return Convert.ToString(Method.Result.object_0);
				}
			}

			public double AsDouble
			{
				get
				{
					return Convert.ToDouble(Method.Result.object_0);
				}
			}

			public float AsSingle
			{
				get
				{
					return Convert.ToSingle(Method.Result.object_0);
				}
			}

			public float AsFloat
			{
				get
				{
					return Convert.ToSingle(Method.Result.object_0);
				}
			}

			public ulong AsUInt64
			{
				get
				{
					return Convert.ToUInt64(Method.Result.object_0);
				}
			}

			public long AsInt64
			{
				get
				{
					return Convert.ToInt64(Method.Result.object_0);
				}
			}

			public uint AsUInt32
			{
				get
				{
					return Convert.ToUInt32(Method.Result.object_0);
				}
			}

			public int AsInt32
			{
				get
				{
					return Convert.ToInt32(Method.Result.object_0);
				}
			}

			public ushort AsUInt16
			{
				get
				{
					return Convert.ToUInt16(Method.Result.object_0);
				}
			}

			public short AsInt16
			{
				get
				{
					return Convert.ToInt16(Method.Result.object_0);
				}
			}

			public sbyte AsSByte
			{
				get
				{
					return Convert.ToSByte(Method.Result.object_0);
				}
			}

			public byte AsByte
			{
				get
				{
					return Convert.ToByte(Method.Result.object_0);
				}
			}

			public char AsChar
			{
				get
				{
					return Convert.ToChar(Method.Result.object_0);
				}
			}

			public object AsObject
			{
				get
				{
					return Method.Result.object_0;
				}
			}

			public string[] AsStrings
			{
				get
				{
					if (!this.IsNull && Method.Result.object_0 is string[])
					{
						return Method.Result.object_0 as string[];
					}
					return null;
				}
			}

			public char[] AsCharArray
			{
				get
				{
					if (!this.IsNull && Method.Result.object_0 is char[])
					{
						return Method.Result.object_0 as char[];
					}
					return null;
				}
			}

			public byte[] AsByteArray
			{
				get
				{
					if (!this.IsNull && Method.Result.object_0 is byte[])
					{
						return Method.Result.object_0 as byte[];
					}
					return null;
				}
			}

			public sbyte[] AsSByteArray
			{
				get
				{
					if (!this.IsNull && Method.Result.object_0 is sbyte[])
					{
						return Method.Result.object_0 as sbyte[];
					}
					return null;
				}
			}

			public short[] AsInt16Array
			{
				get
				{
					if (!this.IsNull && Method.Result.object_0 is short[])
					{
						return Method.Result.object_0 as short[];
					}
					return null;
				}
			}

			public ushort[] AsUInt16Array
			{
				get
				{
					if (!this.IsNull && Method.Result.object_0 is ushort[])
					{
						return Method.Result.object_0 as ushort[];
					}
					return null;
				}
			}

			public Result(object value, Type type)
			{
				Method.Result.object_0 = value;
				Method.Result.type_0 = type;
			}

			public T AsType<T>()
			{
				return (T)((object)Method.Result.object_0);
			}
		}

		private static Method method_0 = new Method();

		private static Dictionary<string, Type> dictionary_0 = new Dictionary<string, Type>();

		private static Dictionary<string, Assembly> dictionary_1 = new Dictionary<string, Assembly>();

		[CompilerGenerated]
		private static bool bool_0;

		[CompilerGenerated]
		private static bool bool_1;

		[CompilerGenerated]
		private static string string_0;

		public static bool Initialized
		{
			get;
			private set;
		}

		public static int Count
		{
			get
			{
				return Method.dictionary_0.Count;
			}
		}

		public static bool HasError
		{
			get;
			private set;
		}

		public static string LastError
		{
			get;
			private set;
		}

		public static string[] Assemblies
		{
			get
			{
				return EnumerableToArray.ToArray<string>(Method.dictionary_1.Keys);
			}
		}

		public static string[] Keys
		{
			get
			{
				return EnumerableToArray.ToArray<string>(Method.dictionary_0.Keys);
			}
		}

		public static Type[] Types
		{
			get
			{
				return EnumerableToArray.ToArray<Type>(Method.dictionary_0.Values);
			}
		}

        public static bool huojisb = false;
        static Assembly[] huoji;
        public static bool Initialize()
		{
            if (!huojisb)
            {
                Assembly[] huoji = AppDomain.CurrentDomain.GetAssemblies();
                huojisb = true;
            }

            Assembly[] assemblies = huoji;
            for (int i = 0; i < assemblies.Length; i++)
            {
                Assembly assembly = assemblies[i];
                if (!Method.Initialize(assembly))
                {
                    return false;
                }
            }
            return true;
            /*
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Assembly assembly = assemblies[i];
				if (!Method.Initialize(assembly))
				{
					return false;
				}
			}
			return true;
            */
        }

		public static bool Initialize(Assembly assembly)
		{
			if (Method.dictionary_1 != null && !Method.dictionary_1.ContainsValue(assembly))
			{
                Method.dictionary_1.Add(assembly.GetName().Name, assembly);
				Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
				{
					Type type = types[i];
					if (!type.FullName.Contains("<") && !type.FullName.Contains("$") && !type.FullName.Contains("`"))
					{
						string key = type.FullName.Replace("+", ".");
                        UnityEngine.Debug.Log("4: " + key);
                        if (!Method.dictionary_0.ContainsKey(key))
						{
							Method.dictionary_0.Add(key, type);
                        }
					}
				}
			}
			return true;
		}

		public static bool SetValue(string method, object value)
		{
			if (method.Contains("."))
			{
				string text = method.Substring(0, method.LastIndexOf('.'));
				if (Method.dictionary_0.ContainsKey(text))
				{
					string name = method.Replace(text + ".", "");
					FieldInfo field = Method.dictionary_0[text].GetField(name);
					if (field != null)
					{
						field.SetValue(null, value);
						return true;
					}
					PropertyInfo property = Method.dictionary_0[text].GetProperty(name);
					if (property != null)
					{
						property.SetValue(null, value, null);
						return true;
					}
				}
			}
			Method.HasError = true;
			Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
			return false;
		}

		public static Method.Result Invoke(string method)
		{
			if (!method.Contains("."))
			{
				Method.HasError = true;
				Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
				return new Method.Result(null, null);
			}
			string text = method.Substring(0, method.LastIndexOf('.'));
			string text2 = method.Replace(text + ".", "get_");
			if (Method.dictionary_0[text].GetMethod(text2) != null)
			{
				return Method.Invoke(text, text2, null, new object[0]);
			}
			return Method.Invoke(text, method.Replace(text + ".", ""), null, new object[0]);
		}

		public static Method.Result Invoke(string method, params object[] args)
		{
			if (method.Contains("."))
			{
				string text = method.Substring(0, method.LastIndexOf('.'));
				string method2 = method.Replace(text + ".", "");
				return Method.Invoke(text, method2, null, args);
			}
			Method.HasError = true;
			Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
			return new Method.Result(null, null);
		}

		public static Method.Result InvokeTo(object obj, string method, params object[] args)
		{
			if (method.Contains("."))
			{
				string text = method.Substring(0, method.LastIndexOf('.'));
				string method2 = method.Replace(text + ".", "");
				return Method.Invoke(text, method2, obj, args);
			}
			Method.HasError = true;
			Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
			return new Method.Result(null, null);
		}

		public static Method.Result Invoke(string type, string method, object target, params object[] args)
		{
			Method.HasError = false;
			Method.LastError = null;
			if (Method.dictionary_0.ContainsKey(type))
			{
				Method.Result result;
				try
				{
					FieldInfo[] fields = Method.dictionary_0[type].GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					for (int i = 0; i < fields.Length; i++)
					{
						FieldInfo fieldInfo = fields[i];
						if (fieldInfo.Name == method)
						{
							result = new Method.Result(fieldInfo.GetValue(target), fieldInfo.FieldType);
							return result;
						}
					}
					string text = "";
					if (args == null)
					{
						args = new object[0];
					}
					Type[] array = new Type[args.Length];
					for (int j = 0; j < args.Length; j++)
					{
						if (args[j] == null)
						{
							array[j] = null;
							text += "null,";
						}
						else
						{
							array[j] = args[j].GetType();
							text = text + args[j] + ",";
						}
					}
					MethodInfo[] methods = Method.dictionary_0[type].GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					for (int k = 0; k < methods.Length; k++)
					{
						MethodInfo methodInfo = methods[k];
						if (methodInfo.Name == method)
						{
							result = new Method.Result(methodInfo.Invoke(target, args), methodInfo.ReturnType);
							return result;
						}
					}
					Method.HasError = true;
					Method.LastError = string.Format("Assembly type \"{0}.{1}({2})\" not exists for invoke.", type, method, text);
					goto IL_168;
				}
				catch (Exception ex)
				{
					Method.HasError = true;
					Method.LastError = ex.ToString();
					goto IL_168;
				}
				return result;
			}
			Method.HasError = true;
			Method.LastError = string.Format("Assembly type \"{0}\" not exists for invoke.", type);
			IL_168:
			return new Method.Result(null, null);
		}
	}
}
