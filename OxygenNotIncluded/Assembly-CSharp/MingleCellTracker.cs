using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
[AddComponentMenu("KMonoBehaviour/scripts/MingleCellTracker")]
public class MingleCellTracker : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06001C3B RID: 7227 RVA: 0x00096F58 File Offset: 0x00095158
	public void Sim1000ms(float dt)
	{
		this.mingleCells.Clear();
		RoomProber roomProber = Game.Instance.roomProber;
		MinionGroupProber minionGroupProber = MinionGroupProber.Get();
		foreach (Room room in roomProber.rooms)
		{
			if (room.roomType == Db.Get().RoomTypes.RecRoom)
			{
				for (int i = room.cavity.minY; i <= room.cavity.maxY; i++)
				{
					for (int j = room.cavity.minX; j <= room.cavity.maxX; j++)
					{
						int num = Grid.XYToCell(j, i);
						if (roomProber.GetCavityForCell(num) == room.cavity && minionGroupProber.IsReachable(num) && !Grid.HasLadder[num] && !Grid.HasTube[num] && !Grid.IsLiquid(num) && Grid.Element[num].id == SimHashes.Oxygen)
						{
							this.mingleCells.Add(num);
						}
					}
				}
			}
		}
	}

	// Token: 0x04000F8D RID: 3981
	public List<int> mingleCells = new List<int>();
}
