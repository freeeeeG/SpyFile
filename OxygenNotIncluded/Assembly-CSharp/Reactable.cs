using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040F RID: 1039
public abstract class Reactable
{
	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060015E6 RID: 5606 RVA: 0x000737E6 File Offset: 0x000719E6
	public bool IsValid
	{
		get
		{
			return this.partitionerEntry.IsValid();
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060015E7 RID: 5607 RVA: 0x000737F3 File Offset: 0x000719F3
	// (set) Token: 0x060015E8 RID: 5608 RVA: 0x000737FB File Offset: 0x000719FB
	public float creationTime { get; private set; }

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060015E9 RID: 5609 RVA: 0x00073804 File Offset: 0x00071A04
	public bool IsReacting
	{
		get
		{
			return this.reactor != null;
		}
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x00073814 File Offset: 0x00071A14
	public Reactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, bool follow_transform = false, float globalCooldown = 0f, float localCooldown = 0f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f, ObjectLayer overrideLayer = ObjectLayer.NumLayers)
	{
		this.rangeHeight = range_height;
		this.rangeWidth = range_width;
		this.id = id;
		this.gameObject = gameObject;
		this.choreType = chore_type;
		this.globalCooldown = globalCooldown;
		this.localCooldown = localCooldown;
		this.lifeSpan = lifeSpan;
		this.initialDelay = ((max_initial_delay > 0f) ? UnityEngine.Random.Range(0f, max_initial_delay) : 0f);
		this.creationTime = GameClock.Instance.GetTime();
		ObjectLayer objectLayer = (overrideLayer == ObjectLayer.NumLayers) ? this.reactionLayer : overrideLayer;
		ReactionMonitor.Def def = gameObject.GetDef<ReactionMonitor.Def>();
		if (overrideLayer != objectLayer && def != null)
		{
			objectLayer = def.ReactionLayer;
		}
		this.reactionLayer = objectLayer;
		this.Initialize(follow_transform);
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x000738F0 File Offset: 0x00071AF0
	public void Initialize(bool followTransform)
	{
		this.UpdateLocation();
		if (followTransform)
		{
			this.transformId = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.gameObject.transform, new System.Action(this.UpdateLocation), "Reactable follow transform");
		}
	}

	// Token: 0x060015EC RID: 5612 RVA: 0x00073927 File Offset: 0x00071B27
	public void Begin(GameObject reactor)
	{
		this.reactor = reactor;
		this.lastTriggerTime = GameClock.Instance.GetTime();
		this.InternalBegin();
	}

	// Token: 0x060015ED RID: 5613 RVA: 0x00073948 File Offset: 0x00071B48
	public void End()
	{
		this.InternalEnd();
		if (this.reactor != null)
		{
			GameObject gameObject = this.reactor;
			this.InternalEnd();
			this.reactor = null;
			if (gameObject != null)
			{
				ReactionMonitor.Instance smi = gameObject.GetSMI<ReactionMonitor.Instance>();
				if (smi != null)
				{
					smi.StopReaction();
				}
			}
		}
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x00073998 File Offset: 0x00071B98
	public bool CanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		float time = GameClock.Instance.GetTime();
		float num = time - this.creationTime;
		float num2 = time - this.lastTriggerTime;
		if (num < this.initialDelay || num2 < this.globalCooldown)
		{
			return false;
		}
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		Chore chore = (component != null) ? component.choreDriver.GetCurrentChore() : null;
		if (chore == null || this.choreType.priority <= chore.choreType.priority)
		{
			return false;
		}
		int num3 = 0;
		while (this.additionalPreconditions != null && num3 < this.additionalPreconditions.Count)
		{
			if (!this.additionalPreconditions[num3](reactor, transition))
			{
				return false;
			}
			num3++;
		}
		return this.InternalCanBegin(reactor, transition);
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x00073A52 File Offset: 0x00071C52
	public bool IsExpired()
	{
		return GameClock.Instance.GetTime() - this.creationTime > this.lifeSpan;
	}

	// Token: 0x060015F0 RID: 5616
	public abstract bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition);

	// Token: 0x060015F1 RID: 5617
	public abstract void Update(float dt);

	// Token: 0x060015F2 RID: 5618
	protected abstract void InternalBegin();

	// Token: 0x060015F3 RID: 5619
	protected abstract void InternalEnd();

	// Token: 0x060015F4 RID: 5620
	protected abstract void InternalCleanup();

	// Token: 0x060015F5 RID: 5621 RVA: 0x00073A70 File Offset: 0x00071C70
	public void Cleanup()
	{
		this.End();
		this.InternalCleanup();
		if (this.transformId != -1)
		{
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transformId, new System.Action(this.UpdateLocation));
			this.transformId = -1;
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x00073AC8 File Offset: 0x00071CC8
	private void UpdateLocation()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.gameObject != null)
		{
			this.sourceCell = Grid.PosToCell(this.gameObject);
			Extents extents = new Extents(Grid.PosToXY(this.gameObject.transform.GetPosition()).x - this.rangeWidth / 2, Grid.PosToXY(this.gameObject.transform.GetPosition()).y - this.rangeHeight / 2, this.rangeWidth, this.rangeHeight);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("Reactable", this, extents, GameScenePartitioner.Instance.objectLayers[(int)this.reactionLayer], null);
		}
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x00073B89 File Offset: 0x00071D89
	public Reactable AddPrecondition(Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		this.additionalPreconditions.Add(precondition);
		return this;
	}

	// Token: 0x060015F8 RID: 5624 RVA: 0x00073BAB File Offset: 0x00071DAB
	public void InsertPrecondition(int index, Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		index = Math.Min(index, this.additionalPreconditions.Count);
		this.additionalPreconditions.Insert(index, precondition);
	}

	// Token: 0x04000C39 RID: 3129
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04000C3A RID: 3130
	protected GameObject gameObject;

	// Token: 0x04000C3B RID: 3131
	public HashedString id;

	// Token: 0x04000C3C RID: 3132
	public bool preventChoreInterruption = true;

	// Token: 0x04000C3D RID: 3133
	public int sourceCell;

	// Token: 0x04000C3E RID: 3134
	private int rangeWidth;

	// Token: 0x04000C3F RID: 3135
	private int rangeHeight;

	// Token: 0x04000C40 RID: 3136
	private int transformId = -1;

	// Token: 0x04000C41 RID: 3137
	public float globalCooldown;

	// Token: 0x04000C42 RID: 3138
	public float localCooldown;

	// Token: 0x04000C43 RID: 3139
	public float lifeSpan = float.PositiveInfinity;

	// Token: 0x04000C44 RID: 3140
	private float lastTriggerTime = -2.1474836E+09f;

	// Token: 0x04000C45 RID: 3141
	private float initialDelay;

	// Token: 0x04000C47 RID: 3143
	protected GameObject reactor;

	// Token: 0x04000C48 RID: 3144
	private ChoreType choreType;

	// Token: 0x04000C49 RID: 3145
	protected LoggerFSS log;

	// Token: 0x04000C4A RID: 3146
	private List<Reactable.ReactablePrecondition> additionalPreconditions;

	// Token: 0x04000C4B RID: 3147
	private ObjectLayer reactionLayer;

	// Token: 0x0200108F RID: 4239
	// (Invoke) Token: 0x06007608 RID: 30216
	public delegate bool ReactablePrecondition(GameObject go, Navigator.ActiveTransition transition);
}
