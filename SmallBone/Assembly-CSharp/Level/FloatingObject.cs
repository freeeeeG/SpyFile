using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Level
{
	// Token: 0x02000485 RID: 1157
	public sealed class FloatingObject : MonoBehaviour
	{
		// Token: 0x06001604 RID: 5636 RVA: 0x00044F90 File Offset: 0x00043190
		private void Awake()
		{
			this._awakePosition = base.transform.localPosition;
			this._awakeRotation = base.transform.localRotation;
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00044FB9 File Offset: 0x000431B9
		private void OnEnable()
		{
			if (this._startOnEnable)
			{
				base.StartCoroutine(this.CFloat());
			}
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x00044FD0 File Offset: 0x000431D0
		public void Float()
		{
			base.StartCoroutine(this.CFloat());
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x00044FDF File Offset: 0x000431DF
		private IEnumerator CFloat()
		{
			FloatingObject.<>c__DisplayClass10_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.verticalAmount = (this._vertical.awakePointRandom ? UnityEngine.Random.Range(0f, 6.2831855f) : 0f);
			CS$<>8__locals1.horizontalAmount = (this._horizontal.awakePointRandom ? UnityEngine.Random.Range(0f, 6.2831855f) : 0f);
			CS$<>8__locals1.rotationAmount = (this._rotation.awakePointRandom ? UnityEngine.Random.Range(0f, 6.2831855f) : 0f);
			if (this._vertical.directionRandom)
			{
				this._vertical.speed *= (float)(MMMaths.RandomBool() ? 1 : -1);
			}
			if (this._horizontal.directionRandom)
			{
				this._horizontal.speed *= (float)(MMMaths.RandomBool() ? 1 : -1);
			}
			if (this._rotation.directionRandom)
			{
				this._rotation.speed *= (float)(MMMaths.RandomBool() ? 1 : -1);
			}
			for (;;)
			{
				yield return null;
				this.<CFloat>g__UpdatePosition|10_0(ref CS$<>8__locals1);
				this.<CFloat>g__UpdateRotation|10_1(ref CS$<>8__locals1);
			}
			yield break;
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x00044FEE File Offset: 0x000431EE
		private float GetSineAmplitudeIn(float point)
		{
			return Mathf.Sin(point * 360f * 0.017453292f);
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00045014 File Offset: 0x00043214
		[CompilerGenerated]
		private void <CFloat>g__UpdatePosition|10_0(ref FloatingObject.<>c__DisplayClass10_0 A_1)
		{
			float deltaTime = Chronometer.global.deltaTime;
			A_1.verticalAmount += deltaTime * this._vertical.speed;
			A_1.horizontalAmount += deltaTime * this._horizontal.speed;
			float num = this.GetSineAmplitudeIn(A_1.horizontalAmount) * this._horizontal.extent;
			float num2 = this.GetSineAmplitudeIn(A_1.verticalAmount) * this._vertical.extent;
			base.transform.localPosition = new Vector2(this._awakePosition.x + num, this._awakePosition.y + num2);
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x000450C4 File Offset: 0x000432C4
		[CompilerGenerated]
		private void <CFloat>g__UpdateRotation|10_1(ref FloatingObject.<>c__DisplayClass10_0 A_1)
		{
			float deltaTime = Chronometer.global.deltaTime;
			A_1.rotationAmount += deltaTime * this._rotation.speed;
			float num = this.GetSineAmplitudeIn(A_1.rotationAmount) * this._rotation.extent;
			base.transform.localRotation = Quaternion.AngleAxis(num + this._awakeRotation.eulerAngles.z, Vector3.forward);
		}

		// Token: 0x04001346 RID: 4934
		[Header("Initial Value")]
		[SerializeField]
		private bool _startOnEnable = true;

		// Token: 0x04001347 RID: 4935
		[SerializeField]
		private FloatingObject.Setting _vertical;

		// Token: 0x04001348 RID: 4936
		[SerializeField]
		private FloatingObject.Setting _horizontal;

		// Token: 0x04001349 RID: 4937
		[SerializeField]
		private FloatingObject.Setting _rotation;

		// Token: 0x0400134A RID: 4938
		private Vector2 _awakePosition;

		// Token: 0x0400134B RID: 4939
		private Quaternion _awakeRotation;

		// Token: 0x02000486 RID: 1158
		[Serializable]
		private class Setting
		{
			// Token: 0x0400134C RID: 4940
			[SerializeField]
			internal bool directionRandom;

			// Token: 0x0400134D RID: 4941
			[SerializeField]
			internal bool awakePointRandom;

			// Token: 0x0400134E RID: 4942
			[SerializeField]
			internal float extent;

			// Token: 0x0400134F RID: 4943
			[SerializeField]
			internal float speed;
		}
	}
}
