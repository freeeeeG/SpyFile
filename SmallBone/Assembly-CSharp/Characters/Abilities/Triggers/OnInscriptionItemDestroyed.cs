using System;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B50 RID: 2896
	[Serializable]
	public sealed class OnInscriptionItemDestroyed : Trigger
	{
		// Token: 0x06003A28 RID: 14888 RVA: 0x000ABE65 File Offset: 0x000AA065
		public override void Attach(Character character)
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged += this.HandleItemInstanceChanged;
			this.HandleItemInstanceChanged();
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x000ABE88 File Offset: 0x000AA088
		private void HandleItemInstanceChanged()
		{
			foreach (Inscription.Key key in this._keys)
			{
				foreach (Item item in Singleton<Service>.Instance.gearManager.GetItemInstanceByKeyword(key))
				{
					item.onDiscard -= this.TryInvoke;
					item.onDiscard += this.TryInvoke;
				}
			}
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x000ABF14 File Offset: 0x000AA114
		public void TryInvoke(Gear gear)
		{
			if (gear.destructible)
			{
				base.Invoke();
			}
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x000ABF24 File Offset: 0x000AA124
		public override void Detach()
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= this.HandleItemInstanceChanged;
			foreach (Inscription.Key key in this._keys)
			{
				foreach (Item item in Singleton<Service>.Instance.gearManager.GetItemInstanceByKeyword(key))
				{
					item.onDiscard -= this.TryInvoke;
				}
			}
		}

		// Token: 0x04002E3F RID: 11839
		[SerializeField]
		private Inscription.Key[] _keys;
	}
}
