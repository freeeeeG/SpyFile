using System;
using UnityEngine;

// Token: 0x02000B2F RID: 2863
public class FrontendGUI : ScrollingListUIController
{
	// Token: 0x06003A02 RID: 14850 RVA: 0x001142C4 File Offset: 0x001126C4
	public void SetNames(FrontendListEntry.NameData[] _nameData)
	{
		if (this.IsSame(this.m_nameData, _nameData))
		{
			return;
		}
		this.m_nameData = _nameData;
		base.OnSetNames();
	}

	// Token: 0x06003A03 RID: 14851 RVA: 0x001142E8 File Offset: 0x001126E8
	private bool IsSame(FrontendListEntry.NameData[] _a, FrontendListEntry.NameData[] _b)
	{
		if (_a.Length == _b.Length)
		{
			for (int i = 0; i < _a.Length; i++)
			{
				if (!_a[i].Equals(_b[i]))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06003A04 RID: 14852 RVA: 0x00114329 File Offset: 0x00112729
	protected override ScrollingListUIContainer.NameData[] GetNameData()
	{
		return this.m_nameData.ConvertAll((FrontendListEntry.NameData x) => x);
	}

	// Token: 0x04002EF9 RID: 12025
	[SerializeField]
	private FrontendListEntry.NameData[] m_nameData;
}
