using System;
using System.Collections;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200079B RID: 1947
	public class Stuck : HitOperation
	{
		// Token: 0x060027CE RID: 10190 RVA: 0x00077E58 File Offset: 0x00076058
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
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
			spritePoolObject.poolObject.StartCoroutine(this.Despawn(spritePoolObject));
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x00077F52 File Offset: 0x00076152
		private IEnumerator Despawn(Effects.SpritePoolObject effect)
		{
			yield return Chronometer.global.WaitForSeconds(this._lifeTime);
			if (this._despawnEffect != null)
			{
				Vector2 v = new Vector2(effect.poolObject.transform.position.x + this._despawnEffectSpawnOffset.x, effect.poolObject.transform.position.y + this._despawnEffectSpawnOffset.y);
				this._despawnEffect.Spawn(v, 0f, 1f);
			}
			effect.poolObject.Despawn();
			yield break;
		}

		// Token: 0x040021EC RID: 8684
		[SerializeField]
		[Information("0일 경우 삭제 되지 않음", InformationAttribute.InformationType.Info, false)]
		private float _lifeTime;

		// Token: 0x040021ED RID: 8685
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040021EE RID: 8686
		[SerializeField]
		private Sprite _spriteToReplace;

		// Token: 0x040021EF RID: 8687
		[SerializeField]
		private EffectInfo _despawnEffect;

		// Token: 0x040021F0 RID: 8688
		[SerializeField]
		private Vector2 _despawnEffectSpawnOffset;
	}
}
