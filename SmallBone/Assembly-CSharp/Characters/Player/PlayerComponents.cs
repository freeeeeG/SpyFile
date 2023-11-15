using System;
using Characters.Abilities;

namespace Characters.Player
{
	// Token: 0x020007F6 RID: 2038
	public class PlayerComponents : IDisposable
	{
		// Token: 0x0600296E RID: 10606 RVA: 0x0007E8FC File Offset: 0x0007CAFC
		public PlayerComponents(Character character)
		{
			this.character = character;
			this.inventory = new Inventory(character);
			this.combatDetector = new CombatDetector(character);
			this.minionLeader = new MinionLeader(character);
			this.visibility = character.gameObject.AddComponent<Visibility>();
			this.savableAbilityManager = character.GetComponent<SavableAbilityManager>();
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x0007E957 File Offset: 0x0007CB57
		public void Dispose()
		{
			this.minionLeader.Dispose();
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x0007E964 File Offset: 0x0007CB64
		public void Initialize()
		{
			this.inventory.Initialize();
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x0007E971 File Offset: 0x0007CB71
		public void Update(float deltaTime)
		{
			this.combatDetector.Update(deltaTime);
		}

		// Token: 0x0400239C RID: 9116
		public readonly Character character;

		// Token: 0x0400239D RID: 9117
		public readonly Inventory inventory;

		// Token: 0x0400239E RID: 9118
		public readonly CombatDetector combatDetector;

		// Token: 0x0400239F RID: 9119
		public readonly MinionLeader minionLeader;

		// Token: 0x040023A0 RID: 9120
		public readonly Visibility visibility;

		// Token: 0x040023A1 RID: 9121
		public readonly SavableAbilityManager savableAbilityManager;
	}
}
