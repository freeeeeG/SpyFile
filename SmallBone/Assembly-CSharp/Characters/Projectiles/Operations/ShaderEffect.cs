using System;
using FX.SpriteEffects;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x02000794 RID: 1940
	public sealed class ShaderEffect : CharacterHitOperation
	{
		// Token: 0x060027B5 RID: 10165 RVA: 0x000773F1 File Offset: 0x000755F1
		private void Awake()
		{
			this._duration = Mathf.Max(new float[]
			{
				this._colorOverlay.duration,
				this._colorBlend.duration,
				this._outline.duration
			});
		}

		// Token: 0x060027B6 RID: 10166 RVA: 0x00077430 File Offset: 0x00075630
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
		{
			if (character.spriteEffectStack == null)
			{
				return;
			}
			this._effect = new GenericSpriteEffect(this._priority, this._duration, this._proportionalToTenacity ? ((float)character.stat.GetFinal(Stat.Kind.StoppingResistance)) : 1f, this._colorOverlay, this._colorBlend, this._outline, this._grayScale);
			character.spriteEffectStack.Add(this._effect);
		}

		// Token: 0x040021CA RID: 8650
		[SerializeField]
		private int _priority;

		// Token: 0x040021CB RID: 8651
		[SerializeField]
		private bool _proportionalToTenacity;

		// Token: 0x040021CC RID: 8652
		[SerializeField]
		private GenericSpriteEffect.ColorOverlay _colorOverlay;

		// Token: 0x040021CD RID: 8653
		[SerializeField]
		private GenericSpriteEffect.ColorBlend _colorBlend;

		// Token: 0x040021CE RID: 8654
		[SerializeField]
		private GenericSpriteEffect.Outline _outline;

		// Token: 0x040021CF RID: 8655
		[SerializeField]
		private GenericSpriteEffect.GrayScale _grayScale;

		// Token: 0x040021D0 RID: 8656
		private float _duration;

		// Token: 0x040021D1 RID: 8657
		private GenericSpriteEffect _effect;
	}
}
