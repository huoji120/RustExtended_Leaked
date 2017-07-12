using Facepunch.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using uLink;
using UnityEngine;

namespace Magma
{
	public class Util
	{
		private Dictionary<string, Type> typeCache = new Dictionary<string, Type>();

		private static Util util;

		public void ConsoleLog(string str, bool adminOnly = false)
		{
			foreach (Player current in Server.GetServer().Players)
			{
				if (current.PlayerClient != null)
				{
					uLink.NetworkPlayer netPlayer = current.PlayerClient.netPlayer;
					if (current.PlayerClient.netPlayer != uLink.NetworkPlayer.unassigned && current.PlayerClient.netPlayer.isClient && current.PlayerClient.netPlayer.isConnected)
					{
						if (!adminOnly)
						{
							ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleMessage", current.PlayerClient.netPlayer, str);
						}
						else if (current.Admin)
						{
							ConsoleNetworker.singleton.networkView.RPC<string>("CL_ConsoleMessage", current.PlayerClient.netPlayer, str);
						}
					}
				}
			}
		}

		public object CreateArrayInstance(string name, int size)
		{
			Type type;
			object result;
			if (!this.TryFindType(name.Replace('.', '+'), out type))
			{
				result = null;
			}
			else if (type.BaseType.Name == "ScriptableObject")
			{
				result = ScriptableObject.CreateInstance(name);
			}
			else
			{
				result = Array.CreateInstance(type, size);
			}
			return result;
		}

		public object CreateInstance(string name, params object[] args)
		{
			Type type;
			object result;
			if (!this.TryFindType(name.Replace('.', '+'), out type))
			{
				result = null;
			}
			else if (type.BaseType.Name == "ScriptableObject")
			{
				result = ScriptableObject.CreateInstance(name);
			}
			else
			{
				result = Activator.CreateInstance(type, args);
			}
			return result;
		}

		public Quaternion CreateQuat(float x, float y, float z, float w)
		{
			return new Quaternion(x, y, z, w);
		}

		public Vector3 CreateVector(float x, float y, float z)
		{
			return new Vector3(x, y, z);
		}

		public void DestroyObject(GameObject go)
		{
			NetCull.Destroy(go);
		}

		public static string GetAbsoluteFilePath(string fileName)
		{
			return Util.GetMagmaFolder() + fileName;
		}

		public static string GetMagmaFolder()
		{
			return Data.PATH;
		}

		public static string GetRootFolder()
		{
			return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
		}

		public static string GetServerFolder()
		{
			return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))) + "\\rust_server_Data\\";
		}

		public object GetStaticField(string className, string field)
		{
			Type type;
			object result;
			if (this.TryFindType(className.Replace('.', '+'), out type))
			{
				FieldInfo field2 = type.GetField(field, BindingFlags.Static | BindingFlags.Public);
				if (field2 != null)
				{
					result = field2.GetValue(null);
					return result;
				}
			}
			result = null;
			return result;
		}

		public static Util GetUtil()
		{
			if (Util.util == null)
			{
				Util.util = new Util();
			}
			return Util.util;
		}

		public float GetVectorsDistance(Vector3 v1, Vector3 v2)
		{
			return TransformHelpers.Dist2D(v1, v2);
		}

		public static Hashtable HashtableFromFile(string path)
		{
			FileStream stream = new FileStream(path, FileMode.Open);
			StreamReader streamReader = new StreamReader(stream);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			return (Hashtable)binaryFormatter.Deserialize(streamReader.BaseStream);
		}

		public static void HashtableToFile(Hashtable ht, string path)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				StreamWriter streamWriter = new StreamWriter(fileStream);
				binaryFormatter.Serialize(streamWriter.BaseStream, ht);
			}
		}

		public Vector3 Infront(Player p, float length)
		{
			return p.PlayerClient.controllable.transform.position + p.PlayerClient.controllable.transform.forward * length;
		}

		public object InvokeStatic(string className, string method, ParamsList args)
		{
			Type type;
			object result;
			if (!this.TryFindType(className.Replace('.', '+'), out type))
			{
				result = null;
			}
			else
			{
				MethodInfo method2 = type.GetMethod(method, BindingFlags.Static);
				if (method2 == null)
				{
					result = null;
				}
				else if (method2.ReturnType == typeof(void))
				{
					method2.Invoke(null, args.ToArray());
					result = true;
				}
				else
				{
					result = method2.Invoke(null, args.ToArray());
				}
			}
			return result;
		}

		public bool IsNull(object obj)
		{
			return obj == null;
		}

		public void Log(string str)
		{
			Console.WriteLine(str);
		}

		public Match Regex(string input, string match)
		{
			return new Regex(input).Match(match);
		}

		public Quaternion RotateX(Quaternion q, float angle)
		{
			return q *= Quaternion.Euler(angle, 0f, 0f);
		}

		public Quaternion RotateY(Quaternion q, float angle)
		{
			return q *= Quaternion.Euler(0f, angle, 0f);
		}

		public Quaternion RotateZ(Quaternion q, float angle)
		{
			return q *= Quaternion.Euler(0f, 0f, angle);
		}

		public static void say(uLink.NetworkPlayer player, string playername, string arg)
		{
			ConsoleNetworker.SendClientCommand(player, "chat.add " + playername + " " + arg);
		}

		public static void sayAll(string arg)
		{
			ConsoleNetworker.Broadcast("chat.add " + Facepunch.Utility.String.QuoteSafe(Server.GetServer().server_message_name) + " " + Facepunch.Utility.String.QuoteSafe(arg));
		}

		public static void sayUser(uLink.NetworkPlayer player, string arg)
		{
			ConsoleNetworker.SendClientCommand(player, "chat.add " + Facepunch.Utility.String.QuoteSafe(Server.GetServer().server_message_name) + " " + Facepunch.Utility.String.QuoteSafe(arg));
		}

		public static void sayUser(uLink.NetworkPlayer player, string customName, string arg)
		{
			ConsoleNetworker.SendClientCommand(player, "chat.add " + Facepunch.Utility.String.QuoteSafe(customName) + " " + Facepunch.Utility.String.QuoteSafe(arg));
		}

		public void SetStaticField(string className, string field, object val)
		{
			Type type;
			if (this.TryFindType(className.Replace('.', '+'), out type))
			{
				FieldInfo field2 = type.GetField(field, BindingFlags.Static | BindingFlags.Public);
				if (field2 != null)
				{
					field2.SetValue(null, Convert.ChangeType(val, field2.FieldType));
				}
			}
		}

		public bool TryFindType(string typeName, out Type t)
		{
			lock (this.typeCache)
			{
				if (!this.typeCache.TryGetValue(typeName, out t))
				{
					Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
					for (int i = 0; i < assemblies.Length; i++)
					{
						Assembly assembly = assemblies[i];
						t = assembly.GetType(typeName);
						if (t != null)
						{
							break;
						}
					}
					this.typeCache[typeName] = t;
				}
			}
			return t != null;
		}
	}
}
