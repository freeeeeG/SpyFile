using System;
using Level;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007F8 RID: 2040
	public class PlayerDieHeadParts : MonoBehaviour
	{
		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002975 RID: 10613 RVA: 0x0007EA2B File Offset: 0x0007CC2B
		// (set) Token: 0x06002976 RID: 10614 RVA: 0x0007EA32 File Offset: 0x0007CC32
		public static PlayerDieHeadParts instance { get; private set; }

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002977 RID: 10615 RVA: 0x0007EA3A File Offset: 0x0007CC3A
		public DroppedParts parts
		{
			get
			{
				return this._parts;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002978 RID: 10616 RVA: 0x0007EA42 File Offset: 0x0007CC42
		// (set) Token: 0x06002979 RID: 10617 RVA: 0x0007EA4F File Offset: 0x0007CC4F
		public Sprite sprite
		{
			get
			{
				return this._spriteRenderer.sprite;
			}
			set
			{
				this._spriteRenderer.sprite = value;
			}
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x0007EA5D File Offset: 0x0007CC5D
		private void Awake()
		{
			PlayerDieHeadParts.instance = this;
		}

		// Token: 0x040023A5 RID: 9125
		[SerializeField]
		[GetComponent]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040023A6 RID: 9126
		[SerializeField]
		private DroppedParts _parts;
	}
}
