using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000080 RID: 128
public abstract class UI_Icon : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	// Token: 0x06000484 RID: 1156
	public abstract void OnPointerClick(PointerEventData eventData);

	// Token: 0x06000485 RID: 1157
	public abstract void OnPointerEnter(PointerEventData eventData);

	// Token: 0x06000486 RID: 1158
	public abstract void OnPointerExit(PointerEventData eventData);

	// Token: 0x06000487 RID: 1159 RVA: 0x0001C340 File Offset: 0x0001A540
	protected virtual void Start()
	{
		base.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0001C358 File Offset: 0x0001A558
	protected virtual void Outline_Show_Above()
	{
		UI_Setting inst = UI_Setting.Inst;
		this.outline.enabled = true;
		this.outline.effectColor = inst.outline_ColorAbove;
		this.outline.effectDistance = new Vector2(inst.outline_Distance, inst.outline_Distance);
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0001C3A4 File Offset: 0x0001A5A4
	protected virtual void Outline_Show_Selected()
	{
		UI_Setting inst = UI_Setting.Inst;
		this.outline.enabled = true;
		this.outline.effectColor = inst.outline_ColorSelected;
		this.outline.effectDistance = new Vector2(inst.outline_Distance, inst.outline_Distance);
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0001C3F0 File Offset: 0x0001A5F0
	protected virtual void Outline_Close()
	{
		UI_Setting inst = UI_Setting.Inst;
		this.outline.enabled = false;
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0001C404 File Offset: 0x0001A604
	protected virtual void OutlineNew_Show_Selected()
	{
		this.imageOutline.enabled = true;
		this.imageOutline.color = Color.white;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x0001C424 File Offset: 0x0001A624
	protected virtual void OutlineNew_Show_Above()
	{
		UI_Setting inst = UI_Setting.Inst;
		this.imageOutline.enabled = true;
		this.imageOutline.color = inst.outline_ColorAbove;
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0001C454 File Offset: 0x0001A654
	protected virtual void OutlineNew_Close()
	{
		this.imageOutline.enabled = false;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0001C464 File Offset: 0x0001A664
	public void TextSetNormal()
	{
		foreach (Text text in base.GetComponentsInChildren<Text>())
		{
			text.color = this.textNormalColor;
			text.fontStyle = this.textNormalStyle;
		}
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0001C4A0 File Offset: 0x0001A6A0
	protected void TextSetHighlight()
	{
		foreach (Text text in base.GetComponentsInChildren<Text>())
		{
			text.color = this.textHighlightColor;
			text.fontStyle = this.textHighlightStyle;
		}
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0001C4DC File Offset: 0x0001A6DC
	protected void UpdateLockIcon()
	{
		if (this.iconLock == null)
		{
			Debug.LogError("iconLock==null!");
			return;
		}
		if (this.ifUnlocked())
		{
			this.iconLock.SetActive(false);
			return;
		}
		this.iconLock.SetActive(true);
	}

	// Token: 0x06000491 RID: 1169
	protected abstract bool ifUnlocked();

	// Token: 0x040003DF RID: 991
	[SerializeField]
	protected Outline outline;

	// Token: 0x040003E0 RID: 992
	[SerializeField]
	protected Image imageOutline;

	// Token: 0x040003E1 RID: 993
	[SerializeField]
	protected Color textNormalColor = Color.gray;

	// Token: 0x040003E2 RID: 994
	[SerializeField]
	protected Color textHighlightColor = Color.black;

	// Token: 0x040003E3 RID: 995
	[SerializeField]
	private FontStyle textNormalStyle;

	// Token: 0x040003E4 RID: 996
	[SerializeField]
	private FontStyle textHighlightStyle = FontStyle.Bold;

	// Token: 0x040003E5 RID: 997
	[SerializeField]
	private GameObject iconLock;
}
