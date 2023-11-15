using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x02000554 RID: 1364
public static class DevToolCommandPaletteUtil
{
	// Token: 0x0600213F RID: 8511 RVA: 0x000B2018 File Offset: 0x000B0218
	public static List<DevToolCommandPalette.Command> GenerateDefaultCommandPalette()
	{
		List<DevToolCommandPalette.Command> list = new List<DevToolCommandPalette.Command>();
		using (List<Type>.Enumerator enumerator = ReflectionUtil.CollectTypesThatInheritOrImplement<DevTool>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Type devToolType = enumerator.Current;
				if (!devToolType.IsAbstract && ReflectionUtil.HasDefaultConstructor(devToolType))
				{
					list.Add(new DevToolCommandPalette.Command("Open DevTool: \"" + DevToolUtil.GenerateDevToolName(devToolType) + "\"", delegate()
					{
						DevToolUtil.Open((DevTool)Activator.CreateInstance(devToolType));
					}));
				}
			}
		}
		return list;
	}
}
