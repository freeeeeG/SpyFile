using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B52 RID: 2898
	[Serializable]
	public class OnJump : Trigger
	{
		// Token: 0x06003A2E RID: 14894 RVA: 0x000AA8C2 File Offset: 0x000A8AC2
		public OnJump()
		{
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x000ABFC0 File Offset: 0x000AA1C0
		public OnJump(JumpTypeBoolArray types)
		{
			this._types = types;
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x000ABFCF File Offset: 0x000AA1CF
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.movement.onJump += this.OnCharacterJump;
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x000ABFF4 File Offset: 0x000AA1F4
		private void OnCharacterJump(Movement.JumpType jumpType, float jumpHeight)
		{
			if (!this._types[jumpType])
			{
				return;
			}
			base.Invoke();
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x000AC00B File Offset: 0x000AA20B
		public override void Detach()
		{
			this._character.movement.onJump -= this.OnCharacterJump;
		}

		// Token: 0x04002E40 RID: 11840
		[SerializeField]
		private JumpTypeBoolArray _types;

		// Token: 0x04002E41 RID: 11841
		private Character _character;
	}
}
