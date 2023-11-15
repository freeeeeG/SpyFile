using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200039C RID: 924
[AddComponentMenu("KMonoBehaviour/scripts/BrainScheduler")]
public class BrainScheduler : KMonoBehaviour, IRenderEveryTick, ICPULoad
{
	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06001352 RID: 4946 RVA: 0x000656F0 File Offset: 0x000638F0
	private bool isAsyncPathProbeEnabled
	{
		get
		{
			return !TuningData<BrainScheduler.Tuning>.Get().disableAsyncPathProbes;
		}
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x00065700 File Offset: 0x00063900
	protected override void OnPrefabInit()
	{
		this.brainGroups.Add(new BrainScheduler.DupeBrainGroup());
		this.brainGroups.Add(new BrainScheduler.CreatureBrainGroup());
		Components.Brains.Register(new Action<Brain>(this.OnAddBrain), new Action<Brain>(this.OnRemoveBrain));
		CPUBudget.AddRoot(this);
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			CPUBudget.AddChild(this, brainGroup, brainGroup.LoadBalanceThreshold());
		}
		CPUBudget.FinalizeChildren(this);
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x000657A8 File Offset: 0x000639A8
	private void OnAddBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.AddBrain(brain);
				test = true;
			}
			Navigator component = brain.GetComponent<Navigator>();
			if (component != null)
			{
				component.executePathProbeTaskAsync = this.isAsyncPathProbeEnabled;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x00065830 File Offset: 0x00063A30
	private void OnRemoveBrain(Brain brain)
	{
		bool test = false;
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				test = true;
				brainGroup.RemoveBrain(brain);
			}
			Navigator component = brain.GetComponent<Navigator>();
			if (component != null)
			{
				component.executePathProbeTaskAsync = false;
			}
		}
		DebugUtil.Assert(test);
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x000658B4 File Offset: 0x00063AB4
	public void PrioritizeBrain(Brain brain)
	{
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			if (brain.HasTag(brainGroup.tag))
			{
				brainGroup.PrioritizeBrain(brain);
			}
		}
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x00065918 File Offset: 0x00063B18
	public float GetEstimatedFrameTime()
	{
		return TuningData<BrainScheduler.Tuning>.Get().frameTime;
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x00065924 File Offset: 0x00063B24
	public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
	{
		return false;
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x00065928 File Offset: 0x00063B28
	public void RenderEveryTick(float dt)
	{
		if (Game.IsQuitting() || KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		foreach (BrainScheduler.BrainGroup brainGroup in this.brainGroups)
		{
			brainGroup.RenderEveryTick(dt, this.isAsyncPathProbeEnabled);
		}
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x00065990 File Offset: 0x00063B90
	protected override void OnForcedCleanUp()
	{
		CPUBudget.Remove(this);
		base.OnForcedCleanUp();
	}

	// Token: 0x04000A5C RID: 2652
	public const float millisecondsPerFrame = 33.33333f;

	// Token: 0x04000A5D RID: 2653
	public const float secondsPerFrame = 0.033333328f;

	// Token: 0x04000A5E RID: 2654
	public const float framesPerSecond = 30.000006f;

	// Token: 0x04000A5F RID: 2655
	private List<BrainScheduler.BrainGroup> brainGroups = new List<BrainScheduler.BrainGroup>();

	// Token: 0x02000FCF RID: 4047
	private class Tuning : TuningData<BrainScheduler.Tuning>
	{
		// Token: 0x040056CC RID: 22220
		public bool disableAsyncPathProbes;

		// Token: 0x040056CD RID: 22221
		public float frameTime = 5f;
	}

	// Token: 0x02000FD0 RID: 4048
	private abstract class BrainGroup : ICPULoad
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06007341 RID: 29505 RVA: 0x002C1FAB File Offset: 0x002C01AB
		// (set) Token: 0x06007342 RID: 29506 RVA: 0x002C1FB3 File Offset: 0x002C01B3
		public Tag tag { get; private set; }

		// Token: 0x06007343 RID: 29507 RVA: 0x002C1FBC File Offset: 0x002C01BC
		protected BrainGroup(Tag tag)
		{
			this.tag = tag;
			this.probeSize = this.InitialProbeSize();
			this.probeCount = this.InitialProbeCount();
			string str = tag.ToString();
			this.increaseLoadLabel = "IncLoad" + str;
			this.decreaseLoadLabel = "DecLoad" + str;
		}

		// Token: 0x06007344 RID: 29508 RVA: 0x002C203F File Offset: 0x002C023F
		public void AddBrain(Brain brain)
		{
			this.brains.Add(brain);
		}

		// Token: 0x06007345 RID: 29509 RVA: 0x002C2050 File Offset: 0x002C0250
		public void RemoveBrain(Brain brain)
		{
			int num = this.brains.IndexOf(brain);
			if (num != -1)
			{
				this.brains.RemoveAt(num);
				this.OnRemoveBrain(num, ref this.nextUpdateBrain);
				this.OnRemoveBrain(num, ref this.nextPathProbeBrain);
			}
			if (this.priorityBrains.Contains(brain))
			{
				List<Brain> list = new List<Brain>(this.priorityBrains);
				list.Remove(brain);
				this.priorityBrains = new Queue<Brain>(list);
			}
		}

		// Token: 0x06007346 RID: 29510 RVA: 0x002C20C2 File Offset: 0x002C02C2
		public void PrioritizeBrain(Brain brain)
		{
			this.priorityBrains.Enqueue(brain);
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06007347 RID: 29511 RVA: 0x002C20D0 File Offset: 0x002C02D0
		// (set) Token: 0x06007348 RID: 29512 RVA: 0x002C20D8 File Offset: 0x002C02D8
		public int probeSize { get; private set; }

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06007349 RID: 29513 RVA: 0x002C20E1 File Offset: 0x002C02E1
		// (set) Token: 0x0600734A RID: 29514 RVA: 0x002C20E9 File Offset: 0x002C02E9
		public int probeCount { get; private set; }

		// Token: 0x0600734B RID: 29515 RVA: 0x002C20F4 File Offset: 0x002C02F4
		public bool AdjustLoad(float currentFrameTime, float frameTimeDelta)
		{
			bool flag = frameTimeDelta > 0f;
			int num = 0;
			int num2 = Math.Max(this.probeCount, Math.Min(this.brains.Count, CPUBudget.coreCount));
			num += num2 - this.probeCount;
			this.probeCount = num2;
			float num3 = Math.Min(1f, (float)this.probeCount / (float)CPUBudget.coreCount);
			float num4 = num3 * (float)this.probeSize;
			float num5 = num3 * (float)this.probeSize;
			float num6 = currentFrameTime / num5;
			float num7 = frameTimeDelta / num6;
			if (num == 0)
			{
				float num8 = num4 + num7 / (float)CPUBudget.coreCount;
				int num9 = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int)(num8 / num3));
				num += num9 - this.probeSize;
				this.probeSize = num9;
			}
			if (num == 0)
			{
				int num10 = Math.Max(1, (int)num3 + (flag ? 1 : -1));
				int probeSize = MathUtil.Clamp(this.MinProbeSize(), this.IdealProbeSize(), (int)((num5 + num7) / (float)num10));
				int num11 = Math.Min(this.brains.Count, num10 * CPUBudget.coreCount);
				num += num11 - this.probeCount;
				this.probeCount = num11;
				this.probeSize = probeSize;
			}
			if (num == 0 && flag)
			{
				int num12 = this.probeSize + this.ProbeSizeStep();
				num += num12 - this.probeSize;
				this.probeSize = num12;
			}
			if (num >= 0 && num <= 0 && this.brains.Count > 0)
			{
				global::Debug.LogWarning("AdjustLoad() failed");
			}
			return num != 0;
		}

		// Token: 0x0600734C RID: 29516 RVA: 0x002C226E File Offset: 0x002C046E
		private void IncrementBrainIndex(ref int brainIndex)
		{
			brainIndex++;
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x0600734D RID: 29517 RVA: 0x002C2288 File Offset: 0x002C0488
		private void ClampBrainIndex(ref int brainIndex)
		{
			brainIndex = MathUtil.Clamp(0, this.brains.Count - 1, brainIndex);
		}

		// Token: 0x0600734E RID: 29518 RVA: 0x002C22A1 File Offset: 0x002C04A1
		private void OnRemoveBrain(int removedIndex, ref int brainIndex)
		{
			if (removedIndex < brainIndex)
			{
				brainIndex--;
				return;
			}
			if (brainIndex == this.brains.Count)
			{
				brainIndex = 0;
			}
		}

		// Token: 0x0600734F RID: 29519 RVA: 0x002C22C4 File Offset: 0x002C04C4
		private void AsyncPathProbe()
		{
			this.pathProbeJob.Reset(null);
			for (int num = 0; num != this.brains.Count; num++)
			{
				this.ClampBrainIndex(ref this.nextPathProbeBrain);
				Brain brain = this.brains[this.nextPathProbeBrain];
				if (brain.IsRunning())
				{
					Navigator component = brain.GetComponent<Navigator>();
					if (component != null)
					{
						component.executePathProbeTaskAsync = true;
						component.PathProber.potentialCellsPerUpdate = this.probeSize;
						component.pathProbeTask.Update();
						this.pathProbeJob.Add(component.pathProbeTask);
						if (this.pathProbeJob.Count == this.probeCount)
						{
							break;
						}
					}
				}
				this.IncrementBrainIndex(ref this.nextPathProbeBrain);
			}
			CPUBudget.Start(this);
			GlobalJobManager.Run(this.pathProbeJob);
			CPUBudget.End(this);
		}

		// Token: 0x06007350 RID: 29520 RVA: 0x002C239C File Offset: 0x002C059C
		public void RenderEveryTick(float dt, bool isAsyncPathProbeEnabled)
		{
			if (isAsyncPathProbeEnabled)
			{
				this.AsyncPathProbe();
			}
			int num = this.InitialProbeCount();
			int num2 = 0;
			while (num2 != this.brains.Count && num != 0)
			{
				this.ClampBrainIndex(ref this.nextUpdateBrain);
				Brain brain;
				if (this.priorityBrains.Count > 0)
				{
					brain = this.priorityBrains.Dequeue();
				}
				else
				{
					brain = this.brains[this.nextUpdateBrain];
					this.IncrementBrainIndex(ref this.nextUpdateBrain);
				}
				if (brain.IsRunning())
				{
					brain.UpdateBrain();
					num--;
				}
				num2++;
			}
		}

		// Token: 0x06007351 RID: 29521 RVA: 0x002C242C File Offset: 0x002C062C
		public void AccumulatePathProbeIterations(Dictionary<string, int> pathProbeIterations)
		{
			foreach (Brain brain in this.brains)
			{
				Navigator component = brain.GetComponent<Navigator>();
				if (!(component == null) && !pathProbeIterations.ContainsKey(brain.name))
				{
					pathProbeIterations.Add(brain.name, component.PathProber.updateCount);
				}
			}
		}

		// Token: 0x06007352 RID: 29522
		protected abstract int InitialProbeCount();

		// Token: 0x06007353 RID: 29523
		protected abstract int InitialProbeSize();

		// Token: 0x06007354 RID: 29524
		protected abstract int MinProbeSize();

		// Token: 0x06007355 RID: 29525
		protected abstract int IdealProbeSize();

		// Token: 0x06007356 RID: 29526
		protected abstract int ProbeSizeStep();

		// Token: 0x06007357 RID: 29527
		public abstract float GetEstimatedFrameTime();

		// Token: 0x06007358 RID: 29528
		public abstract float LoadBalanceThreshold();

		// Token: 0x040056CF RID: 22223
		private List<Brain> brains = new List<Brain>();

		// Token: 0x040056D0 RID: 22224
		private Queue<Brain> priorityBrains = new Queue<Brain>();

		// Token: 0x040056D1 RID: 22225
		private string increaseLoadLabel;

		// Token: 0x040056D2 RID: 22226
		private string decreaseLoadLabel;

		// Token: 0x040056D3 RID: 22227
		private WorkItemCollection<Navigator.PathProbeTask, object> pathProbeJob = new WorkItemCollection<Navigator.PathProbeTask, object>();

		// Token: 0x040056D4 RID: 22228
		private int nextUpdateBrain;

		// Token: 0x040056D5 RID: 22229
		private int nextPathProbeBrain;
	}

	// Token: 0x02000FD1 RID: 4049
	private class DupeBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x06007359 RID: 29529 RVA: 0x002C24B0 File Offset: 0x002C06B0
		public DupeBrainGroup() : base(GameTags.DupeBrain)
		{
		}

		// Token: 0x0600735A RID: 29530 RVA: 0x002C24BD File Offset: 0x002C06BD
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x0600735B RID: 29531 RVA: 0x002C24C9 File Offset: 0x002C06C9
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x0600735C RID: 29532 RVA: 0x002C24D5 File Offset: 0x002C06D5
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x0600735D RID: 29533 RVA: 0x002C24E1 File Offset: 0x002C06E1
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x0600735E RID: 29534 RVA: 0x002C24ED File Offset: 0x002C06ED
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x0600735F RID: 29535 RVA: 0x002C24F9 File Offset: 0x002C06F9
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x06007360 RID: 29536 RVA: 0x002C2505 File Offset: 0x002C0705
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.DupeBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x02001FBC RID: 8124
		public class Tuning : TuningData<BrainScheduler.DupeBrainGroup.Tuning>
		{
			// Token: 0x04008EDE RID: 36574
			public int initialProbeCount = 1;

			// Token: 0x04008EDF RID: 36575
			public int initialProbeSize = 1000;

			// Token: 0x04008EE0 RID: 36576
			public int minProbeSize = 100;

			// Token: 0x04008EE1 RID: 36577
			public int idealProbeSize = 1000;

			// Token: 0x04008EE2 RID: 36578
			public int probeSizeStep = 100;

			// Token: 0x04008EE3 RID: 36579
			public float estimatedFrameTime = 2f;

			// Token: 0x04008EE4 RID: 36580
			public float loadBalanceThreshold = 0.1f;
		}
	}

	// Token: 0x02000FD2 RID: 4050
	private class CreatureBrainGroup : BrainScheduler.BrainGroup
	{
		// Token: 0x06007361 RID: 29537 RVA: 0x002C2511 File Offset: 0x002C0711
		public CreatureBrainGroup() : base(GameTags.CreatureBrain)
		{
		}

		// Token: 0x06007362 RID: 29538 RVA: 0x002C251E File Offset: 0x002C071E
		protected override int InitialProbeCount()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeCount;
		}

		// Token: 0x06007363 RID: 29539 RVA: 0x002C252A File Offset: 0x002C072A
		protected override int InitialProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().initialProbeSize;
		}

		// Token: 0x06007364 RID: 29540 RVA: 0x002C2536 File Offset: 0x002C0736
		protected override int MinProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().minProbeSize;
		}

		// Token: 0x06007365 RID: 29541 RVA: 0x002C2542 File Offset: 0x002C0742
		protected override int IdealProbeSize()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().idealProbeSize;
		}

		// Token: 0x06007366 RID: 29542 RVA: 0x002C254E File Offset: 0x002C074E
		protected override int ProbeSizeStep()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().probeSizeStep;
		}

		// Token: 0x06007367 RID: 29543 RVA: 0x002C255A File Offset: 0x002C075A
		public override float GetEstimatedFrameTime()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().estimatedFrameTime;
		}

		// Token: 0x06007368 RID: 29544 RVA: 0x002C2566 File Offset: 0x002C0766
		public override float LoadBalanceThreshold()
		{
			return TuningData<BrainScheduler.CreatureBrainGroup.Tuning>.Get().loadBalanceThreshold;
		}

		// Token: 0x02001FBD RID: 8125
		public class Tuning : TuningData<BrainScheduler.CreatureBrainGroup.Tuning>
		{
			// Token: 0x04008EE5 RID: 36581
			public int initialProbeCount = 5;

			// Token: 0x04008EE6 RID: 36582
			public int initialProbeSize = 1000;

			// Token: 0x04008EE7 RID: 36583
			public int minProbeSize = 100;

			// Token: 0x04008EE8 RID: 36584
			public int idealProbeSize = 300;

			// Token: 0x04008EE9 RID: 36585
			public int probeSizeStep = 100;

			// Token: 0x04008EEA RID: 36586
			public float estimatedFrameTime = 1f;

			// Token: 0x04008EEB RID: 36587
			public float loadBalanceThreshold = 0.1f;
		}
	}
}
