using System;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DD3 RID: 3539
	public class BuffToTarget : CharacterOperation
	{
		// Token: 0x0600470B RID: 18187 RVA: 0x000CE3EC File Offset: 0x000CC5EC
		public override void Run(Character owner)
		{
			Stat.ValuesWithEvent.OnDetachDelegate onDetach = null;
			if (this._effect != null)
			{
				PoolObject spawnedEffect = this._effect.Spawn(this._target.transform.position, true);
				VisualEffect.PostProcess(spawnedEffect, this._target, 1f, 0f, Vector3.zero, true, true, true);
				SpriteRenderer component = spawnedEffect.GetComponent<SpriteRenderer>();
				SpriteRenderer mainRenderer = this._target.spriteEffectStack.mainRenderer;
				component.sortingLayerID = mainRenderer.sortingLayerID;
				component.sortingOrder = mainRenderer.sortingOrder + this._offset;
				onDetach = delegate(Stat s)
				{
					spawnedEffect.Despawn();
				};
			}
			this._target.stat.AttachOrUpdateTimedValues(this._stat, this._duration, onDetach);
		}

		// Token: 0x040035EC RID: 13804
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x040035ED RID: 13805
		[SerializeField]
		private PoolObject _effect;

		// Token: 0x040035EE RID: 13806
		[SerializeField]
		private int _offset = 1;

		// Token: 0x040035EF RID: 13807
		[SerializeField]
		private float _duration = 1f;

		// Token: 0x040035F0 RID: 13808
		[SerializeField]
		private Character _target;
	}
}
