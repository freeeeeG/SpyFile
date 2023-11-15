using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear;
using Characters.Movements;
using Level;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
	// Token: 0x020006EA RID: 1770
	public class DropLootOnDie : MonoBehaviour
	{
		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060023C9 RID: 9161 RVA: 0x0006B48E File Offset: 0x0006968E
		public int gold
		{
			get
			{
				return this._gold;
			}
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060023CA RID: 9162 RVA: 0x0006B496 File Offset: 0x00069696
		public DropLootOnDie.DarkQuartzPossibility.Reorderable darkQuartzes
		{
			get
			{
				return this._darkQuartzes;
			}
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x0006B49E File Offset: 0x0006969E
		private void Awake()
		{
			this._character.health.onDie += this.OnDie;
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x0006B4BC File Offset: 0x000696BC
		private void OnDie()
		{
			if (this._character.health.dead)
			{
				return;
			}
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			Movement movement = this._character.movement;
			Push push = (movement != null) ? movement.push : null;
			Vector2 force = Vector2.zero;
			if (push != null && !push.expired)
			{
				force = push.direction * push.totalForce;
			}
			levelManager.DropGold(this._gold, this._count, base.transform.position, force);
			levelManager.DropDarkQuartz(this._darkQuartzes.Take(), base.transform.position, force);
			List<DropMovement> list = new List<DropMovement>();
			Potion potion = this._potionPossibilities.Get();
			if (potion != null)
			{
				Potion potion2 = levelManager.DropPotion(potion, base.transform.position);
				list.Add(potion2.dropMovement);
			}
			if (MMMaths.PercentChance(this._gearChance))
			{
				Gear gear = Singleton<Service>.Instance.levelManager.DropGear(this._gear, base.transform.position);
				list.Add(gear.dropped.dropMovement);
			}
			DropMovement.SetMultiDropHorizontalInterval(list);
		}

		// Token: 0x04001E78 RID: 7800
		private const float _droppedGearHorizontalInterval = 1.5f;

		// Token: 0x04001E79 RID: 7801
		private const float _droppedGearHorizontalSpeed = 2f;

		// Token: 0x04001E7A RID: 7802
		[GetComponent]
		[SerializeField]
		private Character _character;

		// Token: 0x04001E7B RID: 7803
		[FormerlySerializedAs("_amount")]
		[SerializeField]
		private int _gold;

		// Token: 0x04001E7C RID: 7804
		[SerializeField]
		private DropLootOnDie.DarkQuartzPossibility.Reorderable _darkQuartzes;

		// Token: 0x04001E7D RID: 7805
		[SerializeField]
		private int _count;

		// Token: 0x04001E7E RID: 7806
		[SerializeField]
		private Gear _gear;

		// Token: 0x04001E7F RID: 7807
		[Range(0f, 100f)]
		[SerializeField]
		private int _gearChance;

		// Token: 0x04001E80 RID: 7808
		[SerializeField]
		private PotionPossibilities _potionPossibilities;

		// Token: 0x020006EB RID: 1771
		[Serializable]
		public class DarkQuartzPossibility
		{
			// Token: 0x04001E81 RID: 7809
			[Range(0f, 100f)]
			public int weight;

			// Token: 0x04001E82 RID: 7810
			public CustomFloat amount;

			// Token: 0x020006EC RID: 1772
			[Serializable]
			public class Reorderable : ReorderableArray<DropLootOnDie.DarkQuartzPossibility>
			{
				// Token: 0x060023CF RID: 9167 RVA: 0x0006B5E4 File Offset: 0x000697E4
				public int Take()
				{
					if (this.values.Length == 0)
					{
						return 0;
					}
					int maxExclusive = this.values.Sum((DropLootOnDie.DarkQuartzPossibility v) => v.weight);
					int num = UnityEngine.Random.Range(0, maxExclusive) + 1;
					for (int i = 0; i < this.values.Length; i++)
					{
						num -= this.values[i].weight;
						if (num <= 0)
						{
							return (int)this.values[i].amount.value;
						}
					}
					return 0;
				}

				// Token: 0x060023D0 RID: 9168 RVA: 0x0006B670 File Offset: 0x00069870
				public float GetAverage()
				{
					float num = (float)this.values.Sum((DropLootOnDie.DarkQuartzPossibility v) => v.weight);
					float num2 = 0f;
					foreach (DropLootOnDie.DarkQuartzPossibility darkQuartzPossibility in this.values)
					{
						num2 += (float)((int)darkQuartzPossibility.amount.value * darkQuartzPossibility.weight) / num;
					}
					return num2;
				}
			}
		}

		// Token: 0x020006EE RID: 1774
		[Serializable]
		private class GearInfo
		{
			// Token: 0x04001E86 RID: 7814
			[SerializeField]
			private Gear _gear;

			// Token: 0x04001E87 RID: 7815
			[Range(1f, 100f)]
			[SerializeField]
			private int _weight;
		}
	}
}
