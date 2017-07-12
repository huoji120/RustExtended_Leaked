using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RustExtended
{
	public class Method
	{
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
				return Method.dictionary_1.Keys.ToArray<string>();
			}
		}

		public static string[] Keys
		{
			get
			{
				return Method.dictionary_0.Keys.ToArray<string>();
			}
		}

		public static Type[] Types
		{
			get
			{
				return Method.dictionary_0.Values.ToArray<Type>();
			}
		}

		public static bool Initialize()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Assembly assembly = assemblies[i];
				if (Method.dictionary_1 != null && !Method.dictionary_1.ContainsValue(assembly))
				{
					Method.dictionary_1.Add(assembly.GetName().Name, assembly);
					Type[] types = assembly.GetTypes();
					for (int j = 0; j < types.Length; j++)
					{
						Type type = types[j];
						if (!type.FullName.Contains("<") && !type.FullName.Contains("$") && !type.FullName.Contains("`"))
						{
							string key = type.FullName.Replace("+", ".");
							if (!Method.dictionary_0.ContainsKey(key))
							{
								Method.dictionary_0.Add(key, type);
							}
						}
					}
				}
			}
			return true;
		}

		public static bool SetValue(string method, object value)
		{
			bool result;
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
						result = true;
						return result;
					}
					PropertyInfo property = Method.dictionary_0[text].GetProperty(name);
					if (property != null)
					{
						property.SetValue(null, value, null);
						result = true;
						return result;
					}
				}
			}
			Method.HasError = true;
			Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
			result = false;
			return result;
		}

		public static InvokeResult Invoke(string method)
		{
			InvokeResult result;
			if (!method.Contains("."))
			{
				Method.HasError = true;
				Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
				result = new InvokeResult(null, null);
			}
			else
			{
				string text = method.Substring(0, method.LastIndexOf('.'));
				string text2 = method.Replace(text + ".", "get_");
				if (Method.dictionary_0[text].GetMethod(text2) != null)
				{
					result = Method.Invoke(text, text2, null, new object[0]);
				}
				else
				{
					result = Method.Invoke(text, method.Replace(text + ".", ""), null, new object[0]);
				}
			}
			return result;
		}

		public static InvokeResult Invoke(string method, params object[] args)
		{
			InvokeResult result;
			if (method.Contains("."))
			{
				string text = method.Substring(0, method.LastIndexOf('.'));
				string method2 = method.Replace(text + ".", "");
				result = Method.Invoke(text, method2, null, args);
			}
			else
			{
				Method.HasError = true;
				Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
				result = new InvokeResult(null, null);
			}
			return result;
		}

		public static InvokeResult InvokeTo(object obj, string method, params object[] args)
		{
			InvokeResult result;
			if (method.Contains("."))
			{
				string text = method.Substring(0, method.LastIndexOf('.'));
				string method2 = method.Replace(text + ".", "");
				result = Method.Invoke(text, method2, obj, args);
			}
			else
			{
				Method.HasError = true;
				Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
				result = new InvokeResult(null, null);
			}
			return result;
		}

		public static InvokeResult Invoke(string type, string method, object target, params object[] args)
		{
			Method.HasError = false;
			Method.LastError = null;
			InvokeResult result;
			if (Method.dictionary_0.ContainsKey(type))
			{
				try
				{
					FieldInfo[] fields = Method.dictionary_0[type].GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					for (int i = 0; i < fields.Length; i++)
					{
						FieldInfo fieldInfo = fields[i];
						if (fieldInfo.Name == method)
						{
							InvokeResult invokeResult = new InvokeResult(fieldInfo.GetValue(target), fieldInfo.FieldType);
							result = invokeResult;
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
							InvokeResult invokeResult = new InvokeResult(methodInfo.Invoke(target, args), methodInfo.ReturnType);
							result = invokeResult;
							return result;
						}
					}
					Method.HasError = true;
					Method.LastError = string.Format("Assembly type \"{0}.{1}({2})\" not exists for invoke.", type, method, text);
					goto IL_1C8;
				}
				catch (Exception ex)
				{
					Method.HasError = true;
					Method.LastError = ex.ToString();
					goto IL_1C8;
				}
			}
			Method.HasError = true;
			Method.LastError = string.Format("Assembly type \"{0}\" not exists for invoke.", type);
			IL_1C8:
			result = new InvokeResult(null, null);
			return result;
		}
	}
}
