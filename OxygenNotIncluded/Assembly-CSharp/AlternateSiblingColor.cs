using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A92 RID: 2706
public class AlternateSiblingColor : KMonoBehaviour
{
	// Token: 0x060052D3 RID: 21203 RVA: 0x001DBBDC File Offset: 0x001D9DDC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int siblingIndex = base.transform.GetSiblingIndex();
		this.RefreshColor(siblingIndex % 2 == 0);
	}

	// Token: 0x060052D4 RID: 21204 RVA: 0x001DBC07 File Offset: 0x001D9E07
	private void RefreshColor(bool evenIndex)
	{
		if (this.image == null)
		{
			return;
		}
		this.image.color = (evenIndex ? this.evenColor : this.oddColor);
	}

	// Token: 0x060052D5 RID: 21205 RVA: 0x001DBC34 File Offset: 0x001D9E34
	private void Update()
	{
		if (this.mySiblingIndex != base.transform.GetSiblingIndex())
		{
			this.mySiblingIndex = base.transform.GetSiblingIndex();
			this.RefreshColor(this.mySiblingIndex % 2 == 0);
		}
	}

	// Token: 0x04003746 RID: 14150
	public Color evenColor;

	// Token: 0x04003747 RID: 14151
	public Color oddColor;

	// Token: 0x04003748 RID: 14152
	public Image image;

	// Token: 0x04003749 RID: 14153
	private int mySiblingIndex;
}
