using System;
using System.Collections.Generic;

// Token: 0x020009FF RID: 2559
public class TagCollection : IReadonlyTags
{
	// Token: 0x06004C88 RID: 19592 RVA: 0x001AD019 File Offset: 0x001AB219
	public TagCollection()
	{
	}

	// Token: 0x06004C89 RID: 19593 RVA: 0x001AD02C File Offset: 0x001AB22C
	public TagCollection(int[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(initialTags[i]);
		}
	}

	// Token: 0x06004C8A RID: 19594 RVA: 0x001AD068 File Offset: 0x001AB268
	public TagCollection(string[] initialTags)
	{
		for (int i = 0; i < initialTags.Length; i++)
		{
			this.tags.Add(Hash.SDBMLower(initialTags[i]));
		}
	}

	// Token: 0x06004C8B RID: 19595 RVA: 0x001AD0A8 File Offset: 0x001AB2A8
	public TagCollection(TagCollection initialTags)
	{
		if (initialTags != null && initialTags.tags != null)
		{
			this.tags.UnionWith(initialTags.tags);
		}
	}

	// Token: 0x06004C8C RID: 19596 RVA: 0x001AD0D8 File Offset: 0x001AB2D8
	public TagCollection Append(TagCollection others)
	{
		foreach (int item in others.tags)
		{
			this.tags.Add(item);
		}
		return this;
	}

	// Token: 0x06004C8D RID: 19597 RVA: 0x001AD134 File Offset: 0x001AB334
	public void AddTag(string tag)
	{
		this.tags.Add(Hash.SDBMLower(tag));
	}

	// Token: 0x06004C8E RID: 19598 RVA: 0x001AD148 File Offset: 0x001AB348
	public void AddTag(int tag)
	{
		this.tags.Add(tag);
	}

	// Token: 0x06004C8F RID: 19599 RVA: 0x001AD157 File Offset: 0x001AB357
	public void RemoveTag(string tag)
	{
		this.tags.Remove(Hash.SDBMLower(tag));
	}

	// Token: 0x06004C90 RID: 19600 RVA: 0x001AD16B File Offset: 0x001AB36B
	public void RemoveTag(int tag)
	{
		this.tags.Remove(tag);
	}

	// Token: 0x06004C91 RID: 19601 RVA: 0x001AD17A File Offset: 0x001AB37A
	public bool HasTag(string tag)
	{
		return this.tags.Contains(Hash.SDBMLower(tag));
	}

	// Token: 0x06004C92 RID: 19602 RVA: 0x001AD18D File Offset: 0x001AB38D
	public bool HasTag(int tag)
	{
		return this.tags.Contains(tag);
	}

	// Token: 0x06004C93 RID: 19603 RVA: 0x001AD19C File Offset: 0x001AB39C
	public bool HasTags(int[] searchTags)
	{
		for (int i = 0; i < searchTags.Length; i++)
		{
			if (!this.tags.Contains(searchTags[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040031E6 RID: 12774
	private HashSet<int> tags = new HashSet<int>();
}
