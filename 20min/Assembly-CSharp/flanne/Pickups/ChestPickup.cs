using System;
using System.Collections;
using UnityEngine;

namespace flanne.Pickups
{
	// Token: 0x0200017C RID: 380
	public class ChestPickup : MonoBehaviour
	{
		// Token: 0x0600095C RID: 2396 RVA: 0x0002667B File Offset: 0x0002487B
		private void Start()
		{
			this._xpFountainCoroutine = null;
			this.OP = ObjectPooler.SharedInstance;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0002668F File Offset: 0x0002488F
		private void OnEnable()
		{
			this.spriteRenderer.sprite = this.chestClosed;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x000266A4 File Offset: 0x000248A4
		private void OnTriggerEnter2D(Collider2D other)
		{
			if ((other.tag == "Player" || other.tag == "MapBounds") && this._xpFountainCoroutine == null)
			{
				this.PostNotification(ChestPickup.ChestPickupEvent, null);
				this._xpFountainCoroutine = this.XPFountainCR();
				base.StartCoroutine(this._xpFountainCoroutine);
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00026702 File Offset: 0x00024902
		private IEnumerator XPFountainCR()
		{
			yield return new WaitForSeconds(0.1f);
			this.spriteRenderer.sprite = this.chestOpen;
			int num;
			for (int i = 0; i < this.amountOfXP; i = num + 1)
			{
				GameObject pooledObject = this.OP.GetPooledObject(this.xpOPTag);
				pooledObject.transform.position = base.transform.position;
				pooledObject.SetActive(true);
				Vector3 to = new Vector3(pooledObject.transform.position.x + Random.Range(-1f, 1f), pooledObject.transform.position.y + Random.Range(-1f, 1f), 0f);
				LeanTween.move(pooledObject, to, 0.5f);
				SoundEffectSO soundEffectSO = this.xpSpawnSFX;
				if (soundEffectSO != null)
				{
					soundEffectSO.Play(null);
				}
				yield return new WaitForSeconds(0.1f);
				num = i;
			}
			this._xpFountainCoroutine = null;
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x040006CB RID: 1739
		public static string ChestPickupEvent = "ChestPickup.ChestPickupEvent";

		// Token: 0x040006CC RID: 1740
		[SerializeField]
		private int amountOfXP;

		// Token: 0x040006CD RID: 1741
		[SerializeField]
		private string xpOPTag;

		// Token: 0x040006CE RID: 1742
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x040006CF RID: 1743
		[SerializeField]
		private Sprite chestOpen;

		// Token: 0x040006D0 RID: 1744
		[SerializeField]
		private Sprite chestClosed;

		// Token: 0x040006D1 RID: 1745
		[SerializeField]
		private SoundEffectSO xpSpawnSFX;

		// Token: 0x040006D2 RID: 1746
		private IEnumerator _xpFountainCoroutine;

		// Token: 0x040006D3 RID: 1747
		private ObjectPooler OP;
	}
}
