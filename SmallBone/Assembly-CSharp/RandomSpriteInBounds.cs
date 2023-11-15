using System;
using System.Collections;
using Characters.Projectiles;
using FX;
using UnityEngine;

// Token: 0x02000072 RID: 114
public class RandomSpriteInBounds : MonoBehaviour
{
	// Token: 0x06000211 RID: 529 RVA: 0x00009094 File Offset: 0x00007294
	private void Awake()
	{
		if (this._projectile == null)
		{
			this._iProjectile = base.GetComponentInParent<IProjectile>();
		}
		else
		{
			this._iProjectile = this._projectile;
		}
		this._spriteRenderers = new SpriteRenderer[this._positions.Length];
		for (int i = 0; i < this._positions.Length; i++)
		{
			this._spriteRenderers[i] = this._positions[i].GetComponent<SpriteRenderer>();
		}
	}

	// Token: 0x06000212 RID: 530 RVA: 0x00009104 File Offset: 0x00007304
	private void OnEnable()
	{
		this.DeactiveAll();
		int count = (int)this._countRange.value;
		this.Activate(count);
		base.StartCoroutine(this.CEffects(count));
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000913C File Offset: 0x0000733C
	private void Activate(int count)
	{
		for (int i = 0; i < count; i++)
		{
			Vector2 b = UnityEngine.Random.insideUnitCircle * 2f;
			this._positions[i].position = this._origin.position + b;
			this._spriteRenderers[i].sprite = this._sprites.Random<Sprite>();
			this._spriteRenderers[i].enabled = true;
		}
	}

	// Token: 0x06000214 RID: 532 RVA: 0x000091B3 File Offset: 0x000073B3
	private IEnumerator CEffects(int count)
	{
		yield return null;
		for (int i = 0; i < count; i++)
		{
			this._spawnEffect.Spawn(this._positions[i].position, this._iProjectile.owner, 0f, 1f);
		}
		yield break;
	}

	// Token: 0x06000215 RID: 533 RVA: 0x000091CC File Offset: 0x000073CC
	private void DeactiveAll()
	{
		for (int i = 0; i < this._positions.Length; i++)
		{
			this._spriteRenderers[i].enabled = false;
		}
	}

	// Token: 0x040001C6 RID: 454
	[SerializeField]
	private Projectile _projectile;

	// Token: 0x040001C7 RID: 455
	[SerializeField]
	private Transform _origin;

	// Token: 0x040001C8 RID: 456
	[SerializeField]
	private EffectInfo _spawnEffect;

	// Token: 0x040001C9 RID: 457
	[SerializeField]
	private Sprite[] _sprites;

	// Token: 0x040001CA RID: 458
	[SerializeField]
	private CustomFloat _countRange;

	// Token: 0x040001CB RID: 459
	[SerializeField]
	private BoxCollider2D _range;

	// Token: 0x040001CC RID: 460
	[SerializeField]
	private Transform[] _positions;

	// Token: 0x040001CD RID: 461
	private SpriteRenderer[] _spriteRenderers;

	// Token: 0x040001CE RID: 462
	private IProjectile _iProjectile;
}
