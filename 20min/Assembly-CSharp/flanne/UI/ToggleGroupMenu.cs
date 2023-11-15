using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000236 RID: 566
	public class ToggleGroupMenu<T> : MonoBehaviour where T : ScriptableObject
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000C6D RID: 3181 RVA: 0x0002D894 File Offset: 0x0002BA94
		// (remove) Token: 0x06000C6E RID: 3182 RVA: 0x0002D8CC File Offset: 0x0002BACC
		public event EventHandler<T> ConfirmEvent;

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0002D904 File Offset: 0x0002BB04
		public T toggledData
		{
			get
			{
				Toggle toggle = this.toggleGroup.ActiveToggles().FirstOrDefault<Toggle>();
				if (toggle == null)
				{
					return default(T);
				}
				return toggle.GetComponent<DataUIBinding<T>>().data;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x0002D939 File Offset: 0x0002BB39
		// (set) Token: 0x06000C71 RID: 3185 RVA: 0x0002D941 File Offset: 0x0002BB41
		public int currIndex { get; private set; }

		// Token: 0x06000C72 RID: 3186 RVA: 0x0002D94A File Offset: 0x0002BB4A
		private void OnPointerEnter(int index)
		{
			if (this.description != null)
			{
				this.description.data = this.entries[index].data;
			}
			SoundEffectSO soundEffectSO = this.hoverSFX;
			if (soundEffectSO == null)
			{
				return;
			}
			soundEffectSO.Play(null);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0002D988 File Offset: 0x0002BB88
		private void OnPointerExit()
		{
			if (this.description != null)
			{
				this.description.data = this.toggledData;
			}
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0002D9AC File Offset: 0x0002BBAC
		private void OnToggleChanged(int index)
		{
			if (this.saveLastSelectionToPlayerPrefs)
			{
				PlayerPrefs.SetInt(this.playerPrefsKey, index);
			}
			if (this.description != null)
			{
				this.description.data = this.toggledData;
			}
			if (this.playerInput != null && this.playerInput.currentControlScheme == "Gamepad" && this.toggles[index].isOn && this.currIndex == index)
			{
				this.OnConfirmClicked();
			}
			if (this.currIndex != index)
			{
				this.currIndex = index;
				SoundEffectSO soundEffectSO = this.clickSFX;
				if (soundEffectSO == null)
				{
					return;
				}
				soundEffectSO.Play(null);
			}
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0002DA51 File Offset: 0x0002BC51
		public void OnConfirmClicked()
		{
			EventHandler<T> confirmEvent = this.ConfirmEvent;
			if (confirmEvent == null)
			{
				return;
			}
			confirmEvent(this, this.toggledData);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0002DA6C File Offset: 0x0002BC6C
		private void Start()
		{
			if (this.descriptionObj != null)
			{
				this.description = this.descriptionObj.GetComponent<DataUIBinding<T>>();
			}
			this.entries = new List<DataUIBinding<T>>();
			this.pointerDetectors = new List<PointerDetector>();
			for (int i = 0; i < this.toggles.Length; i++)
			{
				int index = i;
				PointerDetector component = this.toggles[i].GetComponent<PointerDetector>();
				component.onEnter.AddListener(delegate()
				{
					this.OnPointerEnter(index);
				});
				component.onExit.AddListener(new UnityAction(this.OnPointerExit));
				component.onSelect.AddListener(delegate()
				{
					this.OnPointerEnter(index);
				});
				component.onDeselect.AddListener(new UnityAction(this.OnPointerExit));
				this.pointerDetectors.Add(component);
				DataUIBinding<T> component2 = this.toggles[i].GetComponent<DataUIBinding<T>>();
				this.entries.Add(component2);
			}
			int num;
			if (this.saveLastSelectionToPlayerPrefs)
			{
				num = PlayerPrefs.GetInt(this.playerPrefsKey, 0);
				num = Mathf.Clamp(num, 0, this.entries.Count - 1);
			}
			else
			{
				num = 0;
			}
			if (this.description != null)
			{
				this.description.data = this.entries[num].data;
			}
			this.toggles[num].isOn = true;
			this.currIndex = num;
			for (int j = 0; j < this.toggles.Length; j++)
			{
				int index = j;
				this.toggles[j].onValueChanged.AddListener(delegate(bool <p0>)
				{
					this.OnToggleChanged(index);
				});
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0002DC29 File Offset: 0x0002BE29
		public void SetData(int index, T data)
		{
			this.entries[index].data = data;
			if (this.description != null)
			{
				this.description.data = this.toggledData;
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0002DC5C File Offset: 0x0002BE5C
		public void SetActive(int index, bool isActive)
		{
			this.entries[index].gameObject.SetActive(isActive);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0002D988 File Offset: 0x0002BB88
		public void RefreshDescription()
		{
			if (this.description != null)
			{
				this.description.data = this.toggledData;
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0002DC78 File Offset: 0x0002BE78
		public void RefreshToggleData()
		{
			foreach (DataUIBinding<T> dataUIBinding in this.entries)
			{
				dataUIBinding.Refresh();
			}
		}

		// Token: 0x040008B0 RID: 2224
		[SerializeField]
		private Toggle[] toggles;

		// Token: 0x040008B1 RID: 2225
		[SerializeField]
		private ToggleGroup toggleGroup;

		// Token: 0x040008B2 RID: 2226
		[SerializeField]
		private GameObject descriptionObj;

		// Token: 0x040008B3 RID: 2227
		[SerializeField]
		private SoundEffectSO hoverSFX;

		// Token: 0x040008B4 RID: 2228
		[SerializeField]
		private SoundEffectSO clickSFX;

		// Token: 0x040008B5 RID: 2229
		[SerializeField]
		private bool saveLastSelectionToPlayerPrefs;

		// Token: 0x040008B6 RID: 2230
		[SerializeField]
		private string playerPrefsKey;

		// Token: 0x040008B7 RID: 2231
		[SerializeField]
		private PlayerInput playerInput;

		// Token: 0x040008B8 RID: 2232
		private const string gamepadScheme = "Gamepad";

		// Token: 0x040008B9 RID: 2233
		private const string mouseScheme = "Keyboard&Mouse";

		// Token: 0x040008BA RID: 2234
		private DataUIBinding<T> description;

		// Token: 0x040008BB RID: 2235
		private List<DataUIBinding<T>> entries;

		// Token: 0x040008BC RID: 2236
		private List<PointerDetector> pointerDetectors;
	}
}
