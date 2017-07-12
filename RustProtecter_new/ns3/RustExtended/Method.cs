using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace RustExtended
{
	public class Method
	{
		private static Method method_0 = new Method();
		public static Dictionary<string, Type> dictionary_0 = new Dictionary<string, Type>();
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
		public static InvokeResult Invoke(string method)
		{
            if (method == "System.Reflection.MethodInfo.op_Inequality")
            {
                Method.HasError = true;
                Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
                return new InvokeResult(null, null);
            }

            if (!method.Contains("."))
			{
				Method.HasError = true;
				Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
				return new InvokeResult(null, null);
			}
			string text = method.Substring(0, method.LastIndexOf('.'));
			string text2 = method.Replace(text + ".", "get_");
			if (Method.dictionary_0[text].GetMethod(text2) != null)
			{
				return Method.Invoke(text, text2, null, new object[0]);
			}
			return Method.Invoke(text, method.Replace(text + ".", ""), null, new object[0]);
		}
		public static InvokeResult Invoke(string method, params object[] args)
		{
			if (method.Contains("."))
			{
				string text = method.Substring(0, method.LastIndexOf('.'));
				string method2 = method.Replace(text + ".", "");
				return Method.Invoke(text, method2, null, args);
			}
			Method.HasError = true;
			Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
			return new InvokeResult(null, null);
		}
		public static InvokeResult InvokeTo(object obj, string method, params object[] args)
		{
			if (method.Contains("."))
			{
				string text = method.Substring(0, method.LastIndexOf('.'));
				string method2 = method.Replace(text + ".", "");
				return Method.Invoke(text, method2, obj, args);
			}
			Method.HasError = true;
			Method.LastError = string.Format("Assembly type not defined for method \"{0}\".", method);
			return new InvokeResult(null, null);
		}
		public static InvokeResult Invoke(string type, string method, object target, params object[] args)
		{
			Method.HasError = false;
			Method.LastError = null;
			if (Method.dictionary_0.ContainsKey(type))
			{
				InvokeResult result;
				try
				{
					FieldInfo[] fields = Method.dictionary_0[type].GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					for (int i = 0; i < fields.Length; i++)
					{
						FieldInfo fieldInfo = fields[i];
						if (fieldInfo.Name == method)
						{
							result = new InvokeResult(fieldInfo.GetValue(target), fieldInfo.FieldType);
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
							result = new InvokeResult(methodInfo.Invoke(target, args), methodInfo.ReturnType);
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
			return new InvokeResult(null, null);
		}
	}
}
