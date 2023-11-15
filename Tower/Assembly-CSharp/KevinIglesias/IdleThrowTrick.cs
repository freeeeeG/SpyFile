using System;
using System.Collections;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000D1 RID: 209
	public class IdleThrowTrick : MonoBehaviour
	{
		// Token: 0x06000302 RID: 770 RVA: 0x0000D44E File Offset: 0x0000B64E
		public void Start()
		{
			this.characterRoot = base.transform;
			this.zeroPosition = this.propToThrow.localPosition;
			this.zeroRotation = this.propToThrow.localRotation;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000D480 File Offset: 0x0000B680
		public void Update()
		{
			if (this.retargeter.localPosition.y > 0f)
			{
				if (!this.trickActive)
				{
					this.SpinProp();
					this.trickActive = true;
					return;
				}
			}
			else
			{
				if (this.trickActive)
				{
					if (this.spinCO != null)
					{
						base.StopCoroutine(this.spinCO);
					}
					this.propToThrow.SetParent(this.hand);
					this.propToThrow.localPosition = this.zeroPosition;
					this.propToThrow.localRotation = this.zeroRotation;
				}
				this.trickActive = false;
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000D510 File Offset: 0x0000B710
		public void SpinProp()
		{
			if (this.spinCO != null)
			{
				base.StopCoroutine(this.spinCO);
			}
			this.spinCO = this.StartSpin();
			base.StartCoroutine(this.spinCO);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000D53F File Offset: 0x0000B73F
		private IEnumerator StartSpin()
		{
			this.startPosition = this.propToThrow.position;
			this.startRotation = this.propToThrow.localRotation;
			this.endPosition = new Vector3(this.propToThrow.position.x, this.propToThrow.position.y + this.trickDistance, this.propToThrow.position.z);
			this.propToThrow.SetParent(this.characterRoot);
			float i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * this.trickTranslationSpeed;
				this.propToThrow.position = Vector3.Lerp(this.startPosition, this.endPosition, Mathf.Sin(i * 3.1415927f * 0.5f));
				this.propToThrow.transform.Rotate(0f, 0f, -this.trickRotationSpeed, Space.World);
				yield return 0;
			}
			this.startPosition = new Vector3(this.startPosition.x, this.startPosition.y - 0.11f, this.startPosition.z);
			i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * this.trickTranslationSpeed;
				this.propToThrow.position = Vector3.Lerp(this.endPosition, this.startPosition, 1f - Mathf.Cos(i * 3.1415927f * 0.5f));
				this.propToThrow.transform.Rotate(0f, 0f, -this.trickRotationSpeed, Space.World);
				yield return 0;
			}
			yield break;
		}

		// Token: 0x0400029E RID: 670
		public Transform retargeter;

		// Token: 0x0400029F RID: 671
		public Transform propToThrow;

		// Token: 0x040002A0 RID: 672
		public Transform hand;

		// Token: 0x040002A1 RID: 673
		public float trickDistance;

		// Token: 0x040002A2 RID: 674
		public float trickTranslationSpeed;

		// Token: 0x040002A3 RID: 675
		public float trickRotationSpeed;

		// Token: 0x040002A4 RID: 676
		public bool trickActive;

		// Token: 0x040002A5 RID: 677
		private Transform characterRoot;

		// Token: 0x040002A6 RID: 678
		private Vector3 zeroPosition;

		// Token: 0x040002A7 RID: 679
		private Quaternion zeroRotation;

		// Token: 0x040002A8 RID: 680
		private Vector3 startPosition;

		// Token: 0x040002A9 RID: 681
		private Quaternion startRotation;

		// Token: 0x040002AA RID: 682
		private Vector3 endPosition;

		// Token: 0x040002AB RID: 683
		private Quaternion endRotation;

		// Token: 0x040002AC RID: 684
		private IEnumerator spinCO;
	}
}
