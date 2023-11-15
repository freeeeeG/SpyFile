using System;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x020005BB RID: 1467
[SerializationConfig(MemberSerialization.OptIn)]
public class AutoMiner : StateMachineComponent<AutoMiner.Instance>, ISim1000ms
{
	// Token: 0x170001AE RID: 430
	// (get) Token: 0x060023EA RID: 9194 RVA: 0x000C4771 File Offset: 0x000C2971
	private bool HasDigCell
	{
		get
		{
			return this.dig_cell != Grid.InvalidCell;
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x060023EB RID: 9195 RVA: 0x000C4783 File Offset: 0x000C2983
	private bool RotationComplete
	{
		get
		{
			return this.HasDigCell && this.rotation_complete;
		}
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000C47A4 File Offset: 0x000C29A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hitEffectPrefab = Assets.GetPrefab("fx_dig_splash");
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		string name = component.name + ".gun";
		this.arm_go = new GameObject(name);
		this.arm_go.SetActive(false);
		this.arm_go.transform.parent = component.transform;
		this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
		string sound = GlobalAssets.GetSound(this.rotateSoundName, false);
		this.rotateSound = RuntimeManager.PathToEventReference(sound);
		this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
		this.arm_anim_ctrl.AnimFiles = new KAnimFile[]
		{
			component.AnimFiles[0]
		};
		this.arm_anim_ctrl.initialAnim = "gun";
		this.arm_anim_ctrl.isMovable = true;
		this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
		component.SetSymbolVisiblity("gun_target", false);
		bool flag;
		Vector3 position = component.GetSymbolTransform(new HashedString("gun_target"), out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
		this.arm_go.transform.SetPosition(position);
		this.arm_go.SetActive(true);
		this.link = new KAnimLink(component, this.arm_anim_ctrl);
		base.Subscribe<AutoMiner>(-592767678, AutoMiner.OnOperationalChangedDelegate);
		this.RotateArm(this.rotatable.GetRotatedOffset(Quaternion.Euler(0f, 0f, -45f) * Vector3.up), true, 0f);
		this.StopDig();
		base.smi.StartSM();
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x000C4972 File Offset: 0x000C2B72
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000C497A File Offset: 0x000C2B7A
	public void Sim1000ms(float dt)
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		this.RefreshDiggableCell();
		this.operational.SetActive(this.HasDigCell, false);
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x000C49A2 File Offset: 0x000C2BA2
	private void OnOperationalChanged(object data)
	{
		if (!(bool)data)
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x000C49C0 File Offset: 0x000C2BC0
	public void UpdateRotation(float dt)
	{
		if (this.HasDigCell)
		{
			Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.TileMain);
			a.z = 0f;
			Vector3 position = this.arm_go.transform.GetPosition();
			position.z = 0f;
			Vector3 target_dir = Vector3.Normalize(a - position);
			this.RotateArm(target_dir, false, dt);
		}
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x000C4A22 File Offset: 0x000C2C22
	private Element GetTargetElement()
	{
		if (this.HasDigCell)
		{
			return Grid.Element[this.dig_cell];
		}
		return null;
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000C4A3C File Offset: 0x000C2C3C
	public void StartDig()
	{
		Element targetElement = this.GetTargetElement();
		base.Trigger(-1762453998, targetElement);
		this.CreateHitEffect();
		this.arm_anim_ctrl.Play("gun_digging", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000C4A82 File Offset: 0x000C2C82
	public void StopDig()
	{
		base.Trigger(939543986, null);
		this.DestroyHitEffect();
		this.arm_anim_ctrl.Play("gun", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000C4AB8 File Offset: 0x000C2CB8
	public void UpdateDig(float dt)
	{
		if (!this.HasDigCell)
		{
			return;
		}
		if (!this.rotation_complete)
		{
			return;
		}
		Diggable.DoDigTick(this.dig_cell, dt);
		float percentComplete = Grid.Damage[this.dig_cell];
		this.mining_sounds.SetPercentComplete(percentComplete);
		Vector3 a = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		a.z = 0f;
		Vector3 position = this.arm_go.transform.GetPosition();
		position.z = 0f;
		float sqrMagnitude = (a - position).sqrMagnitude;
		this.arm_anim_ctrl.GetBatchInstanceData().SetClipRadius(position.x, position.y, sqrMagnitude, true);
		if (!AutoMiner.ValidDigCell(this.dig_cell))
		{
			this.dig_cell = Grid.InvalidCell;
			this.rotation_complete = false;
		}
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x000C4B84 File Offset: 0x000C2D84
	private void CreateHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.DestroyHitEffect();
		}
		Vector3 position = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
		this.hitEffect = GameUtil.KInstantiate(this.hitEffectPrefab, position, Grid.SceneLayer.FXFront2, null, 0);
		this.hitEffect.SetActive(true);
		KBatchedAnimController component = this.hitEffect.GetComponent<KBatchedAnimController>();
		component.sceneLayer = Grid.SceneLayer.FXFront2;
		component.initialMode = KAnim.PlayMode.Loop;
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000C4C0B File Offset: 0x000C2E0B
	private void DestroyHitEffect()
	{
		if (this.hitEffectPrefab == null)
		{
			return;
		}
		if (this.hitEffect != null)
		{
			this.hitEffect.DeleteObject();
			this.hitEffect = null;
		}
	}

	// Token: 0x060023F8 RID: 9208 RVA: 0x000C4C3C File Offset: 0x000C2E3C
	private void RefreshDiggableCell()
	{
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num;
		int num2;
		Grid.CellToXY(cell2, out num, out num2);
		float num3 = float.MaxValue;
		int num4 = Grid.InvalidCell;
		Vector3 a = Grid.CellToPos(cell2);
		bool flag = false;
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num5 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num5))
				{
					int x;
					int y;
					Grid.CellToXY(num5, out x, out y);
					if (Grid.IsValidCell(num5) && AutoMiner.ValidDigCell(num5) && Grid.TestLineOfSight(num, num2, x, y, new Func<int, bool>(AutoMiner.DigBlockingCB), false, false))
					{
						if (num5 == this.dig_cell)
						{
							flag = true;
						}
						Vector3 b = Grid.CellToPos(num5);
						float num6 = Vector3.Distance(a, b);
						if (num6 < num3)
						{
							num3 = num6;
							num4 = num5;
						}
					}
				}
			}
		}
		if (!flag && this.dig_cell != num4)
		{
			this.dig_cell = num4;
			this.rotation_complete = false;
		}
	}

	// Token: 0x060023F9 RID: 9209 RVA: 0x000C4DAB File Offset: 0x000C2FAB
	private static bool ValidDigCell(int cell)
	{
		return Grid.Solid[cell] && !Grid.Foundation[cell] && Grid.Element[cell].hardness < 150;
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x000C4DE1 File Offset: 0x000C2FE1
	public static bool DigBlockingCB(int cell)
	{
		return (Grid.Foundation[cell] && Grid.Solid[cell]) || Grid.Element[cell].hardness >= 150;
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000C4E18 File Offset: 0x000C3018
	private void RotateArm(Vector3 target_dir, bool warp, float dt)
	{
		if (this.rotation_complete)
		{
			return;
		}
		float num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot;
		num = MathUtil.Wrap(-180f, 180f, num);
		this.rotation_complete = Mathf.Approximately(num, 0f);
		float num2 = num;
		if (warp)
		{
			this.rotation_complete = true;
		}
		else
		{
			num2 = Mathf.Clamp(num2, -this.turn_rate * dt, this.turn_rate * dt);
		}
		this.arm_rot += num2;
		this.arm_rot = MathUtil.Wrap(-180f, 180f, this.arm_rot);
		this.arm_go.transform.rotation = Quaternion.Euler(0f, 0f, this.arm_rot);
		if (!this.rotation_complete)
		{
			this.StartRotateSound();
			this.looping_sounds.SetParameter(this.rotateSound, AutoMiner.HASH_ROTATION, this.arm_rot);
			return;
		}
		this.StopRotateSound();
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000C4F0D File Offset: 0x000C310D
	private void StartRotateSound()
	{
		if (!this.rotate_sound_playing)
		{
			this.looping_sounds.StartSound(this.rotateSound);
			this.rotate_sound_playing = true;
		}
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000C4F30 File Offset: 0x000C3130
	private void StopRotateSound()
	{
		if (this.rotate_sound_playing)
		{
			this.looping_sounds.StopSound(this.rotateSound);
			this.rotate_sound_playing = false;
		}
	}

	// Token: 0x0400148F RID: 5263
	private static HashedString HASH_ROTATION = "rotation";

	// Token: 0x04001490 RID: 5264
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001491 RID: 5265
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04001492 RID: 5266
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001493 RID: 5267
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04001494 RID: 5268
	[MyCmpReq]
	private MiningSounds mining_sounds;

	// Token: 0x04001495 RID: 5269
	public int x;

	// Token: 0x04001496 RID: 5270
	public int y;

	// Token: 0x04001497 RID: 5271
	public int width;

	// Token: 0x04001498 RID: 5272
	public int height;

	// Token: 0x04001499 RID: 5273
	public CellOffset vision_offset;

	// Token: 0x0400149A RID: 5274
	private KBatchedAnimController arm_anim_ctrl;

	// Token: 0x0400149B RID: 5275
	private GameObject arm_go;

	// Token: 0x0400149C RID: 5276
	private LoopingSounds looping_sounds;

	// Token: 0x0400149D RID: 5277
	private string rotateSoundName = "AutoMiner_rotate";

	// Token: 0x0400149E RID: 5278
	private EventReference rotateSound;

	// Token: 0x0400149F RID: 5279
	private KAnimLink link;

	// Token: 0x040014A0 RID: 5280
	private float arm_rot = 45f;

	// Token: 0x040014A1 RID: 5281
	private float turn_rate = 180f;

	// Token: 0x040014A2 RID: 5282
	private bool rotation_complete;

	// Token: 0x040014A3 RID: 5283
	private bool rotate_sound_playing;

	// Token: 0x040014A4 RID: 5284
	private GameObject hitEffectPrefab;

	// Token: 0x040014A5 RID: 5285
	private GameObject hitEffect;

	// Token: 0x040014A6 RID: 5286
	private int dig_cell = Grid.InvalidCell;

	// Token: 0x040014A7 RID: 5287
	private static readonly EventSystem.IntraObjectHandler<AutoMiner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AutoMiner>(delegate(AutoMiner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x02001246 RID: 4678
	public class Instance : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.GameInstance
	{
		// Token: 0x06007C84 RID: 31876 RVA: 0x002E1AB0 File Offset: 0x002DFCB0
		public Instance(AutoMiner master) : base(master)
		{
		}
	}

	// Token: 0x02001247 RID: 4679
	public class States : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner>
	{
		// Token: 0x06007C85 RID: 31877 RVA: 0x002E1ABC File Offset: 0x002DFCBC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.moving, (AutoMiner.Instance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.moving.Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopRotateSound();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateRotation(dt);
			}, UpdateRate.SIM_33ms, false).Transition(this.on.digging, new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete), UpdateRate.SIM_200ms);
			this.on.digging.Enter(delegate(AutoMiner.Instance smi)
			{
				smi.master.StartDig();
			}).Exit(delegate(AutoMiner.Instance smi)
			{
				smi.master.StopDig();
			}).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (AutoMiner.Instance smi) => !smi.GetComponent<Operational>().IsActive).Update(delegate(AutoMiner.Instance smi, float dt)
			{
				smi.master.UpdateDig(dt);
			}, UpdateRate.SIM_200ms, false).Transition(this.on.moving, GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Not(new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete)), UpdateRate.SIM_200ms);
		}

		// Token: 0x06007C86 RID: 31878 RVA: 0x002E1D38 File Offset: 0x002DFF38
		public static bool RotationComplete(AutoMiner.Instance smi)
		{
			return smi.master.RotationComplete;
		}

		// Token: 0x04005F05 RID: 24325
		public StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.BoolParameter transferring;

		// Token: 0x04005F06 RID: 24326
		public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State off;

		// Token: 0x04005F07 RID: 24327
		public AutoMiner.States.ReadyStates on;

		// Token: 0x020020BA RID: 8378
		public class ReadyStates : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State
		{
			// Token: 0x04009243 RID: 37443
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State idle;

			// Token: 0x04009244 RID: 37444
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State moving;

			// Token: 0x04009245 RID: 37445
			public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State digging;
		}
	}
}
