using System;
using Characters;
using UnityEngine;

namespace UI.Witch
{
	// Token: 0x020003E8 RID: 1000
	public class Tree : MonoBehaviour
	{
		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060012AD RID: 4781 RVA: 0x00037DD5 File Offset: 0x00035FD5
		public TreeElement[] elements
		{
			get
			{
				return this._elements;
			}
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00037DE0 File Offset: 0x00035FE0
		public void SetInteractable(bool value)
		{
			TreeElement[] elements = this._elements;
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i].interactable = value;
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00037E0C File Offset: 0x0003600C
		public void Initialize(Panel panel)
		{
			this._panel = panel;
			for (int i = 0; i < this._elements.Length; i++)
			{
				this._elements[i].Initialize(panel);
			}
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x00037E44 File Offset: 0x00036044
		public void Set(WitchBonus.Tree tree)
		{
			this._tree = tree;
			for (int i = 0; i < this._elements.Length; i++)
			{
				this._elements[i].Set(tree.list[i]);
			}
		}

		// Token: 0x04000FAB RID: 4011
		private Panel _panel;

		// Token: 0x04000FAC RID: 4012
		[SerializeField]
		private TreeElement[] _elements;

		// Token: 0x04000FAD RID: 4013
		private WitchBonus.Tree _tree;
	}
}
