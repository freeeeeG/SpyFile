using System;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x02001117 RID: 4375
	public class Slave : MonoBehaviour
	{
		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x06005520 RID: 21792 RVA: 0x000FE4A1 File Offset: 0x000FC6A1
		// (set) Token: 0x06005521 RID: 21793 RVA: 0x000FE4A9 File Offset: 0x000FC6A9
		public Master master { get; private set; }

		// Token: 0x06005522 RID: 21794 RVA: 0x000FE4B4 File Offset: 0x000FC6B4
		public void Initialize(Master master)
		{
			this.master = master;
			this._character.health.onDied += delegate()
			{
				master.RemoveSlave(this);
			};
		}

		// Token: 0x04004439 RID: 17465
		[SerializeField]
		private Character _character;
	}
}
