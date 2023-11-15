using System;
using System.Collections;
using UnityEngine;

namespace KevinIglesias
{
	// Token: 0x020000D0 RID: 208
	public class ChangeSpear : MonoBehaviour
	{
		// Token: 0x060002FD RID: 765 RVA: 0x0000D3A2 File Offset: 0x0000B5A2
		public void Start()
		{
			this.characterRoot = base.transform;
			this.zeroPosition = this.spear.localPosition;
			this.zeroRotation = this.spear.localEulerAngles;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000D3D2 File Offset: 0x0000B5D2
		public void Update()
		{
			if (this.retargeter.localPosition.y > 0f)
			{
				if (!this.changeActive)
				{
					this.DoChangeSpear();
					this.changeActive = true;
					return;
				}
			}
			else
			{
				this.changeActive = false;
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000D408 File Offset: 0x0000B608
		public void DoChangeSpear()
		{
			if (this.changeCO != null)
			{
				base.StopCoroutine(this.changeCO);
			}
			this.changeCO = this.StartChange();
			base.StartCoroutine(this.changeCO);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000D437 File Offset: 0x0000B637
		private IEnumerator StartChange()
		{
			this.spear.SetParent(this.characterRoot);
			this.startPosition = this.spear.position;
			this.startRotation = this.spear.localRotation;
			this.endPosition = new Vector3(this.spear.position.x, this.spear.position.y + 0.2f, this.spear.position.z);
			float yRotation = 0.7f;
			if (this.secondTime)
			{
				yRotation = -0.25f;
			}
			float i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * 5f;
				this.spear.position = Vector3.Lerp(this.startPosition, this.endPosition, Mathf.Sin(i * 3.1415927f * 0.5f));
				this.spear.transform.Rotate(0.7f, yRotation, 0f, Space.World);
				yield return 0;
			}
			this.startPosition = new Vector3(this.startPosition.x, this.startPosition.y, this.startPosition.z);
			i = 0f;
			while (i < 1f)
			{
				i += Time.deltaTime * 5f;
				this.spear.position = Vector3.Lerp(this.endPosition, this.startPosition, 1f - Mathf.Cos(i * 3.1415927f * 0.5f));
				this.spear.transform.Rotate(0.7f, yRotation, 0f, Space.World);
				yield return 0;
			}
			this.spear.SetParent(this.hand);
			this.spear.localPosition = this.zeroPosition;
			this.spear.localEulerAngles = this.zeroRotation;
			if (!this.secondTime)
			{
				this.spear.localEulerAngles = new Vector3(this.spear.localEulerAngles.x + 180f, this.spear.localEulerAngles.y, this.spear.localEulerAngles.z);
				this.secondTime = true;
			}
			else
			{
				this.secondTime = false;
			}
			yield break;
		}

		// Token: 0x04000291 RID: 657
		public Transform retargeter;

		// Token: 0x04000292 RID: 658
		public Transform spear;

		// Token: 0x04000293 RID: 659
		public Transform hand;

		// Token: 0x04000294 RID: 660
		public bool changeActive;

		// Token: 0x04000295 RID: 661
		public bool secondTime;

		// Token: 0x04000296 RID: 662
		private Transform characterRoot;

		// Token: 0x04000297 RID: 663
		private Vector3 zeroPosition;

		// Token: 0x04000298 RID: 664
		private Vector3 zeroRotation;

		// Token: 0x04000299 RID: 665
		private Vector3 startPosition;

		// Token: 0x0400029A RID: 666
		private Quaternion startRotation;

		// Token: 0x0400029B RID: 667
		private Vector3 endPosition;

		// Token: 0x0400029C RID: 668
		private Quaternion endRotation;

		// Token: 0x0400029D RID: 669
		private IEnumerator changeCO;
	}
}
