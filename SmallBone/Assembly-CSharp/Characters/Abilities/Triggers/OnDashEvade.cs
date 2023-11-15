using System;
using Characters.Actions;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B31 RID: 2865
	[Serializable]
	public sealed class OnDashEvade : Trigger
	{
		// Token: 0x060039E6 RID: 14822 RVA: 0x000AB000 File Offset: 0x000A9200
		public override void Attach(Character character)
		{
			this._character = character;
			this._character.onEvade += this.OnEvade;
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x000AB020 File Offset: 0x000A9220
		private void OnEvade(ref Damage damage)
		{
			foreach (Characters.Actions.Action action in this._character.actions)
			{
				if (action.type == Characters.Actions.Action.Type.Dash && action.running)
				{
					base.Invoke();
					break;
				}
			}
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x000AB08C File Offset: 0x000A928C
		public override void Detach()
		{
			this._character.onEvade -= this.OnEvade;
		}

		// Token: 0x04002DF4 RID: 11764
		private Character _character;
	}
}
