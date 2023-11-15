using System;
using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011CE RID: 4558
	public sealed class CheckCollision : Condition
	{
		// Token: 0x06005994 RID: 22932 RVA: 0x0010A7EE File Offset: 0x001089EE
		private void Awake()
		{
			this._operationInfos.Initialize();
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x0010A7FC File Offset: 0x001089FC
		protected override bool Check(AIController controller)
		{
			this._operationInfos.gameObject.SetActive(true);
			this._operationInfos.Run(controller.character);
			Vector2 size = this._box.size;
			Vector2 point = this._point.position + this._box.offset;
			CheckCollision._nonAllocOverlapper.contactFilter.SetLayerMask(this._checkLayer);
			ReadonlyBoundedList<Collider2D> results = CheckCollision._nonAllocOverlapper.OverlapBox(point, size, 0f).results;
			Debug.DrawLine(controller.character.transform.position, this._point.position, Color.red, 10f);
			return results.Count > 0;
		}

		// Token: 0x04004854 RID: 18516
		[UnityEditor.Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _operationInfos;

		// Token: 0x04004855 RID: 18517
		[SerializeField]
		private Transform _point;

		// Token: 0x04004856 RID: 18518
		[SerializeField]
		private BoxCollider2D _box;

		// Token: 0x04004857 RID: 18519
		[SerializeField]
		private LayerMask _checkLayer;

		// Token: 0x04004858 RID: 18520
		private static readonly NonAllocOverlapper _nonAllocOverlapper = new NonAllocOverlapper(1);
	}
}
