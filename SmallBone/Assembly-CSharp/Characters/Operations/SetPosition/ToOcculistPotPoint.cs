using System;
using Characters.Monsters;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000ED4 RID: 3796
	public sealed class ToOcculistPotPoint : Policy
	{
		// Token: 0x06004A8B RID: 19083 RVA: 0x000D9764 File Offset: 0x000D7964
		static ToOcculistPotPoint()
		{
			ToOcculistPotPoint._leftNonAllocCaster.contactFilter.SetLayerMask(1 << Layers.terrainMask);
			ToOcculistPotPoint._rightNonAllocCaster = new NonAllocCaster(1);
			ToOcculistPotPoint._rightNonAllocCaster.contactFilter.SetLayerMask(1 << Layers.terrainMask);
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x000D97D0 File Offset: 0x000D79D0
		public override Vector2 GetPosition()
		{
			Vector2 result;
			this.MoveAmountFromBaseHead(out result);
			this.MoveAmountFromAnother(ref result);
			return result;
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x000D97F0 File Offset: 0x000D79F0
		private void MoveAmountFromBaseHead(out Vector2 position)
		{
			position = new Vector2(this._base.collider.bounds.center.x, this._base.collider.bounds.max.y + this._distanceFromBaseHead.value);
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x000D9850 File Offset: 0x000D7A50
		private void MoveAmountFromAnother(ref Vector2 position)
		{
			float value = this._minDistanceFromAnother.value;
			float value2 = this._stride.value;
			if (this.CanSetPosition(position, value))
			{
				return;
			}
			int i = 1;
			int num = 10;
			Vector2 vector = position;
			while (i < num)
			{
				vector = position + (float)i * value2 * Vector2.right;
				if (this.CanSetPosition(vector, value))
				{
					break;
				}
				vector = position + (float)i * value2 * Vector2.left;
				if (this.CanSetPosition(vector, value))
				{
					break;
				}
				i++;
			}
			if (i < num)
			{
				position = vector;
			}
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x000D98F4 File Offset: 0x000D7AF4
		private bool CanSetPosition(Vector2 position, float minDistance)
		{
			if (!Map.Instance.IsInMap(position))
			{
				return false;
			}
			ReadonlyBoundedList<RaycastHit2D> results = ToOcculistPotPoint._leftNonAllocCaster.RayCast(position, Vector2.left, this._sizeOfPot).results;
			ReadonlyBoundedList<RaycastHit2D> results2 = ToOcculistPotPoint._rightNonAllocCaster.RayCast(position, Vector2.right, this._sizeOfPot).results;
			if (results.Count > 0 && results2.Count > 0)
			{
				return false;
			}
			float num = minDistance * minDistance;
			foreach (Monster monster in this._occulistPotContainer.monsters)
			{
				if (Vector2.SqrMagnitude(position - monster.transform.position) < num)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040039AA RID: 14762
		[SerializeField]
		private MonsterContainer _occulistPotContainer;

		// Token: 0x040039AB RID: 14763
		[SerializeField]
		private Character _base;

		// Token: 0x040039AC RID: 14764
		[SerializeField]
		private float _sizeOfPot = 1f;

		// Token: 0x040039AD RID: 14765
		[SerializeField]
		private CustomFloat _distanceFromBaseHead;

		// Token: 0x040039AE RID: 14766
		[SerializeField]
		private CustomFloat _minDistanceFromAnother;

		// Token: 0x040039AF RID: 14767
		[SerializeField]
		private CustomFloat _stride;

		// Token: 0x040039B0 RID: 14768
		private static readonly NonAllocCaster _leftNonAllocCaster = new NonAllocCaster(1);

		// Token: 0x040039B1 RID: 14769
		private static readonly NonAllocCaster _rightNonAllocCaster;
	}
}
