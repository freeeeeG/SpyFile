using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class EquippableBalloonConfig : IEquipmentConfig
{
	// Token: 0x06000239 RID: 569 RVA: 0x0000FD9F File Offset: 0x0000DF9F
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000FDA8 File Offset: 0x0000DFA8
	public EquipmentDef CreateEquipmentDef()
	{
		List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("EquippableBalloon", EQUIPMENT.TOYS.SLOT, SimHashes.Carbon, EQUIPMENT.TOYS.BALLOON_MASS, EQUIPMENT.VESTS.COOL_VEST_ICON0, null, null, 0, attributeModifiers, null, false, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, null, null);
		equipmentDef.OnEquipCallBack = new Action<Equippable>(this.OnEquipBalloon);
		equipmentDef.OnUnequipCallBack = new Action<Equippable>(this.OnUnequipBalloon);
		return equipmentDef;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000FE10 File Offset: 0x0000E010
	private void OnEquipBalloon(Equippable eq)
	{
		if (!eq.IsNullOrDestroyed() && !eq.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target;
			Effects component = kmonoBehaviour.GetComponent<Effects>();
			KSelectable component2 = kmonoBehaviour.GetComponent<KSelectable>();
			if (!component.IsNullOrDestroyed())
			{
				component.Add("HasBalloon", false);
				EquippableBalloon component3 = eq.GetComponent<EquippableBalloon>();
				EquippableBalloon.StatesInstance data = (EquippableBalloon.StatesInstance)component3.GetSMI();
				component2.AddStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon, data);
				this.SpawnFxInstanceFor(kmonoBehaviour);
				component3.ApplyBalloonOverrideToBalloonFx();
			}
		}
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000FEB8 File Offset: 0x0000E0B8
	private void OnUnequipBalloon(Equippable eq)
	{
		if (!eq.IsNullOrDestroyed() && !eq.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			MinionAssignablesProxy component = soleOwner.GetComponent<MinionAssignablesProxy>();
			if (!component.target.IsNullOrDestroyed())
			{
				KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)component.target;
				Effects component2 = kmonoBehaviour.GetComponent<Effects>();
				KSelectable component3 = kmonoBehaviour.GetComponent<KSelectable>();
				if (!component2.IsNullOrDestroyed())
				{
					component2.Remove("HasBalloon");
					component3.RemoveStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon, false);
					this.DestroyFxInstanceFor(kmonoBehaviour);
				}
			}
		}
		Util.KDestroyGameObject(eq.gameObject);
	}

	// Token: 0x0600023D RID: 573 RVA: 0x0000FF60 File Offset: 0x0000E160
	public void DoPostConfigure(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
		Equippable equippable = go.GetComponent<Equippable>();
		if (equippable.IsNullOrDestroyed())
		{
			equippable = go.AddComponent<Equippable>();
		}
		equippable.hideInCodex = true;
		equippable.unequippable = false;
		go.AddOrGet<EquippableBalloon>();
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0000FFA9 File Offset: 0x0000E1A9
	private void SpawnFxInstanceFor(KMonoBehaviour target)
	{
		new BalloonFX.Instance(target.GetComponent<KMonoBehaviour>()).StartSM();
	}

	// Token: 0x0600023F RID: 575 RVA: 0x0000FFBB File Offset: 0x0000E1BB
	private void DestroyFxInstanceFor(KMonoBehaviour target)
	{
		target.GetSMI<BalloonFX.Instance>().StopSM("Unequipped");
	}

	// Token: 0x04000144 RID: 324
	public const string ID = "EquippableBalloon";
}
