using System;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x0200113E RID: 4414
	public class VineColorSetter : MonoBehaviour
	{
		// Token: 0x060055E7 RID: 21991 RVA: 0x000FFFDB File Offset: 0x000FE1DB
		public void SetColorPhase2()
		{
			this.SetColor(this._phase2Color);
		}

		// Token: 0x060055E8 RID: 21992 RVA: 0x000FFFE9 File Offset: 0x000FE1E9
		public void SetColorRecovered()
		{
			this.SetColor(this._recoveredColor);
		}

		// Token: 0x060055E9 RID: 21993 RVA: 0x000FFFF8 File Offset: 0x000FE1F8
		private void SetColor(Gradient gradient)
		{
			LineRenderer[] vines = this._vines;
			for (int i = 0; i < vines.Length; i++)
			{
				vines[i].colorGradient = gradient;
			}
		}

		// Token: 0x040044D9 RID: 17625
		[SerializeField]
		private Gradient _phase2Color;

		// Token: 0x040044DA RID: 17626
		[SerializeField]
		private Gradient _recoveredColor;

		// Token: 0x040044DB RID: 17627
		[SerializeField]
		private LineRenderer[] _vines;
	}
}
