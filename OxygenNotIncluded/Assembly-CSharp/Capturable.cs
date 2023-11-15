using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200070E RID: 1806
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Capturable")]
public class Capturable : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x17000373 RID: 883
	// (get) Token: 0x0600319F RID: 12703 RVA: 0x00107B1F File Offset: 0x00105D1F
	public bool IsMarkedForCapture
	{
		get
		{
			return this.markedForCapture;
		}
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x00107B28 File Offset: 0x00105D28
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Capturables.Add(this);
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.multitoolContext = "capture";
		this.multitoolHitEffectTag = "fx_capture_splash";
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x00107BE8 File Offset: 0x00105DE8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Capturable>(1623392196, Capturable.OnDeathDelegate);
		base.Subscribe<Capturable>(493375141, Capturable.OnRefreshUserMenuDelegate);
		base.Subscribe<Capturable>(-1582839653, Capturable.OnTagsChangedDelegate);
		if (this.markedForCapture)
		{
			Prioritizable.AddRef(base.gameObject);
		}
		this.UpdateStatusItem();
		this.UpdateChore();
		base.SetWorkTime(10f);
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x00107C58 File Offset: 0x00105E58
	protected override void OnCleanUp()
	{
		Components.Capturables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x00107C6C File Offset: 0x00105E6C
	public override Vector3 GetTargetPoint()
	{
		Vector3 result = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			result = component.bounds.center;
		}
		result.z = 0f;
		return result;
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x00107CB1 File Offset: 0x00105EB1
	private void OnDeath(object data)
	{
		this.allowCapture = false;
		this.markedForCapture = false;
		this.UpdateChore();
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x00107CC7 File Offset: 0x00105EC7
	private void OnTagsChanged(object data)
	{
		this.MarkForCapture(this.markedForCapture);
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x00107CD8 File Offset: 0x00105ED8
	public void MarkForCapture(bool mark)
	{
		PrioritySetting priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
		this.MarkForCapture(mark, priority, false);
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x00107CF8 File Offset: 0x00105EF8
	public void MarkForCapture(bool mark, PrioritySetting priority, bool updateMarkedPriority = false)
	{
		mark = (mark && this.IsCapturable());
		if (this.markedForCapture && !mark)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		else if (!this.markedForCapture && mark)
		{
			Prioritizable.AddRef(base.gameObject);
			Prioritizable component = base.GetComponent<Prioritizable>();
			if (component)
			{
				component.SetMasterPriority(priority);
			}
		}
		else if (updateMarkedPriority && this.markedForCapture && mark)
		{
			Prioritizable component2 = base.GetComponent<Prioritizable>();
			if (component2)
			{
				component2.SetMasterPriority(priority);
			}
		}
		this.markedForCapture = mark;
		this.UpdateStatusItem();
		this.UpdateChore();
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x00107D94 File Offset: 0x00105F94
	public bool IsCapturable()
	{
		return this.allowCapture && !base.gameObject.HasTag(GameTags.Trapped) && !base.gameObject.HasTag(GameTags.Stored) && !base.gameObject.HasTag(GameTags.Creatures.Bagged);
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x00107DE8 File Offset: 0x00105FE8
	private void OnRefreshUserMenu(object data)
	{
		if (!this.IsCapturable())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.markedForCapture) ? new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CAPTURE.NAME, delegate()
		{
			this.MarkForCapture(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CAPTURE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CANCELCAPTURE.NAME, delegate()
		{
			this.MarkForCapture(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELCAPTURE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x00107E8C File Offset: 0x0010608C
	private void UpdateStatusItem()
	{
		this.shouldShowSkillPerkStatusItem = this.markedForCapture;
		base.UpdateStatusItem(null);
		if (this.markedForCapture)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderCapture, this);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderCapture, false);
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x00107EF0 File Offset: 0x001060F0
	private void UpdateChore()
	{
		if (this.markedForCapture && this.chore == null)
		{
			this.chore = new WorkChore<Capturable>(Db.Get().ChoreTypes.Capture, this, null, true, null, new Action<Chore>(this.OnChoreBegins), new Action<Chore>(this.OnChoreEnds), true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			return;
		}
		if (!this.markedForCapture && this.chore != null)
		{
			this.chore.Cancel("not marked for capture");
			this.chore = null;
		}
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x00107F78 File Offset: 0x00106178
	private void OnChoreBegins(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.root);
			smi.GetComponent<Navigator>().Stop(false, true);
		}
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x00107FB4 File Offset: 0x001061B4
	private void OnChoreEnds(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.GetDefaultState());
		}
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x00107FE1 File Offset: 0x001061E1
	protected override void OnStartWork(Worker worker)
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Stunned, false);
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x00107FF4 File Offset: 0x001061F4
	protected override void OnStopWork(Worker worker)
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Stunned);
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x00108008 File Offset: 0x00106208
	protected override void OnCompleteWork(Worker worker)
	{
		int num = this.NaturalBuildingCell();
		if (Grid.Solid[num])
		{
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				num = num2;
			}
		}
		this.MarkForCapture(false);
		this.baggable.SetWrangled();
		this.baggable.transform.SetPosition(Grid.CellToPosCCC(num, Grid.SceneLayer.Ore));
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x00108074 File Offset: 0x00106274
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.allowCapture)
		{
			descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_WRANGLE, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_WRANGLE, Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x04001DBB RID: 7611
	[MyCmpAdd]
	private Baggable baggable;

	// Token: 0x04001DBC RID: 7612
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04001DBD RID: 7613
	public bool allowCapture = true;

	// Token: 0x04001DBE RID: 7614
	[Serialize]
	private bool markedForCapture;

	// Token: 0x04001DBF RID: 7615
	private Chore chore;

	// Token: 0x04001DC0 RID: 7616
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnDeathDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04001DC1 RID: 7617
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001DC2 RID: 7618
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnTagsChanged(data);
	});
}
