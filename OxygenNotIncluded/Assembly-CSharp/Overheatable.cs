using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
[SkipSaveFileSerialization]
public class Overheatable : StateMachineComponent<Overheatable.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06001D27 RID: 7463 RVA: 0x0009AE19 File Offset: 0x00099019
	public void ResetTemperature()
	{
		base.GetComponent<PrimaryElement>().Temperature = 293.15f;
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x0009AE2C File Offset: 0x0009902C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overheatTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.OverheatTemperature);
		this.fatalTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.FatalTemperature);
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x0009AE80 File Offset: 0x00099080
	private void InitializeModifiers()
	{
		if (this.modifiersInitialized)
		{
			return;
		}
		this.modifiersInitialized = true;
		AttributeModifier modifier = new AttributeModifier(this.overheatTemp.Id, this.baseOverheatTemp, UI.TOOLTIPS.BASE_VALUE, false, false, true);
		AttributeModifier modifier2 = new AttributeModifier(this.fatalTemp.Id, this.baseFatalTemp, UI.TOOLTIPS.BASE_VALUE, false, false, true);
		this.GetAttributes().Add(modifier);
		this.GetAttributes().Add(modifier2);
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x0009AF00 File Offset: 0x00099100
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.InitializeModifiers();
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		if (handle.IsValid() && GameComps.StructureTemperatures.IsEnabled(handle))
		{
			GameComps.StructureTemperatures.Disable(handle);
			GameComps.StructureTemperatures.Enable(handle);
		}
		base.smi.StartSM();
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06001D2B RID: 7467 RVA: 0x0009AF61 File Offset: 0x00099161
	public float OverheatTemperature
	{
		get
		{
			this.InitializeModifiers();
			if (this.overheatTemp == null)
			{
				return 10000f;
			}
			return this.overheatTemp.GetTotalValue();
		}
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x0009AF84 File Offset: 0x00099184
	public Notification CreateOverheatedNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(MISC.NOTIFICATIONS.BUILDINGOVERHEATED.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06001D2D RID: 7469 RVA: 0x0009AFE4 File Offset: 0x000991E4
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP, text);
	}

	// Token: 0x06001D2E RID: 7470 RVA: 0x0009B04C File Offset: 0x0009924C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.overheatTemp != null && this.fatalTemp != null)
		{
			string formattedValue = this.overheatTemp.GetFormattedValue();
			string formattedValue2 = this.fatalTemp.GetFormattedValue();
			string text = UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP;
			text = text + "\n\n" + this.overheatTemp.GetAttributeValueTooltip();
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVERHEAT_TEMP, formattedValue, formattedValue2), string.Format(text, formattedValue, formattedValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.baseOverheatTemp != 0f)
		{
			string formattedTemperature = GameUtil.GetFormattedTemperature(this.baseOverheatTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			string formattedTemperature2 = GameUtil.GetFormattedTemperature(this.baseFatalTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			string format = UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP;
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVERHEAT_TEMP, formattedTemperature, formattedTemperature2), string.Format(format, formattedTemperature, formattedTemperature2), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x0400102B RID: 4139
	private bool modifiersInitialized;

	// Token: 0x0400102C RID: 4140
	private AttributeInstance overheatTemp;

	// Token: 0x0400102D RID: 4141
	private AttributeInstance fatalTemp;

	// Token: 0x0400102E RID: 4142
	public float baseOverheatTemp;

	// Token: 0x0400102F RID: 4143
	public float baseFatalTemp;

	// Token: 0x0200118B RID: 4491
	public class StatesInstance : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.GameInstance
	{
		// Token: 0x060079E9 RID: 31209 RVA: 0x002DA748 File Offset: 0x002D8948
		public StatesInstance(Overheatable smi) : base(smi)
		{
		}

		// Token: 0x060079EA RID: 31210 RVA: 0x002DA754 File Offset: 0x002D8954
		public void TryDoOverheatDamage()
		{
			if (Time.time - this.lastOverheatDamageTime < 7.5f)
			{
				return;
			}
			this.lastOverheatDamageTime += 7.5f;
			base.master.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = 1,
				source = BUILDINGS.DAMAGESOURCES.BUILDING_OVERHEATED,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.OVERHEAT,
				fullDamageEffectName = "smoke_damage_kanim"
			});
		}

		// Token: 0x04005CA0 RID: 23712
		public float lastOverheatDamageTime;
	}

	// Token: 0x0200118C RID: 4492
	public class States : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable>
	{
		// Token: 0x060079EB RID: 31211 RVA: 0x002DA7DC File Offset: 0x002D89DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.safeTemperature;
			this.root.EventTransition(GameHashes.BuildingBroken, this.invulnerable, null);
			this.invulnerable.EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(Overheatable.StatesInstance smi)
			{
				smi.master.ResetTemperature();
			}).EventTransition(GameHashes.BuildingPartiallyRepaired, this.safeTemperature, null);
			this.safeTemperature.TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null).EventTransition(GameHashes.BuildingOverheated, this.overheated, null);
			this.overheated.Enter(delegate(Overheatable.StatesInstance smi)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_OverheatingBuildings, true);
			}).EventTransition(GameHashes.BuildingNoLongerOverheated, this.safeTemperature, null).ToggleStatusItem(Db.Get().BuildingStatusItems.Overheated, null).ToggleNotification((Overheatable.StatesInstance smi) => smi.master.CreateOverheatedNotification()).TriggerOnEnter(GameHashes.TooHotWarning, null).Enter("InitOverheatTime", delegate(Overheatable.StatesInstance smi)
			{
				smi.lastOverheatDamageTime = Time.time;
			}).Update("OverheatDamage", delegate(Overheatable.StatesInstance smi, float dt)
			{
				smi.TryDoOverheatDamage();
			}, UpdateRate.SIM_4000ms, false);
		}

		// Token: 0x04005CA1 RID: 23713
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State invulnerable;

		// Token: 0x04005CA2 RID: 23714
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State safeTemperature;

		// Token: 0x04005CA3 RID: 23715
		public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State overheated;
	}
}
