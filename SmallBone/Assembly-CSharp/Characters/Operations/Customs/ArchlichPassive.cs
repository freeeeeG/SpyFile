using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FCF RID: 4047
	public class ArchlichPassive : CharacterOperation
	{
		// Token: 0x06004E53 RID: 20051 RVA: 0x000EA81E File Offset: 0x000E8A1E
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(5);
			this._searchRange.enabled = false;
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x000EA838 File Offset: 0x000E8A38
		public override void Run(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			this._searchRange.enabled = true;
			this._overlapper.OverlapCollider(this._searchRange);
			this._searchRange.enabled = false;
			List<Target> components = this._overlapper.results.GetComponents(true);
			if (components.Count == 0)
			{
				return;
			}
			float num = UnityEngine.Random.value * 3.1415927f * 2f;
			foreach (Target target in components)
			{
				Bounds bounds = target.collider.bounds;
				Vector3 center = bounds.center;
				float num2 = (bounds.size.x + bounds.size.y) / 2f;
				int num3 = 5 - components.Count + 1;
				for (int i = 0; i < num3; i++)
				{
					num += (1f + UnityEngine.Random.value) / (float)(num3 * 2) * 3.1415927f * 2f;
					Vector3 position = center;
					position.x += Mathf.Cos(num) * 2f;
					position.y += Mathf.Sin(num) * 2f;
					OperationInfos operationInfos = this._operationRunner.Spawn().operationInfos;
					operationInfos.transform.SetPositionAndRotation(position, Quaternion.Euler(0f, 0f, num * 57.29578f + 180f));
					operationInfos.Initialize();
					operationInfos.Run(owner);
				}
			}
		}

		// Token: 0x04003E5C RID: 15964
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;

		// Token: 0x04003E5D RID: 15965
		[SerializeField]
		private Collider2D _searchRange;

		// Token: 0x04003E5E RID: 15966
		private const int _spawnCount = 5;

		// Token: 0x04003E5F RID: 15967
		private const int _radius = 2;

		// Token: 0x04003E60 RID: 15968
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x04003E61 RID: 15969
		private NonAllocOverlapper _overlapper;
	}
}
