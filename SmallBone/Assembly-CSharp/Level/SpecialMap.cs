using System;
using Data;
using Services;
using UnityEngine;

namespace Level
{
	// Token: 0x02000527 RID: 1319
	[RequireComponent(typeof(Map))]
	public class SpecialMap : MonoBehaviour
	{
		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x060019EC RID: 6636 RVA: 0x00051361 File Offset: 0x0004F561
		public Map map
		{
			get
			{
				return this._map;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x00051369 File Offset: 0x0004F569
		public SpecialMap.Type type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x060019EE RID: 6638 RVA: 0x00051371 File Offset: 0x0004F571
		// (set) Token: 0x060019EF RID: 6639 RVA: 0x0005137E File Offset: 0x0004F57E
		public bool encountered
		{
			get
			{
				return SpecialMap.GetEncoutered(this._type);
			}
			set
			{
				SpecialMap.SetEncoutered(this._type, value);
			}
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x0005138C File Offset: 0x0004F58C
		public static bool GetEncoutered(SpecialMap.Type type)
		{
			return GameData.Progress.specialMapEncountered.GetData(type);
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00051399 File Offset: 0x0004F599
		public static void SetEncoutered(SpecialMap.Type type, bool value)
		{
			GameData.Progress.specialMapEncountered.SetData(type, value);
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x000513A7 File Offset: 0x0004F5A7
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			this.encountered = true;
		}

		// Token: 0x040016AF RID: 5807
		[SerializeField]
		[GetComponent]
		private Map _map;

		// Token: 0x040016B0 RID: 5808
		[SerializeField]
		private SpecialMap.Type _type;

		// Token: 0x02000528 RID: 1320
		public enum Type
		{
			// Token: 0x040016B2 RID: 5810
			Gauntlet,
			// Token: 0x040016B3 RID: 5811
			MysticalRuin,
			// Token: 0x040016B4 RID: 5812
			TimeCostChest,
			// Token: 0x040016B5 RID: 5813
			Treasure,
			// Token: 0x040016B6 RID: 5814
			TroopDefence,
			// Token: 0x040016B7 RID: 5815
			DangerouseChests,
			// Token: 0x040016B8 RID: 5816
			EssenceSanctuary
		}
	}
}
