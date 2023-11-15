using System;

// Token: 0x0200077D RID: 1917
[Serializable]
public struct EffectorValues
{
	// Token: 0x06003511 RID: 13585 RVA: 0x0011FB70 File Offset: 0x0011DD70
	public EffectorValues(int amt, int rad)
	{
		this.amount = amt;
		this.radius = rad;
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x0011FB80 File Offset: 0x0011DD80
	public override bool Equals(object obj)
	{
		return obj is EffectorValues && this.Equals((EffectorValues)obj);
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x0011FB98 File Offset: 0x0011DD98
	public bool Equals(EffectorValues p)
	{
		return p != null && (this == p || (!(base.GetType() != p.GetType()) && this.amount == p.amount && this.radius == p.radius));
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x0011FC06 File Offset: 0x0011DE06
	public override int GetHashCode()
	{
		return this.amount ^ this.radius;
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x0011FC15 File Offset: 0x0011DE15
	public static bool operator ==(EffectorValues lhs, EffectorValues rhs)
	{
		if (lhs == null)
		{
			return rhs == null;
		}
		return lhs.Equals(rhs);
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x0011FC33 File Offset: 0x0011DE33
	public static bool operator !=(EffectorValues lhs, EffectorValues rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x04002028 RID: 8232
	public int amount;

	// Token: 0x04002029 RID: 8233
	public int radius;
}
