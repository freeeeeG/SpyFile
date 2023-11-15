using System;
using System.Collections;
using Level.Traps;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011ED RID: 4589
	public class Escort : Trap
	{
		// Token: 0x06005A15 RID: 23061 RVA: 0x0010BB44 File Offset: 0x00109D44
		private void Start()
		{
			float num = 0f;
			EscortOrb[] orbs = this._orbs;
			for (int i = 0; i < orbs.Length; i++)
			{
				orbs[i].Initialize(num);
				num += 6.2831855f / (float)this._orbs.Length;
			}
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x0001913A File Offset: 0x0001733A
		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x0010BB88 File Offset: 0x00109D88
		private void Update()
		{
			EscortOrb[] orbs = this._orbs;
			for (int i = 0; i < orbs.Length; i++)
			{
				orbs[i].Move(this._radius);
			}
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x0010BBB8 File Offset: 0x00109DB8
		public void ChangeSize()
		{
			if (this._converged)
			{
				base.StartCoroutine(this.CChangeSizeUp());
				return;
			}
			base.StartCoroutine(this.CChangeSizeDown());
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x0010BBDD File Offset: 0x00109DDD
		private IEnumerator CChangeSize()
		{
			for (;;)
			{
				this._elapsed = 0f;
				while (this._elapsed < this._sizeChangeDuration)
				{
					this._elapsed += Chronometer.global.deltaTime;
					yield return null;
				}
				yield return this.CChangeSizeUp();
				this._elapsed = 0f;
				while (this._elapsed < this._sizeChangeDuration)
				{
					this._elapsed += Chronometer.global.deltaTime;
					yield return null;
				}
				yield return this.CChangeSizeDown();
			}
			yield break;
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x0010BBEC File Offset: 0x00109DEC
		private IEnumerator CChangeSizeUp()
		{
			float elapsed = 0f;
			float duration = 1f;
			float start = this._radius;
			float end = this._radius * 3f;
			while (elapsed < duration)
			{
				elapsed += Chronometer.global.deltaTime;
				this._radius = Mathf.Lerp(start, end, elapsed / duration);
				yield return null;
			}
			this._converged = false;
			yield break;
		}

		// Token: 0x06005A1B RID: 23067 RVA: 0x0010BBFB File Offset: 0x00109DFB
		private IEnumerator CChangeSizeDown()
		{
			float elapsed = 0f;
			float duration = 1f;
			float start = this._radius;
			float end = this._radius / 3f;
			while (elapsed < duration)
			{
				elapsed += Chronometer.global.deltaTime;
				this._radius = Mathf.Lerp(start, end, elapsed / duration);
				yield return null;
			}
			this._converged = true;
			yield break;
		}

		// Token: 0x040048C3 RID: 18627
		[SerializeField]
		private float _radius = 10f;

		// Token: 0x040048C4 RID: 18628
		[SerializeField]
		private float _sizeChangeDuration = 15f;

		// Token: 0x040048C5 RID: 18629
		[SerializeField]
		private EscortOrb[] _orbs;

		// Token: 0x040048C6 RID: 18630
		private float _elapsed;

		// Token: 0x040048C7 RID: 18631
		private bool _converged = true;
	}
}
