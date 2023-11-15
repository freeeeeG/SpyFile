using System;
using Data;
using Platforms;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007FB RID: 2043
	public class PlayerKillCounter : MonoBehaviour
	{
		// Token: 0x06002984 RID: 10628 RVA: 0x0007EBC8 File Offset: 0x0007CDC8
		private void Awake()
		{
			Character character = this._character;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(this.CountKill));
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x0007EBF4 File Offset: 0x0007CDF4
		private void CountKill(ITarget target, ref Damage damage)
		{
			if (target.character == null)
			{
				return;
			}
			switch (target.character.type)
			{
			case Character.Type.TrashMob:
			case Character.Type.Boss:
				GameData.Progress.kills++;
				return;
			case Character.Type.Named:
				break;
			case Character.Type.Adventurer:
				GameData.Progress.kills++;
				GameData.Progress.eliteKills++;
				GameData.Progress.totalAdventurerKills++;
				if (GameData.Progress.totalAdventurerKills >= 100)
				{
					Achievement.Type.HeroSlayer.Set();
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x040023AB RID: 9131
		[SerializeField]
		private Character _character;
	}
}
