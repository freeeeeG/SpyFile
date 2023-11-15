using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000A39 RID: 2617
public class InputModuleSwitch : MonoBehaviour
{
	// Token: 0x06004ECF RID: 20175 RVA: 0x001BC7D0 File Offset: 0x001BA9D0
	private void Update()
	{
		if (this.lastMousePosition != Input.mousePosition && KInputManager.currentControllerIsGamepad)
		{
			KInputManager.currentControllerIsGamepad = false;
			KInputManager.InputChange.Invoke();
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			this.virtualInput.enabled = KInputManager.currentControllerIsGamepad;
			if (this.standaloneInput.enabled)
			{
				this.standaloneInput.enabled = false;
				this.virtualInput.forceModuleActive = true;
				this.ChangeInputHandler();
				return;
			}
		}
		else
		{
			this.lastMousePosition = Input.mousePosition;
			this.standaloneInput.enabled = true;
			if (this.virtualInput.enabled)
			{
				this.virtualInput.enabled = false;
				this.standaloneInput.forceModuleActive = true;
				this.ChangeInputHandler();
			}
		}
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x001BC88C File Offset: 0x001BAA8C
	private void ChangeInputHandler()
	{
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.usedMenus.Count; i++)
		{
			if (inputManager.usedMenus[i].Equals(null))
			{
				inputManager.usedMenus.RemoveAt(i);
			}
		}
		if (inputManager.GetControllerCount() > 1)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				Cursor.visible = false;
				inputManager.GetController(1).inputHandler.TransferHandles(inputManager.GetController(0).inputHandler);
				return;
			}
			Cursor.visible = true;
			inputManager.GetController(0).inputHandler.TransferHandles(inputManager.GetController(1).inputHandler);
		}
	}

	// Token: 0x04003343 RID: 13123
	public VirtualInputModule virtualInput;

	// Token: 0x04003344 RID: 13124
	public StandaloneInputModule standaloneInput;

	// Token: 0x04003345 RID: 13125
	private Vector3 lastMousePosition;
}
