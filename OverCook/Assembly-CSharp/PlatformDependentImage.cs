using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000145 RID: 325
public class PlatformDependentImage : Image
{
	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0002AD7D File Offset: 0x0002917D
	public Sprite PcSprite
	{
		get
		{
			return this.m_pcSprite;
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060005CA RID: 1482 RVA: 0x0002AD85 File Offset: 0x00029185
	private Sprite PlatformSprite
	{
		get
		{
			return this.m_pcSprite;
		}
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x0002AD8D File Offset: 0x0002918D
	protected override void Awake()
	{
		if (Application.isPlaying)
		{
			base.sprite = this.PlatformSprite;
		}
		base.Awake();
	}

	// Token: 0x040004C5 RID: 1221
	[SerializeField]
	private Sprite m_pcSprite;
}
