using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000287 RID: 647
public class KeyBingdingSetter : MonoBehaviour
{
	// Token: 0x06001006 RID: 4102 RVA: 0x0002AED8 File Offset: 0x000290D8
	private void GetCurrentKeyDown()
	{
		if (!Input.anyKeyDown)
		{
			return;
		}
		for (int i = 0; i < this.keyCodes.Length; i++)
		{
			if (Input.GetKeyDown(this.keyCodes[i]))
			{
				if (Singleton<InputManager>.Instance.SetKeyBinding(this.m_KeyAction, this.keyCodes[i]))
				{
					this.SetContent();
				}
				this.m_Toggle.isOn = false;
				return;
			}
		}
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x0002AF3C File Offset: 0x0002913C
	public void SetContent()
	{
		this.keyTxt.text = Singleton<InputManager>.Instance.GetKeyForAction(this.m_KeyAction).ToString();
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0002AF72 File Offset: 0x00029172
	public void ClickSetToggle(bool value)
	{
		this.isCheckingInput = value;
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x0002AF7B File Offset: 0x0002917B
	private void Update()
	{
		if (this.isCheckingInput)
		{
			this.GetCurrentKeyDown();
		}
	}

	// Token: 0x0400084E RID: 2126
	[SerializeField]
	private KeyBindingActions m_KeyAction;

	// Token: 0x0400084F RID: 2127
	[SerializeField]
	private Text keyTxt;

	// Token: 0x04000850 RID: 2128
	[SerializeField]
	private Toggle m_Toggle;

	// Token: 0x04000851 RID: 2129
	private bool isCheckingInput;

	// Token: 0x04000852 RID: 2130
	private readonly KeyCode[] keyCodes = (from KeyCode k in Enum.GetValues(typeof(KeyCode))
	where k < KeyCode.Mouse0
	select k).ToArray<KeyCode>();
}
