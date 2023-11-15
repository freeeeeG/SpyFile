using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B30 RID: 2864
public class KToggleMenu : KScreen
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06005852 RID: 22610 RVA: 0x00205C44 File Offset: 0x00203E44
	// (remove) Token: 0x06005853 RID: 22611 RVA: 0x00205C7C File Offset: 0x00203E7C
	public event KToggleMenu.OnSelect onSelect;

	// Token: 0x06005854 RID: 22612 RVA: 0x00205CB1 File Offset: 0x00203EB1
	public void Setup(IList<KToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x06005855 RID: 22613 RVA: 0x00205CC0 File Offset: 0x00203EC0
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x06005856 RID: 22614 RVA: 0x00205CC8 File Offset: 0x00203EC8
	private void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				UnityEngine.Object.Destroy(ktoggle.gameObject);
			}
		}
		this.toggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform parent = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			if (toggleInfo == null)
			{
				this.toggles.Add(null);
			}
			else
			{
				KToggle ktoggle2 = UnityEngine.Object.Instantiate<KToggle>(this.prefab, Vector3.zero, Quaternion.identity);
				ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
				ktoggle2.transform.SetParent(parent, false);
				ktoggle2.group = this.group;
				ktoggle2.onClick += delegate()
				{
					this.OnClick(idx);
				};
				ktoggle2.GetComponentsInChildren<Text>(true)[0].text = toggleInfo.text;
				toggleInfo.toggle = ktoggle2;
				this.toggles.Add(ktoggle2);
			}
		}
	}

	// Token: 0x06005857 RID: 22615 RVA: 0x00205E40 File Offset: 0x00204040
	public int GetSelected()
	{
		return KToggleMenu.selected;
	}

	// Token: 0x06005858 RID: 22616 RVA: 0x00205E47 File Offset: 0x00204047
	private void OnClick(int i)
	{
		UISounds.PlaySound(UISounds.Sound.ClickObject);
		if (this.onSelect == null)
		{
			return;
		}
		this.onSelect(this.toggleInfo[i]);
	}

	// Token: 0x06005859 RID: 22617 RVA: 0x00205E70 File Offset: 0x00204070
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			global::Action hotKey = this.toggleInfo[i].hotKey;
			if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
			{
				this.toggles[i].Click();
				return;
			}
		}
	}

	// Token: 0x04003BBA RID: 15290
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x04003BBB RID: 15291
	[SerializeField]
	private KToggle prefab;

	// Token: 0x04003BBC RID: 15292
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x04003BBE RID: 15294
	protected IList<KToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x04003BBF RID: 15295
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x04003BC0 RID: 15296
	private static int selected = -1;

	// Token: 0x02001A3E RID: 6718
	// (Invoke) Token: 0x06009679 RID: 38521
	public delegate void OnSelect(KToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001A3F RID: 6719
	public class ToggleInfo
	{
		// Token: 0x0600967C RID: 38524 RVA: 0x0033C9CA File Offset: 0x0033ABCA
		public ToggleInfo(string text, object user_data = null, global::Action hotKey = global::Action.NumActions)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotKey;
		}

		// Token: 0x04007900 RID: 30976
		public string text;

		// Token: 0x04007901 RID: 30977
		public object userData;

		// Token: 0x04007902 RID: 30978
		public KToggle toggle;

		// Token: 0x04007903 RID: 30979
		public global::Action hotKey;
	}
}
