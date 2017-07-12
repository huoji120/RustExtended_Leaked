using System;
using System.Reflection;

internal class Class2
{
	internal delegate void Delegate0(UnityEngine.Object o);

	internal static Module module_0 = typeof(Class2).Assembly.ManifestModule;

	internal static void tQfHwaGG2Ndgb(int typemdt)
	{
		Type type = Class2.module_0.ResolveType(33554432 + typemdt);
		FieldInfo[] fields = type.GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			FieldInfo fieldInfo = fields[i];
			MethodInfo method = (MethodInfo)Class2.module_0.ResolveMethod(fieldInfo.MetadataToken + 100663296);
			fieldInfo.SetValue(null, (MulticastDelegate)Delegate.CreateDelegate(type, method));
		}
	}
}
