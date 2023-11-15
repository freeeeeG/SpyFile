using System;
using System.Collections;
using Characters;
using Characters.Movements;
using UnityEngine;

namespace Level
{
	// Token: 0x020004B5 RID: 1205
	public class DroppedCorpse : DestructibleObject
	{
		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00049205 File Offset: 0x00047405
		public bool randomize
		{
			get
			{
				return this._randomize;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x0004920D File Offset: 0x0004740D
		public bool collideWithTerrain
		{
			get
			{
				return this._collideWithTerrain;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x00049215 File Offset: 0x00047415
		public SpriteRenderer spriteRenderer
		{
			get
			{
				return this._spriteRenderer;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001746 RID: 5958 RVA: 0x0004921D File Offset: 0x0004741D
		public override Collider2D collider
		{
			get
			{
				return this._boxCollider;
			}
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x00049225 File Offset: 0x00047425
		private void Awake()
		{
			if (this._rigidbody == null)
			{
				this._rigidbody = base.GetComponent<Rigidbody2D>();
			}
			base.transform.SetParent(Map.Instance.transform);
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00049258 File Offset: 0x00047458
		public void Emit()
		{
			Push push = this._owner.movement.push;
			base.transform.position = this._owner.transform.position;
			if (this._owner.lookingDirection == Character.LookingDirection.Right)
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
				this._spriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
				this._reward.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
				this._spriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
				this._reward.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			this.Initialize(push, 1f);
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x0004937C File Offset: 0x0004757C
		private void Initialize(Push push, float multiplier = 1f)
		{
			Vector2 vector = Vector2.zero;
			if (push != null && !push.expired)
			{
				vector = push.direction * push.totalForce;
			}
			vector *= multiplier;
			this.Initialize(vector, multiplier);
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x000493BC File Offset: 0x000475BC
		private void Initialize(Vector2 force, float multiplier = 1f)
		{
			force *= multiplier;
			this._rigidbody.AddForce(force, ForceMode2D.Impulse);
			this._spriteRenderer.sortingOrder = DroppedCorpse.orderInLayerCount++;
			if (this._duration > 0f)
			{
				base.StartCoroutine(this.CFadeOut());
			}
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x00049411 File Offset: 0x00047611
		private IEnumerator CFadeOut()
		{
			yield return Chronometer.global.WaitForSeconds(this._duration);
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x00049420 File Offset: 0x00047620
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

		// Token: 0x0400145E RID: 5214
		private static int orderInLayerCount;

		// Token: 0x0400145F RID: 5215
		[SerializeField]
		private Character _owner;

		// Token: 0x04001460 RID: 5216
		[SerializeField]
		private bool _randomize;

		// Token: 0x04001461 RID: 5217
		[SerializeField]
		private bool _collideWithTerrain = true;

		// Token: 0x04001462 RID: 5218
		[Information("0이면 영구지속", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private float _duration;

		// Token: 0x04001463 RID: 5219
		[SerializeField]
		private float _fadeOutDuration;

		// Token: 0x04001464 RID: 5220
		[SerializeField]
		private AnimationCurve _fadeOut;

		// Token: 0x04001465 RID: 5221
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04001466 RID: 5222
		[SerializeField]
		private Rigidbody2D _rigidbody;

		// Token: 0x04001467 RID: 5223
		[SerializeField]
		private BoxCollider2D _boxCollider;

		// Token: 0x04001468 RID: 5224
		[SerializeField]
		private Vector2 _additionalForce = new Vector2(0f, 0.05f);

		// Token: 0x04001469 RID: 5225
		[SerializeField]
		private AdventurerGearReward _reward;
	}
}
