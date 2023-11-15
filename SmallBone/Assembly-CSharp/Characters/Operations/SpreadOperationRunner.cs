using System;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E3B RID: 3643
	public class SpreadOperationRunner : CharacterOperation
	{
		// Token: 0x0600488E RID: 18574 RVA: 0x000D3077 File Offset: 0x000D1277
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._operationRunner = null;
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x000D3088 File Offset: 0x000D1288
		public override void Run(Character owner)
		{
			float z = this._origin.transform.rotation.eulerAngles.z;
			if (z == 0f)
			{
				this.SpreadDown(this._origin.transform.position, owner);
				return;
			}
			if (z == 90f)
			{
				this.SpreadRight(this._origin.transform.position, owner);
				return;
			}
			if (z == 180f)
			{
				this.SpreadUp(this._origin.transform.position, owner);
				return;
			}
			if (z != 270f)
			{
				return;
			}
			this.SpreadLeft(this._origin.transform.position, owner);
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x000D3148 File Offset: 0x000D1348
		private ValueTuple<bool, RaycastHit2D> TryRayCast(Vector2 origin, Vector2 direction, float distance)
		{
			SpreadOperationRunner._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner._nonAllocCaster.RayCast(origin, direction, distance).results;
			if (results.Count > 0)
			{
				return new ValueTuple<bool, RaycastHit2D>(true, results[0]);
			}
			return new ValueTuple<bool, RaycastHit2D>(false, default(RaycastHit2D));
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x000D31A4 File Offset: 0x000D13A4
		private bool TrySpread(Vector2 origin, Vector2 direction, Vector2 groundDirection, int count, float rotation, Character owner)
		{
			Vector2 origin2 = origin;
			float num = direction.x;
			if (num != 1f)
			{
				if (num == -1f)
				{
					origin2 = new Vector2(origin.x, origin.y + 0.5f);
				}
			}
			else
			{
				origin2 = new Vector2(origin.x, origin.y + 0.5f);
			}
			num = direction.y;
			if (num != 1f)
			{
				if (num == -1f)
				{
					origin2 = new Vector2(origin.x + 0.5f, origin.y);
				}
			}
			else
			{
				origin2 = new Vector2(origin.x + 0.5f, origin.y);
			}
			float value = this._distance.value;
			SpreadOperationRunner._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			if (SpreadOperationRunner._nonAllocCaster.RayCast(origin2, direction, value * (float)count).results.Count > 0)
			{
				return false;
			}
			Vector2 origin3 = origin;
			num = direction.x;
			if (num != 1f)
			{
				if (num == -1f)
				{
					origin3 = new Vector2(origin.x - value * (float)count, origin.y + 1f);
				}
			}
			else
			{
				origin3 = new Vector2(origin.x + value * (float)count, origin.y + 1f);
			}
			num = direction.y;
			if (num != 1f)
			{
				if (num == -1f)
				{
					origin3 = new Vector2(origin.x + 1f, origin.y - value * (float)count);
				}
			}
			else
			{
				origin3 = new Vector2(origin.x + 1f, origin.y + value * (float)count);
			}
			ValueTuple<bool, RaycastHit2D> valueTuple = this.TryRayCast(origin3, groundDirection, 2f);
			bool item = valueTuple.Item1;
			RaycastHit2D item2 = valueTuple.Item2;
			if (!item)
			{
				return false;
			}
			OperationInfos operationInfos = this._operationRunner.Spawn().operationInfos;
			operationInfos.transform.SetPositionAndRotation(item2.point, Quaternion.Euler(0f, 0f, rotation));
			operationInfos.transform.localScale = new Vector3(1f, 1f, 1f);
			operationInfos.Run(owner);
			return true;
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x000D33D0 File Offset: 0x000D15D0
		private void SpreadDown(Vector2 origin, Character owner)
		{
			float value = this._count.value;
			int num = 1;
			while ((float)num <= value && this.TrySpread(origin, Vector2.right, Vector2.down, num, 0f, owner))
			{
				num++;
			}
			int num2 = 1;
			while ((float)num2 <= value && this.TrySpread(origin, Vector2.left, Vector2.down, num2, 0f, owner))
			{
				num2++;
			}
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x000D3438 File Offset: 0x000D1638
		private void SpreadUp(Vector2 origin, Character owner)
		{
			float value = this._count.value;
			int num = 1;
			while ((float)num <= value && this.TrySpread(origin, Vector2.right, Vector2.up, num, 180f, owner))
			{
				num++;
			}
			int num2 = 1;
			while ((float)num2 <= value && this.TrySpread(origin, Vector2.left, Vector2.up, num2, 180f, owner))
			{
				num2++;
			}
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x000D34A0 File Offset: 0x000D16A0
		private void SpreadRight(Vector2 origin, Character owner)
		{
			float value = this._count.value;
			int num = 1;
			while ((float)num <= value && this.TrySpread(origin, Vector2.up, Vector2.right, num, 90f, owner))
			{
				num++;
			}
			int num2 = 1;
			while ((float)num2 <= value && this.TrySpread(origin, Vector2.down, Vector2.right, num2, 90f, owner))
			{
				num2++;
			}
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x000D3508 File Offset: 0x000D1708
		private void SpreadLeft(Vector2 origin, Character owner)
		{
			float value = this._count.value;
			int num = 1;
			while ((float)num <= value && this.TrySpread(origin, Vector2.up, Vector2.left, num, 270f, owner))
			{
				num++;
			}
			int num2 = 1;
			while ((float)num2 <= value && this.TrySpread(origin, Vector2.down, Vector2.left, num2, 270f, owner))
			{
				num2++;
			}
		}

		// Token: 0x040037A7 RID: 14247
		[SerializeField]
		private OperationRunner _operationRunner;

		// Token: 0x040037A8 RID: 14248
		[SerializeField]
		private Transform _origin;

		// Token: 0x040037A9 RID: 14249
		[SerializeField]
		private CustomFloat _count;

		// Token: 0x040037AA RID: 14250
		[SerializeField]
		private CustomFloat _distance;

		// Token: 0x040037AB RID: 14251
		[SerializeField]
		private LayerMask _groundMask;

		// Token: 0x040037AC RID: 14252
		private static readonly NonAllocCaster _nonAllocCaster = new NonAllocCaster(1);
	}
}
