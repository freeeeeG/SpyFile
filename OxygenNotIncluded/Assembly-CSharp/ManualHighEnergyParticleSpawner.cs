using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200064B RID: 1611
[SerializationConfig(MemberSerialization.OptIn)]
public class ManualHighEnergyParticleSpawner : StateMachineComponent<ManualHighEnergyParticleSpawner.StatesInstance>, IHighEnergyParticleDirection
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06002A5E RID: 10846 RVA: 0x000E240E File Offset: 0x000E060E
	// (set) Token: 0x06002A5F RID: 10847 RVA: 0x000E2418 File Offset: 0x000E0618
	public EightDirection Direction
	{
		get
		{
			return this._direction;
		}
		set
		{
			this._direction = value;
			if (this.directionController != null)
			{
				this.directionController.SetRotation((float)(45 * EightDirectionUtil.GetDirectionIndex(this._direction)));
				this.directionController.controller.enabled = false;
				this.directionController.controller.enabled = true;
			}
		}
	}

	// Token: 0x06002A60 RID: 10848 RVA: 0x000E2470 File Offset: 0x000E0670
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ManualHighEnergyParticleSpawner>(-905833192, ManualHighEnergyParticleSpawner.OnCopySettingsDelegate);
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x000E248C File Offset: 0x000E068C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.radiationEmitter.SetEmitting(false);
		this.directionController = new EightDirectionController(base.GetComponent<KBatchedAnimController>(), "redirector_target", "redirect", EightDirectionController.Offset.Infront);
		this.Direction = this.Direction;
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x000E24EC File Offset: 0x000E06EC
	private void OnCopySettings(object data)
	{
		ManualHighEnergyParticleSpawner component = ((GameObject)data).GetComponent<ManualHighEnergyParticleSpawner>();
		if (component != null)
		{
			this.Direction = component.Direction;
		}
	}

	// Token: 0x06002A63 RID: 10851 RVA: 0x000E251C File Offset: 0x000E071C
	public void LauncherUpdate()
	{
		if (this.particleStorage.Particles > 0f)
		{
			int highEnergyParticleOutputCell = base.GetComponent<Building>().GetHighEnergyParticleOutputCell();
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
			gameObject.SetActive(true);
			if (gameObject != null)
			{
				HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
				component.payload = this.particleStorage.ConsumeAndGet(this.particleStorage.Particles);
				component.SetDirection(this.Direction);
				this.directionController.PlayAnim("redirect_send", KAnim.PlayMode.Once);
				this.directionController.controller.Queue("redirect", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x040018BD RID: 6333
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x040018BE RID: 6334
	[MyCmpGet]
	private RadiationEmitter radiationEmitter;

	// Token: 0x040018BF RID: 6335
	[Serialize]
	private EightDirection _direction;

	// Token: 0x040018C0 RID: 6336
	private EightDirectionController directionController;

	// Token: 0x040018C1 RID: 6337
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040018C2 RID: 6338
	private static readonly EventSystem.IntraObjectHandler<ManualHighEnergyParticleSpawner> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ManualHighEnergyParticleSpawner>(delegate(ManualHighEnergyParticleSpawner component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200131C RID: 4892
	public class StatesInstance : GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.GameInstance
	{
		// Token: 0x06007FB0 RID: 32688 RVA: 0x002ED925 File Offset: 0x002EBB25
		public StatesInstance(ManualHighEnergyParticleSpawner smi) : base(smi)
		{
		}

		// Token: 0x06007FB1 RID: 32689 RVA: 0x002ED92E File Offset: 0x002EBB2E
		public bool IsComplexFabricatorWorkable(object data)
		{
			return data as ComplexFabricatorWorkable != null;
		}
	}

	// Token: 0x0200131D RID: 4893
	public class States : GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner>
	{
		// Token: 0x06007FB2 RID: 32690 RVA: 0x002ED93C File Offset: 0x002EBB3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.Enter(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.radiationEmitter.SetEmitting(false);
			}).TagTransition(GameTags.Operational, this.ready, false);
			this.ready.DefaultState(this.ready.idle).TagTransition(GameTags.Operational, this.inoperational, true).Update(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi, float dt)
			{
				smi.master.LauncherUpdate();
			}, UpdateRate.SIM_200ms, false);
			this.ready.idle.EventHandlerTransition(GameHashes.WorkableStartWork, this.ready.working, (ManualHighEnergyParticleSpawner.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data));
			this.ready.working.Enter(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.radiationEmitter.SetEmitting(true);
			}).EventHandlerTransition(GameHashes.WorkableCompleteWork, this.ready.idle, (ManualHighEnergyParticleSpawner.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data)).EventHandlerTransition(GameHashes.WorkableStopWork, this.ready.idle, (ManualHighEnergyParticleSpawner.StatesInstance smi, object data) => smi.IsComplexFabricatorWorkable(data)).Exit(delegate(ManualHighEnergyParticleSpawner.StatesInstance smi)
			{
				smi.master.radiationEmitter.SetEmitting(false);
			});
		}

		// Token: 0x0400617C RID: 24956
		public GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State inoperational;

		// Token: 0x0400617D RID: 24957
		public ManualHighEnergyParticleSpawner.States.ReadyStates ready;

		// Token: 0x020020F3 RID: 8435
		public class ReadyStates : GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State
		{
			// Token: 0x04009382 RID: 37762
			public GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State idle;

			// Token: 0x04009383 RID: 37763
			public GameStateMachine<ManualHighEnergyParticleSpawner.States, ManualHighEnergyParticleSpawner.StatesInstance, ManualHighEnergyParticleSpawner, object>.State working;
		}
	}
}
