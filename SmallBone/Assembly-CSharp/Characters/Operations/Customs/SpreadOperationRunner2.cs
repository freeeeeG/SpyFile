using System;
using PhysicsUtils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FEE RID: 4078
	public class SpreadOperationRunner2 : CharacterOperation
	{
		// Token: 0x06004EC6 RID: 20166 RVA: 0x000EC58C File Offset: 0x000EA78C
		public override void Run(Character owner)
		{
			ValueTuple<bool, RaycastHit2D> valueTuple = this.TryRayCast(this._origin.position, Vector2.left);
			bool item = valueTuple.Item1;
			RaycastHit2D item2 = valueTuple.Item2;
			if (item)
			{
				this.SpreadLeft(item2.point, owner);
			}
			ValueTuple<bool, RaycastHit2D> valueTuple2 = this.TryRayCast(this._origin.position, Vector2.right);
			bool item3 = valueTuple2.Item1;
			RaycastHit2D item4 = valueTuple2.Item2;
			if (item3)
			{
				this.SpreadRight(item4.point, owner);
			}
			ValueTuple<bool, RaycastHit2D> valueTuple3 = this.TryRayCast(this._origin.position, Vector2.up);
			bool item5 = valueTuple3.Item1;
			RaycastHit2D item6 = valueTuple3.Item2;
			if (item5)
			{
				this.SpreadUp(item6.point, owner);
			}
			ValueTuple<bool, RaycastHit2D> valueTuple4 = this.TryRayCast(this._origin.position, Vector2.down);
			bool item7 = valueTuple4.Item1;
			RaycastHit2D item8 = valueTuple4.Item2;
			if (item7)
			{
				this.SpreadDown(item8.point, owner);
			}
		}

		// Token: 0x06004EC7 RID: 20167 RVA: 0x000EC684 File Offset: 0x000EA884
		private ValueTuple<bool, RaycastHit2D> TryRayCast(Vector2 origin, Vector2 direction)
		{
			SpreadOperationRunner2._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner2._nonAllocCaster.RayCast(origin, direction, this._distance).results;
			if (results.Count > 0)
			{
				return new ValueTuple<bool, RaycastHit2D>(true, results[0]);
			}
			return new ValueTuple<bool, RaycastHit2D>(false, default(RaycastHit2D));
		}

		// Token: 0x06004EC8 RID: 20168 RVA: 0x000EC6E4 File Offset: 0x000EA8E4
		private void SpreadUp(Vector2 origin, Character owner)
		{
			SpreadOperationRunner2._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner2._nonAllocCaster.RayCast(origin, Vector2.up, this._distance).results;
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
				short num = SpreadOperationRunner2.spriteLayer;
				SpreadOperationRunner2.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x06004EC9 RID: 20169 RVA: 0x000EC7FC File Offset: 0x000EA9FC
		private void SpreadDown(Vector2 origin, Character owner)
		{
			SpreadOperationRunner2._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner2._nonAllocCaster.RayCast(origin, Vector2.down, this._distance).results;
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
				short num = SpreadOperationRunner2.spriteLayer;
				SpreadOperationRunner2.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x000EC8D4 File Offset: 0x000EAAD4
		private void SpreadLeft(Vector2 origin, Character owner)
		{
			SpreadOperationRunner2._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner2._nonAllocCaster.RayCast(origin, Vector2.left, this._distance).results;
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
				short num = SpreadOperationRunner2.spriteLayer;
				SpreadOperationRunner2.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x06004ECB RID: 20171 RVA: 0x000EC9AC File Offset: 0x000EABAC
		private void SpreadRight(Vector2 origin, Character owner)
		{
			SpreadOperationRunner2._nonAllocCaster.contactFilter.SetLayerMask(this._groundMask);
			ReadonlyBoundedList<RaycastHit2D> results = SpreadOperationRunner2._nonAllocCaster.RayCast(origin, Vector2.right, this._distance).results;
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
				short num = SpreadOperationRunner2.spriteLayer;
				SpreadOperationRunner2.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x04003EE4 RID: 16100
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003EE5 RID: 16101
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		internal OperationRunner _operationRunner;

		// Token: 0x04003EE6 RID: 16102
		[SerializeField]
		private LayerMask _groundMask;

		// Token: 0x04003EE7 RID: 16103
		[SerializeField]
		private Transform _origin;

		// Token: 0x04003EE8 RID: 16104
		[SerializeField]
		private float _distance;

		// Token: 0x04003EE9 RID: 16105
		private static readonly NonAllocCaster _nonAllocCaster = new NonAllocCaster(1);
	}
}
