using System;
using flanne.RuneSystem;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.UI
{
	// Token: 0x02000210 RID: 528
	public class RuneRowUI : MonoBehaviour
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0002C208 File Offset: 0x0002A408
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x0002C23C File Offset: 0x0002A43C
		public int toggleIndex
		{
			get
			{
				for (int i = 0; i < this.runes.Length; i++)
				{
					if (this.runes[i].toggleOn)
					{
						return i;
					}
				}
				return -1;
			}
			set
			{
				if (value == -1)
				{
					RuneUnlocker[] array = this.runes;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].toggleOn = false;
					}
					return;
				}
				this.runes[value].toggleOn = true;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0002C27C File Offset: 0x0002A47C
		public RuneData toggledRune
		{
			get
			{
				int toggleIndex = this.toggleIndex;
				if (toggleIndex != -1)
				{
					return this.runes[toggleIndex].rune.data;
				}
				return null;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0002C2A8 File Offset: 0x0002A4A8
		public int totalLevels
		{
			get
			{
				int num = 0;
				foreach (RuneUnlocker runeUnlocker in this.runes)
				{
					num += runeUnlocker.level;
				}
				return num;
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0002C2DA File Offset: 0x0002A4DA
		private void OnLevelChange()
		{
			UnityEvent unityEvent = this.onLevelChanged;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0002C2EC File Offset: 0x0002A4EC
		private void Awake()
		{
			this.onLevelChanged = new UnityEvent();
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0002C2FC File Offset: 0x0002A4FC
		private void Start()
		{
			RuneUnlocker[] array = this.runes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].onLevel.AddListener(new UnityAction(this.OnLevelChange));
			}
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0002C338 File Offset: 0x0002A538
		private void OnDestroy()
		{
			RuneUnlocker[] array = this.runes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].onLevel.RemoveListener(new UnityAction(this.OnLevelChange));
			}
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0002C374 File Offset: 0x0002A574
		public void Lock()
		{
			RuneUnlocker[] array = this.runes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].locked = true;
			}
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0002C3A0 File Offset: 0x0002A5A0
		public void UnLock()
		{
			RuneUnlocker[] array = this.runes;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].locked = false;
			}
		}

		// Token: 0x04000849 RID: 2121
		[NonSerialized]
		public UnityEvent onLevelChanged;

		// Token: 0x0400084A RID: 2122
		public int levelRequirement;

		// Token: 0x0400084B RID: 2123
		[SerializeField]
		private RuneUnlocker[] runes;
	}
}
