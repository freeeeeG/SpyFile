using System;
using FX;
using FX.SpriteEffects;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F0F RID: 3855
	public class ColorChangeEffect : CharacterOperation
	{
		// Token: 0x06004B4C RID: 19276 RVA: 0x000DD9E0 File Offset: 0x000DBBE0
		private void Awake()
		{
			this._originalDuration = this._curve.duration;
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x000DD9F4 File Offset: 0x000DBBF4
		public override void Run(Character owner)
		{
			if (this._proportionalToTenacity)
			{
				this._curve.duration = this._originalDuration * (float)owner.stat.GetFinal(Stat.Kind.StoppingResistance);
			}
			ISpriteEffectStack spriteEffectStack = owner.spriteEffectStack;
			if (spriteEffectStack == null)
			{
				return;
			}
			spriteEffectStack.Add(new EasedColorOverlay(0, this._startColor, this._endColor, this._curve));
		}

		// Token: 0x04003A78 RID: 14968
		private const int priority = 0;

		// Token: 0x04003A79 RID: 14969
		[SerializeField]
		private Color _startColor = Color.white;

		// Token: 0x04003A7A RID: 14970
		[SerializeField]
		private Color _endColor = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04003A7B RID: 14971
		[SerializeField]
		private Curve _curve;

		// Token: 0x04003A7C RID: 14972
		[SerializeField]
		private bool _proportionalToTenacity;

		// Token: 0x04003A7D RID: 14973
		private float _originalDuration;
	}
}
