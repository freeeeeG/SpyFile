using System;
using System.Collections;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000AC9 RID: 2761
	public class ValueByLevel : MonoBehaviour
	{
		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x060038B4 RID: 14516 RVA: 0x000A7320 File Offset: 0x000A5520
		// (set) Token: 0x060038B5 RID: 14517 RVA: 0x000A7328 File Offset: 0x000A5528
		public int level { private get; set; }

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x060038B6 RID: 14518 RVA: 0x000A7331 File Offset: 0x000A5531
		public IList values
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x000A7339 File Offset: 0x000A5539
		public float GetValue()
		{
			return this._values[this.level];
		}

		// Token: 0x04002D1C RID: 11548
		[SerializeField]
		private float[] _values;
	}
}
