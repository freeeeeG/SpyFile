using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x0200015A RID: 346
	public class SpawnerRune : Rune
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00024F0D File Offset: 0x0002310D
		private float cooldown
		{
			get
			{
				return this.player.stats[StatType.SummonAttackSpeed].ModifyInverse(this.baseCooldown - this.cooldownReductionPerLevel * (float)this.level);
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00024F3A File Offset: 0x0002313A
		protected override void Init()
		{
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.spawnPrefab.name, this.spawnPrefab, 10, true);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00024F68 File Offset: 0x00023168
		private void Update()
		{
			this._timer += Time.deltaTime;
			if (this._timer > this.cooldown)
			{
				this._timer -= this.cooldown;
				GameObject pooledObject = this.OP.GetPooledObject(this.spawnPrefab.name);
				pooledObject.transform.position = base.transform.position;
				Spawn component = pooledObject.GetComponent<Spawn>();
				if (component != null)
				{
					component.player = this.player;
				}
				pooledObject.SetActive(true);
				SoundEffectSO soundEffectSO = this.soundFX;
				if (soundEffectSO == null)
				{
					return;
				}
				soundEffectSO.Play(null);
			}
		}

		// Token: 0x04000691 RID: 1681
		[SerializeField]
		private GameObject spawnPrefab;

		// Token: 0x04000692 RID: 1682
		[SerializeField]
		private float cooldownReductionPerLevel;

		// Token: 0x04000693 RID: 1683
		[SerializeField]
		private float baseCooldown;

		// Token: 0x04000694 RID: 1684
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000695 RID: 1685
		private float _timer;

		// Token: 0x04000696 RID: 1686
		private ObjectPooler OP;
	}
}
