using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000710 RID: 1808
public class CreatureLightToggleController : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>
{
	// Token: 0x060031B8 RID: 12728 RVA: 0x001082C8 File Offset: 0x001064C8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.light_off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.light_off.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(false);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_on, new Func<CreatureLightToggleController.Instance, object, bool>(CreatureLightToggleController.ShouldProduceLight));
		this.turning_off.BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.dim, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_off, (CreatureLightToggleController.Instance smi) => smi.IsOff(), UpdateRate.SIM_200ms);
		this.light_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.turning_off, (CreatureLightToggleController.Instance smi, object obj) => !CreatureLightToggleController.ShouldProduceLight(smi, obj));
		this.turning_on.Enter(delegate(CreatureLightToggleController.Instance smi)
		{
			smi.SwitchLight(true);
		}).BatchUpdate(delegate(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, float time_delta)
		{
			CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.brighten, time_delta);
		}, UpdateRate.SIM_200ms).Transition(this.light_on, (CreatureLightToggleController.Instance smi) => smi.IsOn(), UpdateRate.SIM_200ms);
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x00108457 File Offset: 0x00106657
	public static bool ShouldProduceLight(CreatureLightToggleController.Instance smi, object obj)
	{
		return !smi.prefabID.HasTag(GameTags.Creatures.Overcrowded) && !smi.prefabID.HasTag(GameTags.Creatures.TrappedInCargoBay);
	}

	// Token: 0x04001DC6 RID: 7622
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_off;

	// Token: 0x04001DC7 RID: 7623
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_off;

	// Token: 0x04001DC8 RID: 7624
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_on;

	// Token: 0x04001DC9 RID: 7625
	private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_on;

	// Token: 0x02001470 RID: 5232
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001471 RID: 5233
	public new class Instance : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.GameInstance
	{
		// Token: 0x0600849D RID: 33949 RVA: 0x00302E24 File Offset: 0x00301024
		public Instance(IStateMachineTarget master, CreatureLightToggleController.Def def) : base(master, def)
		{
			this.prefabID = base.gameObject.GetComponent<KPrefabID>();
			this.light = master.GetComponent<Light2D>();
			this.originalLux = this.light.Lux;
			this.originalRange = this.light.Range;
		}

		// Token: 0x0600849E RID: 33950 RVA: 0x00302E78 File Offset: 0x00301078
		public void SwitchLight(bool on)
		{
			this.light.enabled = on;
		}

		// Token: 0x0600849F RID: 33951 RVA: 0x00302E88 File Offset: 0x00301088
		public static void ModifyBrightness(List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances, CreatureLightToggleController.Instance.ModifyLuxDelegate modify_lux, float time_delta)
		{
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
			for (int num = 0; num != instances.Count; num++)
			{
				UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry entry = instances[num];
				entry.lastUpdateTime = 0f;
				instances[num] = entry;
				CreatureLightToggleController.Instance data = entry.data;
				modify_lux(data, time_delta);
				data.light.Range = data.originalRange * (float)data.light.Lux / (float)data.originalLux;
				data.light.RefreshShapeAndPosition();
				if (data.light.RefreshShapeAndPosition() != Light2D.RefreshResult.None)
				{
					CreatureLightToggleController.Instance.modify_brightness_job.Add(new CreatureLightToggleController.Instance.ModifyBrightnessTask(data.light.emitter));
				}
			}
			GlobalJobManager.Run(CreatureLightToggleController.Instance.modify_brightness_job);
			for (int num2 = 0; num2 != CreatureLightToggleController.Instance.modify_brightness_job.Count; num2++)
			{
				CreatureLightToggleController.Instance.modify_brightness_job.GetWorkItem(num2).Finish();
			}
			CreatureLightToggleController.Instance.modify_brightness_job.Reset(null);
		}

		// Token: 0x060084A0 RID: 33952 RVA: 0x00302F79 File Offset: 0x00301179
		public bool IsOff()
		{
			return this.light.Lux == 0;
		}

		// Token: 0x060084A1 RID: 33953 RVA: 0x00302F89 File Offset: 0x00301189
		public bool IsOn()
		{
			return this.light.Lux >= this.originalLux;
		}

		// Token: 0x04006569 RID: 25961
		private const float DIM_TIME = 25f;

		// Token: 0x0400656A RID: 25962
		private const float GLOW_TIME = 15f;

		// Token: 0x0400656B RID: 25963
		private int originalLux;

		// Token: 0x0400656C RID: 25964
		private float originalRange;

		// Token: 0x0400656D RID: 25965
		private Light2D light;

		// Token: 0x0400656E RID: 25966
		public KPrefabID prefabID;

		// Token: 0x0400656F RID: 25967
		private static WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object> modify_brightness_job = new WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object>();

		// Token: 0x04006570 RID: 25968
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate dim = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 25f;
			instance.light.Lux = Mathf.FloorToInt(Mathf.Max(0f, (float)instance.light.Lux - num * time_delta));
		};

		// Token: 0x04006571 RID: 25969
		public static CreatureLightToggleController.Instance.ModifyLuxDelegate brighten = delegate(CreatureLightToggleController.Instance instance, float time_delta)
		{
			float num = (float)instance.originalLux / 15f;
			instance.light.Lux = Mathf.CeilToInt(Mathf.Min((float)instance.originalLux, (float)instance.light.Lux + num * time_delta));
		};

		// Token: 0x02002162 RID: 8546
		private struct ModifyBrightnessTask : IWorkItem<object>
		{
			// Token: 0x0600AA13 RID: 43539 RVA: 0x00373E6C File Offset: 0x0037206C
			public ModifyBrightnessTask(LightGridManager.LightGridEmitter emitter)
			{
				this.emitter = emitter;
				emitter.RemoveFromGrid();
			}

			// Token: 0x0600AA14 RID: 43540 RVA: 0x00373E7B File Offset: 0x0037207B
			public void Run(object context)
			{
				this.emitter.UpdateLitCells();
			}

			// Token: 0x0600AA15 RID: 43541 RVA: 0x00373E88 File Offset: 0x00372088
			public void Finish()
			{
				this.emitter.AddToGrid(false);
			}

			// Token: 0x04009586 RID: 38278
			private LightGridManager.LightGridEmitter emitter;
		}

		// Token: 0x02002163 RID: 8547
		// (Invoke) Token: 0x0600AA17 RID: 43543
		public delegate void ModifyLuxDelegate(CreatureLightToggleController.Instance instance, float time_delta);
	}
}
