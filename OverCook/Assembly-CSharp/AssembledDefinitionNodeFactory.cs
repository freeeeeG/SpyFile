using System;
using System.Runtime.CompilerServices;

// Token: 0x020009B8 RID: 2488
public static class AssembledDefinitionNodeFactory
{
	// Token: 0x060030B9 RID: 12473 RVA: 0x000E4E87 File Offset: 0x000E3287
	private static T CreateNode<T>() where T : AssembledDefinitionNode, new()
	{
		return Activator.CreateInstance<T>();
	}

	// Token: 0x060030BA RID: 12474 RVA: 0x000E4E90 File Offset: 0x000E3290
	public static int GetNodeType(AssembledDefinitionNode node)
	{
		return AssembledDefinitionNodeFactory.m_typeLookup.FindIndex_Predicate((Type x) => x == node.GetType());
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x000E4EC0 File Offset: 0x000E32C0
	public static AssembledDefinitionNode CreateNode(int type)
	{
		return AssembledDefinitionNodeFactory.m_createLookup[type]();
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x000E4ED0 File Offset: 0x000E32D0
	// Note: this type is marked as 'beforefieldinit'.
	static AssembledDefinitionNodeFactory()
	{
		AssembledDefinitionNodeFactory.CreateMethod[] array = new AssembledDefinitionNodeFactory.CreateMethod[6];
		int num = 0;
		if (AssembledDefinitionNodeFactory.<>f__mg$cache0 == null)
		{
			AssembledDefinitionNodeFactory.<>f__mg$cache0 = new AssembledDefinitionNodeFactory.CreateMethod(AssembledDefinitionNodeFactory.CreateNode<NullAssembledNode>);
		}
		array[num] = AssembledDefinitionNodeFactory.<>f__mg$cache0;
		int num2 = 1;
		if (AssembledDefinitionNodeFactory.<>f__mg$cache1 == null)
		{
			AssembledDefinitionNodeFactory.<>f__mg$cache1 = new AssembledDefinitionNodeFactory.CreateMethod(AssembledDefinitionNodeFactory.CreateNode<IngredientAssembledNode>);
		}
		array[num2] = AssembledDefinitionNodeFactory.<>f__mg$cache1;
		int num3 = 2;
		if (AssembledDefinitionNodeFactory.<>f__mg$cache2 == null)
		{
			AssembledDefinitionNodeFactory.<>f__mg$cache2 = new AssembledDefinitionNodeFactory.CreateMethod(AssembledDefinitionNodeFactory.CreateNode<CompositeAssembledNode>);
		}
		array[num3] = AssembledDefinitionNodeFactory.<>f__mg$cache2;
		int num4 = 3;
		if (AssembledDefinitionNodeFactory.<>f__mg$cache3 == null)
		{
			AssembledDefinitionNodeFactory.<>f__mg$cache3 = new AssembledDefinitionNodeFactory.CreateMethod(AssembledDefinitionNodeFactory.CreateNode<CookedCompositeAssembledNode>);
		}
		array[num4] = AssembledDefinitionNodeFactory.<>f__mg$cache3;
		int num5 = 4;
		if (AssembledDefinitionNodeFactory.<>f__mg$cache4 == null)
		{
			AssembledDefinitionNodeFactory.<>f__mg$cache4 = new AssembledDefinitionNodeFactory.CreateMethod(AssembledDefinitionNodeFactory.CreateNode<MixedCompositeAssembledNode>);
		}
		array[num5] = AssembledDefinitionNodeFactory.<>f__mg$cache4;
		int num6 = 5;
		if (AssembledDefinitionNodeFactory.<>f__mg$cache5 == null)
		{
			AssembledDefinitionNodeFactory.<>f__mg$cache5 = new AssembledDefinitionNodeFactory.CreateMethod(AssembledDefinitionNodeFactory.CreateNode<ItemAssembledNode>);
		}
		array[num6] = AssembledDefinitionNodeFactory.<>f__mg$cache5;
		AssembledDefinitionNodeFactory.m_createLookup = array;
	}

	// Token: 0x04002728 RID: 10024
	public const int kBitsPerType = 4;

	// Token: 0x04002729 RID: 10025
	private static Type[] m_typeLookup = new Type[]
	{
		typeof(NullAssembledNode),
		typeof(IngredientAssembledNode),
		typeof(CompositeAssembledNode),
		typeof(CookedCompositeAssembledNode),
		typeof(MixedCompositeAssembledNode),
		typeof(ItemAssembledNode)
	};

	// Token: 0x0400272A RID: 10026
	private static AssembledDefinitionNodeFactory.CreateMethod[] m_createLookup;

	// Token: 0x0400272B RID: 10027
	[CompilerGenerated]
	private static AssembledDefinitionNodeFactory.CreateMethod <>f__mg$cache0;

	// Token: 0x0400272C RID: 10028
	[CompilerGenerated]
	private static AssembledDefinitionNodeFactory.CreateMethod <>f__mg$cache1;

	// Token: 0x0400272D RID: 10029
	[CompilerGenerated]
	private static AssembledDefinitionNodeFactory.CreateMethod <>f__mg$cache2;

	// Token: 0x0400272E RID: 10030
	[CompilerGenerated]
	private static AssembledDefinitionNodeFactory.CreateMethod <>f__mg$cache3;

	// Token: 0x0400272F RID: 10031
	[CompilerGenerated]
	private static AssembledDefinitionNodeFactory.CreateMethod <>f__mg$cache4;

	// Token: 0x04002730 RID: 10032
	[CompilerGenerated]
	private static AssembledDefinitionNodeFactory.CreateMethod <>f__mg$cache5;

	// Token: 0x020009B9 RID: 2489
	// (Invoke) Token: 0x060030BE RID: 12478
	private delegate AssembledDefinitionNode CreateMethod();
}
