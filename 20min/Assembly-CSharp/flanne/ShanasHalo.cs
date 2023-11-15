using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000E9 RID: 233
	public class ShanasHalo : MonoBehaviour
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x0001E6A4 File Offset: 0x0001C8A4
		public void CollectPiece()
		{
			this.counter++;
			if (this.counter >= this.piecesRequired)
			{
				this.DropHalo();
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001E6C8 File Offset: 0x0001C8C8
		public void DropHalo()
		{
			Object.Instantiate<GameObject>(this.haloPrefab).transform.position = base.transform.position + new Vector3(0f, -1.5f, 0f);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001E720 File Offset: 0x0001C920
		public void ActivateSprite()
		{
			this.haloSpriteObj.SetActive(true);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x0400049C RID: 1180
		[SerializeField]
		private int piecesRequired;

		// Token: 0x0400049D RID: 1181
		[SerializeField]
		private GameObject haloPrefab;

		// Token: 0x0400049E RID: 1182
		[SerializeField]
		private GameObject haloSpriteObj;

		// Token: 0x0400049F RID: 1183
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x040004A0 RID: 1184
		private int counter = 1;
	}
}
