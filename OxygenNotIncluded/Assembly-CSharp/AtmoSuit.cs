using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000599 RID: 1433
[AddComponentMenu("KMonoBehaviour/scripts/AtmoSuit")]
public class AtmoSuit : KMonoBehaviour
{
	// Token: 0x060022F7 RID: 8951 RVA: 0x000BF86B File Offset: 0x000BDA6B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<AtmoSuit>(-1697596308, AtmoSuit.OnStorageChangedDelegate);
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000BF884 File Offset: 0x000BDA84
	private void RefreshStatusEffects(object data)
	{
		if (this == null)
		{
			return;
		}
		Equippable component = base.GetComponent<Equippable>();
		bool flag = base.GetComponent<Storage>().Has(GameTags.AnyWater);
		if (component.assignee != null && flag)
		{
			Ownables soleOwner = component.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					AssignableSlotInstance slot = ((KMonoBehaviour)component.assignee).GetComponent<Equipment>().GetSlot(component.slot);
					Effects component2 = targetGameObject.GetComponent<Effects>();
					if (component2 != null && !component2.HasEffect("SoiledSuit") && !slot.IsUnassigning())
					{
						component2.Add("SoiledSuit", true);
					}
				}
			}
		}
	}

	// Token: 0x040013FC RID: 5116
	private static readonly EventSystem.IntraObjectHandler<AtmoSuit> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<AtmoSuit>(delegate(AtmoSuit component, object data)
	{
		component.RefreshStatusEffects(data);
	});
}
