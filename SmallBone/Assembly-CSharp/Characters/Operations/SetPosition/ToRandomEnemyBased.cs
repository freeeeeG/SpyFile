using System;
using System.Collections.Generic;
using Characters.Operations.FindOptions;
using Level;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EF4 RID: 3828
	public sealed class ToRandomEnemyBased : Policy
	{
		// Token: 0x06004B00 RID: 19200 RVA: 0x000DC782 File Offset: 0x000DA982
		public override Vector2 GetPosition(Character owner)
		{
			if (this._characterCollider == null)
			{
				this._characterCollider = owner.collider;
			}
			return this.GetPosition();
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x000DC7A4 File Offset: 0x000DA9A4
		public override Vector2 GetPosition()
		{
			List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
			allEnemies.PseudoShuffle<Character>();
			Character character = this.SelectTargetFromCondition(allEnemies);
			if (character == null)
			{
				return base.transform.position;
			}
			character.transform.position;
			return this.Clamp(this._distanceRange.value, character);
		}

		// Token: 0x06004B02 RID: 19202 RVA: 0x000DC80C File Offset: 0x000DAA0C
		private Character SelectTargetFromCondition(List<Character> allEnemies)
		{
			foreach (Character character in allEnemies)
			{
				bool flag = true;
				ICondition[] condition = this._condition;
				for (int i = 0; i < condition.Length; i++)
				{
					if (!condition[i].Satisfied(character))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return character;
				}
			}
			return null;
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x000DC88C File Offset: 0x000DAA8C
		private Vector2 Clamp(float amount, Character target)
		{
			Collider2D lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
			if (lastStandingCollider == null)
			{
				target.movement.TryGetClosestBelowCollider(out lastStandingCollider, Layers.footholdMask, this._belowRayDistance);
			}
			float min = lastStandingCollider.bounds.min.x + this._characterCollider.bounds.size.x;
			float max = lastStandingCollider.bounds.max.x - this._characterCollider.bounds.size.x;
			Vector3 vector = target.transform.position;
			if (target.lookingDirection == Character.LookingDirection.Right)
			{
				vector = this.ClampX(vector, this._behind ? (vector.x - amount) : (vector.x + amount), min, max);
			}
			else
			{
				vector = this.ClampX(vector, this._behind ? (vector.x + amount) : (vector.x - amount), min, max);
			}
			return vector;
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x000DC9A8 File Offset: 0x000DABA8
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

		// Token: 0x04003A37 RID: 14903
		[SerializeField]
		private Collider2D _characterCollider;

		// Token: 0x04003A38 RID: 14904
		[SerializeField]
		private float _belowRayDistance = 100f;

		// Token: 0x04003A39 RID: 14905
		[SerializeField]
		[Header("타겟과의 거리옵션")]
		private bool _behind;

		// Token: 0x04003A3A RID: 14906
		[SerializeField]
		private CustomFloat _distanceRange;

		// Token: 0x04003A3B RID: 14907
		[SerializeReference]
		[SubclassSelector]
		[Header("만족 조건")]
		private ICondition[] _condition;
	}
}
