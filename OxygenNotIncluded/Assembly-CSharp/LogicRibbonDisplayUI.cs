using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B53 RID: 2899
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicRibbonDisplayUI : KMonoBehaviour
{
	// Token: 0x0600598A RID: 22922 RVA: 0x0020BE18 File Offset: 0x0020A018
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.wire1.raycastTarget = false;
		this.wire2.raycastTarget = false;
		this.wire3.raycastTarget = false;
		this.wire4.raycastTarget = false;
	}

	// Token: 0x0600598B RID: 22923 RVA: 0x0020BEA4 File Offset: 0x0020A0A4
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 color = this.colourDisconnected;
		List<Color32> list = new List<Color32>();
		for (int i = 0; i < this.bitDepth; i++)
		{
			list.Add((network == null) ? color : (network.IsBitActive(i) ? this.colourOn : this.colourOff));
		}
		if (this.wire1.color != list[0])
		{
			this.wire1.color = list[0];
		}
		if (this.wire2.color != list[1])
		{
			this.wire2.color = list[1];
		}
		if (this.wire3.color != list[2])
		{
			this.wire3.color = list[2];
		}
		if (this.wire4.color != list[3])
		{
			this.wire4.color = list[3];
		}
	}

	// Token: 0x04003C95 RID: 15509
	[SerializeField]
	private Image wire1;

	// Token: 0x04003C96 RID: 15510
	[SerializeField]
	private Image wire2;

	// Token: 0x04003C97 RID: 15511
	[SerializeField]
	private Image wire3;

	// Token: 0x04003C98 RID: 15512
	[SerializeField]
	private Image wire4;

	// Token: 0x04003C99 RID: 15513
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x04003C9A RID: 15514
	private Color32 colourOn;

	// Token: 0x04003C9B RID: 15515
	private Color32 colourOff;

	// Token: 0x04003C9C RID: 15516
	private Color32 colourDisconnected = new Color(255f, 255f, 255f, 255f);

	// Token: 0x04003C9D RID: 15517
	private int bitDepth = 4;
}
