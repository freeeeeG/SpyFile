using System;
using KSerialization;

// Token: 0x02000924 RID: 2340
public class RepairableEquipment : KMonoBehaviour
{
	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x060043DC RID: 17372 RVA: 0x0017C97A File Offset: 0x0017AB7A
	// (set) Token: 0x060043DD RID: 17373 RVA: 0x0017C987 File Offset: 0x0017AB87
	public EquipmentDef def
	{
		get
		{
			return this.defHandle.Get<EquipmentDef>();
		}
		set
		{
			this.defHandle.Set<EquipmentDef>(value);
		}
	}

	// Token: 0x060043DE RID: 17374 RVA: 0x0017C998 File Offset: 0x0017AB98
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.def.AdditionalTags != null)
		{
			foreach (Tag tag in this.def.AdditionalTags)
			{
				base.GetComponent<KPrefabID>().AddTag(tag, false);
			}
		}
	}

	// Token: 0x060043DF RID: 17375 RVA: 0x0017C9E8 File Offset: 0x0017ABE8
	protected override void OnSpawn()
	{
		if (!this.facadeID.IsNullOrWhiteSpace())
		{
			KAnim.Build.Symbol symbol = Db.GetEquippableFacades().Get(this.facadeID).AnimFile.GetData().build.GetSymbol("object");
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			component.TryRemoveSymbolOverride("object", 0);
			component.AddSymbolOverride("object", symbol, 0);
		}
	}

	// Token: 0x04002D00 RID: 11520
	public DefHandle defHandle;

	// Token: 0x04002D01 RID: 11521
	[Serialize]
	public string facadeID;
}
