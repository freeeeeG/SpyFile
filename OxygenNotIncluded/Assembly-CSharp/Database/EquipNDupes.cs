using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000D40 RID: 3392
	public class EquipNDupes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A9D RID: 27293 RVA: 0x00299089 File Offset: 0x00297289
		public EquipNDupes(AssignableSlot equipmentSlot, int numToEquip)
		{
			this.equipmentSlot = equipmentSlot;
			this.numToEquip = numToEquip;
		}

		// Token: 0x06006A9E RID: 27294 RVA: 0x002990A0 File Offset: 0x002972A0
		public override bool Success()
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return num >= this.numToEquip;
		}

		// Token: 0x06006A9F RID: 27295 RVA: 0x00299120 File Offset: 0x00297320
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.equipmentSlot = Db.Get().AssignableSlots.Get(id);
			this.numToEquip = reader.ReadInt32();
		}

		// Token: 0x06006AA0 RID: 27296 RVA: 0x00299158 File Offset: 0x00297358
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CLOTHE_DUPES, complete ? this.numToEquip : num, this.numToEquip);
		}

		// Token: 0x04004DA2 RID: 19874
		private AssignableSlot equipmentSlot;

		// Token: 0x04004DA3 RID: 19875
		private int numToEquip;
	}
}
