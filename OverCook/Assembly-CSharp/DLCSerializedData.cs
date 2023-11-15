using System;
using UnityEngine;

// Token: 0x020006F7 RID: 1783
[Serializable]
public class DLCSerializedData<T>
{
	// Token: 0x1700029A RID: 666
	// (get) Token: 0x060021CB RID: 8651 RVA: 0x0005EB6C File Offset: 0x0005CF6C
	public T[] AllData
	{
		get
		{
			return new T[]
			{
				this.m_Data,
				this.m_DLC02Data,
				this.m_DLC03Data,
				this.m_DLC04Data,
				this.m_DLC05Data,
				this.m_DLC06Data,
				this.m_DLC07Data,
				this.m_DLC08Data,
				this.m_DLC09Data,
				this.m_DLC10Data,
				this.m_DLC11Data,
				this.m_DLC13Data
			};
		}
	}

	// Token: 0x04001A05 RID: 6661
	[SerializeField]
	private T m_Data = default(T);

	// Token: 0x04001A06 RID: 6662
	[SerializeField]
	private T m_DLC02Data = default(T);

	// Token: 0x04001A07 RID: 6663
	[SerializeField]
	private T m_DLC03Data = default(T);

	// Token: 0x04001A08 RID: 6664
	[SerializeField]
	private T m_DLC04Data = default(T);

	// Token: 0x04001A09 RID: 6665
	[SerializeField]
	private T m_DLC05Data = default(T);

	// Token: 0x04001A0A RID: 6666
	[SerializeField]
	private T m_DLC06Data = default(T);

	// Token: 0x04001A0B RID: 6667
	[SerializeField]
	private T m_DLC07Data = default(T);

	// Token: 0x04001A0C RID: 6668
	[SerializeField]
	private T m_DLC08Data = default(T);

	// Token: 0x04001A0D RID: 6669
	[SerializeField]
	private T m_DLC09Data = default(T);

	// Token: 0x04001A0E RID: 6670
	[SerializeField]
	private T m_DLC10Data = default(T);

	// Token: 0x04001A0F RID: 6671
	[SerializeField]
	private T m_DLC11Data = default(T);

	// Token: 0x04001A10 RID: 6672
	[SerializeField]
	private T m_DLC13Data = default(T);
}
