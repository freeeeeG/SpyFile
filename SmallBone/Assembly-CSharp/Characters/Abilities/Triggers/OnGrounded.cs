using System;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B42 RID: 2882
	[Serializable]
	public class OnGrounded : Trigger
	{
		// Token: 0x06003A11 RID: 14865 RVA: 0x000AB7E4 File Offset: 0x000A99E4
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.movement.onGrounded += base.Invoke;
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x000AB809 File Offset: 0x000A9A09
		public override void Detach()
		{
			this._character.movement.onGrounded -= base.Invoke;
		}

		// Token: 0x04002E14 RID: 11796
		private Character _character;
	}
}
