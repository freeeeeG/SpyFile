using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B2F RID: 2863
public class KPopupMenu : KScreen
{
	// Token: 0x0600584D RID: 22605 RVA: 0x00205B40 File Offset: 0x00203D40
	public void SetOptions(IList<string> options)
	{
		List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
		for (int i = 0; i < options.Count; i++)
		{
			int index = i;
			string option = options[i];
			list.Add(new KButtonMenu.ButtonInfo(option, global::Action.NumActions, delegate()
			{
				this.SelectOption(option, index);
			}, null, null));
		}
		this.Buttons = list.ToArray();
	}

	// Token: 0x0600584E RID: 22606 RVA: 0x00205BB8 File Offset: 0x00203DB8
	public void OnClick()
	{
		if (this.Buttons != null)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.buttonMenu.SetButtons(this.Buttons);
			this.buttonMenu.RefreshButtons();
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600584F RID: 22607 RVA: 0x00205C0F File Offset: 0x00203E0F
	public void SelectOption(string option, int index)
	{
		if (this.OnSelect != null)
		{
			this.OnSelect(option, index);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005850 RID: 22608 RVA: 0x00205C32 File Offset: 0x00203E32
	public IList<KButtonMenu.ButtonInfo> GetButtons()
	{
		return this.Buttons;
	}

	// Token: 0x04003BB7 RID: 15287
	[SerializeField]
	private KButtonMenu buttonMenu;

	// Token: 0x04003BB8 RID: 15288
	private KButtonMenu.ButtonInfo[] Buttons;

	// Token: 0x04003BB9 RID: 15289
	public Action<string, int> OnSelect;
}
