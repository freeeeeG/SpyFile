using System;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200066F RID: 1647
	public class Orb : MonoBehaviour
	{
		// Token: 0x060020FB RID: 8443 RVA: 0x00063730 File Offset: 0x00061930
		private void Awake()
		{
			this._onEnable.Initialize();
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x0006373D File Offset: 0x0006193D
		public void Initialize(float startAngle)
		{
			this._radian = startAngle;
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x00063746 File Offset: 0x00061946
		private void OnEnable()
		{
			base.StartCoroutine(this._onEnable.CRun(this._owner));
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x00063760 File Offset: 0x00061960
		public void MoveCenteredOn(Vector3 pivot, float radious, float amount)
		{
			Vector3 v = pivot - this._owner.transform.position;
			this._radian += amount;
			this._owner.movement.MoveHorizontal(v + new Vector2(Mathf.Cos(this._radian), Mathf.Sin(this._radian)) * radious);
		}

		// Token: 0x04001C12 RID: 7186
		[SerializeField]
		private Character _owner;

		// Token: 0x04001C13 RID: 7187
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _onEnable;

		// Token: 0x04001C14 RID: 7188
		private float _radian;
	}
}
