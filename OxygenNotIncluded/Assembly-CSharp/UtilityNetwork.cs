using System;

// Token: 0x02000A15 RID: 2581
public class UtilityNetwork
{
	// Token: 0x06004D42 RID: 19778 RVA: 0x001B18B8 File Offset: 0x001AFAB8
	public virtual void AddItem(object item)
	{
	}

	// Token: 0x06004D43 RID: 19779 RVA: 0x001B18BA File Offset: 0x001AFABA
	public virtual void RemoveItem(object item)
	{
	}

	// Token: 0x06004D44 RID: 19780 RVA: 0x001B18BC File Offset: 0x001AFABC
	public virtual void ConnectItem(object item)
	{
	}

	// Token: 0x06004D45 RID: 19781 RVA: 0x001B18BE File Offset: 0x001AFABE
	public virtual void DisconnectItem(object item)
	{
	}

	// Token: 0x06004D46 RID: 19782 RVA: 0x001B18C0 File Offset: 0x001AFAC0
	public virtual void Reset(UtilityNetworkGridNode[] grid)
	{
	}

	// Token: 0x04003262 RID: 12898
	public int id;

	// Token: 0x04003263 RID: 12899
	public ConduitType conduitType;
}
