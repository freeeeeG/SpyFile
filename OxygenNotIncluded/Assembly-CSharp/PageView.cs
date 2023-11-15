using System;
using UnityEngine;

// Token: 0x02000BBA RID: 3002
[AddComponentMenu("KMonoBehaviour/scripts/PageView")]
public class PageView : KMonoBehaviour
{
	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06005DE0 RID: 24032 RVA: 0x00225EB2 File Offset: 0x002240B2
	public int ChildrenPerPage
	{
		get
		{
			return this.childrenPerPage;
		}
	}

	// Token: 0x06005DE1 RID: 24033 RVA: 0x00225EBA File Offset: 0x002240BA
	private void Update()
	{
		if (this.oldChildCount != base.transform.childCount)
		{
			this.oldChildCount = base.transform.childCount;
			this.RefreshPage();
		}
	}

	// Token: 0x06005DE2 RID: 24034 RVA: 0x00225EE8 File Offset: 0x002240E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.nextButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.currentPage = (this.currentPage + 1) % this.pageCount;
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
		MultiToggle multiToggle2 = this.prevButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.currentPage--;
			if (this.currentPage < 0)
			{
				this.currentPage += this.pageCount;
			}
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
	}

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06005DE3 RID: 24035 RVA: 0x00225F4C File Offset: 0x0022414C
	private int pageCount
	{
		get
		{
			int num = base.transform.childCount / this.childrenPerPage;
			if (base.transform.childCount % this.childrenPerPage != 0)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x06005DE4 RID: 24036 RVA: 0x00225F88 File Offset: 0x00224188
	private void RefreshPage()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (i < this.currentPage * this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else if (i >= this.currentPage * this.childrenPerPage + this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else
			{
				base.transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		this.pageLabel.SetText((this.currentPage % this.pageCount + 1).ToString() + "/" + this.pageCount.ToString());
	}

	// Token: 0x04003F3E RID: 16190
	[SerializeField]
	private MultiToggle nextButton;

	// Token: 0x04003F3F RID: 16191
	[SerializeField]
	private MultiToggle prevButton;

	// Token: 0x04003F40 RID: 16192
	[SerializeField]
	private LocText pageLabel;

	// Token: 0x04003F41 RID: 16193
	[SerializeField]
	private int childrenPerPage = 8;

	// Token: 0x04003F42 RID: 16194
	private int currentPage;

	// Token: 0x04003F43 RID: 16195
	private int oldChildCount;

	// Token: 0x04003F44 RID: 16196
	public Action<int> OnChangePage;
}
