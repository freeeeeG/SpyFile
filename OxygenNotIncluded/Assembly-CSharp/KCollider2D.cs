using System;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
public abstract class KCollider2D : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06001BB5 RID: 7093 RVA: 0x000943F0 File Offset: 0x000925F0
	// (set) Token: 0x06001BB6 RID: 7094 RVA: 0x000943F8 File Offset: 0x000925F8
	public Vector2 offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			this._offset = value;
			this.MarkDirty(false);
		}
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x00094408 File Offset: 0x00092608
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x00094417 File Offset: 0x00092617
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(base.transform, new Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
		this.MarkDirty(true);
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x00094442 File Offset: 0x00092642
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(base.transform, new Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x00094478 File Offset: 0x00092678
	public void MarkDirty(bool force = false)
	{
		bool flag = force || this.partitionerEntry.IsValid();
		if (!flag)
		{
			return;
		}
		Extents extents = this.GetExtents();
		if (!force && this.cachedExtents.x == extents.x && this.cachedExtents.y == extents.y && this.cachedExtents.width == extents.width && this.cachedExtents.height == extents.height)
		{
			return;
		}
		this.cachedExtents = extents;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (flag)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add(base.name, this, this.cachedExtents, GameScenePartitioner.Instance.collisionLayer, null);
		}
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x00094534 File Offset: 0x00092734
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving)
		{
			this.MarkDirty(false);
			SimAndRenderScheduler.instance.Add(this, false);
			return;
		}
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x00094558 File Offset: 0x00092758
	private static void OnMovementStateChanged(Transform transform, bool is_moving)
	{
		transform.GetComponent<KCollider2D>().OnMovementStateChanged(is_moving);
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x00094566 File Offset: 0x00092766
	public void RenderEveryTick(float dt)
	{
		this.MarkDirty(false);
	}

	// Token: 0x06001BBE RID: 7102
	public abstract bool Intersects(Vector2 pos);

	// Token: 0x06001BBF RID: 7103
	public abstract Extents GetExtents();

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06001BC0 RID: 7104
	public abstract Bounds bounds { get; }

	// Token: 0x04000F63 RID: 3939
	[SerializeField]
	public Vector2 _offset;

	// Token: 0x04000F64 RID: 3940
	private Extents cachedExtents;

	// Token: 0x04000F65 RID: 3941
	private HandleVector<int>.Handle partitionerEntry;
}
