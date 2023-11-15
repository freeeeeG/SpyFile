using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F1A RID: 3866
	public sealed class SetOwnerColor : CharacterOperation
	{
		// Token: 0x06004B6E RID: 19310 RVA: 0x000DE000 File Offset: 0x000DC200
		public override void Run(Character owner)
		{
			if (owner.spriteEffectStack == null || owner.spriteEffectStack.mainRenderer == null)
			{
				return;
			}
			this._owner = owner;
			this._originColor = owner.spriteEffectStack.mainRenderer.color;
			if (this._curve.duration == 0f)
			{
				owner.spriteEffectStack.mainRenderer.color = this._color;
				return;
			}
			this._changeReference = this.StartCoroutineWithReference(this.CChangeColor());
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x000DE081 File Offset: 0x000DC281
		private IEnumerator CChangeColor()
		{
			float elapsed = 0f;
			SpriteRenderer renderer = this._owner.spriteEffectStack.mainRenderer;
			while (elapsed <= this._curve.duration)
			{
				renderer.color = Color.Lerp(this._originColor, this._color, this._curve.Evaluate(elapsed / this._curve.duration));
				elapsed += this._owner.chronometer.master.deltaTime;
				yield return null;
			}
			renderer.color = this._originColor;
			yield break;
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x000DE090 File Offset: 0x000DC290
		public override void Stop()
		{
			base.Stop();
			this._changeReference.Stop();
			this._owner.spriteEffectStack.mainRenderer.color = this._originColor;
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x000CD13F File Offset: 0x000CB33F
		private void OnDisable()
		{
			this.Stop();
		}

		// Token: 0x04003AA6 RID: 15014
		[SerializeField]
		private Color _color;

		// Token: 0x04003AA7 RID: 15015
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003AA8 RID: 15016
		private Character _owner;

		// Token: 0x04003AA9 RID: 15017
		private Color _originColor;

		// Token: 0x04003AAA RID: 15018
		private CoroutineReference _changeReference;
	}
}
