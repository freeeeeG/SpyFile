using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Hero
{
	// Token: 0x02001269 RID: 4713
	public class PillarOfLightContainer : MonoBehaviour
	{
		// Token: 0x06005D71 RID: 23921 RVA: 0x00112F80 File Offset: 0x00111180
		private void Start()
		{
			this._pillars = new List<PillarOfLight>(this._container.childCount);
			foreach (object obj in this._container)
			{
				PillarOfLight component = ((Transform)obj).GetComponent<PillarOfLight>();
				if (component == null)
				{
					Debug.LogError("child has not Pillar Of Light");
				}
				this._pillars.Add(component);
			}
		}

		// Token: 0x06005D72 RID: 23922 RVA: 0x0011300C File Offset: 0x0011120C
		public void AddPillar(PillarOfLight pillar)
		{
			pillar.transform.SetParent(this._container);
		}

		// Token: 0x06005D73 RID: 23923 RVA: 0x00113020 File Offset: 0x00111220
		public void Sign(Character owner)
		{
			foreach (PillarOfLight pillarOfLight in this._pillars)
			{
				pillarOfLight.Sign(owner);
			}
		}

		// Token: 0x06005D74 RID: 23924 RVA: 0x00113074 File Offset: 0x00111274
		public void Attack(Character owner)
		{
			foreach (PillarOfLight pillarOfLight in this._pillars)
			{
				pillarOfLight.Attack(owner);
			}
		}

		// Token: 0x04004B02 RID: 19202
		[SerializeField]
		private Transform _container;

		// Token: 0x04004B03 RID: 19203
		private List<PillarOfLight> _pillars;
	}
}
