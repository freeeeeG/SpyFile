using System;
using System.Collections;
using UnityEngine;

namespace flanne.Pickups
{
	// Token: 0x02000180 RID: 384
	public class Pickup : MonoBehaviour
	{
		// Token: 0x0600096A RID: 2410 RVA: 0x00002F51 File Offset: 0x00001151
		protected virtual void UsePickup(GameObject pickupper)
		{
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x000267F0 File Offset: 0x000249F0
		private void OnTriggerEnter2D(Collider2D other)
		{
			if ((other.tag == "Pickupper" || other.tag == "MapBounds") && this.pickUpCoroutine == null)
			{
				this.pickUpCoroutine = this.PickupCR(PlayerController.Instance.gameObject);
				base.StartCoroutine(this.pickUpCoroutine);
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0002684C File Offset: 0x00024A4C
		private IEnumerator PickupCR(GameObject pickupper)
		{
			base.transform.SetParent(pickupper.transform);
			int tweenID = LeanTween.moveLocal(base.gameObject, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInBack).id;
			while (LeanTween.isTweening(tweenID))
			{
				yield return null;
			}
			this.UsePickup(pickupper);
			if (this.soundFX != null)
			{
				this.soundFX.Play(null);
			}
			this.pickUpCoroutine = null;
			base.transform.SetParent(ObjectPooler.SharedInstance.transform);
			base.transform.localPosition = Vector3.zero;
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x040006D8 RID: 1752
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x040006D9 RID: 1753
		private IEnumerator pickUpCoroutine;
	}
}
