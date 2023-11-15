using System;
using System.Linq;

// Token: 0x02000397 RID: 919
public static class StringSearchableListUtil
{
	// Token: 0x0600131F RID: 4895 RVA: 0x00064F80 File Offset: 0x00063180
	public static bool DoAnyTagsMatchFilter(string[] lowercaseTags, in string filter)
	{
		string text = filter.Trim().ToLowerInvariant();
		string[] source = text.Split(new char[]
		{
			' '
		});
		for (int i = 0; i < lowercaseTags.Length; i++)
		{
			string tag = lowercaseTags[i];
			if (StringSearchableListUtil.DoesTagMatchFilter(tag, text))
			{
				return true;
			}
			if ((from f in source
			select StringSearchableListUtil.DoesTagMatchFilter(tag, f)).All((bool result) => result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x0006501B File Offset: 0x0006321B
	public static bool DoesTagMatchFilter(string lowercaseTag, in string filter)
	{
		return string.IsNullOrWhiteSpace(filter) || lowercaseTag.Contains(filter);
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x00065035 File Offset: 0x00063235
	public static bool ShouldUseFilter(string filter)
	{
		return !string.IsNullOrWhiteSpace(filter);
	}
}
