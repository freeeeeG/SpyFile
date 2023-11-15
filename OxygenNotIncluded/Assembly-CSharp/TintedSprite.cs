using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000590 RID: 1424
[DebuggerDisplay("{name}")]
[Serializable]
public class TintedSprite : ISerializationCallbackReceiver
{
	// Token: 0x06002276 RID: 8822 RVA: 0x000BD466 File Offset: 0x000BB666
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06002277 RID: 8823 RVA: 0x000BD468 File Offset: 0x000BB668
	public void OnBeforeSerialize()
	{
		if (this.sprite != null)
		{
			this.name = this.sprite.name;
		}
	}

	// Token: 0x040013AD RID: 5037
	[ReadOnly]
	public string name;

	// Token: 0x040013AE RID: 5038
	public Sprite sprite;

	// Token: 0x040013AF RID: 5039
	public Color color;
}
