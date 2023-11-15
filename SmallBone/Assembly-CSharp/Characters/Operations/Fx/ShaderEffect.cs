using System;
using FX.SpriteEffects;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F1C RID: 3868
	public class ShaderEffect : CharacterOperation
	{
		// Token: 0x06004B79 RID: 19321 RVA: 0x000DE1C4 File Offset: 0x000DC3C4
		private void Awake()
		{
			if (this._colorOverlay.enabled)
			{
				this._duration = this._colorOverlay.duration;
			}
			if (this._colorBlend.enabled)
			{
				this._duration = Mathf.Max(this._duration, this._colorBlend.duration);
			}
			if (this._outline.enabled)
			{
				this._duration = Mathf.Max(this._duration, this._outline.duration);
			}
			if (this._grayScale.enabled)
			{
				this._duration = Mathf.Max(this._duration, this._grayScale.duration);
			}
		}

		// Token: 0x06004B7A RID: 19322 RVA: 0x000DE26C File Offset: 0x000DC46C
		public override void Run(Character owner)
		{
			this._owner = owner;
			if (this._owner.spriteEffectStack == null)
			{
				return;
			}
			this._effect = new GenericSpriteEffect(this._priority, this._duration, this._proportionalToTenacity ? ((float)owner.stat.GetFinal(Stat.Kind.StoppingResistance)) : 1f, this._colorOverlay, this._colorBlend, this._outline, this._grayScale);
			this._owner.spriteEffectStack.Add(this._effect);
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x000DE2F3 File Offset: 0x000DC4F3
		public override void Stop()
		{
			if (this._effect == null || this._owner.spriteEffectStack == null)
			{
				return;
			}
			this._owner.spriteEffectStack.Remove(this._effect);
		}

		// Token: 0x04003AB0 RID: 15024
		[SerializeField]
		private int _priority;

		// Token: 0x04003AB1 RID: 15025
		[SerializeField]
		private bool _proportionalToTenacity;

		// Token: 0x04003AB2 RID: 15026
		[SerializeField]
		private GenericSpriteEffect.ColorOverlay _colorOverlay;

		// Token: 0x04003AB3 RID: 15027
		[SerializeField]
		private GenericSpriteEffect.ColorBlend _colorBlend;

		// Token: 0x04003AB4 RID: 15028
		[SerializeField]
		private GenericSpriteEffect.Outline _outline;

		// Token: 0x04003AB5 RID: 15029
		[SerializeField]
		private GenericSpriteEffect.GrayScale _grayScale;

		// Token: 0x04003AB6 RID: 15030
		private float _duration;

		// Token: 0x04003AB7 RID: 15031
		private Character _owner;

		// Token: 0x04003AB8 RID: 15032
		private GenericSpriteEffect _effect;
	}
}
