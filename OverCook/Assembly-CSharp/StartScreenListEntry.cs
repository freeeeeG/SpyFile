using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B12 RID: 2834
public class StartScreenListEntry : FrontendListEntry
{
	// Token: 0x06003951 RID: 14673 RVA: 0x0010FFD0 File Offset: 0x0010E3D0
	public override void SetNameData(ScrollingListUIContainer.NameData _nameData)
	{
		StartScreenListEntry.NameData nameData = _nameData as StartScreenListEntry.NameData;
		GameObject gameObject = base.transform.Find("SelectedText").gameObject;
		Text text = gameObject.RequestComponentRecursive<Text>();
		Outline[] array = text.gameObject.RequestComponents<Outline>();
		text.text = nameData.Name;
		text.color = this.GetTextColour(nameData);
		for (int i = 0; i < array.Length; i++)
		{
			Color effectColor = array[i].effectColor;
			effectColor.r = text.color.r;
			effectColor.g = text.color.g;
			effectColor.b = text.color.b;
			array[i].effectColor = effectColor;
		}
		GameObject gameObject2 = base.transform.Find("AvailableText").gameObject;
		Text text2 = gameObject2.RequestComponentRecursive<Text>();
		text2.enabled = nameData.IsAvailable;
		text2.text = nameData.AvailableMessage;
	}

	// Token: 0x06003952 RID: 14674 RVA: 0x001100D0 File Offset: 0x0010E4D0
	protected Color GetTextColour(StartScreenListEntry.NameData _data)
	{
		if (!_data.Enable)
		{
			return new Color(0.5f, 0.5f, 0.5f, 1f);
		}
		return _data.EnabledColor;
	}

	// Token: 0x02000B13 RID: 2835
	[Serializable]
	public new class NameData : FrontendListEntry.NameData
	{
		// Token: 0x06003953 RID: 14675 RVA: 0x001100FD File Offset: 0x0010E4FD
		public NameData(string _name, bool _enabled, bool _completed) : base(_name, _enabled, _completed)
		{
			this.EnabledColor = Color.black;
			this.IsAvailable = false;
			this.AvailableMessage = string.Empty;
		}

		// Token: 0x04002E20 RID: 11808
		public Color EnabledColor;

		// Token: 0x04002E21 RID: 11809
		public bool IsAvailable;

		// Token: 0x04002E22 RID: 11810
		public string AvailableMessage;
	}
}
