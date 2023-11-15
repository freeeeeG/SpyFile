using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000263 RID: 611
public class ItemSlot : MonoBehaviour
{
	// Token: 0x06000F50 RID: 3920 RVA: 0x00028B70 File Offset: 0x00026D70
	public virtual void SetContent(ContentAttribute attribute, ToggleGroup group)
	{
		this.m_Toggle = base.GetComponent<Toggle>();
		this.m_Toggle.group = group;
		this.contenAtt = attribute;
		this.isLock = this.contenAtt.isLock;
		this.lockIcon.SetActive(this.isLock);
		this.icon.sprite = this.contenAtt.Icon;
		this.nameTxt.text = (this.isLock ? "" : GameMultiLang.GetTraduction(this.contenAtt.Name));
		this.icon.color = (this.isLock ? this.lockColor : Color.white);
		this.nameTxt.color = (this.isLock ? Color.gray : this.normalColor);
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00028C3E File Offset: 0x00026E3E
	public virtual void OnItemSelect(bool value)
	{
		Singleton<Sound>.Instance.PlayEffect("Sound_Click");
	}

	// Token: 0x040007AF RID: 1967
	[SerializeField]
	protected ContentAttribute contenAtt;

	// Token: 0x040007B0 RID: 1968
	[SerializeField]
	private Image icon;

	// Token: 0x040007B1 RID: 1969
	[SerializeField]
	private Text nameTxt;

	// Token: 0x040007B2 RID: 1970
	[SerializeField]
	private Color normalColor;

	// Token: 0x040007B3 RID: 1971
	[SerializeField]
	private Color lockColor;

	// Token: 0x040007B4 RID: 1972
	[SerializeField]
	private GameObject lockIcon;

	// Token: 0x040007B5 RID: 1973
	protected bool isLock;

	// Token: 0x040007B6 RID: 1974
	private Toggle m_Toggle;
}
