using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pause
{
	// Token: 0x0200042A RID: 1066
	public class SettingsNavigationSetter : MonoBehaviour
	{
		// Token: 0x0600144D RID: 5197 RVA: 0x0003E634 File Offset: 0x0003C834
		private void Awake()
		{
			this._dialogue = base.GetComponent<Dialogue>();
			this._graphicsSettings = this._graphics.GetComponentsInChildren<Selectable>();
			this._audioSettings = this._audio.GetComponentsInChildren<Selectable>();
			this._dataSettings = this._data.GetComponentsInChildren<Selectable>();
			this._gamePlaySettings = this._gamePlay.GetComponentsInChildren<Selectable>();
			this.SetNavigation(this._graphicsSettings, null, this._dataSettings);
			this.SetNavigation(this._audioSettings, this._dataSettings, this._gamePlaySettings);
			this.SetNavigation(this._dataSettings, this._graphicsSettings, this._audioSettings);
			this.SetNavigation(this._gamePlaySettings, this._audioSettings, null);
			Navigation navigation = this._return.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnUp = this._gamePlaySettings.Last<Selectable>();
			navigation.selectOnDown = this._graphicsSettings.First<Selectable>();
			this._return.navigation = navigation;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0003E72B File Offset: 0x0003C92B
		private void OnEnable()
		{
			this.Focus();
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0003E733 File Offset: 0x0003C933
		private void Focus()
		{
			this._dialogue.Focus(this._graphicsSettings.First<Selectable>());
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0003E74C File Offset: 0x0003C94C
		private void SetNavigation(Selectable[] target, Selectable[] up, Selectable[] down)
		{
			for (int i = 0; i < target.Length; i++)
			{
				Navigation navigation = target[i].navigation;
				navigation.mode = Navigation.Mode.Explicit;
				Selectable selectOnUp;
				if ((selectOnUp = this.GetElementAtSafe<Selectable>(target, i - 1)) == null)
				{
					selectOnUp = (this.GetElementAtSafe<Selectable>(up, ((up != null) ? up.Length : 0) - 1) ?? this._return);
				}
				navigation.selectOnUp = selectOnUp;
				Selectable selectOnDown;
				if ((selectOnDown = this.GetElementAtSafe<Selectable>(target, i + 1)) == null)
				{
					selectOnDown = (this.GetElementAtSafe<Selectable>(down, 0) ?? this._return);
				}
				navigation.selectOnDown = selectOnDown;
				target[i].navigation = navigation;
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0003E7DC File Offset: 0x0003C9DC
		private T GetElementAtSafe<T>(IList<T> list, int index)
		{
			if (list == null)
			{
				return default(T);
			}
			if (index < 0 || index >= list.Count)
			{
				return default(T);
			}
			return list[index];
		}

		// Token: 0x04001141 RID: 4417
		[SerializeField]
		private GameObject _graphics;

		// Token: 0x04001142 RID: 4418
		private Selectable[] _graphicsSettings;

		// Token: 0x04001143 RID: 4419
		[SerializeField]
		private GameObject _audio;

		// Token: 0x04001144 RID: 4420
		private Selectable[] _audioSettings;

		// Token: 0x04001145 RID: 4421
		[SerializeField]
		private GameObject _data;

		// Token: 0x04001146 RID: 4422
		private Selectable[] _dataSettings;

		// Token: 0x04001147 RID: 4423
		[SerializeField]
		private GameObject _gamePlay;

		// Token: 0x04001148 RID: 4424
		private Selectable[] _gamePlaySettings;

		// Token: 0x04001149 RID: 4425
		[SerializeField]
		private Selectable _return;

		// Token: 0x0400114A RID: 4426
		private Dialogue _dialogue;
	}
}
