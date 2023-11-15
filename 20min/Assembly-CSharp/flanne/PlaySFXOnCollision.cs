using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200006B RID: 107
	public class PlaySFXOnCollision : MonoBehaviour
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x0001764C File Offset: 0x0001584C
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.tag.Contains(this.hitTag))
			{
				this.soundFX.Play(null);
			}
		}

		// Token: 0x04000297 RID: 663
		[SerializeField]
		private string hitTag;

		// Token: 0x04000298 RID: 664
		[SerializeField]
		private SoundEffectSO soundFX;
	}
}
