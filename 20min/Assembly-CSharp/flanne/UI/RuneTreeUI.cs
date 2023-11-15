using System;
using System.Collections.Generic;
using flanne.RuneSystem;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.UI
{
	// Token: 0x02000211 RID: 529
	public class RuneTreeUI : MonoBehaviour
	{
		// Token: 0x06000BE8 RID: 3048 RVA: 0x0002C3CB File Offset: 0x0002A5CB
		private void OnLevelChange()
		{
			this.Refresh();
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0002C3D4 File Offset: 0x0002A5D4
		private void Start()
		{
			RuneRowUI[] array = this.rows;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].onLevelChanged.AddListener(new UnityAction(this.OnLevelChange));
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0002C410 File Offset: 0x0002A610
		private void OnDestroy()
		{
			RuneRowUI[] array = this.rows;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].onLevelChanged.AddListener(new UnityAction(this.OnLevelChange));
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0002C44C File Offset: 0x0002A64C
		public int[] GetSelections()
		{
			int[] array = new int[this.rows.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.rows[i].toggleIndex;
			}
			return array;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0002C488 File Offset: 0x0002A688
		public void SetSelections(int[] selections)
		{
			if (selections == null)
			{
				return;
			}
			for (int i = 0; i < selections.Length; i++)
			{
				this.rows[i].toggleIndex = selections[i];
			}
			this.Refresh();
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0002C4C0 File Offset: 0x0002A6C0
		public List<RuneData> GetActiveRunes()
		{
			List<RuneData> list = new List<RuneData>();
			for (int i = 0; i < this.rows.Length; i++)
			{
				RuneData toggledRune = this.rows[i].toggledRune;
				if (toggledRune != null)
				{
					list.Add(toggledRune);
				}
			}
			return list;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0002C508 File Offset: 0x0002A708
		public void Refresh()
		{
			int num = 0;
			foreach (RuneRowUI runeRowUI in this.rows)
			{
				num += runeRowUI.totalLevels;
			}
			foreach (RuneRowUI runeRowUI2 in this.rows)
			{
				if (runeRowUI2.levelRequirement <= num)
				{
					runeRowUI2.UnLock();
				}
				else
				{
					runeRowUI2.Lock();
				}
			}
		}

		// Token: 0x0400084C RID: 2124
		[SerializeField]
		private RuneRowUI[] rows;
	}
}
