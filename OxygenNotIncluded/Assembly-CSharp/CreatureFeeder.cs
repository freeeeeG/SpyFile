using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000AE1 RID: 2785
[AddComponentMenu("KMonoBehaviour/scripts/CreatureFeeder")]
public class CreatureFeeder : KMonoBehaviour
{
	// Token: 0x060055BE RID: 21950 RVA: 0x001F3169 File Offset: 0x001F1369
	protected override void OnSpawn()
	{
		Components.CreatureFeeders.Add(this.GetMyWorldId(), this);
		base.Subscribe<CreatureFeeder>(-1452790913, CreatureFeeder.OnAteFromStorageDelegate);
	}

	// Token: 0x060055BF RID: 21951 RVA: 0x001F318D File Offset: 0x001F138D
	protected override void OnCleanUp()
	{
		Components.CreatureFeeders.Remove(this.GetMyWorldId(), this);
	}

	// Token: 0x060055C0 RID: 21952 RVA: 0x001F31A0 File Offset: 0x001F13A0
	private void OnAteFromStorage(object data)
	{
		if (string.IsNullOrEmpty(this.effectId))
		{
			return;
		}
		(data as GameObject).GetComponent<Effects>().Add(this.effectId, true);
	}

	// Token: 0x04003981 RID: 14721
	public string effectId;

	// Token: 0x04003982 RID: 14722
	private static readonly EventSystem.IntraObjectHandler<CreatureFeeder> OnAteFromStorageDelegate = new EventSystem.IntraObjectHandler<CreatureFeeder>(delegate(CreatureFeeder component, object data)
	{
		component.OnAteFromStorage(data);
	});
}
