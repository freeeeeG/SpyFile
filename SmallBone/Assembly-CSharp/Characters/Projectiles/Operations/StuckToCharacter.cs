using System;
using System.Collections;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200079D RID: 1949
	public class StuckToCharacter : CharacterHitOperation
	{
		// Token: 0x060027D7 RID: 10199 RVA: 0x00078059 File Offset: 0x00076259
		private IEnumerator CDespawn(Effects.SpritePoolObject effect, Character target)
		{
			float elapsed = 0f;
			while (elapsed < this._lifeTime && !target.health.dead)
			{
				yield return null;
				elapsed += Chronometer.global.deltaTime;
			}
			if (this._despawnEffect != null)
			{
				Vector2 v = new Vector2(effect.poolObject.transform.position.x + this._despawnEffectSpawnOffset.x, effect.poolObject.transform.position.y + this._despawnEffectSpawnOffset.y);
				this._despawnEffect.Spawn(v, 0f, 1f);
			}
			effect.poolObject.Despawn();
			yield break;
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x00078076 File Offset: 0x00076276
		private IEnumerator CSetPosiiton(Transform effect, Collider2D hitCollider, Character target)
		{
			Bounds before = hitCollider.bounds;
			effect.transform.position = hitCollider.bounds.center;
			while (effect.gameObject.activeInHierarchy && !target.health.dead)
			{
				yield return null;
				if (before != hitCollider.bounds)
				{
					effect.transform.position = hitCollider.bounds.center;
					before = hitCollider.bounds;
				}
			}
			yield break;
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x00078094 File Offset: 0x00076294
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character character)
		{
			if (this._spriteRenderer == null)
			{
				this._spriteRenderer = projectile.GetComponentInParent<SpriteRenderer>();
			}
			if (this._spriteRenderer == null)
			{
				return;
			}
			Effects.SpritePoolObject spritePoolObject = Effects.sprite.Spawn();
			spritePoolObject.spriteRenderer.CopyFrom(this._spriteRenderer);
			if (this._spriteToReplace != null)
			{
				spritePoolObject.spriteRenderer.sprite = this._spriteToReplace;
			}
			SpriteRenderer spriteRenderer = spritePoolObject.spriteRenderer;
			int sortingOrder = spriteRenderer.sortingOrder;
			spriteRenderer.sortingOrder = sortingOrder - 1;
			spritePoolObject.spriteRenderer.color = this._spriteRenderer.color;
			Transform transform = spritePoolObject.poolObject.transform;
			transform.position = raycastHit.point;
			transform.localScale = this._spriteRenderer.transform.lossyScale;
			transform.rotation = this._spriteRenderer.transform.rotation;
			spritePoolObject.poolObject.transform.SetParent(character.attachWithFlip.transform);
			spritePoolObject.poolObject.StartCoroutine(this.CDespawn(spritePoolObject, character));
			spritePoolObject.poolObject.StartCoroutine(this.CSetPosiiton(spritePoolObject.poolObject.transform, raycastHit.collider, character));
		}

		// Token: 0x040021F5 RID: 8693
		[Information("0일 경우 삭제 되지 않음", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private float _lifeTime;

		// Token: 0x040021F6 RID: 8694
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040021F7 RID: 8695
		[SerializeField]
		private Sprite _spriteToReplace;

		// Token: 0x040021F8 RID: 8696
		[SerializeField]
		private EffectInfo _despawnEffect;

		// Token: 0x040021F9 RID: 8697
		[SerializeField]
		private Vector2 _despawnEffectSpawnOffset;
	}
}
