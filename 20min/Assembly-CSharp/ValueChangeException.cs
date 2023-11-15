using System;
using System.Collections.Generic;

// Token: 0x02000034 RID: 52
public class ValueChangeException : BaseException
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060003C3 RID: 963 RVA: 0x00014A38 File Offset: 0x00012C38
	public float delta
	{
		get
		{
			return this.toValue - this.fromValue;
		}
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00014A47 File Offset: 0x00012C47
	public ValueChangeException(float fromValue, float toValue) : base(true)
	{
		this.fromValue = fromValue;
		this.toValue = toValue;
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00014A5E File Offset: 0x00012C5E
	public void AddModifier(ValueModifier m)
	{
		if (this.modifiers == null)
		{
			this.modifiers = new List<ValueModifier>();
		}
		this.modifiers.Add(m);
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00014A80 File Offset: 0x00012C80
	public float GetModifiedValue()
	{
		if (this.modifiers == null)
		{
			return this.toValue;
		}
		float result = this.toValue;
		this.modifiers.Sort(new Comparison<ValueModifier>(this.Compare));
		for (int i = 0; i < this.modifiers.Count; i++)
		{
			result = this.modifiers[i].Modify(this.fromValue, result);
		}
		return result;
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00014AEC File Offset: 0x00012CEC
	private int Compare(ValueModifier x, ValueModifier y)
	{
		return x.sortOrder.CompareTo(y.sortOrder);
	}

	// Token: 0x040001D1 RID: 465
	public readonly float fromValue;

	// Token: 0x040001D2 RID: 466
	public readonly float toValue;

	// Token: 0x040001D3 RID: 467
	private List<ValueModifier> modifiers;
}
