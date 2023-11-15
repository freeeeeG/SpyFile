using System;
using BT;
using BT.SharedValues;
using Characters.Minions;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F3D RID: 3901
	public class SummonMinionToTarget : CharacterOperation
	{
		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06004BE3 RID: 19427 RVA: 0x000D9CB3 File Offset: 0x000D7EB3
		private Vector2 _default
		{
			get
			{
				return base.transform.position;
			}
		}

		// Token: 0x06004BE4 RID: 19428 RVA: 0x000DF3B8 File Offset: 0x000DD5B8
		public override void Run(Character owner)
		{
			if (owner.playerComponents == null)
			{
				return;
			}
			this.Spawn(owner);
		}

		// Token: 0x06004BE5 RID: 19429 RVA: 0x000DF3CC File Offset: 0x000DD5CC
		private void Spawn(Character owner)
		{
			Character character = TargetFinder.FindClosestTarget(this._findRange, this._minionCollider, this._targetLayer.Evaluate(this._minionCollider.gameObject));
			Context context = Context.Create();
			Vector2 vector = this.GetPosition(character);
			if (this._snapToGround)
			{
				RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.down, this._groundFindingDistance, Layers.groundMask);
				if (hit)
				{
					vector = hit.point;
				}
			}
			Minion minion = owner.playerComponents.minionLeader.Summon(this._minion, vector, this._overrideSetting);
			context.Add(Key.Target, new SharedValue<Character>(character));
			context.Add(Key.OwnerTransform, new SharedValue<Transform>(minion.transform));
			context.Add(Key.OwnerCharacter, new SharedValue<Character>(minion.character));
			if (character != null)
			{
				minion.character.ForceToLookAt(character.transform.position.x);
				minion.GetComponentInChildren<BehaviourTreeRunner>().Run(context);
				return;
			}
			minion.character.ForceToLookAt(owner.lookingDirection);
			minion.GetComponentInChildren<BehaviourTreeRunner>().Run();
		}

		// Token: 0x06004BE6 RID: 19430 RVA: 0x000DF4F4 File Offset: 0x000DD6F4
		private Vector2 GetPosition(Character target)
		{
			if (target == null)
			{
				return base.transform.position;
			}
			Vector2 result = target.transform.position;
			this.Clamp(ref result, this._amount.value, target);
			if (!this._snapToGround)
			{
				return result;
			}
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					target.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
					if (lastStandingCollider == null)
					{
						return this._default;
					}
				}
			}
			else
			{
				target.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, 100f);
				if (lastStandingCollider == null)
				{
					return this._default;
				}
			}
			float x = target.transform.position.x;
			float y = lastStandingCollider.bounds.max.y;
			return new Vector2(x, y);
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x000DF5EC File Offset: 0x000DD7EC
		private void Clamp(ref Vector2 result, float amount, Character target)
		{
			Collider2D lastStandingCollider;
			if (this._lastStandingCollider)
			{
				lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					target.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._groundFindingDistance);
				}
			}
			else
			{
				target.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._groundFindingDistance);
			}
			float min = lastStandingCollider.bounds.min.x + this._minionCollider.bounds.size.x;
			float max = lastStandingCollider.bounds.max.x - this._minionCollider.bounds.size.x;
			if (target.lookingDirection == Character.LookingDirection.Right)
			{
				result = this.ClampX(result, this._behind ? (result.x - amount) : (result.x + amount), min, max);
				return;
			}
			result = this.ClampX(result, this._behind ? (result.x + amount) : (result.x - amount), min, max);
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x000DF714 File Offset: 0x000DD914
		private Vector2 ClampX(Vector2 result, float x, float min, float max)
		{
			float num = 0.05f;
			if (x <= min)
			{
				result = new Vector2(min + num, result.y);
			}
			else if (x >= max)
			{
				result = new Vector2(max - num, result.y);
			}
			else
			{
				result = new Vector2(x, result.y);
			}
			return result;
		}

		// Token: 0x04003B15 RID: 15125
		[SerializeField]
		private Minion _minion;

		// Token: 0x04003B16 RID: 15126
		[Information("비워둘 경우 Default 설정 값을 적용", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private MinionSetting _overrideSetting;

		// Token: 0x04003B17 RID: 15127
		[SerializeField]
		[Information("해당 개수만큼 하수인을 미리 로드해두어 하수인이 소환되는 순간의 프레임 드랍을 없애줌", InformationAttribute.InformationType.Info, false)]
		private int _preloadCount = 1;

		// Token: 0x04003B18 RID: 15128
		[SerializeField]
		[Space]
		private bool _snapToGround;

		// Token: 0x04003B19 RID: 15129
		[SerializeField]
		[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
		private float _groundFindingDistance = 7f;

		// Token: 0x04003B1A RID: 15130
		[SerializeField]
		private TargetLayer _targetLayer;

		// Token: 0x04003B1B RID: 15131
		[SerializeField]
		private bool _lastStandingCollider;

		// Token: 0x04003B1C RID: 15132
		[SerializeField]
		private bool _behind;

		// Token: 0x04003B1D RID: 15133
		[SerializeField]
		private Collider2D _findRange;

		// Token: 0x04003B1E RID: 15134
		[SerializeField]
		private Collider2D _minionCollider;

		// Token: 0x04003B1F RID: 15135
		[SerializeField]
		private CustomFloat _amount;
	}
}
