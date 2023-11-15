using System;
using System.Collections.Generic;
using Level;
using Services;
using Singletons;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Gear.Weapons
{
	// Token: 0x02000825 RID: 2085
	public sealed class GraveDiggerGraveContainer : MonoBehaviour
	{
		// Token: 0x06002B0F RID: 11023 RVA: 0x00084A74 File Offset: 0x00082C74
		private void Awake()
		{
			this._owner = Singleton<Service>.Instance.levelManager.player;
			this._graves = new List<GraveDiggerGrave>();
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.Clear;
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x00084AB1 File Offset: 0x00082CB1
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.Clear;
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x00084AD8 File Offset: 0x00082CD8
		public void Clear()
		{
			foreach (GraveDiggerGrave graveDiggerGrave in this._graves)
			{
				graveDiggerGrave.Despawn();
			}
			this._graves.Clear();
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x00084B34 File Offset: 0x00082D34
		private void Update()
		{
			this.hasActivatedGrave = false;
			foreach (GraveDiggerGrave graveDiggerGrave in this._graves)
			{
				if (this._graveActivatingRange.OverlapPoint(graveDiggerGrave.position))
				{
					graveDiggerGrave.Activate();
					this.hasActivatedGrave = true;
				}
				else
				{
					graveDiggerGrave.Deactivate();
				}
			}
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x00084BB4 File Offset: 0x00082DB4
		public void Add(GraveDiggerGrave grave)
		{
			this._graves.Add(grave);
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x00084BC2 File Offset: 0x00082DC2
		public bool Remove(GraveDiggerGrave grave)
		{
			return this._graves.Remove(grave);
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x00084BD0 File Offset: 0x00082DD0
		public void SummonMinionFromGraves(Minion minionPrefab)
		{
			this.SummonMinionFromGraves(minionPrefab, int.MaxValue);
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x00084BE0 File Offset: 0x00082DE0
		public void SummonMinionFromGraves(Minion minionPrefab, int maxCount)
		{
			if (maxCount <= 0)
			{
				return;
			}
			this._graves.Sort(delegate(GraveDiggerGrave a, GraveDiggerGrave b)
			{
				float x = this._owner.transform.position.x;
				float num2 = math.abs(x - a.position.x);
				float num3 = math.abs(x - b.position.x);
				if (num2 < num3)
				{
					return 1;
				}
				return -1;
			});
			int count = this._graves.Count;
			for (int i = 0; i < maxCount; i++)
			{
				int num = count - i - 1;
				if (num < 0)
				{
					break;
				}
				GraveDiggerGrave graveDiggerGrave = this._graves[num];
				if (!graveDiggerGrave.activated)
				{
					maxCount++;
				}
				else
				{
					this._owner.playerComponents.minionLeader.Summon(minionPrefab, graveDiggerGrave.transform.position, null);
					graveDiggerGrave.Despawn();
					this._graves.RemoveAt(num);
				}
			}
		}

		// Token: 0x040024B2 RID: 9394
		private Character _owner;

		// Token: 0x040024B3 RID: 9395
		private List<GraveDiggerGrave> _graves;

		// Token: 0x040024B4 RID: 9396
		[SerializeField]
		private Collider2D _graveActivatingRange;

		// Token: 0x040024B5 RID: 9397
		public bool hasActivatedGrave;
	}
}
