using System;
using Characters.Operations;
using PhysicsUtils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200079A RID: 1946
	public class SpreadOperationRunner : HitOperation
	{
		// Token: 0x060027C6 RID: 10182 RVA: 0x0007794F File Offset: 0x00075B4F
		private void OnDestroy()
		{
			this._operationRunner = null;
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x00077958 File Offset: 0x00075B58
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			Character owner = projectile.owner;
			ValueTuple<bool, RaycastHit2D> valueTuple = this.TryRayCast(projectile.transform.position, Vector2.left);
			bool item = valueTuple.Item1;
			RaycastHit2D item2 = valueTuple.Item2;
			if (item)
			{
				this.SpreadLeft(item2.point, owner);
			}
			ValueTuple<bool, RaycastHit2D> valueTuple2 = this.TryRayCast(projectile.transform.position, Vector2.right);
			bool item3 = valueTuple2.Item1;
			RaycastHit2D item4 = valueTuple2.Item2;
			if (item3)
			{
				this.SpreadRight(item4.point, owner);
			}
			ValueTuple<bool, RaycastHit2D> valueTuple3 = this.TryRayCast(projectile.transform.position, Vector2.up);
			bool item5 = valueTuple3.Item1;
			RaycastHit2D item6 = valueTuple3.Item2;
			if (item5)
			{
				this.SpreadUp(item6.point, owner);
			}
			ValueTuple<bool, RaycastHit2D> valueTuple4 = this.TryRayCast(projectile.transform.position, Vector2.down);
			bool item7 = valueTuple4.Item1;
			RaycastHit2D item8 = valueTuple4.Item2;
			if (item7)
			{
				this.SpreadDown(item8.point, owner);
			}
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x00077A58 File Offset: 0x00075C58
		private ValueTuple<bool, RaycastHit2D> TryRayCast(Vector2 origin, Vector2 direction)
		{
			SpreadOperationRunner._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner._nonAllocCaster.RayCast(origin, direction, this._distance).results;
			if (results.Count > 0)
			{
				return new ValueTuple<bool, RaycastHit2D>(true, results[0]);
			}
			return new ValueTuple<bool, RaycastHit2D>(false, default(RaycastHit2D));
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x00077AB8 File Offset: 0x00075CB8
		private void SpreadUp(Vector2 origin, Character owner)
		{
			SpreadOperationRunner._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner._nonAllocCaster.RayCast(origin, Vector2.up, this._distance).results;
			if (results.Count == 0)
			{
				return;
			}
			RaycastHit2D raycastHit2D = results[0];
			OperationRunner operationRunner = this._operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			if (raycastHit2D.collider.gameObject.layer == 17)
			{
				operationInfos.transform.SetPositionAndRotation(raycastHit2D.point, Quaternion.Euler(0f, 0f, 0f));
			}
			else
			{
				operationInfos.transform.SetPositionAndRotation(raycastHit2D.point, Quaternion.Euler(0f, 0f, 180f));
			}
			operationInfos.transform.localScale = new Vector3(1f, 1f, 1f);
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SpreadOperationRunner.spriteLayer;
				SpreadOperationRunner.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x00077BD0 File Offset: 0x00075DD0
		private void SpreadDown(Vector2 origin, Character owner)
		{
			SpreadOperationRunner._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner._nonAllocCaster.RayCast(origin, Vector2.down, this._distance).results;
			if (results.Count == 0)
			{
				return;
			}
			RaycastHit2D raycastHit2D = results[0];
			OperationRunner operationRunner = this._operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			operationInfos.transform.SetPositionAndRotation(raycastHit2D.point, Quaternion.Euler(0f, 0f, 0f));
			operationInfos.transform.localScale = new Vector3(1f, 1f, 1f);
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SpreadOperationRunner.spriteLayer;
				SpreadOperationRunner.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x00077CA8 File Offset: 0x00075EA8
		private void SpreadLeft(Vector2 origin, Character owner)
		{
			SpreadOperationRunner._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner._nonAllocCaster.RayCast(origin, Vector2.left, this._distance).results;
			if (results.Count == 0)
			{
				return;
			}
			RaycastHit2D raycastHit2D = results[0];
			OperationRunner operationRunner = this._operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			operationInfos.transform.SetPositionAndRotation(raycastHit2D.point, Quaternion.Euler(0f, 0f, 270f));
			operationInfos.transform.localScale = new Vector3(1f, 1f, 1f);
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SpreadOperationRunner.spriteLayer;
				SpreadOperationRunner.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x00077D80 File Offset: 0x00075F80
		private void SpreadRight(Vector2 origin, Character owner)
		{
			SpreadOperationRunner._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner._nonAllocCaster.RayCast(origin, Vector2.right, this._distance).results;
			if (results.Count == 0)
			{
				return;
			}
			RaycastHit2D raycastHit2D = results[0];
			OperationRunner operationRunner = this._operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			operationInfos.transform.SetPositionAndRotation(raycastHit2D.point, Quaternion.Euler(0f, 0f, 90f));
			operationInfos.transform.localScale = new Vector3(1f, 1f, 1f);
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SpreadOperationRunner.spriteLayer;
				SpreadOperationRunner.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x040021E7 RID: 8679
		private static short spriteLayer = short.MinValue;

		// Token: 0x040021E8 RID: 8680
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		internal OperationRunner _operationRunner;

		// Token: 0x040021E9 RID: 8681
		[SerializeField]
		private LayerMask _groundMask;

		// Token: 0x040021EA RID: 8682
		[SerializeField]
		private float _distance;

		// Token: 0x040021EB RID: 8683
		private static readonly NonAllocCaster _nonAllocCaster = new NonAllocCaster(1);
	}
}
