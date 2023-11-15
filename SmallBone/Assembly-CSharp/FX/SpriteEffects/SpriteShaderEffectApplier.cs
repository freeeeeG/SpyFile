using System;
using UnityEngine;

namespace FX.SpriteEffects
{
	// Token: 0x02000276 RID: 630
	public class SpriteShaderEffectApplier : MonoBehaviour
	{
		// Token: 0x06000C50 RID: 3152 RVA: 0x00021D25 File Offset: 0x0001FF25
		private void Awake()
		{
			this._duration = Mathf.Max(new float[]
			{
				this._colorOverlay.duration,
				this._colorBlend.duration,
				this._outline.duration
			});
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00021D62 File Offset: 0x0001FF62
		private void Start()
		{
			if (this._playOnStart)
			{
				this.ApplyEffect();
			}
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00021D74 File Offset: 0x0001FF74
		public void ApplyEffect()
		{
			if (this._spriteEffectStack == null)
			{
				return;
			}
			this._effect = new GenericSpriteEffect(this._priority, this._duration, 1f, this._colorOverlay, this._colorBlend, this._outline, this._grayScale);
			this._spriteEffectStack.Add(this._effect);
		}

		// Token: 0x04000A57 RID: 2647
		[SerializeField]
		private SpriteEffectStack _spriteEffectStack;

		// Token: 0x04000A58 RID: 2648
		[SerializeField]
		private bool _playOnStart;

		// Token: 0x04000A59 RID: 2649
		[SerializeField]
		private int _priority;

		// Token: 0x04000A5A RID: 2650
		[SerializeField]
		private GenericSpriteEffect.ColorOverlay _colorOverlay;

		// Token: 0x04000A5B RID: 2651
		[SerializeField]
		private GenericSpriteEffect.ColorBlend _colorBlend;

		// Token: 0x04000A5C RID: 2652
		[SerializeField]
		private GenericSpriteEffect.Outline _outline;

		// Token: 0x04000A5D RID: 2653
		[SerializeField]
		private GenericSpriteEffect.GrayScale _grayScale;

		// Token: 0x04000A5E RID: 2654
		private float _duration;

		// Token: 0x04000A5F RID: 2655
		private GenericSpriteEffect _effect;
	}
}
