using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000AEB RID: 2795
public class ToggleOption : BaseUIOption<IOption>
{
	// Token: 0x060038A3 RID: 14499 RVA: 0x0010B613 File Offset: 0x00109A13
	protected override void Awake()
	{
		base.Awake();
		this.SyncUIWithOption();
		this.m_Toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleButtonPressed));
	}

	// Token: 0x060038A4 RID: 14500 RVA: 0x0010B640 File Offset: 0x00109A40
	public override void SyncUIWithOption()
	{
		if (this.m_Toggle != null && this.m_Option != null)
		{
			this.m_Toggle.isOn = (this.m_Option.GetOption() == 1);
		}
	}

	// Token: 0x060038A5 RID: 14501 RVA: 0x0010B68C File Offset: 0x00109A8C
	private void OnToggleButtonPressed(bool bValue)
	{
		int option = (!bValue) ? 0 : 1;
		this.m_Option.SetOption(option);
		GameUtils.TriggerAudio(GameOneShotAudioTag.UISelect, base.gameObject.layer);
	}

	// Token: 0x04002D54 RID: 11604
	[SerializeField]
	private T17Toggle m_Toggle;
}
