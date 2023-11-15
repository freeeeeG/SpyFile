using System;

// Token: 0x020003D3 RID: 979
public struct PrioritySetting : IComparable<PrioritySetting>
{
	// Token: 0x0600145F RID: 5215 RVA: 0x0006BEF0 File Offset: 0x0006A0F0
	public override int GetHashCode()
	{
		return ((int)((int)this.priority_class << 28)).GetHashCode() ^ this.priority_value.GetHashCode();
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x0006BF1A File Offset: 0x0006A11A
	public static bool operator ==(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x0006BF2F File Offset: 0x0006A12F
	public static bool operator !=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return !lhs.Equals(rhs);
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x0006BF47 File Offset: 0x0006A147
	public static bool operator <=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x0006BF57 File Offset: 0x0006A157
	public static bool operator >=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x0006BF67 File Offset: 0x0006A167
	public static bool operator <(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x0006BF74 File Offset: 0x0006A174
	public static bool operator >(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x0006BF81 File Offset: 0x0006A181
	public override bool Equals(object obj)
	{
		return obj is PrioritySetting && ((PrioritySetting)obj).priority_class == this.priority_class && ((PrioritySetting)obj).priority_value == this.priority_value;
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x0006BFB8 File Offset: 0x0006A1B8
	public int CompareTo(PrioritySetting other)
	{
		if (this.priority_class > other.priority_class)
		{
			return 1;
		}
		if (this.priority_class < other.priority_class)
		{
			return -1;
		}
		if (this.priority_value > other.priority_value)
		{
			return 1;
		}
		if (this.priority_value < other.priority_value)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x0006C006 File Offset: 0x0006A206
	public PrioritySetting(PriorityScreen.PriorityClass priority_class, int priority_value)
	{
		this.priority_class = priority_class;
		this.priority_value = priority_value;
	}

	// Token: 0x04000AF8 RID: 2808
	public PriorityScreen.PriorityClass priority_class;

	// Token: 0x04000AF9 RID: 2809
	public int priority_value;
}
