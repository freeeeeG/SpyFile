using System;
using Characters.Projectiles;
using Level.Traps;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006A1 RID: 1697
	public struct Attacker
	{
		// Token: 0x060021E1 RID: 8673 RVA: 0x00065ADC File Offset: 0x00063CDC
		public static implicit operator Attacker(Character character)
		{
			return new Attacker(character);
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x00065AE4 File Offset: 0x00063CE4
		public static implicit operator Attacker(Trap trap)
		{
			return new Attacker(trap);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x00065AEC File Offset: 0x00063CEC
		public static implicit operator Attacker(CharacterStatus characterStatus)
		{
			return new Attacker(characterStatus);
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x00065AF4 File Offset: 0x00063CF4
		public Attacker(Character character)
		{
			this.character = character;
			this.projectile = null;
			this.trap = null;
			this.characterStatus = null;
			this.transform = character.transform;
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x00065B1E File Offset: 0x00063D1E
		public Attacker(Character character, IProjectile projectile)
		{
			this.character = character;
			this.projectile = projectile;
			this.trap = null;
			this.characterStatus = null;
			this.transform = projectile.transform;
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x00065B48 File Offset: 0x00063D48
		public Attacker(Trap trap)
		{
			this.character = null;
			this.projectile = null;
			this.trap = trap;
			this.characterStatus = null;
			this.transform = trap.transform;
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x00065B72 File Offset: 0x00063D72
		public Attacker(CharacterStatus characterStatus)
		{
			this.character = null;
			this.projectile = null;
			this.trap = null;
			this.characterStatus = characterStatus;
			this.transform = characterStatus.transform;
		}

		// Token: 0x04001CD7 RID: 7383
		public readonly Character character;

		// Token: 0x04001CD8 RID: 7384
		public readonly IProjectile projectile;

		// Token: 0x04001CD9 RID: 7385
		public readonly Trap trap;

		// Token: 0x04001CDA RID: 7386
		public readonly CharacterStatus characterStatus;

		// Token: 0x04001CDB RID: 7387
		public readonly Transform transform;
	}
}
