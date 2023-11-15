using System;

// Token: 0x02000958 RID: 2392
public class ScenePartitionerEntry
{
	// Token: 0x0600464C RID: 17996 RVA: 0x0018DCE4 File Offset: 0x0018BEE4
	public ScenePartitionerEntry(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, ScenePartitioner partitioner, Action<object> event_callback)
	{
		if (x < 0 || y < 0 || width >= 0)
		{
		}
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.layer = layer.layer;
		this.partitioner = partitioner;
		this.eventCallback = event_callback;
		this.obj = obj;
	}

	// Token: 0x0600464D RID: 17997 RVA: 0x0018DD4D File Offset: 0x0018BF4D
	public void UpdatePosition(int x, int y)
	{
		this.partitioner.UpdatePosition(x, y, this);
	}

	// Token: 0x0600464E RID: 17998 RVA: 0x0018DD5D File Offset: 0x0018BF5D
	public void UpdatePosition(Extents e)
	{
		this.partitioner.UpdatePosition(e, this);
	}

	// Token: 0x0600464F RID: 17999 RVA: 0x0018DD6C File Offset: 0x0018BF6C
	public void Release()
	{
		if (this.partitioner != null)
		{
			this.partitioner.Remove(this);
		}
	}

	// Token: 0x04002E91 RID: 11921
	public int x;

	// Token: 0x04002E92 RID: 11922
	public int y;

	// Token: 0x04002E93 RID: 11923
	public int width;

	// Token: 0x04002E94 RID: 11924
	public int height;

	// Token: 0x04002E95 RID: 11925
	public int layer;

	// Token: 0x04002E96 RID: 11926
	public int queryId;

	// Token: 0x04002E97 RID: 11927
	public ScenePartitioner partitioner;

	// Token: 0x04002E98 RID: 11928
	public Action<object> eventCallback;

	// Token: 0x04002E99 RID: 11929
	public object obj;
}
