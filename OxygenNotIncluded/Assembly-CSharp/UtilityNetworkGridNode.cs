using System;

// Token: 0x02000A1E RID: 2590
public struct UtilityNetworkGridNode : IEquatable<UtilityNetworkGridNode>
{
	// Token: 0x06004D7C RID: 19836 RVA: 0x001B22CF File Offset: 0x001B04CF
	public bool Equals(UtilityNetworkGridNode other)
	{
		return this.connections == other.connections && this.networkIdx == other.networkIdx;
	}

	// Token: 0x06004D7D RID: 19837 RVA: 0x001B22F0 File Offset: 0x001B04F0
	public override bool Equals(object obj)
	{
		return ((UtilityNetworkGridNode)obj).Equals(this);
	}

	// Token: 0x06004D7E RID: 19838 RVA: 0x001B2311 File Offset: 0x001B0511
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06004D7F RID: 19839 RVA: 0x001B2323 File Offset: 0x001B0523
	public static bool operator ==(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return x.Equals(y);
	}

	// Token: 0x06004D80 RID: 19840 RVA: 0x001B232D File Offset: 0x001B052D
	public static bool operator !=(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return !x.Equals(y);
	}

	// Token: 0x0400327C RID: 12924
	public UtilityConnections connections;

	// Token: 0x0400327D RID: 12925
	public int networkIdx;

	// Token: 0x0400327E RID: 12926
	public const int InvalidNetworkIdx = -1;
}
