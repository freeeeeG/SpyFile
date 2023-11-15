using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x0200061E RID: 1566
public class LadderBed : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>
{
	// Token: 0x06002787 RID: 10119 RVA: 0x000D68CA File Offset: 0x000D4ACA
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x040016D9 RID: 5849
	public static string lightBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_LightShake", false);

	// Token: 0x040016DA RID: 5850
	public static string noDupeBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_Shake", false);

	// Token: 0x040016DB RID: 5851
	public static string LADDER_BED_COUNT_BELOW_PARAMETER = "bed_count";

	// Token: 0x020012E1 RID: 4833
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040060F4 RID: 24820
		public CellOffset[] offsets;
	}

	// Token: 0x020012E2 RID: 4834
	public new class Instance : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>.GameInstance
	{
		// Token: 0x06007EE3 RID: 32483 RVA: 0x002EA904 File Offset: 0x002E8B04
		public Instance(IStateMachineTarget master, LadderBed.Def def) : base(master, def)
		{
			ScenePartitionerLayer scenePartitionerLayer = GameScenePartitioner.Instance.objectLayers[40];
			this.m_cell = Grid.PosToCell(master.gameObject);
			foreach (CellOffset offset in def.offsets)
			{
				int cell = Grid.OffsetCell(this.m_cell, offset);
				if (Grid.IsValidCell(this.m_cell) && Grid.IsValidCell(cell))
				{
					this.m_partitionEntires.Add(GameScenePartitioner.Instance.Add("LadderBed.Constructor", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnMoverChanged)));
					this.OnMoverChanged(null);
				}
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			this.OnAttachmentChanged(null);
			base.Subscribe(-717201811, new Action<object>(this.OnSleepDisturbedByMovement));
			master.GetComponent<KAnimControllerBase>().GetLayering().GetLink().syncTint = false;
		}

		// Token: 0x06007EE4 RID: 32484 RVA: 0x002EAA1C File Offset: 0x002E8C1C
		private void OnSleepDisturbedByMovement(object obj)
		{
			base.GetComponent<KAnimControllerBase>().Play("interrupt_light", KAnim.PlayMode.Once, 1f, 0f);
			EventInstance instance = SoundEvent.BeginOneShot(LadderBed.lightBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
			instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
			SoundEvent.EndOneShot(instance);
		}

		// Token: 0x06007EE5 RID: 32485 RVA: 0x002EAA86 File Offset: 0x002E8C86
		private void OnAttachmentChanged(object data)
		{
			this.numBelow = AttachableBuilding.CountAttachedBelow(this.m_attachable);
		}

		// Token: 0x06007EE6 RID: 32486 RVA: 0x002EAA9C File Offset: 0x002E8C9C
		private void OnMoverChanged(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable != null && pickupable.gameObject != null && pickupable.HasTag(GameTags.Minion) && pickupable.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
			{
				if (this.m_sleepable.worker == null)
				{
					base.GetComponent<KAnimControllerBase>().Play("interrupt_light_nodupe", KAnim.PlayMode.Once, 1f, 0f);
					EventInstance instance = SoundEvent.BeginOneShot(LadderBed.noDupeBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
					instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
					SoundEvent.EndOneShot(instance);
					return;
				}
				if (pickupable.gameObject != this.m_sleepable.worker.gameObject)
				{
					this.m_sleepable.worker.Trigger(-717201811, null);
				}
			}
		}

		// Token: 0x06007EE7 RID: 32487 RVA: 0x002EAB94 File Offset: 0x002E8D94
		protected override void OnCleanUp()
		{
			foreach (HandleVector<int>.Handle handle in this.m_partitionEntires)
			{
				GameScenePartitioner.Instance.Free(ref handle);
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			base.OnCleanUp();
		}

		// Token: 0x040060F5 RID: 24821
		private List<HandleVector<int>.Handle> m_partitionEntires = new List<HandleVector<int>.Handle>();

		// Token: 0x040060F6 RID: 24822
		private int m_cell;

		// Token: 0x040060F7 RID: 24823
		[MyCmpGet]
		private Ownable m_ownable;

		// Token: 0x040060F8 RID: 24824
		[MyCmpGet]
		private Sleepable m_sleepable;

		// Token: 0x040060F9 RID: 24825
		[MyCmpGet]
		private AttachableBuilding m_attachable;

		// Token: 0x040060FA RID: 24826
		private int numBelow;
	}
}
