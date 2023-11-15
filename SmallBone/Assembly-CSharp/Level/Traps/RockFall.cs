using System;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level.Traps
{
	// Token: 0x02000676 RID: 1654
	public class RockFall : ControlableTrap
	{
		// Token: 0x06002119 RID: 8473 RVA: 0x00063CDE File Offset: 0x00061EDE
		private void Awake()
		{
			this._onGroundedOperations.Initialize();
			this._fallOperations.Initialize();
			this._character.movement.onGrounded += this.OnGrounded;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x00063D12 File Offset: 0x00061F12
		private void OnDestroy()
		{
			this._character.movement.onGrounded -= this.OnGrounded;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x00063D30 File Offset: 0x00061F30
		private void OnGrounded()
		{
			this._character.movement.onGrounded -= this.OnGrounded;
			base.StartCoroutine(this._onGroundedOperations.CRun(this._character));
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x00063D66 File Offset: 0x00061F66
		public override void Activate()
		{
			base.StartCoroutine(this._fallOperations.CRun(this._character));
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x00002191 File Offset: 0x00000391
		public override void Deactivate()
		{
		}

		// Token: 0x04001C34 RID: 7220
		[SerializeField]
		private Character _character;

		// Token: 0x04001C35 RID: 7221
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		[FormerlySerializedAs("_operations")]
		private OperationInfo.Subcomponents _onGroundedOperations;

		// Token: 0x04001C36 RID: 7222
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _fallOperations;
	}
}
