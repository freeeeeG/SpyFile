using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200070C RID: 1804
[AddComponentMenu("KMonoBehaviour/scripts/BudUprootedMonitor")]
public class BudUprootedMonitor : KMonoBehaviour
{
	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06003194 RID: 12692 RVA: 0x001079B9 File Offset: 0x00105BB9
	public bool IsUprooted
	{
		get
		{
			return this.uprooted || base.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
		}
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x001079D5 File Offset: 0x00105BD5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<BudUprootedMonitor>(-216549700, BudUprootedMonitor.OnUprootedDelegate);
	}

	// Token: 0x06003196 RID: 12694 RVA: 0x001079EE File Offset: 0x00105BEE
	public void SetParentObject(KPrefabID id)
	{
		this.parentObject = new Ref<KPrefabID>(id);
		base.Subscribe(id.gameObject, 1969584890, new Action<object>(this.OnLoseParent));
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x00107A1A File Offset: 0x00105C1A
	private void OnLoseParent(object obj)
	{
		if (!this.uprooted && !base.isNull)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			this.uprooted = true;
			base.Trigger(-216549700, null);
		}
	}

	// Token: 0x06003198 RID: 12696 RVA: 0x00107A50 File Offset: 0x00105C50
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003199 RID: 12697 RVA: 0x00107A58 File Offset: 0x00105C58
	public static bool IsObjectUprooted(GameObject plant)
	{
		BudUprootedMonitor component = plant.GetComponent<BudUprootedMonitor>();
		return !(component == null) && component.IsUprooted;
	}

	// Token: 0x04001DB6 RID: 7606
	[Serialize]
	public bool canBeUprooted = true;

	// Token: 0x04001DB7 RID: 7607
	[Serialize]
	private bool uprooted;

	// Token: 0x04001DB8 RID: 7608
	public Ref<KPrefabID> parentObject = new Ref<KPrefabID>();

	// Token: 0x04001DB9 RID: 7609
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001DBA RID: 7610
	private static readonly EventSystem.IntraObjectHandler<BudUprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<BudUprootedMonitor>(delegate(BudUprootedMonitor component, object data)
	{
		if (!component.uprooted)
		{
			component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			component.uprooted = true;
			component.Trigger(-216549700, null);
		}
	});
}
