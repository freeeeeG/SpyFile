using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D3B RID: 3387
	public class BuildRoomType : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A88 RID: 27272 RVA: 0x00298B17 File Offset: 0x00296D17
		public BuildRoomType(RoomType roomType)
		{
			this.roomType = roomType;
		}

		// Token: 0x06006A89 RID: 27273 RVA: 0x00298B28 File Offset: 0x00296D28
		public override bool Success()
		{
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006A8A RID: 27274 RVA: 0x00298B90 File Offset: 0x00296D90
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
		}

		// Token: 0x06006A8B RID: 27275 RVA: 0x00298BBA File Offset: 0x00296DBA
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_A_ROOM, this.roomType.Name);
		}

		// Token: 0x04004D9D RID: 19869
		private RoomType roomType;
	}
}
