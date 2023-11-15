using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200009B RID: 155
	public class BuffPlayerStats : MonoBehaviour
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x0001A5E5 File Offset: 0x000187E5
		private void Start()
		{
			this.player = base.GetComponentInParent<PlayerController>();
			this.stats = this.player.stats;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001A604 File Offset: 0x00018804
		public void AddStack()
		{
			this._stacks++;
			this.ApplyBuff();
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001A61C File Offset: 0x0001881C
		public void RemoveStacks()
		{
			for (int i = 0; i < this._stacks; i++)
			{
				this.RemoveBuff();
			}
			this._stacks = 0;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001A648 File Offset: 0x00018848
		public void ApplyBuff()
		{
			foreach (StatChange statChange in this.statChanges)
			{
				if (statChange.isFlatMod)
				{
					this.stats[statChange.type].AddFlatBonus(statChange.flatValue);
				}
				else if (statChange.value > 0f)
				{
					this.stats[statChange.type].AddMultiplierBonus(statChange.value);
				}
				if (statChange.type == StatType.MaxHP)
				{
					this.player.playerHealth.maxHP = Mathf.FloorToInt(this.stats[statChange.type].Modify((float)this.player.loadedCharacter.startHP));
				}
				if (statChange.type == StatType.CharacterSize)
				{
					this.player.playerSprite.transform.localScale = Vector3.one * this.stats[statChange.type].Modify(1f);
				}
				if (statChange.type == StatType.PickupRange)
				{
					GameObject.FindGameObjectWithTag("Pickupper").transform.localScale = Vector3.one * this.stats[statChange.type].Modify(1f);
				}
				if (statChange.type == StatType.VisionRange)
				{
					GameObject.FindGameObjectWithTag("PlayerVision").transform.localScale = Vector3.one * this.stats[statChange.type].Modify(1f);
				}
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001A7D8 File Offset: 0x000189D8
		public void RemoveBuff()
		{
			foreach (StatChange statChange in this.statChanges)
			{
				if (statChange.isFlatMod)
				{
					this.stats[statChange.type].AddFlatBonus(-1 * statChange.flatValue);
				}
				else if (statChange.value > 0f)
				{
					this.stats[statChange.type].AddMultiplierBonus(-1f * statChange.value);
				}
				if (statChange.type == StatType.MaxHP)
				{
					this.player.playerHealth.maxHP = Mathf.FloorToInt(this.stats[statChange.type].Modify((float)this.player.loadedCharacter.startHP));
				}
				if (statChange.type == StatType.CharacterSize)
				{
					this.player.playerSprite.transform.localScale = Vector3.one * this.stats[statChange.type].Modify(1f);
				}
				if (statChange.type == StatType.PickupRange)
				{
					GameObject.FindGameObjectWithTag("Pickupper").transform.localScale = Vector3.one * this.stats[statChange.type].Modify(1f);
				}
				if (statChange.type == StatType.VisionRange)
				{
					GameObject.FindGameObjectWithTag("PlayerVision").transform.localScale = Vector3.one * this.stats[statChange.type].Modify(1f);
				}
			}
		}

		// Token: 0x0400036D RID: 877
		[SerializeField]
		private StatChange[] statChanges = new StatChange[0];

		// Token: 0x0400036E RID: 878
		private PlayerController player;

		// Token: 0x0400036F RID: 879
		private StatsHolder stats;

		// Token: 0x04000370 RID: 880
		private int _stacks;
	}
}
