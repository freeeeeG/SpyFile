using System;
using UnityEngine;

namespace BT
{
	// Token: 0x0200141B RID: 5147
	public class RangeWander : Node
	{
		// Token: 0x0600652B RID: 25899 RVA: 0x00124FC9 File Offset: 0x001231C9
		protected override void OnInitialize()
		{
			this.Initialize();
			base.OnInitialize();
		}

		// Token: 0x0600652C RID: 25900 RVA: 0x00124FD8 File Offset: 0x001231D8
		protected override NodeState UpdateDeltatime(Context context)
		{
			Transform transform = context.Get<Transform>(Key.OwnerTransform);
			if (transform == null)
			{
				Debug.LogError("OwnerTransform is null");
				return NodeState.Fail;
			}
			float deltaTime = context.deltaTime;
			if (this._isWandering)
			{
				this.Wander(transform, deltaTime);
			}
			this._remainTime -= deltaTime;
			if (this._remainTime <= 0f)
			{
				this.Initialize();
			}
			return NodeState.Success;
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x00125040 File Offset: 0x00123240
		private void Initialize()
		{
			this._speedValue = this._speed.value;
			this._isWandering = MMMaths.RandomBool();
			if (this._customRange == null)
			{
				RaycastHit2D hit = Physics2D.Raycast(base.transform.position, Vector2.down, 9f, Layers.groundMask);
				if (hit)
				{
					this._range = hit.collider;
				}
			}
			else
			{
				this._range = this._customRange;
			}
			if (this._isWandering)
			{
				this.OnStartWander();
				return;
			}
			this.OnStartIdle();
		}

		// Token: 0x0600652E RID: 25902 RVA: 0x001250DC File Offset: 0x001232DC
		private void Wander(Transform owner, float deltaTime)
		{
			this.Flip(owner);
			if (this._direction.x > 0f)
			{
				this._spriteRenderer.flipX = false;
			}
			else
			{
				this._spriteRenderer.flipX = true;
			}
			owner.Translate(this._direction * deltaTime * this._speedValue);
		}

		// Token: 0x0600652F RID: 25903 RVA: 0x00125140 File Offset: 0x00123340
		private void OnStartWander()
		{
			this._direction = (MMMaths.RandomBool() ? Vector2.right : Vector2.left);
			this._remainTime = UnityEngine.Random.Range(this._wanderTime.x, this._wanderTime.y);
			this._animator.Play(RangeWander._walkHash);
		}

		// Token: 0x06006530 RID: 25904 RVA: 0x00125197 File Offset: 0x00123397
		private void OnStartIdle()
		{
			this._remainTime = UnityEngine.Random.Range(this._idleTime.x, this._idleTime.y);
			this._animator.Play(RangeWander._idleHash);
		}

		// Token: 0x06006531 RID: 25905 RVA: 0x001251CC File Offset: 0x001233CC
		private void Flip(Transform owner)
		{
			Bounds bounds = this._range.bounds;
			if (this._direction == Vector2.right && bounds.max.x - 2f < owner.transform.position.x)
			{
				this._direction = Vector2.left;
			}
			if (this._direction == Vector2.left && bounds.min.x + 2f > owner.transform.position.x)
			{
				this._direction = Vector2.right;
			}
		}

		// Token: 0x04005179 RID: 20857
		private static readonly int _idleHash = Animator.StringToHash("Idle");

		// Token: 0x0400517A RID: 20858
		private static readonly int _walkHash = Animator.StringToHash("Walk");

		// Token: 0x0400517B RID: 20859
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x0400517C RID: 20860
		[SerializeField]
		private Animator _animator;

		// Token: 0x0400517D RID: 20861
		[SerializeField]
		private CustomFloat _speed;

		// Token: 0x0400517E RID: 20862
		[MinMaxSlider(0f, 20f)]
		[SerializeField]
		private Vector2 _idleTime;

		// Token: 0x0400517F RID: 20863
		[SerializeField]
		[MinMaxSlider(0f, 20f)]
		private Vector2 _wanderTime;

		// Token: 0x04005180 RID: 20864
		[SerializeField]
		private Collider2D _customRange;

		// Token: 0x04005181 RID: 20865
		private const float _groundFindingRayDistance = 9f;

		// Token: 0x04005182 RID: 20866
		private const float _minDistanceFromSide = 2f;

		// Token: 0x04005183 RID: 20867
		private bool _isWandering;

		// Token: 0x04005184 RID: 20868
		private float _remainTime;

		// Token: 0x04005185 RID: 20869
		private Vector2 _direction;

		// Token: 0x04005186 RID: 20870
		private float _speedValue;

		// Token: 0x04005187 RID: 20871
		private Collider2D _range;
	}
}
