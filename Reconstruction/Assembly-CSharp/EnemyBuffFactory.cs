using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// Token: 0x020000FE RID: 254
public class EnemyBuffFactory
{
	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x0600066E RID: 1646 RVA: 0x000115A5 File Offset: 0x0000F7A5
	private static bool isInitialize
	{
		get
		{
			return EnemyBuffFactory.EnemyBuffDIC != null;
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x000115B0 File Offset: 0x0000F7B0
	public static void Initialize()
	{
		if (EnemyBuffFactory.isInitialize)
		{
			return;
		}
		EnemyBuffFactory.GlobalBuffs = new List<BuffInfo>();
		IEnumerable<Type> enumerable = from myType in Assembly.GetAssembly(typeof(EnemyBuff)).GetTypes()
		where myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(EnemyBuff))
		select myType;
		EnemyBuffFactory.EnemyBuffDIC = new Dictionary<int, Type>();
		foreach (Type type in enumerable)
		{
			EnemyBuff enemyBuff = Activator.CreateInstance(type) as EnemyBuff;
			EnemyBuffFactory.EnemyBuffDIC.Add((int)enemyBuff.BuffName, type);
		}
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00011664 File Offset: 0x0000F864
	public static EnemyBuff GetBuff(int id)
	{
		if (EnemyBuffFactory.EnemyBuffDIC.ContainsKey(id))
		{
			return Activator.CreateInstance(EnemyBuffFactory.EnemyBuffDIC[id]) as EnemyBuff;
		}
		return null;
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0001168A File Offset: 0x0000F88A
	public static void Release()
	{
		EnemyBuffFactory.GlobalBuffs.Clear();
	}

	// Token: 0x040002D2 RID: 722
	public static Dictionary<int, Type> EnemyBuffDIC;

	// Token: 0x040002D3 RID: 723
	public static List<BuffInfo> GlobalBuffs;
}
