using System;
using System.Collections.Generic;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DD5 RID: 3541
	public class BuffToTargets : CharacterOperation
	{
		// Token: 0x0600470F RID: 18191 RVA: 0x000CE4E0 File Offset: 0x000CC6E0
		private void Awake()
		{
			using (List<Character>.Enumerator enumerator = this._targets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Character target = enumerator.Current;
					target.health.onDied += delegate()
					{
						this._targets.Remove(target);
					};
				}
			}
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x000CE55C File Offset: 0x000CC75C
		public override void Run(Character owner)
		{
			Stat.ValuesWithEvent.OnDetachDelegate onDetach = null;
			Character character = this.SelectTarget();
			if (this._effect != null)
			{
				PoolObject spawnedEffect = this._effect.Spawn(character.transform.position, true);
				VisualEffect.PostProcess(spawnedEffect, character, 1f, 0f, Vector3.zero, true, true, true);
				SpriteRenderer component = spawnedEffect.GetComponent<SpriteRenderer>();
				SpriteRenderer mainRenderer = character.spriteEffectStack.mainRenderer;
				component.sortingLayerID = mainRenderer.sortingLayerID;
				component.sortingOrder = mainRenderer.sortingOrder + this._offset;
				onDetach = delegate(Stat s)
				{
					spawnedEffect.Despawn();
				};
			}
			character.stat.AttachOrUpdateTimedValues(this._stat, this._duration, onDetach);
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x000CE61C File Offset: 0x000CC81C
		private Character SelectTarget()
		{
			return this._targets.Random<Character>();
		}

		// Token: 0x040035F2 RID: 13810
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x040035F3 RID: 13811
		[SerializeField]
		private PoolObject _effect;

		// Token: 0x040035F4 RID: 13812
		[SerializeField]
		private int _offset = 1;

		// Token: 0x040035F5 RID: 13813
		[SerializeField]
		private float _duration = 1f;

		// Token: 0x040035F6 RID: 13814
		[SerializeField]
		private List<Character> _targets;
	}
}
