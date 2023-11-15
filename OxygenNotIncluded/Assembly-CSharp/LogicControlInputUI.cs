using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B52 RID: 2898
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicControlInputUI : KMonoBehaviour
{
	// Token: 0x06005987 RID: 22919 RVA: 0x0020BD3C File Offset: 0x00209F3C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.colourDisconnected = GlobalAssets.Instance.colorSet.logicDisconnected;
		this.icon.raycastTarget = false;
		this.border.raycastTarget = false;
	}

	// Token: 0x06005988 RID: 22920 RVA: 0x0020BDC4 File Offset: 0x00209FC4
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 c = (network == null) ? GlobalAssets.Instance.colorSet.logicDisconnected : (network.IsBitActive(0) ? this.colourOn : this.colourOff);
		this.icon.color = c;
	}

	// Token: 0x04003C8F RID: 15503
	[SerializeField]
	private Image icon;

	// Token: 0x04003C90 RID: 15504
	[SerializeField]
	private Image border;

	// Token: 0x04003C91 RID: 15505
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x04003C92 RID: 15506
	private Color32 colourOn;

	// Token: 0x04003C93 RID: 15507
	private Color32 colourOff;

	// Token: 0x04003C94 RID: 15508
	private Color32 colourDisconnected;
}
