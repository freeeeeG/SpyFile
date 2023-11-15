using System;
using UnityEngine;

// Token: 0x02000482 RID: 1154
public class AutoStorageDropper : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>
{
	// Token: 0x06001960 RID: 6496 RVA: 0x00084D4C File Offset: 0x00082F4C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.root.Update(delegate(AutoStorageDropper.Instance smi, float dt)
		{
			smi.UpdateBlockedStatus();
		}, UpdateRate.SIM_200ms, true);
		this.idle.EventTransition(GameHashes.OnStorageChange, this.pre_drop, null).OnSignal(this.checkCanDrop, this.pre_drop, (AutoStorageDropper.Instance smi) => !smi.GetComponent<Storage>().IsEmpty()).ParamTransition<bool>(this.isBlocked, this.blocked, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsTrue);
		this.pre_drop.ScheduleGoTo((AutoStorageDropper.Instance smi) => smi.def.delay, this.dropping);
		this.dropping.Enter(delegate(AutoStorageDropper.Instance smi)
		{
			smi.Drop();
		}).GoTo(this.idle);
		this.blocked.ParamTransition<bool>(this.isBlocked, this.pre_drop, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.OutputTileBlocked, null);
	}

	// Token: 0x04000DFE RID: 3582
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State idle;

	// Token: 0x04000DFF RID: 3583
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State pre_drop;

	// Token: 0x04000E00 RID: 3584
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State dropping;

	// Token: 0x04000E01 RID: 3585
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State blocked;

	// Token: 0x04000E02 RID: 3586
	private StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.BoolParameter isBlocked;

	// Token: 0x04000E03 RID: 3587
	public StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.Signal checkCanDrop;

	// Token: 0x020010FA RID: 4346
	public class DropperFxConfig
	{
		// Token: 0x04005AD0 RID: 23248
		public string animFile;

		// Token: 0x04005AD1 RID: 23249
		public string animName;

		// Token: 0x04005AD2 RID: 23250
		public Grid.SceneLayer layer = Grid.SceneLayer.FXFront;

		// Token: 0x04005AD3 RID: 23251
		public bool useElementTint = true;

		// Token: 0x04005AD4 RID: 23252
		public bool flipX;

		// Token: 0x04005AD5 RID: 23253
		public bool flipY;
	}

	// Token: 0x020010FB RID: 4347
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005AD6 RID: 23254
		public CellOffset dropOffset;

		// Token: 0x04005AD7 RID: 23255
		public bool asOre;

		// Token: 0x04005AD8 RID: 23256
		public SimHashes[] elementFilter;

		// Token: 0x04005AD9 RID: 23257
		public bool invertElementFilter;

		// Token: 0x04005ADA RID: 23258
		public bool blockedBySubstantialLiquid;

		// Token: 0x04005ADB RID: 23259
		public AutoStorageDropper.DropperFxConfig neutralFx;

		// Token: 0x04005ADC RID: 23260
		public AutoStorageDropper.DropperFxConfig leftFx;

		// Token: 0x04005ADD RID: 23261
		public AutoStorageDropper.DropperFxConfig rightFx;

		// Token: 0x04005ADE RID: 23262
		public AutoStorageDropper.DropperFxConfig upFx;

		// Token: 0x04005ADF RID: 23263
		public AutoStorageDropper.DropperFxConfig downFx;

		// Token: 0x04005AE0 RID: 23264
		public Vector3 fxOffset = Vector3.zero;

		// Token: 0x04005AE1 RID: 23265
		public float cooldown = 2f;

		// Token: 0x04005AE2 RID: 23266
		public float delay;
	}

	// Token: 0x020010FC RID: 4348
	public new class Instance : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.GameInstance
	{
		// Token: 0x060077D8 RID: 30680 RVA: 0x002D4D7A File Offset: 0x002D2F7A
		public Instance(IStateMachineTarget master, AutoStorageDropper.Def def) : base(master, def)
		{
		}

		// Token: 0x060077D9 RID: 30681 RVA: 0x002D4D84 File Offset: 0x002D2F84
		public void SetInvertElementFilter(bool value)
		{
			base.def.invertElementFilter = value;
			base.smi.sm.checkCanDrop.Trigger(base.smi);
		}

		// Token: 0x060077DA RID: 30682 RVA: 0x002D4DB0 File Offset: 0x002D2FB0
		public void UpdateBlockedStatus()
		{
			int cell = Grid.PosToCell(base.smi.GetDropPosition());
			bool value = Grid.IsSolidCell(cell) || (base.def.blockedBySubstantialLiquid && Grid.IsSubstantialLiquid(cell, 0.35f));
			base.sm.isBlocked.Set(value, base.smi, false);
		}

		// Token: 0x060077DB RID: 30683 RVA: 0x002D4E10 File Offset: 0x002D3010
		private bool IsFilteredElement(SimHashes element)
		{
			for (int num = 0; num != base.def.elementFilter.Length; num++)
			{
				if (base.def.elementFilter[num] == element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060077DC RID: 30684 RVA: 0x002D4E48 File Offset: 0x002D3048
		private bool AllowedToDrop(SimHashes element)
		{
			return base.def.elementFilter == null || base.def.elementFilter.Length == 0 || (!base.def.invertElementFilter && this.IsFilteredElement(element)) || (base.def.invertElementFilter && !this.IsFilteredElement(element));
		}

		// Token: 0x060077DD RID: 30685 RVA: 0x002D4EA4 File Offset: 0x002D30A4
		public void Drop()
		{
			bool flag = false;
			Element element = null;
			for (int i = this.m_storage.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.m_storage.items[i];
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (this.AllowedToDrop(component.ElementID))
				{
					if (base.def.asOre)
					{
						this.m_storage.Drop(gameObject, true);
						gameObject.transform.SetPosition(this.GetDropPosition());
						element = component.Element;
						flag = true;
					}
					else
					{
						Dumpable component2 = gameObject.GetComponent<Dumpable>();
						if (!component2.IsNullOrDestroyed())
						{
							component2.Dump(this.GetDropPosition());
							element = component.Element;
							flag = true;
						}
					}
				}
			}
			AutoStorageDropper.DropperFxConfig dropperAnim = this.GetDropperAnim();
			if (flag && dropperAnim != null && GameClock.Instance.GetTime() > this.m_timeSinceLastDrop + base.def.cooldown)
			{
				this.m_timeSinceLastDrop = GameClock.Instance.GetTime();
				Vector3 vector = Grid.CellToPosCCC(Grid.PosToCell(this.GetDropPosition()), dropperAnim.layer);
				vector += ((this.m_rotatable != null) ? this.m_rotatable.GetRotatedOffset(base.def.fxOffset) : base.def.fxOffset);
				KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(dropperAnim.animFile, vector, null, false, dropperAnim.layer, false);
				kbatchedAnimController.destroyOnAnimComplete = false;
				kbatchedAnimController.FlipX = dropperAnim.flipX;
				kbatchedAnimController.FlipY = dropperAnim.flipY;
				if (dropperAnim.useElementTint)
				{
					kbatchedAnimController.TintColour = element.substance.colour;
				}
				kbatchedAnimController.Play(dropperAnim.animName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x060077DE RID: 30686 RVA: 0x002D506C File Offset: 0x002D326C
		public AutoStorageDropper.DropperFxConfig GetDropperAnim()
		{
			CellOffset cellOffset = (this.m_rotatable != null) ? this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset) : base.def.dropOffset;
			if (cellOffset.x < 0)
			{
				return base.def.leftFx;
			}
			if (cellOffset.x > 0)
			{
				return base.def.rightFx;
			}
			if (cellOffset.y < 0)
			{
				return base.def.downFx;
			}
			if (cellOffset.y > 0)
			{
				return base.def.upFx;
			}
			return base.def.neutralFx;
		}

		// Token: 0x060077DF RID: 30687 RVA: 0x002D510C File Offset: 0x002D330C
		public Vector3 GetDropPosition()
		{
			if (!(this.m_rotatable != null))
			{
				return base.transform.GetPosition() + base.def.dropOffset.ToVector3();
			}
			return base.transform.GetPosition() + this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset).ToVector3();
		}

		// Token: 0x04005AE3 RID: 23267
		[MyCmpGet]
		private Storage m_storage;

		// Token: 0x04005AE4 RID: 23268
		[MyCmpGet]
		private Rotatable m_rotatable;

		// Token: 0x04005AE5 RID: 23269
		private float m_timeSinceLastDrop;
	}
}
