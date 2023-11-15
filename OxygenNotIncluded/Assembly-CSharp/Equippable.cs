using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020007A0 RID: 1952
[SerializationConfig(MemberSerialization.OptIn)]
public class Equippable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor, IQuality
{
	// Token: 0x06003638 RID: 13880 RVA: 0x0012512E File Offset: 0x0012332E
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x00125136 File Offset: 0x00123336
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x0600363A RID: 13882 RVA: 0x0012513F File Offset: 0x0012333F
	// (set) Token: 0x0600363B RID: 13883 RVA: 0x0012514C File Offset: 0x0012334C
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

	// Token: 0x0600363C RID: 13884 RVA: 0x0012515C File Offset: 0x0012335C
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

	// Token: 0x0600363D RID: 13885 RVA: 0x001251AC File Offset: 0x001233AC
	protected override void OnSpawn()
	{
		if (this.isEquipped)
		{
			if (this.assignee != null && this.assignee is MinionIdentity)
			{
				this.assignee = (this.assignee as MinionIdentity).assignableProxy.Get();
				this.assignee_identityRef.Set(this.assignee as KMonoBehaviour);
			}
			if (this.assignee == null && this.assignee_identityRef.Get() != null)
			{
				this.assignee = this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
			}
			if (this.assignee != null)
			{
				this.assignee.GetSoleOwner().GetComponent<Equipment>().Equip(this);
			}
			else
			{
				global::Debug.LogWarning("Equippable trying to be equipped to missing prefab");
				this.isEquipped = false;
			}
		}
		base.Subscribe<Equippable>(1969584890, Equippable.SetDestroyedTrueDelegate);
	}

	// Token: 0x0600363E RID: 13886 RVA: 0x00125280 File Offset: 0x00123480
	public KAnimFile GetBuildOverride()
	{
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component == null || component.BuildOverride == null)
		{
			return this.def.BuildOverride;
		}
		return Assets.GetAnim(component.BuildOverride);
	}

	// Token: 0x0600363F RID: 13887 RVA: 0x001252C4 File Offset: 0x001234C4
	public override void Assign(IAssignableIdentity new_assignee)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (base.slot != null && new_assignee is MinionIdentity)
		{
			new_assignee = (new_assignee as MinionIdentity).assignableProxy.Get();
		}
		if (base.slot != null && new_assignee is StoredMinionIdentity)
		{
			new_assignee = (new_assignee as StoredMinionIdentity).assignableProxy.Get();
		}
		if (new_assignee is MinionAssignablesProxy)
		{
			AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Equipment>().GetSlot(base.slot);
			if (slot != null)
			{
				Assignable assignable = slot.assignable;
				if (assignable != null)
				{
					assignable.Unassign();
				}
			}
		}
		base.Assign(new_assignee);
	}

	// Token: 0x06003640 RID: 13888 RVA: 0x00125360 File Offset: 0x00123560
	public override void Unassign()
	{
		if (this.isEquipped)
		{
			((this.assignee is MinionIdentity) ? ((MinionIdentity)this.assignee).assignableProxy.Get().GetComponent<Equipment>() : ((KMonoBehaviour)this.assignee).GetComponent<Equipment>()).Unequip(this);
			this.OnUnequip();
		}
		base.Unassign();
	}

	// Token: 0x06003641 RID: 13889 RVA: 0x001253C0 File Offset: 0x001235C0
	public void OnEquip(AssignableSlotInstance slot)
	{
		this.isEquipped = true;
		if (SelectTool.Instance.selected == this.selectable)
		{
			SelectTool.Instance.Select(null, false);
		}
		base.GetComponent<KBatchedAnimController>().enabled = false;
		base.GetComponent<KSelectable>().IsSelectable = false;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		Effects component = slot.gameObject.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<Effects>();
		if (component != null)
		{
			foreach (Effect effect in this.def.EffectImmunites)
			{
				component.AddImmunity(effect, name, true);
			}
		}
		if (this.def.OnEquipCallBack != null)
		{
			this.def.OnEquipCallBack(this);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Equipped, false);
	}

	// Token: 0x06003642 RID: 13890 RVA: 0x001254BC File Offset: 0x001236BC
	public void OnUnequip()
	{
		this.isEquipped = false;
		if (this.destroyed)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Equipped);
		base.GetComponent<KBatchedAnimController>().enabled = true;
		base.GetComponent<KSelectable>().IsSelectable = true;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		if (this.assignee != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					Effects component = targetGameObject.GetComponent<Effects>();
					if (component != null)
					{
						foreach (Effect effect in this.def.EffectImmunites)
						{
							component.RemoveImmunity(effect, name);
						}
					}
				}
			}
		}
		if (this.def.OnUnequipCallBack != null)
		{
			this.def.OnUnequipCallBack(this);
		}
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x001255C0 File Offset: 0x001237C0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.def != null)
		{
			List<Descriptor> equipmentEffects = GameUtil.GetEquipmentEffects(this.def);
			if (this.def.additionalDescriptors != null)
			{
				foreach (Descriptor item in this.def.additionalDescriptors)
				{
					equipmentEffects.Add(item);
				}
			}
			return equipmentEffects;
		}
		return new List<Descriptor>();
	}

	// Token: 0x0400211E RID: 8478
	private global::QualityLevel quality;

	// Token: 0x0400211F RID: 8479
	[MyCmpAdd]
	private EquippableWorkable equippableWorkable;

	// Token: 0x04002120 RID: 8480
	[MyCmpAdd]
	private EquippableFacade facade;

	// Token: 0x04002121 RID: 8481
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002122 RID: 8482
	public DefHandle defHandle;

	// Token: 0x04002123 RID: 8483
	[Serialize]
	public bool isEquipped;

	// Token: 0x04002124 RID: 8484
	private bool destroyed;

	// Token: 0x04002125 RID: 8485
	[Serialize]
	public bool unequippable = true;

	// Token: 0x04002126 RID: 8486
	[Serialize]
	public bool hideInCodex;

	// Token: 0x04002127 RID: 8487
	private static readonly EventSystem.IntraObjectHandler<Equippable> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equippable>(delegate(Equippable component, object data)
	{
		component.destroyed = true;
	});
}
