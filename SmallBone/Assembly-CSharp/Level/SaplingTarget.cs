using System;
using Characters;
using Characters.Minions;
using UnityEngine;

namespace Level
{
	// Token: 0x0200047D RID: 1149
	public class SaplingTarget : MonoBehaviour
	{
		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060015E7 RID: 5607 RVA: 0x00044BD9 File Offset: 0x00042DD9
		public bool spawnable
		{
			get
			{
				return this._spawnable;
			}
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x00044BE4 File Offset: 0x00042DE4
		public Minion SummonEntMinion(Character owner, float lifeTime)
		{
			Vector3 position = base.transform.position;
			Minion result = owner.playerComponents.minionLeader.Summon(this._entMinion, position, this._overrideSetting);
			this._sapling.Despawn();
			this._spawnable = false;
			return result;
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00044C2C File Offset: 0x00042E2C
		private void OnEnable()
		{
			this._spawnableCool = this._spawnableTime;
			this._spawnable = true;
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x00044C2C File Offset: 0x00042E2C
		private void OnDisable()
		{
			this._spawnableCool = this._spawnableTime;
			this._spawnable = true;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x00044C41 File Offset: 0x00042E41
		private void Update()
		{
			if (this._spawnableCool <= 0f)
			{
				return;
			}
			this._spawnableCool -= Chronometer.global.deltaTime;
		}

		// Token: 0x04001328 RID: 4904
		[SerializeField]
		private Minion _entMinion;

		// Token: 0x04001329 RID: 4905
		[SerializeField]
		private EntSapling _sapling;

		// Token: 0x0400132A RID: 4906
		[SerializeField]
		private LayerMask _terrainLayer;

		// Token: 0x0400132B RID: 4907
		[SerializeField]
		private float _spawnableTime = 0.1f;

		// Token: 0x0400132C RID: 4908
		[SerializeField]
		private CharacterSynchronization _sync;

		// Token: 0x0400132D RID: 4909
		[SerializeField]
		private MinionSetting _overrideSetting;

		// Token: 0x0400132E RID: 4910
		private float _spawnableCool;

		// Token: 0x0400132F RID: 4911
		private bool _spawnable = true;
	}
}
