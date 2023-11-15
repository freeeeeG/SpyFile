using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level
{
	// Token: 0x02000540 RID: 1344
	public class SelectableInWave : MonoBehaviour
	{
		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001A6D RID: 6765 RVA: 0x00052DD2 File Offset: 0x00050FD2
		public Character[] characters
		{
			get
			{
				return this._characters;
			}
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x0001FA07 File Offset: 0x0001DC07
		private void Awake()
		{
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00052DDC File Offset: 0x00050FDC
		public SpriteRenderer[] GetIcons()
		{
			List<SpriteRenderer> list = new List<SpriteRenderer>();
			foreach (Character character in this._characters)
			{
				list.Add(character.@base.GetComponentInChildren<SpriteRenderer>());
			}
			list.AddRange(this._additionalIcons);
			return list.ToArray();
		}

		// Token: 0x04001704 RID: 5892
		[SerializeField]
		private Character[] _characters;

		// Token: 0x04001705 RID: 5893
		[SerializeField]
		private SpriteRenderer[] _additionalIcons;
	}
}
