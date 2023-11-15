using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005AC RID: 1452
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/BuildingHP")]
public class BuildingHP : Workable
{
	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x0600238B RID: 9099 RVA: 0x000C2A8E File Offset: 0x000C0C8E
	public int HitPoints
	{
		get
		{
			return this.hitpoints;
		}
	}

	// Token: 0x0600238C RID: 9100 RVA: 0x000C2A96 File Offset: 0x000C0C96
	public void SetHitPoints(int hp)
	{
		this.hitpoints = hp;
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x0600238D RID: 9101 RVA: 0x000C2A9F File Offset: 0x000C0C9F
	public int MaxHitPoints
	{
		get
		{
			return this.building.Def.HitPoints;
		}
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x000C2AB1 File Offset: 0x000C0CB1
	public BuildingHP.DamageSourceInfo GetDamageSourceInfo()
	{
		return this.damageSourceInfo;
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000C2AB9 File Offset: 0x000C0CB9
	protected override void OnLoadLevel()
	{
		this.smi = null;
		base.OnLoadLevel();
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000C2AC8 File Offset: 0x000C0CC8
	public void DoDamage(int damage)
	{
		if (!this.invincible)
		{
			damage = Math.Max(0, damage);
			this.hitpoints = Math.Max(0, this.hitpoints - damage);
			base.Trigger(-1964935036, this);
		}
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x000C2AFC File Offset: 0x000C0CFC
	public void Repair(int repair_amount)
	{
		if (this.hitpoints + repair_amount < this.hitpoints)
		{
			this.hitpoints = this.building.Def.HitPoints;
		}
		else
		{
			this.hitpoints = Math.Min(this.hitpoints + repair_amount, this.building.Def.HitPoints);
		}
		base.Trigger(-1699355994, this);
		if (this.hitpoints >= this.building.Def.HitPoints)
		{
			base.Trigger(-1735440190, this);
		}
	}

	// Token: 0x06002392 RID: 9106 RVA: 0x000C2B84 File Offset: 0x000C0D84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(10f);
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
	}

	// Token: 0x06002393 RID: 9107 RVA: 0x000C2BB8 File Offset: 0x000C0DB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new BuildingHP.SMInstance(this);
		this.smi.StartSM();
		base.Subscribe<BuildingHP>(-794517298, BuildingHP.OnDoBuildingDamageDelegate);
		if (this.destroyOnDamaged)
		{
			base.Subscribe<BuildingHP>(774203113, BuildingHP.DestroyOnDamagedDelegate);
		}
		if (this.hitpoints <= 0)
		{
			base.Trigger(774203113, this);
		}
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x000C2C21 File Offset: 0x000C0E21
	private void DestroyOnDamaged(object data)
	{
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x000C2C30 File Offset: 0x000C0E30
	protected override void OnCompleteWork(Worker worker)
	{
		int num = (int)Db.Get().Attributes.Machinery.Lookup(worker).GetTotalValue();
		int repair_amount = 10 + Math.Max(0, num * 10);
		this.Repair(repair_amount);
	}

	// Token: 0x06002396 RID: 9110 RVA: 0x000C2C6E File Offset: 0x000C0E6E
	private void OnDoBuildingDamage(object data)
	{
		if (this.invincible)
		{
			return;
		}
		this.damageSourceInfo = (BuildingHP.DamageSourceInfo)data;
		this.DoDamage(this.damageSourceInfo.damage);
		this.DoDamagePopFX(this.damageSourceInfo);
		this.DoTakeDamageFX(this.damageSourceInfo);
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x000C2CB0 File Offset: 0x000C0EB0
	private void DoTakeDamageFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.takeDamageEffect != SpawnFXHashes.None)
		{
			BuildingDef def = base.GetComponent<BuildingComplete>().Def;
			int cell = Grid.OffsetCell(Grid.PosToCell(this), 0, def.HeightInCells - 1);
			Game.Instance.SpawnFX(info.takeDamageEffect, cell, 0f);
		}
	}

	// Token: 0x06002398 RID: 9112 RVA: 0x000C2CFC File Offset: 0x000C0EFC
	private void DoDamagePopFX(BuildingHP.DamageSourceInfo info)
	{
		if (info.popString != null && Time.time > this.lastPopTime + this.minDamagePopInterval)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, info.popString, base.gameObject.transform, 1.5f, false);
			this.lastPopTime = Time.time;
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06002399 RID: 9113 RVA: 0x000C2D5C File Offset: 0x000C0F5C
	public bool IsBroken
	{
		get
		{
			return this.hitpoints == 0;
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x0600239A RID: 9114 RVA: 0x000C2D67 File Offset: 0x000C0F67
	public bool NeedsRepairs
	{
		get
		{
			return this.HitPoints < this.building.Def.HitPoints;
		}
	}

	// Token: 0x0400144A RID: 5194
	[Serialize]
	[SerializeField]
	private int hitpoints;

	// Token: 0x0400144B RID: 5195
	[Serialize]
	private BuildingHP.DamageSourceInfo damageSourceInfo;

	// Token: 0x0400144C RID: 5196
	private static readonly EventSystem.IntraObjectHandler<BuildingHP> OnDoBuildingDamageDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.OnDoBuildingDamage(data);
	});

	// Token: 0x0400144D RID: 5197
	private static readonly EventSystem.IntraObjectHandler<BuildingHP> DestroyOnDamagedDelegate = new EventSystem.IntraObjectHandler<BuildingHP>(delegate(BuildingHP component, object data)
	{
		component.DestroyOnDamaged(data);
	});

	// Token: 0x0400144E RID: 5198
	public static List<Meter> kbacQueryList = new List<Meter>();

	// Token: 0x0400144F RID: 5199
	public bool destroyOnDamaged;

	// Token: 0x04001450 RID: 5200
	public bool invincible;

	// Token: 0x04001451 RID: 5201
	[MyCmpGet]
	private Building building;

	// Token: 0x04001452 RID: 5202
	private BuildingHP.SMInstance smi;

	// Token: 0x04001453 RID: 5203
	private float minDamagePopInterval = 4f;

	// Token: 0x04001454 RID: 5204
	private float lastPopTime;

	// Token: 0x02001235 RID: 4661
	public struct DamageSourceInfo
	{
		// Token: 0x06007C33 RID: 31795 RVA: 0x002E03B3 File Offset: 0x002DE5B3
		public override string ToString()
		{
			return this.source;
		}

		// Token: 0x04005EC5 RID: 24261
		public int damage;

		// Token: 0x04005EC6 RID: 24262
		public string source;

		// Token: 0x04005EC7 RID: 24263
		public string popString;

		// Token: 0x04005EC8 RID: 24264
		public SpawnFXHashes takeDamageEffect;

		// Token: 0x04005EC9 RID: 24265
		public string fullDamageEffectName;

		// Token: 0x04005ECA RID: 24266
		public string statusItemID;
	}

	// Token: 0x02001236 RID: 4662
	public class SMInstance : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.GameInstance
	{
		// Token: 0x06007C34 RID: 31796 RVA: 0x002E03BB File Offset: 0x002DE5BB
		public SMInstance(BuildingHP master) : base(master)
		{
		}

		// Token: 0x06007C35 RID: 31797 RVA: 0x002E03C4 File Offset: 0x002DE5C4
		public Notification CreateBrokenMachineNotification()
		{
			return new Notification(MISC.NOTIFICATIONS.BROKENMACHINE.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BROKENMACHINE.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.master.damageSourceInfo.source, false, 0f, null, null, null, true, false, false);
		}

		// Token: 0x06007C36 RID: 31798 RVA: 0x002E0428 File Offset: 0x002DE628
		public void ShowProgressBar(bool show)
		{
			if (show && Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && Grid.IsVisible(Grid.PosToCell(base.gameObject)))
			{
				this.CreateProgressBar();
				return;
			}
			if (this.progressBar != null)
			{
				this.progressBar.gameObject.DeleteObject();
				this.progressBar = null;
			}
		}

		// Token: 0x06007C37 RID: 31799 RVA: 0x002E0488 File Offset: 0x002DE688
		public void UpdateMeter()
		{
			if (this.progressBar == null)
			{
				this.ShowProgressBar(true);
			}
			if (this.progressBar)
			{
				this.progressBar.Update();
			}
		}

		// Token: 0x06007C38 RID: 31800 RVA: 0x002E04B7 File Offset: 0x002DE6B7
		private float HealthPercent()
		{
			return (float)base.smi.master.HitPoints / (float)base.smi.master.building.Def.HitPoints;
		}

		// Token: 0x06007C39 RID: 31801 RVA: 0x002E04E8 File Offset: 0x002DE6E8
		private void CreateProgressBar()
		{
			if (this.progressBar != null)
			{
				return;
			}
			this.progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
			this.progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
			this.progressBar.name = base.smi.master.name + "." + base.smi.master.GetType().Name + " ProgressBar";
			this.progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
			this.progressBar.SetUpdateFunc(new Func<float>(this.HealthPercent));
			this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
			CanvasGroup component = this.progressBar.GetComponent<CanvasGroup>();
			component.interactable = false;
			component.blocksRaycasts = false;
			this.progressBar.Update();
			float d = 0.15f;
			Vector3 vector = base.gameObject.transform.GetPosition() + Vector3.down * d;
			vector.z += 0.05f;
			Rotatable component2 = base.GetComponent<Rotatable>();
			if (component2 == null || component2.GetOrientation() == Orientation.Neutral || base.smi.master.building.Def.WidthInCells < 2 || base.smi.master.building.Def.HeightInCells < 2)
			{
				vector -= Vector3.right * 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2);
			}
			else
			{
				vector += Vector3.left * (1f + 0.5f * (float)(base.smi.master.building.Def.WidthInCells % 2));
			}
			this.progressBar.transform.SetPosition(vector);
			this.progressBar.SetVisibility(true);
		}

		// Token: 0x06007C3A RID: 31802 RVA: 0x002E0718 File Offset: 0x002DE918
		private static string ToolTipResolver(List<Notification> notificationList, object data)
		{
			string text = "";
			for (int i = 0; i < notificationList.Count; i++)
			{
				Notification notification = notificationList[i];
				text += string.Format(BUILDINGS.DAMAGESOURCES.NOTIFICATION_TOOLTIP, notification.NotifierName, (string)notification.tooltipData);
				if (i < notificationList.Count - 1)
				{
					text += "\n";
				}
			}
			return text;
		}

		// Token: 0x06007C3B RID: 31803 RVA: 0x002E0784 File Offset: 0x002DE984
		public void ShowDamagedEffect()
		{
			if (base.master.damageSourceInfo.takeDamageEffect != SpawnFXHashes.None)
			{
				BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
				int cell = Grid.OffsetCell(Grid.PosToCell(base.master), 0, def.HeightInCells - 1);
				Game.Instance.SpawnFX(base.master.damageSourceInfo.takeDamageEffect, cell, 0f);
			}
		}

		// Token: 0x06007C3C RID: 31804 RVA: 0x002E07F0 File Offset: 0x002DE9F0
		public FXAnim.Instance InstantiateDamageFX()
		{
			if (base.master.damageSourceInfo.fullDamageEffectName == null)
			{
				return null;
			}
			BuildingDef def = base.master.GetComponent<BuildingComplete>().Def;
			Vector3 zero = Vector3.zero;
			if (def.HeightInCells > 1)
			{
				zero = new Vector3(0f, (float)(def.HeightInCells - 1), 0f);
			}
			else
			{
				zero = new Vector3(0f, 0.5f, 0f);
			}
			return new FXAnim.Instance(base.smi.master, base.master.damageSourceInfo.fullDamageEffectName, "idle", KAnim.PlayMode.Loop, zero, Color.white);
		}

		// Token: 0x06007C3D RID: 31805 RVA: 0x002E0894 File Offset: 0x002DEA94
		public void SetCrackOverlayValue(float value)
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component == null)
			{
				return;
			}
			component.SetBlendValue(value);
			BuildingHP.kbacQueryList.Clear();
			base.master.GetComponentsInChildren<Meter>(BuildingHP.kbacQueryList);
			for (int i = 0; i < BuildingHP.kbacQueryList.Count; i++)
			{
				BuildingHP.kbacQueryList[i].GetComponent<KBatchedAnimController>().SetBlendValue(value);
			}
		}

		// Token: 0x04005ECB RID: 24267
		private ProgressBar progressBar;
	}

	// Token: 0x02001237 RID: 4663
	public class States : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP>
	{
		// Token: 0x06007C3E RID: 31806 RVA: 0x002E0904 File Offset: 0x002DEB04
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.healthy;
			this.healthy.DefaultState(this.healthy.imperfect).EventTransition(GameHashes.BuildingReceivedDamage, this.damaged, (BuildingHP.SMInstance smi) => smi.master.HitPoints <= 0);
			this.healthy.imperfect.Enter(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(true);
			}).DefaultState(this.healthy.imperfect.playEffect).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).ToggleStatusItem(delegate(BuildingHP.SMInstance smi)
			{
				if (smi.master.damageSourceInfo.statusItemID == null)
				{
					return null;
				}
				return Db.Get().BuildingStatusItems.Get(smi.master.damageSourceInfo.statusItemID);
			}, null).Exit(delegate(BuildingHP.SMInstance smi)
			{
				smi.ShowProgressBar(false);
			});
			this.healthy.imperfect.playEffect.Transition(this.healthy.imperfect.waiting, (BuildingHP.SMInstance smi) => true, UpdateRate.SIM_200ms);
			this.healthy.imperfect.waiting.ScheduleGoTo((BuildingHP.SMInstance smi) => UnityEngine.Random.Range(15f, 30f), this.healthy.imperfect.playEffect);
			this.healthy.perfect.EventTransition(GameHashes.BuildingReceivedDamage, this.healthy.imperfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints < smi.master.building.Def.HitPoints);
			this.damaged.Enter(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, false);
				}
				smi.ShowProgressBar(true);
				smi.master.Trigger(774203113, smi.master);
				smi.SetCrackOverlayValue(1f);
			}).ToggleNotification((BuildingHP.SMInstance smi) => smi.CreateBrokenMachineNotification()).ToggleStatusItem(Db.Get().BuildingStatusItems.Broken, null).ToggleFX((BuildingHP.SMInstance smi) => smi.InstantiateDamageFX()).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (BuildingHP.SMInstance smi) => smi.master.HitPoints == smi.master.building.Def.HitPoints).EventHandler(GameHashes.BuildingPartiallyRepaired, delegate(BuildingHP.SMInstance smi)
			{
				smi.UpdateMeter();
			}).Exit(delegate(BuildingHP.SMInstance smi)
			{
				Operational component = smi.GetComponent<Operational>();
				if (component != null)
				{
					component.SetFlag(BuildingHP.States.healthyFlag, true);
				}
				smi.ShowProgressBar(false);
				smi.SetCrackOverlayValue(0f);
			});
		}

		// Token: 0x06007C3F RID: 31807 RVA: 0x002E0C28 File Offset: 0x002DEE28
		private Chore CreateRepairChore(BuildingHP.SMInstance smi)
		{
			return new WorkChore<BuildingHP>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04005ECC RID: 24268
		private static readonly Operational.Flag healthyFlag = new Operational.Flag("healthy", Operational.Flag.Type.Functional);

		// Token: 0x04005ECD RID: 24269
		public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State damaged;

		// Token: 0x04005ECE RID: 24270
		public BuildingHP.States.Healthy healthy;

		// Token: 0x020020B5 RID: 8373
		public class Healthy : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			// Token: 0x04009213 RID: 37395
			public BuildingHP.States.ImperfectStates imperfect;

			// Token: 0x04009214 RID: 37396
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State perfect;
		}

		// Token: 0x020020B6 RID: 8374
		public class ImperfectStates : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
		{
			// Token: 0x04009215 RID: 37397
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State playEffect;

			// Token: 0x04009216 RID: 37398
			public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State waiting;
		}
	}
}
