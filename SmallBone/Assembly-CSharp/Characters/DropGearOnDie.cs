using System;
using Characters.Gear;
using Services;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006E9 RID: 1769
	public class DropGearOnDie : MonoBehaviour
	{
		// Token: 0x060023C6 RID: 9158 RVA: 0x0006B420 File Offset: 0x00069620
		private void Awake()
		{
			this._character.health.onDie += this.OnDie;
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x0006B440 File Offset: 0x00069640
		private void OnDie()
		{
			if (this._character.health.dead)
			{
				return;
			}
			if (MMMaths.PercentChance(this._chance))
			{
				Singleton<Service>.Instance.levelManager.DropGear(this._gear, base.transform.position);
			}
		}

		// Token: 0x04001E75 RID: 7797
		[SerializeField]
		[GetComponent]
		private Character _character;

		// Token: 0x04001E76 RID: 7798
		[SerializeField]
		private Gear _gear;

		// Token: 0x04001E77 RID: 7799
		[SerializeField]
		[Range(0f, 100f)]
		private int _chance;
	}
}
