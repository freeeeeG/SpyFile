using System;
using System.Collections.Generic;

// Token: 0x02000396 RID: 918
public class StringSearchableList<T>
{
	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600131A RID: 4890 RVA: 0x00064E49 File Offset: 0x00063049
	// (set) Token: 0x0600131B RID: 4891 RVA: 0x00064E51 File Offset: 0x00063051
	public bool didUseFilter { get; private set; }

	// Token: 0x0600131C RID: 4892 RVA: 0x00064E5A File Offset: 0x0006305A
	public StringSearchableList(List<T> allValues, StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.allValues = allValues;
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.filteredValues = new List<T>();
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x00064E86 File Offset: 0x00063086
	public StringSearchableList(StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn)
	{
		this.shouldFilterOutFn = shouldFilterOutFn;
		this.allValues = new List<T>();
		this.filteredValues = new List<T>();
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x00064EB8 File Offset: 0x000630B8
	public void Refilter()
	{
		if (StringSearchableListUtil.ShouldUseFilter(this.filter))
		{
			this.filteredValues.Clear();
			foreach (T t in this.allValues)
			{
				if (!this.shouldFilterOutFn(t, this.filter))
				{
					this.filteredValues.Add(t);
				}
			}
			this.didUseFilter = true;
			return;
		}
		if (this.filteredValues.Count != this.allValues.Count)
		{
			this.filteredValues.Clear();
			this.filteredValues.AddRange(this.allValues);
		}
		this.didUseFilter = false;
	}

	// Token: 0x04000A4A RID: 2634
	public string filter = "";

	// Token: 0x04000A4B RID: 2635
	public List<T> allValues;

	// Token: 0x04000A4C RID: 2636
	public List<T> filteredValues;

	// Token: 0x04000A4E RID: 2638
	public readonly StringSearchableList<T>.ShouldFilterOutFn shouldFilterOutFn;

	// Token: 0x02000FBE RID: 4030
	// (Invoke) Token: 0x0600731B RID: 29467
	public delegate bool ShouldFilterOutFn(T candidateValue, in string filter);
}
