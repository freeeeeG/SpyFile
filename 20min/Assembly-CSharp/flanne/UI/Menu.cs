using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000226 RID: 550
	public class Menu : MonoBehaviour
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000C1D RID: 3101 RVA: 0x0002CBDC File Offset: 0x0002ADDC
		// (remove) Token: 0x06000C1E RID: 3102 RVA: 0x0002CC14 File Offset: 0x0002AE14
		public event EventHandler<InfoEventArgs<int>> ClickEvent;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000C1F RID: 3103 RVA: 0x0002CC4C File Offset: 0x0002AE4C
		// (remove) Token: 0x06000C20 RID: 3104 RVA: 0x0002CC84 File Offset: 0x0002AE84
		public event EventHandler<InfoEventArgs<int>> SelectEvent;

		// Token: 0x06000C21 RID: 3105 RVA: 0x0002CCB9 File Offset: 0x0002AEB9
		public virtual void OnEntryClicked(int index)
		{
			if (this.ClickEvent != null)
			{
				this.ClickEvent(this, new InfoEventArgs<int>(index));
			}
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0002CCD5 File Offset: 0x0002AED5
		public virtual void OnEntrySelected(int index)
		{
			if (this.SelectEvent != null)
			{
				this.SelectEvent(this, new InfoEventArgs<int>(index));
			}
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0002CCF4 File Offset: 0x0002AEF4
		private void Start()
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				int closureIndex = i;
				this.entries[closureIndex].onClick.AddListener(delegate()
				{
					this.OnEntryClicked(closureIndex);
				});
				this.entries[closureIndex].onSelect.AddListener(delegate()
				{
					this.OnEntrySelected(closureIndex);
				});
			}
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0002CD7C File Offset: 0x0002AF7C
		private void OnDestroy()
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				int closureIndex = i;
				this.entries[closureIndex].onClick.RemoveListener(delegate()
				{
					this.OnEntryClicked(closureIndex);
				});
				this.entries[closureIndex].onSelect.RemoveListener(delegate()
				{
					this.OnEntrySelected(closureIndex);
				});
			}
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0002CE04 File Offset: 0x0002B004
		public void SetProperties<T>(int index, T properties) where T : IUIProperties
		{
			if (index >= 0 && index < this.entries.Count)
			{
				Widget<T> component = this.entries[index].GetComponent<Widget<T>>();
				if (component != null)
				{
					component.SetProperties(properties);
					return;
				}
			}
			else
			{
				Debug.LogError("Cannot set property of entry " + index + ". Index out of bounds.");
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0002CE60 File Offset: 0x0002B060
		public void Select(int index)
		{
			if (index >= 0 && index < this.entries.Count)
			{
				this.entries[index].Select();
				return;
			}
			Debug.LogError("Cannot select menu entry " + index + ". Index out of bounds.");
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0002CEA0 File Offset: 0x0002B0A0
		public int SelectFirstAvailable()
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].interactable)
				{
					this.Select(i);
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0002CEE0 File Offset: 0x0002B0E0
		public virtual void Lock(int index)
		{
			this.entries[index].interactable = false;
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0002CEF4 File Offset: 0x0002B0F4
		public void UnLock(int index)
		{
			this.entries[index].interactable = true;
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0002CF08 File Offset: 0x0002B108
		public void SetEntryActive(int index, bool active)
		{
			this.entries[index].gameObject.SetActive(active);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0002CF24 File Offset: 0x0002B124
		public void UnlockAll()
		{
			foreach (MenuEntry menuEntry in this.entries)
			{
				menuEntry.interactable = true;
			}
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x0002CF78 File Offset: 0x0002B178
		public Vector2 GetEntryPosition(int index)
		{
			return this.entries[index].transform.position;
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0002CF95 File Offset: 0x0002B195
		public MenuEntry GetEntry(int index)
		{
			return this.entries[index];
		}

		// Token: 0x04000884 RID: 2180
		[SerializeField]
		protected List<MenuEntry> entries;
	}
}
