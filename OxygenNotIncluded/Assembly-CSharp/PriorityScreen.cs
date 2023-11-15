using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000BC7 RID: 3015
public class PriorityScreen : KScreen
{
	// Token: 0x06005E98 RID: 24216 RVA: 0x0022B7F8 File Offset: 0x002299F8
	public void InstantiateButtons(Action<PrioritySetting> on_click, bool playSelectionSound = true)
	{
		this.onClick = on_click;
		for (int i = 1; i <= 9; i++)
		{
			int num = i;
			PriorityButton priorityButton = global::Util.KInstantiateUI<PriorityButton>(this.buttonPrefab_basic.gameObject, this.buttonPrefab_basic.transform.parent.gameObject, false);
			this.buttons_basic.Add(priorityButton);
			priorityButton.playSelectionSound = playSelectionSound;
			priorityButton.onClick = this.onClick;
			priorityButton.text.text = num.ToString();
			priorityButton.priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, num);
			priorityButton.tooltip.SetSimpleTooltip(string.Format(UI.PRIORITYSCREEN.BASIC, num));
		}
		this.buttonPrefab_basic.gameObject.SetActive(false);
		this.button_emergency.playSelectionSound = playSelectionSound;
		this.button_emergency.onClick = this.onClick;
		this.button_emergency.priority = new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1);
		this.button_emergency.tooltip.SetSimpleTooltip(UI.PRIORITYSCREEN.TOP_PRIORITY);
		this.button_toggleHigh.gameObject.SetActive(false);
		this.PriorityMenuContainer.SetActive(true);
		this.button_priorityMenu.gameObject.SetActive(true);
		this.button_priorityMenu.onClick += this.PriorityButtonClicked;
		this.button_priorityMenu.GetComponent<ToolTip>().SetSimpleTooltip(UI.PRIORITYSCREEN.OPEN_JOBS_SCREEN);
		this.diagram.SetActive(false);
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x06005E99 RID: 24217 RVA: 0x0022B979 File Offset: 0x00229B79
	private void OnClick(PrioritySetting priority)
	{
		if (this.onClick != null)
		{
			this.onClick(priority);
		}
	}

	// Token: 0x06005E9A RID: 24218 RVA: 0x0022B98F File Offset: 0x00229B8F
	public void ShowDiagram(bool show)
	{
		this.diagram.SetActive(show);
	}

	// Token: 0x06005E9B RID: 24219 RVA: 0x0022B99D File Offset: 0x00229B9D
	public void ResetPriority()
	{
		this.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5), false);
	}

	// Token: 0x06005E9C RID: 24220 RVA: 0x0022B9AD File Offset: 0x00229BAD
	public void PriorityButtonClicked()
	{
		ManagementMenu.Instance.TogglePriorities();
	}

	// Token: 0x06005E9D RID: 24221 RVA: 0x0022B9BC File Offset: 0x00229BBC
	private void RefreshButton(PriorityButton b, PrioritySetting priority, bool play_sound)
	{
		if (b.priority == priority)
		{
			b.toggle.Select();
			b.toggle.isOn = true;
			if (play_sound)
			{
				b.toggle.soundPlayer.Play(0);
				return;
			}
		}
		else
		{
			b.toggle.isOn = false;
		}
	}

	// Token: 0x06005E9E RID: 24222 RVA: 0x0022BA10 File Offset: 0x00229C10
	public void SetScreenPriority(PrioritySetting priority, bool play_sound = false)
	{
		if (this.lastSelectedPriority == priority)
		{
			return;
		}
		this.lastSelectedPriority = priority;
		if (priority.priority_class == PriorityScreen.PriorityClass.high)
		{
			this.button_toggleHigh.isOn = true;
		}
		else if (priority.priority_class == PriorityScreen.PriorityClass.basic)
		{
			this.button_toggleHigh.isOn = false;
		}
		for (int i = 0; i < this.buttons_basic.Count; i++)
		{
			this.buttons_basic[i].priority = new PrioritySetting(this.button_toggleHigh.isOn ? PriorityScreen.PriorityClass.high : PriorityScreen.PriorityClass.basic, i + 1);
			this.buttons_basic[i].tooltip.SetSimpleTooltip(string.Format(this.button_toggleHigh.isOn ? UI.PRIORITYSCREEN.HIGH : UI.PRIORITYSCREEN.BASIC, i + 1));
			this.RefreshButton(this.buttons_basic[i], this.lastSelectedPriority, play_sound);
		}
		this.RefreshButton(this.button_emergency, this.lastSelectedPriority, play_sound);
	}

	// Token: 0x06005E9F RID: 24223 RVA: 0x0022BB11 File Offset: 0x00229D11
	public PrioritySetting GetLastSelectedPriority()
	{
		return this.lastSelectedPriority;
	}

	// Token: 0x06005EA0 RID: 24224 RVA: 0x0022BB1C File Offset: 0x00229D1C
	public static void PlayPriorityConfirmSound(PrioritySetting priority)
	{
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Priority_Tool_Confirm", false), Vector3.zero, 1f);
		if (instance.isValid())
		{
			float num = 0f;
			if (priority.priority_class >= PriorityScreen.PriorityClass.high)
			{
				num += 10f;
			}
			if (priority.priority_class >= PriorityScreen.PriorityClass.topPriority)
			{
				num += 0f;
			}
			num += (float)priority.priority_value;
			instance.setParameterByName("priority", num, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x04003FDC RID: 16348
	[SerializeField]
	protected PriorityButton buttonPrefab_basic;

	// Token: 0x04003FDD RID: 16349
	[SerializeField]
	protected GameObject EmergencyContainer;

	// Token: 0x04003FDE RID: 16350
	[SerializeField]
	protected PriorityButton button_emergency;

	// Token: 0x04003FDF RID: 16351
	[SerializeField]
	protected GameObject PriorityMenuContainer;

	// Token: 0x04003FE0 RID: 16352
	[SerializeField]
	protected KButton button_priorityMenu;

	// Token: 0x04003FE1 RID: 16353
	[SerializeField]
	protected KToggle button_toggleHigh;

	// Token: 0x04003FE2 RID: 16354
	[SerializeField]
	protected GameObject diagram;

	// Token: 0x04003FE3 RID: 16355
	protected List<PriorityButton> buttons_basic = new List<PriorityButton>();

	// Token: 0x04003FE4 RID: 16356
	protected List<PriorityButton> buttons_emergency = new List<PriorityButton>();

	// Token: 0x04003FE5 RID: 16357
	private PrioritySetting priority;

	// Token: 0x04003FE6 RID: 16358
	private PrioritySetting lastSelectedPriority = new PrioritySetting(PriorityScreen.PriorityClass.basic, -1);

	// Token: 0x04003FE7 RID: 16359
	private Action<PrioritySetting> onClick;

	// Token: 0x02001B0B RID: 6923
	public enum PriorityClass
	{
		// Token: 0x04007B86 RID: 31622
		idle = -1,
		// Token: 0x04007B87 RID: 31623
		basic,
		// Token: 0x04007B88 RID: 31624
		high,
		// Token: 0x04007B89 RID: 31625
		personalNeeds,
		// Token: 0x04007B8A RID: 31626
		topPriority,
		// Token: 0x04007B8B RID: 31627
		compulsory
	}
}
