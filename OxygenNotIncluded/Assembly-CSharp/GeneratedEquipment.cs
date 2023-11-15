using System;
using System.Collections.Generic;

// Token: 0x02000078 RID: 120
public class GeneratedEquipment
{
	// Token: 0x06000246 RID: 582 RVA: 0x00010198 File Offset: 0x0000E398
	public static void LoadGeneratedEquipment(List<Type> types)
	{
		Type typeFromHandle = typeof(IEquipmentConfig);
		List<Type> list = new List<Type>();
		foreach (Type type in types)
		{
			if (typeFromHandle.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
			{
				list.Add(type);
			}
		}
		foreach (Type type2 in list)
		{
			object obj = Activator.CreateInstance(type2);
			try
			{
				EquipmentConfigManager.Instance.RegisterEquipment(obj as IEquipmentConfig);
			}
			catch (Exception e)
			{
				DebugUtil.LogException(null, "Exception in RegisterEquipment for type " + type2.FullName + " from " + type2.Assembly.GetName().Name, e);
			}
		}
	}
}
