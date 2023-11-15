using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x020006EE RID: 1774
public class GameComps : KComponents
{
	// Token: 0x060030A5 RID: 12453 RVA: 0x00102258 File Offset: 0x00100458
	public GameComps()
	{
		foreach (FieldInfo fieldInfo in typeof(GameComps).GetFields())
		{
			object obj = Activator.CreateInstance(fieldInfo.FieldType);
			fieldInfo.SetValue(null, obj);
			base.Add<IComponentManager>(obj as IComponentManager);
			if (obj is IKComponentManager)
			{
				IKComponentManager inst = obj as IKComponentManager;
				GameComps.AddKComponentManager(fieldInfo.FieldType, inst);
			}
		}
	}

	// Token: 0x060030A6 RID: 12454 RVA: 0x001022CC File Offset: 0x001004CC
	public new void Clear()
	{
		FieldInfo[] fields = typeof(GameComps).GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			IComponentManager componentManager = fields[i].GetValue(null) as IComponentManager;
			if (componentManager != null)
			{
				componentManager.Clear();
			}
		}
	}

	// Token: 0x060030A7 RID: 12455 RVA: 0x0010230F File Offset: 0x0010050F
	public static void AddKComponentManager(Type kcomponent, IKComponentManager inst)
	{
		GameComps.kcomponentManagers[kcomponent] = inst;
	}

	// Token: 0x060030A8 RID: 12456 RVA: 0x0010231D File Offset: 0x0010051D
	public static IKComponentManager GetKComponentManager(Type kcomponent_type)
	{
		return GameComps.kcomponentManagers[kcomponent_type];
	}

	// Token: 0x04001D2E RID: 7470
	public static GravityComponents Gravities;

	// Token: 0x04001D2F RID: 7471
	public static FallerComponents Fallers;

	// Token: 0x04001D30 RID: 7472
	public static InfraredVisualizerComponents InfraredVisualizers;

	// Token: 0x04001D31 RID: 7473
	public static ElementSplitterComponents ElementSplitters;

	// Token: 0x04001D32 RID: 7474
	public static OreSizeVisualizerComponents OreSizeVisualizers;

	// Token: 0x04001D33 RID: 7475
	public static StructureTemperatureComponents StructureTemperatures;

	// Token: 0x04001D34 RID: 7476
	public static DiseaseContainers DiseaseContainers;

	// Token: 0x04001D35 RID: 7477
	public static RequiresFoundation RequiresFoundations;

	// Token: 0x04001D36 RID: 7478
	public static WhiteBoard WhiteBoards;

	// Token: 0x04001D37 RID: 7479
	private static Dictionary<Type, IKComponentManager> kcomponentManagers = new Dictionary<Type, IKComponentManager>();
}
