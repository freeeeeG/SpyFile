using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000277 RID: 631
public class RecipeSlot : MonoBehaviour
{
	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00029DE7 File Offset: 0x00027FE7
	public bool IsSelected
	{
		get
		{
			return this.m_Toggle.isOn;
		}
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x00029DF4 File Offset: 0x00027FF4
	public void Initialize(TurretAttribute att, ToggleGroup group)
	{
		this.m_Anim = base.GetComponent<Animator>();
		this.m_Att = att;
		this.m_Toggle = base.GetComponent<Toggle>();
		this.m_Toggle.group = group;
		this.SetAtt(att);
		this.m_Toggle.interactable = !this.m_Att.isLock;
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x00029E4C File Offset: 0x0002804C
	private void SetAtt(TurretAttribute att)
	{
		this.lockIcon.gameObject.SetActive(att.isLock);
		this.turretName.gameObject.SetActive(!att.isLock);
		this.turretIcon.sprite = att.Icon;
		this.turretName.text = GameMultiLang.GetTraduction(att.Name);
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x00029EAF File Offset: 0x000280AF
	public void OnSelect(bool value)
	{
		this.m_Toggle.isOn = value;
		this.turretIcon.color = (value ? Color.white : this.disableColor);
		this.m_Anim.SetBool("Selected", value);
	}

	// Token: 0x04000807 RID: 2055
	private Toggle m_Toggle;

	// Token: 0x04000808 RID: 2056
	private Animator m_Anim;

	// Token: 0x04000809 RID: 2057
	[SerializeField]
	private Color disableColor;

	// Token: 0x0400080A RID: 2058
	[SerializeField]
	private Image turretIcon;

	// Token: 0x0400080B RID: 2059
	[SerializeField]
	private Text turretName;

	// Token: 0x0400080C RID: 2060
	[SerializeField]
	private Image lockIcon;

	// Token: 0x0400080D RID: 2061
	public TurretAttribute m_Att;
}
