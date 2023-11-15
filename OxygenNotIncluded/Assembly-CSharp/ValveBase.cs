using System;
using KSerialization;
using UnityEngine;

// Token: 0x020006B4 RID: 1716
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ValveBase")]
public class ValveBase : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06002EAD RID: 11949 RVA: 0x000F68DB File Offset: 0x000F4ADB
	// (set) Token: 0x06002EAC RID: 11948 RVA: 0x000F68D2 File Offset: 0x000F4AD2
	public float CurrentFlow
	{
		get
		{
			return this.currentFlow;
		}
		set
		{
			this.currentFlow = value;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06002EAE RID: 11950 RVA: 0x000F68E3 File Offset: 0x000F4AE3
	public HandleVector<int>.Handle AccumulatorHandle
	{
		get
		{
			return this.flowAccumulator;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06002EAF RID: 11951 RVA: 0x000F68EB File Offset: 0x000F4AEB
	public float MaxFlow
	{
		get
		{
			return this.maxFlow;
		}
	}

	// Token: 0x06002EB0 RID: 11952 RVA: 0x000F68F3 File Offset: 0x000F4AF3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.flowAccumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	// Token: 0x06002EB1 RID: 11953 RVA: 0x000F6918 File Offset: 0x000F4B18
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.UpdateAnim();
		this.OnCmpEnable();
	}

	// Token: 0x06002EB2 RID: 11954 RVA: 0x000F6973 File Offset: 0x000F4B73
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.flowAccumulator);
		Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06002EB3 RID: 11955 RVA: 0x000F69B0 File Offset: 0x000F4BB0
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.conduitType);
		ConduitFlow.Conduit conduit = flowManager.GetConduit(this.inputCell);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			this.OnMassTransfer(0f);
			this.UpdateAnim();
			return;
		}
		ConduitFlow.ConduitContents contents = conduit.GetContents(flowManager);
		float num = Mathf.Min(contents.mass, this.currentFlow * dt);
		float num2 = 0f;
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			Game.Instance.accumulators.Accumulate(this.flowAccumulator, num2);
			if (num2 > 0f)
			{
				flowManager.RemoveElement(this.inputCell, num2);
			}
		}
		this.OnMassTransfer(num2);
		this.UpdateAnim();
	}

	// Token: 0x06002EB4 RID: 11956 RVA: 0x000F6AA5 File Offset: 0x000F4CA5
	protected virtual void OnMassTransfer(float amount)
	{
	}

	// Token: 0x06002EB5 RID: 11957 RVA: 0x000F6AA8 File Offset: 0x000F4CA8
	public virtual void UpdateAnim()
	{
		float averageRate = Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
		if (averageRate > 0f)
		{
			int i = 0;
			while (i < this.animFlowRanges.Length)
			{
				if (averageRate <= this.animFlowRanges[i].minFlow)
				{
					if (this.curFlowIdx != i)
					{
						this.curFlowIdx = i;
						this.controller.Play(this.animFlowRanges[i].animName, (averageRate <= 0f) ? KAnim.PlayMode.Once : KAnim.PlayMode.Loop, 1f, 0f);
						return;
					}
					return;
				}
				else
				{
					i++;
				}
			}
			return;
		}
		this.controller.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04001B9E RID: 7070
	[SerializeField]
	public ConduitType conduitType;

	// Token: 0x04001B9F RID: 7071
	[SerializeField]
	public float maxFlow = 0.5f;

	// Token: 0x04001BA0 RID: 7072
	[Serialize]
	private float currentFlow;

	// Token: 0x04001BA1 RID: 7073
	[MyCmpGet]
	protected KBatchedAnimController controller;

	// Token: 0x04001BA2 RID: 7074
	protected HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001BA3 RID: 7075
	private int curFlowIdx = -1;

	// Token: 0x04001BA4 RID: 7076
	private int inputCell;

	// Token: 0x04001BA5 RID: 7077
	private int outputCell;

	// Token: 0x04001BA6 RID: 7078
	[SerializeField]
	public ValveBase.AnimRangeInfo[] animFlowRanges;

	// Token: 0x020013F1 RID: 5105
	[Serializable]
	public struct AnimRangeInfo
	{
		// Token: 0x060082E5 RID: 33509 RVA: 0x002FB499 File Offset: 0x002F9699
		public AnimRangeInfo(float min_flow, string anim_name)
		{
			this.minFlow = min_flow;
			this.animName = anim_name;
		}

		// Token: 0x040063D8 RID: 25560
		public float minFlow;

		// Token: 0x040063D9 RID: 25561
		public string animName;
	}
}
