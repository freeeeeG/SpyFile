using System;
using UnityEngine;

namespace flanne.PerkSystem
{
	// Token: 0x02000184 RID: 388
	[Serializable]
	public class PerkEffect
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x000268E7 File Offset: 0x00024AE7
		public void Equip(PlayerController player)
		{
			if (!this._subscribed)
			{
				this._subscribed = true;
				this.trigger.Triggered += this.OnTriggered;
				this.action.Init();
			}
			this.trigger.OnEquip(player);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00026926 File Offset: 0x00024B26
		public void UnEquip(PlayerController player)
		{
			this._subscribed = false;
			this.trigger.Triggered -= this.OnTriggered;
			this.trigger.OnUnEquip(player);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00026952 File Offset: 0x00024B52
		private void OnTriggered(object sender, GameObject target)
		{
			if (this.limitActivations)
			{
				if (this._activations >= this.limit)
				{
					return;
				}
				this._activations++;
			}
			this.action.Activate(target);
		}

		// Token: 0x040006DD RID: 1757
		[SerializeField]
		private bool limitActivations;

		// Token: 0x040006DE RID: 1758
		[SerializeField]
		private int limit = 1;

		// Token: 0x040006DF RID: 1759
		[NonSerialized]
		private int _activations;

		// Token: 0x040006E0 RID: 1760
		[SerializeReference]
		private Trigger trigger;

		// Token: 0x040006E1 RID: 1761
		[SerializeReference]
		private Action action;

		// Token: 0x040006E2 RID: 1762
		[NonSerialized]
		private bool _subscribed;
	}
}
