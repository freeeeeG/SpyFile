using System;
using UnityEngine;

namespace CutScenes.Shots
{
	// Token: 0x020001CD RID: 461
	public class ShotInfo : MonoBehaviour
	{
		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0001B3C4 File Offset: 0x000195C4
		public Shot shot
		{
			get
			{
				return this._shot;
			}
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001B3CC File Offset: 0x000195CC
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this._tag))
			{
				return this._tag;
			}
			return this.GetAutoName();
		}

		// Token: 0x040007E2 RID: 2018
		[SerializeField]
		private string _tag;

		// Token: 0x040007E3 RID: 2019
		[SerializeField]
		[Shot.SubcomponentAttribute]
		private Shot _shot;

		// Token: 0x020001CE RID: 462
		[Serializable]
		internal class Subcomponents : SubcomponentArray<ShotInfo>
		{
			// Token: 0x060009A0 RID: 2464 RVA: 0x0001B3E8 File Offset: 0x000195E8
			public void Run(Shot onStart, Shot onEnd)
			{
				if (base.components.Length == 0)
				{
					return;
				}
				if (onStart != null)
				{
					onStart.SetNext(base.components[0].shot);
				}
				for (int i = 0; i < base.components.Length - 1; i++)
				{
					base.components[i].shot.SetNext(base.components[i + 1].shot);
				}
				if (onEnd != null)
				{
					base.components[base.components.Length - 1].shot.SetNext(onEnd);
				}
				if (onStart != null)
				{
					onStart.Run();
					return;
				}
				base.components[0].shot.Run();
			}
		}
	}
}
