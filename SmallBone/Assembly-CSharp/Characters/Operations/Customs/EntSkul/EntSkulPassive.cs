using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs.EntSkul
{
	// Token: 0x02001016 RID: 4118
	public class EntSkulPassive : CharacterOperation
	{
		// Token: 0x06004F83 RID: 20355 RVA: 0x000EF2BC File Offset: 0x000ED4BC
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(1);
			this._groundFinder = new RayCaster
			{
				direction = Vector2.down,
				distance = this._groundFinderDirection
			};
			this._groundFinder.contactFilter.SetLayerMask(Layers.groundMask);
			this._searchRange.enabled = false;
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x000EF318 File Offset: 0x000ED518
		public override void Run(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			this._searchRange.enabled = true;
			this._overlapper.OverlapCollider(this._searchRange);
			List<Target> components = this._overlapper.results.GetComponents(true);
			if (components.Count == 0)
			{
				this._searchRange.enabled = false;
				return;
			}
			this._searchRange.enabled = false;
			Target target = components.Random<Target>();
			this._groundFinder.origin = target.transform.position;
			RaycastHit2D hit = this._groundFinder.SingleCast();
			if (!hit)
			{
				return;
			}
			this.SpawnOperationRunner(owner, hit.point);
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x000EF3E1 File Offset: 0x000ED5E1
		private void SpawnOperationRunner(Character owner, Vector3 position)
		{
			OperationInfos operationInfos = this._operationRunner.Spawn().operationInfos;
			operationInfos.transform.SetPositionAndRotation(position, Quaternion.identity);
			operationInfos.Run(owner);
		}

		// Token: 0x04003FB4 RID: 16308
		[SerializeField]
		private Collider2D _searchRange;

		// Token: 0x04003FB5 RID: 16309
		[Tooltip("가시가 항상 바닥에 나와야해서, 적 기준으로 바로 아래쪽 땅을 찾는데 그 때 땅을 찾기 위한 최대 거리를 의미함")]
		[SerializeField]
		private float _groundFinderDirection = 5f;

		// Token: 0x04003FB6 RID: 16310
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x04003FB7 RID: 16311
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003FB8 RID: 16312
		private RayCaster _groundFinder;

		// Token: 0x04003FB9 RID: 16313
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;
	}
}
