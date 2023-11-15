using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B31 RID: 2865
[AddComponentMenu("KMonoBehaviour/scripts/KUISelectable")]
public class KUISelectable : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600585C RID: 22620 RVA: 0x00205EEC File Offset: 0x002040EC
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x0600585D RID: 22621 RVA: 0x00205EEE File Offset: 0x002040EE
	protected override void OnSpawn()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x0600585E RID: 22622 RVA: 0x00205F0C File Offset: 0x0020410C
	public void SetTarget(GameObject target)
	{
		this.target = target;
	}

	// Token: 0x0600585F RID: 22623 RVA: 0x00205F15 File Offset: 0x00204115
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.target != null)
		{
			SelectTool.Instance.SetHoverOverride(this.target.GetComponent<KSelectable>());
		}
	}

	// Token: 0x06005860 RID: 22624 RVA: 0x00205F3A File Offset: 0x0020413A
	public void OnPointerExit(PointerEventData eventData)
	{
		SelectTool.Instance.SetHoverOverride(null);
	}

	// Token: 0x06005861 RID: 22625 RVA: 0x00205F47 File Offset: 0x00204147
	private void OnClick()
	{
		if (this.target != null)
		{
			SelectTool.Instance.Select(this.target.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06005862 RID: 22626 RVA: 0x00205F6D File Offset: 0x0020416D
	protected override void OnCmpDisable()
	{
		if (SelectTool.Instance != null)
		{
			SelectTool.Instance.SetHoverOverride(null);
		}
	}

	// Token: 0x04003BC1 RID: 15297
	private GameObject target;
}
