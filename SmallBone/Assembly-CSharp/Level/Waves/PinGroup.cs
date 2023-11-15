using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000564 RID: 1380
	public sealed class PinGroup : MonoBehaviour
	{
		// Token: 0x06001B2F RID: 6959 RVA: 0x000546CC File Offset: 0x000528CC
		public ICollection<Character> Load()
		{
			this._pins = base.GetComponentsInChildren<Pin>();
			this._characters = new List<Character>(this._pins.Count);
			foreach (Pin pin in this._pins)
			{
				foreach (Character item in pin.Load())
				{
					this._characters.Add(item);
				}
			}
			return this._characters;
		}

		// Token: 0x04001757 RID: 5975
		private ICollection<Pin> _pins;

		// Token: 0x04001758 RID: 5976
		private ICollection<Character> _characters;
	}
}
