using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A94 RID: 2708
public class FrontendListEntry : ScrollingListEntry
{
	// Token: 0x06003592 RID: 13714 RVA: 0x000FA590 File Offset: 0x000F8990
	public override void SetNameData(ScrollingListUIContainer.NameData _nameData)
	{
		FrontendListEntry.NameData nameData = _nameData as FrontendListEntry.NameData;
		Text text = base.gameObject.RequestComponentRecursive<Text>();
		text.text = nameData.Name;
		text.color = this.GetTextColour(nameData);
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x000FA5C9 File Offset: 0x000F89C9
	private Color GetTextColour(FrontendListEntry.NameData _data)
	{
		if (!_data.Enable)
		{
			return new Color(0.5f, 0.5f, 0.5f, 1f);
		}
		return new Color(0f, 0f, 0f, 1f);
	}

	// Token: 0x02000A95 RID: 2709
	[Serializable]
	public class NameData : ScrollingListUIContainer.NameData
	{
		// Token: 0x06003594 RID: 13716 RVA: 0x000FA611 File Offset: 0x000F8A11
		public NameData(string _name, bool _enabled, bool _completed)
		{
			this.Name = _name;
			this.Enable = _enabled;
			this.Completed = _completed;
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000FA62E File Offset: 0x000F8A2E
		public bool Equals(FrontendListEntry.NameData obj)
		{
			return this.Name == obj.Name && this.Enable == obj.Enable && this.Completed == obj.Completed;
		}

		// Token: 0x04002B0F RID: 11023
		public string Name;

		// Token: 0x04002B10 RID: 11024
		public bool Enable;

		// Token: 0x04002B11 RID: 11025
		public bool Completed;
	}
}
