using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs.EntSkul
{
	// Token: 0x02001017 RID: 4119
	public class EntSkulThornyVine : CharacterOperation
	{
		// Token: 0x06004F87 RID: 20359 RVA: 0x000EF434 File Offset: 0x000ED634
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(this._count);
			this._groundFinder = new RayCaster
			{
				direction = Vector2.down,
				distance = this._groundFinderDirection
			};
			this._groundFinder.contactFilter.SetLayerMask(Layers.groundMask);
			this._searchRange.enabled = false;
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x000EF498 File Offset: 0x000ED698
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
			foreach (Target target in components)
			{
				if (!(target.character == null))
				{
					this._groundFinder.origin = target.transform.position;
					RaycastHit2D hit = this._groundFinder.SingleCast();
					if (!hit)
					{
						break;
					}
					this.SpawnOperationRunner(owner, hit.point);
				}
			}
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x000EF5A4 File Offset: 0x000ED7A4
		private void SpawnOperationRunner(Character owner, Vector3 position)
		{
			OperationInfos operationInfos = this._operationRunner.Spawn().operationInfos;
			operationInfos.transform.SetPositionAndRotation(position, Quaternion.identity);
			operationInfos.Run(owner);
		}

		// Token: 0x04003FBA RID: 16314
		[SerializeField]
		private int _count = 15;

		// Token: 0x04003FBB RID: 16315
		[SerializeField]
		private Collider2D _searchRange;

		// Token: 0x04003FBC RID: 16316
		[Tooltip("가시가 항상 바닥에 나와야해서, 적 기준으로 바로 아래쪽 땅을 찾는데 그 때 땅을 찾기 위한 최대 거리를 의미함")]
		[SerializeField]
		private float _groundFinderDirection = 5f;

		// Token: 0x04003FBD RID: 16317
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x04003FBE RID: 16318
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003FBF RID: 16319
		private RayCaster _groundFinder;

		// Token: 0x04003FC0 RID: 16320
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;
	}
}
