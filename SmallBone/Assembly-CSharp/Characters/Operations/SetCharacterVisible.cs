using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E32 RID: 3634
	public class SetCharacterVisible : CharacterOperation
	{
		// Token: 0x06004875 RID: 18549 RVA: 0x000D2AEC File Offset: 0x000D0CEC
		public override void Run(Character owner)
		{
			if (this._findComponentInOwner)
			{
				this._collider = owner.collider;
				this._render = owner.spriteEffectStack.mainRenderer;
			}
			if (this._duration <= 0f)
			{
				this.SetVisible(this._visible);
				return;
			}
			base.StartCoroutine(this.CRun(owner.chronometer.master));
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x000D2B50 File Offset: 0x000D0D50
		private void SetVisible(bool visible)
		{
			if (this._collider != null)
			{
				this._collider.enabled = visible;
			}
			if (this._render != null)
			{
				this._render.enabled = visible;
			}
			if (this._extras == null)
			{
				return;
			}
			GameObject[] extras = this._extras;
			for (int i = 0; i < extras.Length; i++)
			{
				extras[i].SetActive(visible);
			}
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x000D2BB8 File Offset: 0x000D0DB8
		private IEnumerator CRun(Chronometer chronometer)
		{
			this.SetVisible(this._visible);
			if (this._duration != 0f)
			{
				yield return chronometer.WaitForSeconds(this._duration);
			}
			this.SetVisible(!this._visible);
			yield break;
		}

		// Token: 0x04003785 RID: 14213
		[SerializeField]
		private bool _visible;

		// Token: 0x04003786 RID: 14214
		[SerializeField]
		private Renderer _render;

		// Token: 0x04003787 RID: 14215
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04003788 RID: 14216
		[SerializeField]
		private bool _findComponentInOwner;

		// Token: 0x04003789 RID: 14217
		[SerializeField]
		private float _duration;

		// Token: 0x0400378A RID: 14218
		[SerializeField]
		private GameObject[] _extras;
	}
}
