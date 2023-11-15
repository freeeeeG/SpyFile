using System;
using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks
{
	// Token: 0x020013E5 RID: 5093
	public class TryToHit : Attack
	{
		// Token: 0x06006452 RID: 25682 RVA: 0x00123267 File Offset: 0x00121467
		public override IEnumerator CRun(AIController controller)
		{
			base.result = Behaviour.Result.Doing;
			this.gaveDamage = false;
			Character character = controller.character;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(this.Character_onGaveDamage));
			if (this._attack.TryStart())
			{
				while (this._attack.running && !this.gaveDamage)
				{
					yield return null;
				}
			}
			Character character2 = controller.character;
			character2.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character2.onGaveDamage, new GaveDamageDelegate(this.Character_onGaveDamage));
			if (this.gaveDamage)
			{
				base.result = Behaviour.Result.Success;
				yield break;
			}
			base.result = Behaviour.Result.Fail;
			yield break;
		}

		// Token: 0x06006453 RID: 25683 RVA: 0x0012327D File Offset: 0x0012147D
		private void Character_onGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (string.IsNullOrEmpty(this._key))
			{
				this.gaveDamage = true;
				return;
			}
			if (originalDamage.key == this._key)
			{
				this.gaveDamage = true;
			}
		}

		// Token: 0x040050EB RID: 20715
		[SerializeField]
		private Characters.Actions.Action _attack;

		// Token: 0x040050EC RID: 20716
		[SerializeField]
		private string _key;
	}
}
