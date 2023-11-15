using System;
using System.Collections;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000D2 RID: 210
	public class ThrowBigAxe : MonoBehaviour
	{
		// Token: 0x06000307 RID: 775 RVA: 0x0000D556 File Offset: 0x0000B756
		public void Awake()
		{
			this.characterRoot = base.transform;
			this.zeroPosition = this.propToSpin.localPosition;
			this.zeroRotation = this.propToSpin.localRotation;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000D588 File Offset: 0x0000B788
		public void Update()
		{
			if (this.retargeter.localPosition.y > 0f)
			{
				if (!this.spinActive)
				{
					this.SpinProp();
					this.spinActive = true;
					return;
				}
			}
			else
			{
				if (this.spinActive)
				{
					if (this.spinCO != null)
					{
						base.StopCoroutine(this.spinCO);
					}
					this.propToSpin.SetParent(this.hand);
					this.propToSpin.localPosition = this.zeroPosition;
					this.propToSpin.localRotation = this.zeroRotation;
				}
				this.spinActive = false;
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000D618 File Offset: 0x0000B818
		public void SpinProp()
		{
			if (this.spinCO != null)
			{
				base.StopCoroutine(this.spinCO);
			}
			this.spinCO = this.StartSpin();
			base.StartCoroutine(this.spinCO);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000D647 File Offset: 0x0000B847
		private IEnumerator StartSpin()
		{
			this.propToSpin.SetParent(this.characterRoot);
			this.startPosition = this.propToSpin.position;
			this.startRotation = this.propToSpin.localRotation;
			this.endPosition = new Vector3(this.propToSpin.position.x - this.spinDistance, this.propToSpin.position.y, this.propToSpin.position.z);
			this.endPosition += this.endPositionOffset;
			float i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * this.translationSpeed;
				this.propToSpin.position = Vector3.Lerp(this.startPosition, this.endPosition, Mathf.Sin(i * 3.1415927f * 0.5f));
				this.propToSpin.transform.Rotate(0f, -this.spinSpeed, 0f, Space.World);
				yield return 0;
			}
			i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * this.translationSpeed;
				this.propToSpin.position = Vector3.Lerp(this.endPosition, this.startPosition + this.returningPositionOffset, 1f - Mathf.Cos(i * 3.1415927f * 0.5f));
				this.propToSpin.transform.Rotate(0f, -this.spinSpeed, 0f, Space.World);
				yield return 0;
			}
			yield break;
		}

		// Token: 0x040002AD RID: 685
		public Transform retargeter;

		// Token: 0x040002AE RID: 686
		public Transform propToSpin;

		// Token: 0x040002AF RID: 687
		public Transform hand;

		// Token: 0x040002B0 RID: 688
		public float spinDistance;

		// Token: 0x040002B1 RID: 689
		public float translationSpeed;

		// Token: 0x040002B2 RID: 690
		public float spinSpeed;

		// Token: 0x040002B3 RID: 691
		public bool spinActive;

		// Token: 0x040002B4 RID: 692
		public Vector3 endPositionOffset;

		// Token: 0x040002B5 RID: 693
		public Vector3 returningPositionOffset;

		// Token: 0x040002B6 RID: 694
		private Transform characterRoot;

		// Token: 0x040002B7 RID: 695
		private Vector3 zeroPosition;

		// Token: 0x040002B8 RID: 696
		private Quaternion zeroRotation;

		// Token: 0x040002B9 RID: 697
		private Vector3 startPosition;

		// Token: 0x040002BA RID: 698
		private Quaternion startRotation;

		// Token: 0x040002BB RID: 699
		private Vector3 endPosition;

		// Token: 0x040002BC RID: 700
		private Quaternion endRotation;

		// Token: 0x040002BD RID: 701
		private IEnumerator spinCO;
	}
}
