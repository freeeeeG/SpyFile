using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006A6 RID: 1702
public class TemporalTearOpener : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>
{
	// Token: 0x06002E1B RID: 11803 RVA: 0x000F378C File Offset: 0x000F198C
	private static StatusItem CreateColoniesStatusItem()
	{
		StatusItem statusItem = new StatusItem("Temporal_Tear_Opener_Insufficient_Colonies", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
		statusItem.resolveStringCallback = delegate(string str, object data)
		{
			TemporalTearOpener.Instance instance = (TemporalTearOpener.Instance)data;
			str = str.Replace("{progress}", string.Format("({0}/{1})", instance.CountColonies(), EstablishColonies.BASE_COUNT));
			return str;
		};
		return statusItem;
	}

	// Token: 0x06002E1C RID: 11804 RVA: 0x000F37E4 File Offset: 0x000F19E4
	private static StatusItem CreateProgressStatusItem()
	{
		StatusItem statusItem = new StatusItem("Temporal_Tear_Opener_Progress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
		statusItem.resolveStringCallback = delegate(string str, object data)
		{
			TemporalTearOpener.Instance instance = (TemporalTearOpener.Instance)data;
			str = str.Replace("{progress}", GameUtil.GetFormattedPercent(instance.GetPercentComplete(), GameUtil.TimeSlice.None));
			return str;
		};
		return statusItem;
	}

	// Token: 0x06002E1D RID: 11805 RVA: 0x000F383C File Offset: 0x000F1A3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.UpdateMeter();
			if (ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearOpen())
			{
				smi.GoTo(this.opening_tear_finish);
				return;
			}
			smi.GoTo(this.check_requirements);
		}).PlayAnim("off");
		this.check_requirements.DefaultState(this.check_requirements.has_target).Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.GetComponent<HighEnergyParticleStorage>().receiverOpen = false;
			smi.GetComponent<KBatchedAnimController>().Play("port_close", KAnim.PlayMode.Once, 1f, 0f);
			smi.GetComponent<KBatchedAnimController>().Queue("off", KAnim.PlayMode.Loop, 1f, 0f);
		});
		this.check_requirements.has_target.ToggleStatusItem(TemporalTearOpener.s_noTargetStatus, null).UpdateTransition(this.check_requirements.has_los, (TemporalTearOpener.Instance smi, float dt) => ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearRevealed(), UpdateRate.SIM_200ms, false);
		this.check_requirements.has_los.ToggleStatusItem(TemporalTearOpener.s_noLosStatus, null).UpdateTransition(this.check_requirements.enough_colonies, (TemporalTearOpener.Instance smi, float dt) => smi.HasLineOfSight(), UpdateRate.SIM_200ms, false);
		this.check_requirements.enough_colonies.ToggleStatusItem(TemporalTearOpener.s_insufficient_colonies, null).UpdateTransition(this.charging, (TemporalTearOpener.Instance smi, float dt) => smi.HasSufficientColonies(), UpdateRate.SIM_200ms, false);
		this.charging.DefaultState(this.charging.idle).ToggleStatusItem(TemporalTearOpener.s_progressStatus, (TemporalTearOpener.Instance smi) => smi).UpdateTransition(this.check_requirements.has_los, (TemporalTearOpener.Instance smi, float dt) => !smi.HasLineOfSight(), UpdateRate.SIM_200ms, false).UpdateTransition(this.check_requirements.enough_colonies, (TemporalTearOpener.Instance smi, float dt) => !smi.HasSufficientColonies(), UpdateRate.SIM_200ms, false).Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.GetComponent<HighEnergyParticleStorage>().receiverOpen = true;
			smi.GetComponent<KBatchedAnimController>().Play("port_open", KAnim.PlayMode.Once, 1f, 0f);
			smi.GetComponent<KBatchedAnimController>().Queue("inert", KAnim.PlayMode.Loop, 1f, 0f);
		});
		this.charging.idle.EventTransition(GameHashes.OnParticleStorageChanged, this.charging.consuming, (TemporalTearOpener.Instance smi) => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
		this.charging.consuming.EventTransition(GameHashes.OnParticleStorageChanged, this.charging.idle, (TemporalTearOpener.Instance smi) => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty()).UpdateTransition(this.ready, (TemporalTearOpener.Instance smi, float dt) => smi.ConsumeParticlesAndCheckComplete(dt), UpdateRate.SIM_200ms, false);
		this.ready.ToggleNotification((TemporalTearOpener.Instance smi) => new Notification(BUILDING.STATUSITEMS.TEMPORAL_TEAR_OPENER_READY.NOTIFICATION, NotificationType.Good, (List<Notification> a, object b) => BUILDING.STATUSITEMS.TEMPORAL_TEAR_OPENER_READY.NOTIFICATION_TOOLTIP, null, false, 0f, null, null, null, true, false, false));
		this.opening_tear_beam_pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.opening_tear_beam);
		this.opening_tear_beam.Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.CreateBeamFX();
		}).PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, this.opening_tear_finish);
		this.opening_tear_finish.PlayAnim("working_pst").Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.OpenTemporalTear();
		});
	}

	// Token: 0x04001B22 RID: 6946
	private const float MIN_SUNLIGHT_EXPOSURE = 15f;

	// Token: 0x04001B23 RID: 6947
	private static StatusItem s_noLosStatus = new StatusItem("Temporal_Tear_Opener_No_Los", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x04001B24 RID: 6948
	private static StatusItem s_insufficient_colonies = TemporalTearOpener.CreateColoniesStatusItem();

	// Token: 0x04001B25 RID: 6949
	private static StatusItem s_noTargetStatus = new StatusItem("Temporal_Tear_Opener_No_Target", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x04001B26 RID: 6950
	private static StatusItem s_progressStatus = TemporalTearOpener.CreateProgressStatusItem();

	// Token: 0x04001B27 RID: 6951
	private TemporalTearOpener.CheckRequirementsState check_requirements;

	// Token: 0x04001B28 RID: 6952
	private TemporalTearOpener.ChargingState charging;

	// Token: 0x04001B29 RID: 6953
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_beam_pre;

	// Token: 0x04001B2A RID: 6954
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_beam;

	// Token: 0x04001B2B RID: 6955
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_finish;

	// Token: 0x04001B2C RID: 6956
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State ready;

	// Token: 0x020013D2 RID: 5074
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006373 RID: 25459
		public float consumeRate;

		// Token: 0x04006374 RID: 25460
		public float numParticlesToOpen;
	}

	// Token: 0x020013D3 RID: 5075
	private class ChargingState : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State
	{
		// Token: 0x04006375 RID: 25461
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State idle;

		// Token: 0x04006376 RID: 25462
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State consuming;
	}

	// Token: 0x020013D4 RID: 5076
	private class CheckRequirementsState : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State
	{
		// Token: 0x04006377 RID: 25463
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State has_target;

		// Token: 0x04006378 RID: 25464
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State has_los;

		// Token: 0x04006379 RID: 25465
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State enough_colonies;
	}

	// Token: 0x020013D5 RID: 5077
	public new class Instance : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x06008255 RID: 33365 RVA: 0x002F9563 File Offset: 0x002F7763
		public Instance(IStateMachineTarget master, TemporalTearOpener.Def def) : base(master, def)
		{
			this.m_meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			EnterTemporalTearSequence.tearOpenerGameObject = base.gameObject;
		}

		// Token: 0x06008256 RID: 33366 RVA: 0x002F95A0 File Offset: 0x002F77A0
		protected override void OnCleanUp()
		{
			if (EnterTemporalTearSequence.tearOpenerGameObject == base.gameObject)
			{
				EnterTemporalTearSequence.tearOpenerGameObject = null;
			}
			base.OnCleanUp();
		}

		// Token: 0x06008257 RID: 33367 RVA: 0x002F95C0 File Offset: 0x002F77C0
		public bool HasLineOfSight()
		{
			Extents extents = base.GetComponent<Building>().GetExtents();
			int x = extents.x;
			int num = extents.x + extents.width - 1;
			for (int i = x; i <= num; i++)
			{
				int i2 = Grid.XYToCell(i, extents.y);
				if ((float)Grid.ExposedToSunlight[i2] < 15f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008258 RID: 33368 RVA: 0x002F961D File Offset: 0x002F781D
		public bool HasSufficientColonies()
		{
			return this.CountColonies() >= EstablishColonies.BASE_COUNT;
		}

		// Token: 0x06008259 RID: 33369 RVA: 0x002F9630 File Offset: 0x002F7830
		public int CountColonies()
		{
			int num = 0;
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				Activatable component = Components.Telepads[i].GetComponent<Activatable>();
				if (component == null || component.IsActivated)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600825A RID: 33370 RVA: 0x002F967C File Offset: 0x002F787C
		public bool ConsumeParticlesAndCheckComplete(float dt)
		{
			float amount = Mathf.Min(dt * base.def.consumeRate, base.def.numParticlesToOpen - this.m_particlesConsumed);
			float num = base.GetComponent<HighEnergyParticleStorage>().ConsumeAndGet(amount);
			this.m_particlesConsumed += num;
			this.UpdateMeter();
			return this.m_particlesConsumed >= base.def.numParticlesToOpen;
		}

		// Token: 0x0600825B RID: 33371 RVA: 0x002F96E5 File Offset: 0x002F78E5
		public void UpdateMeter()
		{
			this.m_meter.SetPositionPercent(this.GetAmountComplete());
		}

		// Token: 0x0600825C RID: 33372 RVA: 0x002F96F8 File Offset: 0x002F78F8
		private float GetAmountComplete()
		{
			return Mathf.Min(this.m_particlesConsumed / base.def.numParticlesToOpen, 1f);
		}

		// Token: 0x0600825D RID: 33373 RVA: 0x002F9716 File Offset: 0x002F7916
		public float GetPercentComplete()
		{
			return this.GetAmountComplete() * 100f;
		}

		// Token: 0x0600825E RID: 33374 RVA: 0x002F9724 File Offset: 0x002F7924
		public void CreateBeamFX()
		{
			Vector3 position = base.gameObject.transform.position;
			position.y += 3.25f;
			Quaternion rotation = Quaternion.Euler(-90f, 90f, 0f);
			Util.KInstantiate(EffectPrefabs.Instance.OpenTemporalTearBeam, position, rotation, base.gameObject, null, true, 0);
		}

		// Token: 0x0600825F RID: 33375 RVA: 0x002F9782 File Offset: 0x002F7982
		public void OpenTemporalTear()
		{
			ClusterManager.Instance.GetClusterPOIManager().RevealTemporalTear();
			ClusterManager.Instance.GetClusterPOIManager().OpenTemporalTear(this.GetMyWorldId());
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06008260 RID: 33376 RVA: 0x002F97A8 File Offset: 0x002F79A8
		public string SidescreenButtonText
		{
			get
			{
				return BUILDINGS.PREFABS.TEMPORALTEAROPENER.SIDESCREEN.TEXT;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06008261 RID: 33377 RVA: 0x002F97B4 File Offset: 0x002F79B4
		public string SidescreenButtonTooltip
		{
			get
			{
				return BUILDINGS.PREFABS.TEMPORALTEAROPENER.SIDESCREEN.TOOLTIP;
			}
		}

		// Token: 0x06008262 RID: 33378 RVA: 0x002F97C0 File Offset: 0x002F79C0
		public bool SidescreenEnabled()
		{
			return this.GetCurrentState() == base.sm.ready || DebugHandler.InstantBuildMode;
		}

		// Token: 0x06008263 RID: 33379 RVA: 0x002F97DC File Offset: 0x002F79DC
		public bool SidescreenButtonInteractable()
		{
			return this.GetCurrentState() == base.sm.ready || DebugHandler.InstantBuildMode;
		}

		// Token: 0x06008264 RID: 33380 RVA: 0x002F97F8 File Offset: 0x002F79F8
		public void OnSidescreenButtonPressed()
		{
			ConfirmDialogScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<ConfirmDialogScreen>();
			string text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_MESSAGE;
			System.Action on_confirm = delegate()
			{
				this.FireTemporalTearOpener(base.smi);
			};
			System.Action on_cancel = delegate()
			{
			};
			string configurable_text = null;
			System.Action on_configurable_clicked = null;
			string confirm_text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_CONFIRM;
			string cancel_text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_CANCEL;
			component.PopupConfirmDialog(text, on_confirm, on_cancel, configurable_text, on_configurable_clicked, UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_TITLE, confirm_text, cancel_text, null);
		}

		// Token: 0x06008265 RID: 33381 RVA: 0x002F9884 File Offset: 0x002F7A84
		private void FireTemporalTearOpener(TemporalTearOpener.Instance smi)
		{
			smi.GoTo(base.sm.opening_tear_beam_pre);
		}

		// Token: 0x06008266 RID: 33382 RVA: 0x002F9897 File Offset: 0x002F7A97
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06008267 RID: 33383 RVA: 0x002F989B File Offset: 0x002F7A9B
		public void SetButtonTextOverride(ButtonMenuTextOverride text)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008268 RID: 33384 RVA: 0x002F98A2 File Offset: 0x002F7AA2
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x0400637A RID: 25466
		[Serialize]
		private float m_particlesConsumed;

		// Token: 0x0400637B RID: 25467
		private MeterController m_meter;
	}
}
