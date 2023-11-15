using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020000A4 RID: 164
public class Icon_AutoFire : UI_Icon
{
	// Token: 0x060005AD RID: 1453 RVA: 0x000209EF File Offset: 0x0001EBEF
	public override void OnPointerClick(PointerEventData eventData)
	{
		this.open = !this.open;
		this.UpdateIcon();
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x00020A06 File Offset: 0x0001EC06
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.TextSetHighlight();
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00020A0E File Offset: 0x0001EC0E
	public override void OnPointerExit(PointerEventData eventData)
	{
		base.TextSetNormal();
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00020A16 File Offset: 0x0001EC16
	private void Awake()
	{
		Icon_AutoFire.inst = this;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00020A20 File Offset: 0x0001EC20
	private new void Start()
	{
		int jobId = TempData.inst.jobId;
		if (jobId == 2 || jobId == 5 || jobId == 9 || jobId == 11)
		{
			base.gameObject.SetActive(false);
		}
		this.open = false;
		this.UpdateIcon();
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x00020A63 File Offset: 0x0001EC63
	private void Update()
	{
		if (MyInput.GetKeyDownWithSound(KeyCode.F))
		{
			this.open = !this.open;
			this.UpdateIcon();
		}
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x00020A83 File Offset: 0x0001EC83
	private void UpdateIcon()
	{
		this.textLang.text = LanguageText.Inst.autoFire;
		this.icon.gameObject.SetActive(this.open);
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTransform);
	}

	// Token: 0x0400049E RID: 1182
	public static Icon_AutoFire inst;

	// Token: 0x0400049F RID: 1183
	[SerializeField]
	public bool open;

	// Token: 0x040004A0 RID: 1184
	[SerializeField]
	private Text textLang;

	// Token: 0x040004A1 RID: 1185
	[SerializeField]
	private Image icon;

	// Token: 0x040004A2 RID: 1186
	[SerializeField]
	private RectTransform rectTransform;
}
