using System;
using Characters.Movements;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B24 RID: 2852
	[Serializable]
	public class OnBackAttack : Trigger
	{
		// Token: 0x060039CC RID: 14796 RVA: 0x000AAB50 File Offset: 0x000A8D50
		public override void Attach(Character character)
		{
			this._character = character;
			Character character2 = this._character;
			character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x000AAB80 File Offset: 0x000A8D80
		public override void Detach()
		{
			Character character = this._character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(this.OnGaveDamage));
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x000AABAC File Offset: 0x000A8DAC
		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (!(target.character == null))
			{
				Movement movement = target.character.movement;
				if (movement == null || movement.config.type != Movement.Config.Type.Static)
				{
					int num = Math.Sign(this._character.transform.position.x - target.transform.position.x);
					if ((num == -1 && target.character.lookingDirection == Character.LookingDirection.Right) || (num == 1 && target.character.lookingDirection == Character.LookingDirection.Left))
					{
						base.Invoke();
					}
					return;
				}
			}
		}

		// Token: 0x04002DDF RID: 11743
		private Character _character;
	}
}
