using System;
using System.Collections.Generic;
using Services;
using Singletons;

namespace Characters.Abilities.Weapons.Fighter
{
	// Token: 0x02000C13 RID: 3091
	public class ChallengerMarkPassiveComponent : AbilityComponent<ChallengerMarkPassive>
	{
		// Token: 0x06003F85 RID: 16261 RVA: 0x000B863A File Offset: 0x000B683A
		public override void Initialize()
		{
			base.Initialize();
			this._challengerMarks = new List<ChallengerMarkPassive.Instance>(1);
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.Clear;
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x000B8669 File Offset: 0x000B6869
		private void Clear()
		{
			this._challengerMarks.Clear();
		}

		// Token: 0x06003F87 RID: 16263 RVA: 0x000B8676 File Offset: 0x000B6876
		public void Add(ChallengerMarkPassive.Instance instance)
		{
			this._challengerMarks.Add(instance);
		}

		// Token: 0x06003F88 RID: 16264 RVA: 0x000B8684 File Offset: 0x000B6884
		public int Count()
		{
			return this._challengerMarks.Count;
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x000B8691 File Offset: 0x000B6891
		public void Remove(ChallengerMarkPassive.Instance instance)
		{
			this._challengerMarks.Remove(instance);
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x000B86A0 File Offset: 0x000B68A0
		private void OnDestroy()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.Clear;
		}

		// Token: 0x040030E7 RID: 12519
		private List<ChallengerMarkPassive.Instance> _challengerMarks;
	}
}
