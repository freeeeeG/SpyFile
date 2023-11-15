using System;
using Klei;
using UnityEngine;

// Token: 0x020005C7 RID: 1479
public class Chlorinator : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>
{
	// Token: 0x0600247E RID: 9342 RVA: 0x000C6E38 File Offset: 0x000C5038
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.TagTransition(GameTags.Operational, this.ready, false);
		this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle);
		this.ready.idle.EventTransition(GameHashes.OnStorageChange, this.ready.wait, (Chlorinator.StatesInstance smi) => smi.CanEmit()).EnterTransition(this.ready.wait, (Chlorinator.StatesInstance smi) => smi.CanEmit()).Target(this.hopper).PlayAnim("hopper_idle_loop");
		this.ready.wait.ScheduleGoTo(new Func<Chlorinator.StatesInstance, float>(Chlorinator.GetPoppingDelay), this.ready.popPre).EnterTransition(this.ready.idle, (Chlorinator.StatesInstance smi) => !smi.CanEmit()).Target(this.hopper).PlayAnim("hopper_idle_loop");
		this.ready.popPre.Target(this.hopper).PlayAnim("meter_hopper_pre").OnAnimQueueComplete(this.ready.pop);
		this.ready.pop.Enter(delegate(Chlorinator.StatesInstance smi)
		{
			smi.TryEmit();
		}).Target(this.hopper).PlayAnim("meter_hopper_loop").OnAnimQueueComplete(this.ready.popPst);
		this.ready.popPst.Target(this.hopper).PlayAnim("meter_hopper_pst").OnAnimQueueComplete(this.ready.wait);
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x000C7034 File Offset: 0x000C5234
	public static float GetPoppingDelay(Chlorinator.StatesInstance smi)
	{
		return smi.def.popWaitRange.Get();
	}

	// Token: 0x040014ED RID: 5357
	private GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State inoperational;

	// Token: 0x040014EE RID: 5358
	private Chlorinator.ReadyStates ready;

	// Token: 0x040014EF RID: 5359
	public StateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.TargetParameter hopper;

	// Token: 0x02001258 RID: 4696
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005F31 RID: 24369
		public MathUtil.MinMax popWaitRange = new MathUtil.MinMax(0.2f, 0.8f);

		// Token: 0x04005F32 RID: 24370
		public Tag primaryOreTag;

		// Token: 0x04005F33 RID: 24371
		public float primaryOreMassPerOre;

		// Token: 0x04005F34 RID: 24372
		public MathUtil.MinMaxInt primaryOreCount = new MathUtil.MinMaxInt(1, 1);

		// Token: 0x04005F35 RID: 24373
		public Tag secondaryOreTag;

		// Token: 0x04005F36 RID: 24374
		public float secondaryOreMassPerOre;

		// Token: 0x04005F37 RID: 24375
		public MathUtil.MinMaxInt secondaryOreCount = new MathUtil.MinMaxInt(1, 1);

		// Token: 0x04005F38 RID: 24376
		public Vector3 offset = Vector3.zero;

		// Token: 0x04005F39 RID: 24377
		public MathUtil.MinMax initialVelocity = new MathUtil.MinMax(1f, 3f);

		// Token: 0x04005F3A RID: 24378
		public MathUtil.MinMax initialDirectionHalfAngleDegreesRange = new MathUtil.MinMax(160f, 20f);
	}

	// Token: 0x02001259 RID: 4697
	public class ReadyStates : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State
	{
		// Token: 0x04005F3B RID: 24379
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State idle;

		// Token: 0x04005F3C RID: 24380
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State wait;

		// Token: 0x04005F3D RID: 24381
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPre;

		// Token: 0x04005F3E RID: 24382
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State pop;

		// Token: 0x04005F3F RID: 24383
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPst;
	}

	// Token: 0x0200125A RID: 4698
	public class StatesInstance : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.GameInstance
	{
		// Token: 0x06007CC9 RID: 31945 RVA: 0x002E2564 File Offset: 0x002E0764
		public StatesInstance(IStateMachineTarget master, Chlorinator.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<ComplexFabricator>().outStorage;
			KAnimControllerBase component = master.GetComponent<KAnimControllerBase>();
			this.hopperMeter = new MeterController(component, "meter_target", "meter_hopper_pre", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target"
			});
			base.sm.hopper.Set(this.hopperMeter.gameObject, this, false);
		}

		// Token: 0x06007CCA RID: 31946 RVA: 0x002E25D6 File Offset: 0x002E07D6
		public bool CanEmit()
		{
			return !this.storage.IsEmpty();
		}

		// Token: 0x06007CCB RID: 31947 RVA: 0x002E25E8 File Offset: 0x002E07E8
		public void TryEmit()
		{
			this.TryEmit(base.smi.def.primaryOreCount.Get(), base.def.primaryOreTag, base.def.primaryOreMassPerOre);
			this.TryEmit(base.smi.def.secondaryOreCount.Get(), base.def.secondaryOreTag, base.def.secondaryOreMassPerOre);
		}

		// Token: 0x06007CCC RID: 31948 RVA: 0x002E2658 File Offset: 0x002E0858
		private void TryEmit(int oreSpawnCount, Tag emitTag, float amount)
		{
			GameObject gameObject = this.storage.FindFirst(emitTag);
			if (gameObject == null)
			{
				return;
			}
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			Substance substance = component.Element.substance;
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float temperature;
			this.storage.ConsumeAndGetDisease(emitTag, amount, out num, out diseaseInfo, out temperature);
			if (num <= 0f)
			{
				return;
			}
			float mass = num * component.MassPerUnit / (float)oreSpawnCount;
			Vector3 vector = base.smi.gameObject.transform.position;
			vector += base.def.offset;
			bool flag = UnityEngine.Random.value >= 0.5f;
			for (int i = 0; i < oreSpawnCount; i++)
			{
				float f = base.def.initialDirectionHalfAngleDegreesRange.Get() * 3.1415927f / 180f;
				Vector2 normalized = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
				if (flag)
				{
					normalized.x = -normalized.x;
				}
				flag = !flag;
				normalized = normalized.normalized;
				Vector3 v = normalized * base.def.initialVelocity.Get();
				Vector3 vector2 = vector;
				vector2 += normalized * 0.1f;
				GameObject go = substance.SpawnResource(vector2, mass, temperature, diseaseInfo.idx, diseaseInfo.count / oreSpawnCount, false, false, false);
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Chlorinator_popping", false), CameraController.Instance.GetVerticallyScaledPosition(vector2, false), 1f);
				if (GameComps.Fallers.Has(go))
				{
					GameComps.Fallers.Remove(go);
				}
				GameComps.Fallers.Add(go, v);
			}
		}

		// Token: 0x04005F40 RID: 24384
		public Storage storage;

		// Token: 0x04005F41 RID: 24385
		public MeterController hopperMeter;
	}
}
