using System;
using Data;
using Platforms;
using Singletons;
using UnityEngine;

namespace Housing
{
	// Token: 0x0200014B RID: 331
	public class HousingBuilder : MonoBehaviour
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x000133D9 File Offset: 0x000115D9
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x000133E1 File Offset: 0x000115E1
		public BuildLevel Entry
		{
			get
			{
				return this._entry;
			}
			set
			{
				this._entry = value;
			}
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000133EA File Offset: 0x000115EA
		private void Start()
		{
			this.Build();
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000133F4 File Offset: 0x000115F4
		private void Build()
		{
			int housingPoint = GameData.Progress.housingPoint;
			int housingSeen = GameData.Progress.housingSeen;
			this._entry.Build(housingPoint, housingSeen);
			if (housingPoint != housingSeen)
			{
				this.UpdateHousingSeen(housingPoint);
			}
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00013425 File Offset: 0x00011625
		private void UpdateHousingSeen(int housingSeen)
		{
			GameData.Progress.housingSeen = housingSeen;
			GameData.Progress.SaveAll();
			PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001343C File Offset: 0x0001163C
		public BuildLevel GetLevelAfterPoint(int targetOrder)
		{
			return this._entry.GetLevelAfterPoint(targetOrder);
		}

		// Token: 0x040004DA RID: 1242
		[SerializeField]
		private BuildLevel _entry;
	}
}
