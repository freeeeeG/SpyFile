using System;
using System.Collections;
using Level.Objects.DecorationCharacter.Movements;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Command
{
	// Token: 0x0200057C RID: 1404
	[Serializable]
	public class JumpCommand : ICommand
	{
		// Token: 0x06001B98 RID: 7064 RVA: 0x00055F43 File Offset: 0x00054143
		public IEnumerator CRun()
		{
			this._isRight = MMMaths.RandomBool();
			if (!this._movement.isGrounded)
			{
				yield return null;
			}
			this._movement.Jump(this._jumpPower.value);
			yield return null;
			while (!this._movement.isGrounded)
			{
				this._movement.Move(this._isRight ? 0f : 3.1415927f);
				yield return null;
			}
			yield break;
		}

		// Token: 0x040017B8 RID: 6072
		private const float PI = 3.1415927f;

		// Token: 0x040017B9 RID: 6073
		[SerializeField]
		private DecorationCharacterMovement _movement;

		// Token: 0x040017BA RID: 6074
		[SerializeField]
		private CustomFloat _jumpPower;

		// Token: 0x040017BB RID: 6075
		private bool _isRight = true;
	}
}
