using System;
using System.Collections.Generic;

// Token: 0x020006CC RID: 1740
public class CheckedHandleVector<T> where T : new()
{
	// Token: 0x06002F4E RID: 12110 RVA: 0x000F99C8 File Offset: 0x000F7BC8
	public CheckedHandleVector(int initial_size)
	{
		this.handleVector = new HandleVector<T>(initial_size);
		this.isFree = new List<bool>(initial_size);
		for (int i = 0; i < initial_size; i++)
		{
			this.isFree.Add(true);
		}
	}

	// Token: 0x06002F4F RID: 12111 RVA: 0x000F9A18 File Offset: 0x000F7C18
	public HandleVector<T>.Handle Add(T item, string debug_info)
	{
		HandleVector<T>.Handle result = this.handleVector.Add(item);
		if (result.index >= this.isFree.Count)
		{
			this.isFree.Add(false);
		}
		else
		{
			this.isFree[result.index] = false;
		}
		int i = this.handleVector.Items.Count;
		while (i > this.debugInfo.Count)
		{
			this.debugInfo.Add(null);
		}
		this.debugInfo[result.index] = debug_info;
		return result;
	}

	// Token: 0x06002F50 RID: 12112 RVA: 0x000F9AA8 File Offset: 0x000F7CA8
	public T Release(HandleVector<T>.Handle handle)
	{
		if (this.isFree[handle.index])
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Tried to double free checked handle ",
				handle.index,
				"- Debug info:",
				this.debugInfo[handle.index]
			});
		}
		this.isFree[handle.index] = true;
		return this.handleVector.Release(handle);
	}

	// Token: 0x06002F51 RID: 12113 RVA: 0x000F9B27 File Offset: 0x000F7D27
	public T Get(HandleVector<T>.Handle handle)
	{
		return this.handleVector.GetItem(handle);
	}

	// Token: 0x04001C21 RID: 7201
	private HandleVector<T> handleVector;

	// Token: 0x04001C22 RID: 7202
	private List<string> debugInfo = new List<string>();

	// Token: 0x04001C23 RID: 7203
	private List<bool> isFree;
}
