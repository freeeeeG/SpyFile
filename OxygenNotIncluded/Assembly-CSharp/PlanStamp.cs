using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BC1 RID: 3009
[AddComponentMenu("KMonoBehaviour/scripts/PlanStamp")]
public class PlanStamp : KMonoBehaviour
{
	// Token: 0x06005E7A RID: 24186 RVA: 0x0022B0A7 File Offset: 0x002292A7
	public void SetStamp(Sprite sprite, string Text)
	{
		this.StampImage.sprite = sprite;
		this.StampText.text = Text.ToUpper();
	}

	// Token: 0x04003FB3 RID: 16307
	public PlanStamp.StampArt stampSprites;

	// Token: 0x04003FB4 RID: 16308
	[SerializeField]
	private Image StampImage;

	// Token: 0x04003FB5 RID: 16309
	[SerializeField]
	private Text StampText;

	// Token: 0x02001B0A RID: 6922
	[Serializable]
	public struct StampArt
	{
		// Token: 0x04007B7D RID: 31613
		public Sprite UnderConstruction;

		// Token: 0x04007B7E RID: 31614
		public Sprite NeedsResearch;

		// Token: 0x04007B7F RID: 31615
		public Sprite SelectResource;

		// Token: 0x04007B80 RID: 31616
		public Sprite NeedsRepair;

		// Token: 0x04007B81 RID: 31617
		public Sprite NeedsPower;

		// Token: 0x04007B82 RID: 31618
		public Sprite NeedsResource;

		// Token: 0x04007B83 RID: 31619
		public Sprite NeedsGasPipe;

		// Token: 0x04007B84 RID: 31620
		public Sprite NeedsLiquidPipe;
	}
}
