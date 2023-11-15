using System;

// Token: 0x02000C03 RID: 3075
public interface ICheckboxListGroupControl
{
	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06006150 RID: 24912
	string Title { get; }

	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06006151 RID: 24913
	string Description { get; }

	// Token: 0x06006152 RID: 24914
	ICheckboxListGroupControl.ListGroup[] GetData();

	// Token: 0x06006153 RID: 24915
	bool SidescreenEnabled();

	// Token: 0x06006154 RID: 24916
	int CheckboxSideScreenSortOrder();

	// Token: 0x02001B53 RID: 6995
	public struct ListGroup
	{
		// Token: 0x060099A9 RID: 39337 RVA: 0x0034515B File Offset: 0x0034335B
		public ListGroup(string title, ICheckboxListGroupControl.CheckboxItem[] checkboxItems, Func<string, string> resolveTitleCallback = null, System.Action onItemClicked = null)
		{
			this.title = title;
			this.checkboxItems = checkboxItems;
			this.resolveTitleCallback = resolveTitleCallback;
			this.onItemClicked = onItemClicked;
		}

		// Token: 0x04007C74 RID: 31860
		public Func<string, string> resolveTitleCallback;

		// Token: 0x04007C75 RID: 31861
		public System.Action onItemClicked;

		// Token: 0x04007C76 RID: 31862
		public string title;

		// Token: 0x04007C77 RID: 31863
		public ICheckboxListGroupControl.CheckboxItem[] checkboxItems;
	}

	// Token: 0x02001B54 RID: 6996
	public struct CheckboxItem
	{
		// Token: 0x04007C78 RID: 31864
		public string text;

		// Token: 0x04007C79 RID: 31865
		public string tooltip;

		// Token: 0x04007C7A RID: 31866
		public bool isOn;

		// Token: 0x04007C7B RID: 31867
		public Func<string, bool> overrideLinkActions;

		// Token: 0x04007C7C RID: 31868
		public Func<string, object, string> resolveTooltipCallback;
	}
}
