using System;
using System.Reflection;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Pause
{
	// Token: 0x02000414 RID: 1044
	public class ControllerLeftRightNavigation : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
	{
		// Token: 0x060013CD RID: 5069 RVA: 0x0003C6A0 File Offset: 0x0003A8A0
		public void OnSelect(BaseEventData eventData)
		{
			this._selected = true;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0003C6A9 File Offset: 0x0003A8A9
		public void OnDeselect(BaseEventData eventData)
		{
			this._selected = false;
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0003C6B4 File Offset: 0x0003A8B4
		private void Update()
		{
			if (!this._selected)
			{
				return;
			}
			InputDevice activeDevice = InputManager.ActiveDevice;
			if (activeDevice == null)
			{
				return;
			}
			if (this._lefts.Length != 0 && (activeDevice.LeftBumper.WasPressed || activeDevice.LeftTrigger.WasPressed || activeDevice.RightStickLeft.WasPressed))
			{
				foreach (Selectable selectable in this._lefts)
				{
					if (!(selectable == null) && selectable.gameObject.activeSelf)
					{
						this.Focus(selectable);
						return;
					}
				}
			}
			if (this._rights.Length != 0 && (activeDevice.RightBumper.WasPressed || activeDevice.RightTrigger.WasPressed || activeDevice.RightStickRight.WasPressed))
			{
				foreach (Selectable selectable2 in this._rights)
				{
					if (!(selectable2 == null) && selectable2.gameObject.activeSelf)
					{
						this.Focus(selectable2);
						return;
					}
				}
			}
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0003C7A8 File Offset: 0x0003A9A8
		private void Focus(Selectable selectable)
		{
			EventSystem.current.SetSelectedGameObject(selectable.gameObject);
			selectable.Select();
			if (!selectable.interactable)
			{
				return;
			}
			typeof(Selectable).GetMethod("DoStateTransition", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(selectable, new object[]
			{
				3,
				true
			});
		}

		// Token: 0x040010CC RID: 4300
		[SerializeField]
		private Selectable[] _lefts;

		// Token: 0x040010CD RID: 4301
		[SerializeField]
		private Selectable[] _rights;

		// Token: 0x040010CE RID: 4302
		private bool _selected;
	}
}
