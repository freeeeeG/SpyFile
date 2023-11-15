using System;
using System.Collections;
using Characters;
using Characters.Movements;
using FX;
using UnityEngine;

namespace Level
{
	// Token: 0x020004C6 RID: 1222
	public class DroppedParts : DestructibleObject, IPoolObjectCopiable<DroppedParts>
	{
		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x0004A4BF File Offset: 0x000486BF
		public DroppedParts.Priority priority
		{
			get
			{
				return this._priority;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x0004A4C7 File Offset: 0x000486C7
		public Vector2Int count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0004A4CF File Offset: 0x000486CF
		public bool randomize
		{
			get
			{
				return this._randomize;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x0004A4D7 File Offset: 0x000486D7
		public bool collideWithTerrain
		{
			get
			{
				return this._collideWithTerrain;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x0004A4DF File Offset: 0x000486DF
		public PoolObject poolObject
		{
			get
			{
				return this._poolObject;
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060017B3 RID: 6067 RVA: 0x0004A4E7 File Offset: 0x000486E7
		public SpriteRenderer spriteRenderer
		{
			get
			{
				return this._spriteRenderer;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060017B4 RID: 6068 RVA: 0x0004A4EF File Offset: 0x000486EF
		public override Collider2D collider
		{
			get
			{
				return this._circleCollider;
			}
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x0004A4F7 File Offset: 0x000486F7
		private void OnDestroy()
		{
			this._spriteRenderer.sprite = null;
			this._spriteRenderer = null;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x0004A50C File Offset: 0x0004870C
		public void Copy(DroppedParts to)
		{
			if (to._spriteRenderer.sprite != this._spriteRenderer.sprite)
			{
				to._spriteRenderer.sprite = this._spriteRenderer.sprite;
			}
			to._spriteRenderer.color = this._spriteRenderer.color;
			to._spriteRenderer.flipX = this._spriteRenderer.flipX;
			to._spriteRenderer.flipY = this._spriteRenderer.flipY;
			to._spriteRenderer.sharedMaterial = this._spriteRenderer.sharedMaterial;
			to._spriteRenderer.drawMode = this._spriteRenderer.drawMode;
			to._spriteRenderer.sortingLayerID = this._spriteRenderer.sortingLayerID;
			to._spriteRenderer.sortingOrder = this._spriteRenderer.sortingOrder;
			to._spriteRenderer.maskInteraction = this._spriteRenderer.maskInteraction;
			to._spriteRenderer.spriteSortPoint = this._spriteRenderer.spriteSortPoint;
			to._spriteRenderer.renderingLayerMask = this._spriteRenderer.renderingLayerMask;
			to._rigidbody.bodyType = this._rigidbody.bodyType;
			to._rigidbody.sharedMaterial = this._rigidbody.sharedMaterial;
			to._rigidbody.useAutoMass = this._rigidbody.useAutoMass;
			to._rigidbody.constraints = this._rigidbody.constraints;
			to._rigidbody.mass = this._rigidbody.mass;
			to._rigidbody.drag = this._rigidbody.drag;
			to._rigidbody.angularDrag = this._rigidbody.angularDrag;
			to._rigidbody.gravityScale = this._rigidbody.gravityScale;
			to._boxCollider.enabled = this._boxCollider.enabled;
			if (to._boxCollider.enabled)
			{
				to._boxCollider.offset = this._boxCollider.offset;
				to._boxCollider.size = this._boxCollider.size;
				to._boxCollider.edgeRadius = this._boxCollider.edgeRadius;
			}
			to._circleCollider.enabled = this._circleCollider.enabled;
			if (to._circleCollider.enabled)
			{
				to._circleCollider.offset = this._circleCollider.offset;
				to._circleCollider.radius = this._circleCollider.radius;
			}
			to._additionalForce = this._additionalForce;
			to.gameObject.layer = base.gameObject.layer;
			to._priority = this._priority;
			to._count = this._count;
			to._randomize = this._randomize;
			to._collideWithTerrain = this._collideWithTerrain;
			to._duration = this._duration;
			to._fadeOut = this._fadeOut;
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x0004A7F6 File Offset: 0x000489F6
		private void Awake()
		{
			this._rigidbody = base.GetComponent<Rigidbody2D>();
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x0004A804 File Offset: 0x00048A04
		public void Initialize(Push push, float multiplier = 1f, bool interpolate = true)
		{
			Vector2 vector = Vector2.zero;
			if (push != null && !push.expired)
			{
				vector = push.direction * push.totalForce;
			}
			vector *= multiplier;
			this.Initialize(vector, multiplier, interpolate);
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x0004A848 File Offset: 0x00048A48
		public void Initialize(Vector2 force, float multiplier = 1f, bool interpolate = true)
		{
			if (interpolate)
			{
				if (Mathf.Abs(force.x) < 0.66f && Mathf.Abs(force.y) < 0.66f)
				{
					force = UnityEngine.Random.insideUnitCircle;
				}
				force.y = Mathf.Abs(force.y);
				force *= multiplier;
				force = Quaternion.AngleAxis(UnityEngine.Random.Range(-15f, 15f), Vector3.forward) * force * UnityEngine.Random.Range(0.8f, 1.2f);
				force += this._additionalForce;
				this._rigidbody.AddForce(force * UnityEngine.Random.Range(0.5f, 1f), ForceMode2D.Impulse);
				this._rigidbody.AddTorque(UnityEngine.Random.Range(-0.5f, 0.5f), ForceMode2D.Impulse);
			}
			else
			{
				this._rigidbody.AddForce(force, ForceMode2D.Impulse);
			}
			this._spriteRenderer.sortingOrder = DroppedParts.orderInLayerCount++;
			if (this._duration > 0f)
			{
				base.StartCoroutine(this.CFadeOut());
			}
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x0004A96B File Offset: 0x00048B6B
		private IEnumerator CFadeOut()
		{
			yield return Chronometer.global.WaitForSeconds(this._duration);
			if (this._fadeOut.length > 0)
			{
				yield return this.poolObject.CFadeOut(this._spriteRenderer, Chronometer.global, this._fadeOut, this._duration);
			}
			this.poolObject.Despawn();
			yield break;
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0004A97C File Offset: 0x00048B7C
		public override void Hit(Character from, ref Damage damage, Vector2 force)
		{
			if (Mathf.Abs(force.x) < 0.66f && Mathf.Abs(force.y) < 0.66f)
			{
				force = UnityEngine.Random.insideUnitCircle;
			}
			force.y = Mathf.Abs(force.y);
			force *= 3f;
			force = Quaternion.AngleAxis(UnityEngine.Random.Range(-15f, 15f), Vector3.forward) * force * UnityEngine.Random.Range(0.8f, 1.2f);
			if (this._rigidbody.IsTouchingLayers())
			{
				force += this._additionalForce;
			}
			this._rigidbody.AddForce(force * UnityEngine.Random.Range(0.5f, 1f), ForceMode2D.Impulse);
			this._rigidbody.AddTorque(UnityEngine.Random.Range(-0.5f, 0.5f), ForceMode2D.Impulse);
		}

		// Token: 0x040014AF RID: 5295
		private static int orderInLayerCount;

		// Token: 0x040014B0 RID: 5296
		[SerializeField]
		private DroppedParts.Priority _priority;

		// Token: 0x040014B1 RID: 5297
		[MinMaxSlider(0f, 30f)]
		[SerializeField]
		private Vector2Int _count = new Vector2Int(1, 1);

		// Token: 0x040014B2 RID: 5298
		[SerializeField]
		private bool _randomize;

		// Token: 0x040014B3 RID: 5299
		[SerializeField]
		private bool _collideWithTerrain = true;

		// Token: 0x040014B4 RID: 5300
		[SerializeField]
		[Information("0이면 영구지속", InformationAttribute.InformationType.Info, false)]
		private float _duration;

		// Token: 0x040014B5 RID: 5301
		[SerializeField]
		private float _fadeOutDuration;

		// Token: 0x040014B6 RID: 5302
		[SerializeField]
		private AnimationCurve _fadeOut;

		// Token: 0x040014B7 RID: 5303
		[GetComponent]
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x040014B8 RID: 5304
		[GetComponent]
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040014B9 RID: 5305
		[GetComponent]
		[SerializeField]
		private Rigidbody2D _rigidbody;

		// Token: 0x040014BA RID: 5306
		[SerializeField]
		[GetComponent]
		private BoxCollider2D _boxCollider;

		// Token: 0x040014BB RID: 5307
		[SerializeField]
		[GetComponent]
		private CircleCollider2D _circleCollider;

		// Token: 0x040014BC RID: 5308
		[SerializeField]
		private Vector2 _additionalForce = new Vector2(0f, 10f);

		// Token: 0x020004C7 RID: 1223
		public enum Priority
		{
			// Token: 0x040014BE RID: 5310
			High,
			// Token: 0x040014BF RID: 5311
			Middle,
			// Token: 0x040014C0 RID: 5312
			Low
		}
	}
}
