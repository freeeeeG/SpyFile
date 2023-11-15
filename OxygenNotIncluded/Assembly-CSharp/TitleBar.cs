using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C7B RID: 3195
[AddComponentMenu("KMonoBehaviour/scripts/TitleBar")]
public class TitleBar : KMonoBehaviour
{
	// Token: 0x060065E5 RID: 26085 RVA: 0x00260870 File Offset: 0x0025EA70
	public void SetTitle(string Name)
	{
		this.titleText.text = Name;
	}

	// Token: 0x060065E6 RID: 26086 RVA: 0x0026087E File Offset: 0x0025EA7E
	public void SetSubText(string subtext, string tooltip = "")
	{
		this.subtextText.text = subtext;
		this.subtextText.GetComponent<ToolTip>().toolTip = tooltip;
	}

	// Token: 0x060065E7 RID: 26087 RVA: 0x0026089D File Offset: 0x0025EA9D
	public void SetWarningActve(bool state)
	{
		this.WarningNotification.SetActive(state);
	}

	// Token: 0x060065E8 RID: 26088 RVA: 0x002608AB File Offset: 0x0025EAAB
	public void SetWarning(Sprite icon, string label)
	{
		this.SetWarningActve(true);
		this.NotificationIcon.sprite = icon;
		this.NotificationText.text = label;
	}

	// Token: 0x060065E9 RID: 26089 RVA: 0x002608CC File Offset: 0x0025EACC
	public void SetPortrait(GameObject target)
	{
		this.portrait.SetPortrait(target);
	}

	// Token: 0x04004627 RID: 17959
	public LocText titleText;

	// Token: 0x04004628 RID: 17960
	public LocText subtextText;

	// Token: 0x04004629 RID: 17961
	public GameObject WarningNotification;

	// Token: 0x0400462A RID: 17962
	public Text NotificationText;

	// Token: 0x0400462B RID: 17963
	public Image NotificationIcon;

	// Token: 0x0400462C RID: 17964
	public Sprite techIcon;

	// Token: 0x0400462D RID: 17965
	public Sprite materialIcon;

	// Token: 0x0400462E RID: 17966
	public TitleBarPortrait portrait;

	// Token: 0x0400462F RID: 17967
	public bool userEditable;

	// Token: 0x04004630 RID: 17968
	public bool setCameraControllerState = true;
}
