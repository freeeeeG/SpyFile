using System;
using Services;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x02000899 RID: 2201
	public abstract class InscriptionInstance : MonoBehaviour, IComparable<InscriptionInstance>
	{
		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06002EA3 RID: 11939 RVA: 0x0008C728 File Offset: 0x0008A928
		// (set) Token: 0x06002EA4 RID: 11940 RVA: 0x0008C730 File Offset: 0x0008A930
		public Character character { get; private set; }

		// Token: 0x06002EA5 RID: 11941 RVA: 0x0008C739 File Offset: 0x0008A939
		public void Initialize(Character character)
		{
			this.character = character;
			this.Initialize();
		}

		// Token: 0x06002EA6 RID: 11942
		protected abstract void Initialize();

		// Token: 0x06002EA7 RID: 11943
		public abstract void UpdateBonus(bool wasActive, bool wasOmen);

		// Token: 0x06002EA8 RID: 11944
		public abstract void Attach();

		// Token: 0x06002EA9 RID: 11945
		public abstract void Detach();

		// Token: 0x06002EAA RID: 11946 RVA: 0x0008C748 File Offset: 0x0008A948
		public int CompareTo(InscriptionInstance other)
		{
			if (this.keyword.key == other.keyword.key)
			{
				return 0;
			}
			if (this.keyword.key <= other.keyword.key)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x0008C77F File Offset: 0x0008A97F
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			this.Detach();
		}

		// Token: 0x040026CB RID: 9931
		public Inscription keyword;
	}
}
