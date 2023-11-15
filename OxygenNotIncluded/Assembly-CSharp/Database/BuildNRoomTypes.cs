using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D3C RID: 3388
	public class BuildNRoomTypes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A8C RID: 27276 RVA: 0x00298BD6 File Offset: 0x00296DD6
		public BuildNRoomTypes(RoomType roomType, int numToCreate = 1)
		{
			this.roomType = roomType;
			this.numToCreate = numToCreate;
		}

		// Token: 0x06006A8D RID: 27277 RVA: 0x00298BEC File Offset: 0x00296DEC
		public override bool Success()
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return num >= this.numToCreate;
		}

		// Token: 0x06006A8E RID: 27278 RVA: 0x00298C60 File Offset: 0x00296E60
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
			this.numToCreate = reader.ReadInt32();
		}

		// Token: 0x06006A8F RID: 27279 RVA: 0x00298C98 File Offset: 0x00296E98
		public override string GetProgress(bool complete)
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_N_ROOMS, this.roomType.Name, complete ? this.numToCreate : num, this.numToCreate);
		}

		// Token: 0x04004D9E RID: 19870
		private RoomType roomType;

		// Token: 0x04004D9F RID: 19871
		private int numToCreate;
	}
}
