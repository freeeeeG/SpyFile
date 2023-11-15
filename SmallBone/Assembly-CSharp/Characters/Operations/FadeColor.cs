using System;
using System.Collections;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DDA RID: 3546
	public class FadeColor : CharacterOperation
	{
		// Token: 0x0600471B RID: 18203 RVA: 0x000CE72C File Offset: 0x000CC92C
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CFade(owner));
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x000CE73C File Offset: 0x000CC93C
		private IEnumerator CFade(Character owner)
		{
			Color startColor = this._sprite.color;
			Color different = this._target - this._sprite.color;
			float time = 0f;
			while (time < this._duration)
			{
				time += owner.chronometer.master.deltaTime;
				this._sprite.color = startColor + different * (time / this._duration);
				yield return null;
			}
			this._sprite.color = this._target;
			yield break;
		}

		// Token: 0x040035FE RID: 13822
		[SerializeField]
		private SpriteRenderer _sprite;

		// Token: 0x040035FF RID: 13823
		[SerializeField]
		private Color _target;

		// Token: 0x04003600 RID: 13824
		[SerializeField]
		private float _duration;
	}
}
