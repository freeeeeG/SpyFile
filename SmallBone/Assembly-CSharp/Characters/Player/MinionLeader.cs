using System;
using System.Collections.Generic;
using Characters.Minions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Player
{
	// Token: 0x020007F0 RID: 2032
	public class MinionLeader : IDisposable
	{
		// Token: 0x06002940 RID: 10560 RVA: 0x0007E029 File Offset: 0x0007C229
		public MinionLeader(Character player)
		{
			this.player = player;
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.ResetMinionPositions;
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x0007E05E File Offset: 0x0007C25E
		public void Dispose()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.ResetMinionPositions;
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x0007E083 File Offset: 0x0007C283
		public IEnumerable<Minion> GetMinionEnumerable()
		{
			foreach (KeyValuePair<Minion, MinionGroup> keyValuePair in this._minionGroups)
			{
				foreach (Minion minion in keyValuePair.Value)
				{
					yield return minion;
				}
				IEnumerator<Minion> enumerator2 = null;
			}
			Dictionary<Minion, MinionGroup>.Enumerator enumerator = default(Dictionary<Minion, MinionGroup>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x0007E093 File Offset: 0x0007C293
		public IEnumerable<Minion> GetMinionEnumerable(Minion targetMinion)
		{
			MinionGroup minionGroup;
			if (!this._minionGroups.TryGetValue(targetMinion, out minionGroup))
			{
				yield break;
			}
			foreach (Minion minion in minionGroup)
			{
				yield return minion;
			}
			IEnumerator<Minion> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x0007E0AC File Offset: 0x0007C2AC
		public Minion Summon(Minion minionPrefab, Vector3 position, MinionSetting seting)
		{
			MinionGroup minionGroup;
			if (!this._minionGroups.TryGetValue(minionPrefab, out minionGroup))
			{
				minionGroup = new MinionGroup();
				this._minionGroups.Add(minionPrefab, minionGroup);
			}
			if (minionGroup.Count >= minionPrefab.maxCount)
			{
				minionGroup.DespawnOldest();
			}
			return minionPrefab.Summon(this, position, minionGroup, seting);
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x0007E0FC File Offset: 0x0007C2FC
		public void Commands(Minion target, MinionLeader.MinionCommands commands)
		{
			if (!this._minionGroups.ContainsKey(target))
			{
				return;
			}
			MinionGroup minionGroup;
			this._minionGroups.TryGetValue(target, out minionGroup);
			foreach (Minion minion in minionGroup)
			{
				if (commands != null)
				{
					commands(minion);
				}
			}
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x0007E168 File Offset: 0x0007C368
		public void DespawnAll(Minion target)
		{
			if (!this._minionGroups.ContainsKey(target))
			{
				return;
			}
			MinionGroup minionGroup;
			this._minionGroups.TryGetValue(target, out minionGroup);
			minionGroup.DespawnAll();
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x0007E19C File Offset: 0x0007C39C
		private void ResetMinionPositions()
		{
			foreach (KeyValuePair<Minion, MinionGroup> keyValuePair in this._minionGroups)
			{
				foreach (Minion minion in keyValuePair.Value)
				{
					minion.transform.position = this.player.transform.position;
					if (minion.character.movement != null)
					{
						minion.character.movement.controller.ResetBounds();
					}
				}
			}
		}

		// Token: 0x04002384 RID: 9092
		public readonly Character player;

		// Token: 0x04002385 RID: 9093
		private readonly Dictionary<Minion, MinionGroup> _minionGroups = new Dictionary<Minion, MinionGroup>();

		// Token: 0x020007F1 RID: 2033
		// (Invoke) Token: 0x06002949 RID: 10569
		public delegate void MinionCommands(Minion minion);
	}
}
