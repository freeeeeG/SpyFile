using System;
using FX.SpriteEffects;
using UnityEngine;

namespace FX
{
	// Token: 0x02000245 RID: 581
	public class MultipleSpriteEffectStack : MonoBehaviour, ISpriteEffectStack
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0001F7C9 File Offset: 0x0001D9C9
		public SpriteRenderer mainRenderer
		{
			get
			{
				return this._mainRenderer;
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0001F7D1 File Offset: 0x0001D9D1
		private void Awake()
		{
			this.props = new MaterialPropertyBlock();
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0001F7E0 File Offset: 0x0001D9E0
		protected virtual void LateUpdate()
		{
			for (int i = this._effects.Count - 1; i >= 0; i--)
			{
				if (!this._effects[i].Update(Chronometer.global.deltaTime))
				{
					this._effects.RemoveAt(i);
				}
			}
			for (int j = 0; j < this._renderers.Length; j++)
			{
				SpriteEffect.@default.Apply(this._renderers[j]);
				for (int k = 0; k < this._effects.Count; k++)
				{
					this._effects[k].Apply(this._renderers[j]);
				}
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0001F882 File Offset: 0x0001DA82
		public void Add(SpriteEffect effect)
		{
			this._effects.Add(effect.priority, effect);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0001F896 File Offset: 0x0001DA96
		public bool Contains(SpriteEffect effect)
		{
			return this._effects.Contains(effect);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0001F8A4 File Offset: 0x0001DAA4
		public bool Remove(SpriteEffect effect)
		{
			return this._effects.Remove(effect);
		}

		// Token: 0x04000983 RID: 2435
		private readonly PriorityList<SpriteEffect> _effects = new PriorityList<SpriteEffect>();

		// Token: 0x04000984 RID: 2436
		[SerializeField]
		private SpriteRenderer _mainRenderer;

		// Token: 0x04000985 RID: 2437
		[SerializeField]
		private SpriteRenderer[] _renderers;

		// Token: 0x04000986 RID: 2438
		private MaterialPropertyBlock props;
	}
}
