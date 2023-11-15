using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// Token: 0x02000108 RID: 264
public class TurretBuffFactory
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000691 RID: 1681 RVA: 0x00011CBF File Offset: 0x0000FEBF
	private static bool isInitialize
	{
		get
		{
			return TurretBuffFactory.TurretBuffDIC != null;
		}
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00011CCC File Offset: 0x0000FECC
	public static void Initialize()
	{
		if (TurretBuffFactory.isInitialize)
		{
			return;
		}
		IEnumerable<Type> enumerable = from myType in Assembly.GetAssembly(typeof(TurretBuff)).GetTypes()
		where myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(TurretBuff))
		select myType;
		TurretBuffFactory.TurretBuffDIC = new Dictionary<int, Type>();
		foreach (Type type in enumerable)
		{
			TurretBuff turretBuff = Activator.CreateInstance(type) as TurretBuff;
			TurretBuffFactory.TurretBuffDIC.Add((int)turretBuff.TBuffName, type);
		}
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00011D74 File Offset: 0x0000FF74
	public static TurretBuff GetBuff(int id)
	{
		if (TurretBuffFactory.TurretBuffDIC.ContainsKey(id))
		{
			return Activator.CreateInstance(TurretBuffFactory.TurretBuffDIC[id]) as TurretBuff;
		}
		return null;
	}

	// Token: 0x0400030C RID: 780
	public static Dictionary<int, Type> TurretBuffDIC;
}
