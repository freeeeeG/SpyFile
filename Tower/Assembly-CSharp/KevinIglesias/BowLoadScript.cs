using System;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000C6 RID: 198
	public class BowLoadScript : MonoBehaviour
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x0000CD9C File Offset: 0x0000AF9C
		private void Awake()
		{
			if (this.bow != null)
			{
				this.bowSkinnedMeshRenderer = this.bow.GetComponent<SkinnedMeshRenderer>();
			}
			if (this.arrowToDraw != null)
			{
				this.arrowToDraw.gameObject.SetActive(false);
			}
			if (this.arrowToShoot != null)
			{
				this.arrowToShoot.gameObject.SetActive(false);
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000CE08 File Offset: 0x0000B008
		private void Update()
		{
			if (this.bowSkinnedMeshRenderer != null && this.bow != null && this.bowHandRetargeter != null)
			{
				float num = Mathf.InverseLerp(0f, -1f, this.bowHandRetargeter.localPosition.z);
				this.bowSkinnedMeshRenderer.SetBlendShapeWeight(0, num * 100f);
			}
			if (this.arrowToDraw != null && this.arrowToShoot != null && this.arrowHandRetargeter != null)
			{
				if (this.arrowHandRetargeter.localPosition.y <= 0.99f)
				{
					if (this.arrowToShoot != null && this.arrowToDraw != null)
					{
						this.arrowToDraw.gameObject.SetActive(false);
						this.arrowToShoot.gameObject.SetActive(false);
						this.arrowOnHand = false;
						return;
					}
				}
				else
				{
					if (this.arrowHandRetargeter.localPosition.y <= 1.01f)
					{
						this.arrowToShoot.gameObject.SetActive(true);
						this.arrowToDraw.gameObject.SetActive(false);
						this.arrowOnHand = true;
						return;
					}
					if (this.arrowToShoot != null && this.arrowToDraw != null)
					{
						this.arrowToDraw.gameObject.SetActive(true);
						this.arrowToShoot.gameObject.SetActive(false);
						this.arrowOnHand = true;
					}
				}
			}
		}

		// Token: 0x0400026A RID: 618
		public Transform bow;

		// Token: 0x0400026B RID: 619
		public Transform arrowHandRetargeter;

		// Token: 0x0400026C RID: 620
		public Transform bowHandRetargeter;

		// Token: 0x0400026D RID: 621
		private SkinnedMeshRenderer bowSkinnedMeshRenderer;

		// Token: 0x0400026E RID: 622
		public bool arrowOnHand;

		// Token: 0x0400026F RID: 623
		public Transform arrowToDraw;

		// Token: 0x04000270 RID: 624
		public Transform arrowToShoot;
	}
}
