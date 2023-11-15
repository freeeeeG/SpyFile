using System;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DB2 RID: 3506
	[Obsolete("이거 대신 AttachAbility 사용하세요")]
	public class BonusStats : CharacterOperation
	{
		// Token: 0x0600469A RID: 18074 RVA: 0x000CC2C4 File Offset: 0x000CA4C4
		public override void Run(Character owner)
		{
			this._character = owner;
			Stat.ValuesWithEvent.OnDetachDelegate onDetach = null;
			if (this._effect != null)
			{
				PoolObject spawnedEffect = this._effect.Spawn(owner.transform.position, true);
				VisualEffect.PostProcess(spawnedEffect, owner, 1f, 0f, Vector3.zero, true, true, true);
				SpriteRenderer component = spawnedEffect.GetComponent<SpriteRenderer>();
				SpriteRenderer mainRenderer = owner.spriteEffectStack.mainRenderer;
				component.sortingLayerID = mainRenderer.sortingLayerID;
				component.sortingOrder = mainRenderer.sortingOrder + this._offset;
				onDetach = delegate(Stat s)
				{
					spawnedEffect.Despawn();
				};
			}
			if (this._duration == 0f)
			{
				if (!owner.stat.Contains(this._stat))
				{
					owner.stat.AttachValues(this._stat, onDetach);
					return;
				}
			}
			else
			{
				owner.stat.AttachOrUpdateTimedValues(this._stat, this._duration, onDetach);
			}
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x000CC3B7 File Offset: 0x000CA5B7
		public override void Stop()
		{
			Character character = this._character;
			if (character == null)
			{
				return;
			}
			character.stat.DetachValues(this._stat);
		}

		// Token: 0x04003579 RID: 13689
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x0400357A RID: 13690
		[SerializeField]
		private PoolObject _effect;

		// Token: 0x0400357B RID: 13691
		[SerializeField]
		private int _offset = 1;

		// Token: 0x0400357C RID: 13692
		[SerializeField]
		private float _duration = 1f;

		// Token: 0x0400357D RID: 13693
		private Character _character;
	}
}
