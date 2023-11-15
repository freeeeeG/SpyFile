using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C9A RID: 3226
[AddComponentMenu("KMonoBehaviour/scripts/MultiToggle")]
public class MultiToggle : KMonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x170006FC RID: 1788
	// (get) Token: 0x060066C1 RID: 26305 RVA: 0x0026528D File Offset: 0x0026348D
	public int CurrentState
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x060066C2 RID: 26306 RVA: 0x00265295 File Offset: 0x00263495
	public void NextState()
	{
		this.ChangeState((this.state + 1) % this.states.Length);
	}

	// Token: 0x060066C3 RID: 26307 RVA: 0x002652AE File Offset: 0x002634AE
	protected virtual void Update()
	{
		if (this.clickHeldDown)
		{
			this.totalHeldTime += Time.unscaledDeltaTime;
			if (this.totalHeldTime > this.heldTimeThreshold && this.onHold != null)
			{
				this.onHold();
			}
		}
	}

	// Token: 0x060066C4 RID: 26308 RVA: 0x002652EB File Offset: 0x002634EB
	protected override void OnDisable()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			this.RefreshHoverColor();
			this.pointerOver = false;
			this.StopHolding();
		}
	}

	// Token: 0x060066C5 RID: 26309 RVA: 0x0026530D File Offset: 0x0026350D
	public void ChangeState(int new_state_index, bool forceRefreshState)
	{
		if (forceRefreshState)
		{
			this.stateDirty = true;
		}
		this.ChangeState(new_state_index);
	}

	// Token: 0x060066C6 RID: 26310 RVA: 0x00265320 File Offset: 0x00263520
	public void ChangeState(int new_state_index)
	{
		if (!this.stateDirty && new_state_index == this.state)
		{
			return;
		}
		this.stateDirty = false;
		this.state = new_state_index;
		try
		{
			this.toggle_image.sprite = this.states[new_state_index].sprite;
			this.toggle_image.color = this.states[new_state_index].color;
			if (this.states[new_state_index].use_rect_margins)
			{
				this.toggle_image.rectTransform().sizeDelta = this.states[new_state_index].rect_margins;
			}
		}
		catch
		{
			string text = base.gameObject.name;
			Transform transform = base.transform;
			while (transform.parent != null)
			{
				text = text.Insert(0, transform.name + ">");
				transform = transform.parent;
			}
			global::Debug.LogError("Multi Toggle state index out of range: " + text + " idx:" + new_state_index.ToString(), base.gameObject);
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null))
			{
				statePresentationSetting.image_target.sprite = statePresentationSetting.sprite;
				statePresentationSetting.image_target.color = statePresentationSetting.color;
			}
		}
		this.RefreshHoverColor();
	}

	// Token: 0x060066C7 RID: 26311 RVA: 0x0026549C File Offset: 0x0026369C
	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		if (this.states.Length - 1 < this.state)
		{
			global::Debug.LogWarning("Multi toggle has too few / no states");
		}
		bool flag = false;
		if (this.onDoubleClick != null && eventData.clickCount == 2)
		{
			flag = this.onDoubleClick();
		}
		if (this.onClick != null && !flag)
		{
			this.onClick();
		}
		this.RefreshHoverColor();
	}

	// Token: 0x060066C8 RID: 26312 RVA: 0x00265514 File Offset: 0x00263714
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.pointerOver = true;
		if (!KInputManager.isFocused)
		{
			return;
		}
		KInputManager.SetUserActive();
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color_on_hover;
		}
		if (this.states[this.state].use_rect_margins)
		{
			this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
			{
				statePresentationSetting.image_target.color = statePresentationSetting.color_on_hover;
			}
		}
		if (this.onEnter != null)
		{
			this.onEnter();
		}
	}

	// Token: 0x060066C9 RID: 26313 RVA: 0x00265650 File Offset: 0x00263850
	protected void RefreshHoverColor()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (this.pointerOver)
			{
				if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
				{
					this.toggle_image.color = this.states[this.state].color_on_hover;
				}
				foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
				{
					if (!(statePresentationSetting.image_target == null) && !(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
					{
						statePresentationSetting.image_target.color = statePresentationSetting.color_on_hover;
					}
				}
			}
			return;
		}
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color;
		}
		foreach (StatePresentationSetting statePresentationSetting2 in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting2.image_target == null) && statePresentationSetting2.use_color_on_hover)
			{
				statePresentationSetting2.image_target.color = statePresentationSetting2.color;
			}
		}
	}

	// Token: 0x060066CA RID: 26314 RVA: 0x00265814 File Offset: 0x00263A14
	public void OnPointerExit(PointerEventData eventData)
	{
		this.pointerOver = false;
		if (!KInputManager.isFocused)
		{
			return;
		}
		KInputManager.SetUserActive();
		if (this.states.Length == 0)
		{
			return;
		}
		if (this.states[this.state].use_color_on_hover && this.states[this.state].color_on_hover != this.states[this.state].color)
		{
			this.toggle_image.color = this.states[this.state].color;
		}
		if (this.states[this.state].use_rect_margins)
		{
			this.toggle_image.rectTransform().sizeDelta = this.states[this.state].rect_margins;
		}
		foreach (StatePresentationSetting statePresentationSetting in this.states[this.state].additional_display_settings)
		{
			if (!(statePresentationSetting.image_target == null) && statePresentationSetting.use_color_on_hover)
			{
				statePresentationSetting.image_target.color = statePresentationSetting.color;
			}
		}
		if (this.onExit != null)
		{
			this.onExit();
		}
	}

	// Token: 0x060066CB RID: 26315 RVA: 0x00265950 File Offset: 0x00263B50
	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		this.clickHeldDown = true;
		if (this.play_sound_on_click)
		{
			ToggleState toggleState = this.states[this.state];
			string on_click_override_sound_path = toggleState.on_click_override_sound_path;
			bool has_sound_parameter = toggleState.has_sound_parameter;
			if (on_click_override_sound_path == "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
				return;
			}
			if (on_click_override_sound_path != "" && has_sound_parameter)
			{
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound("General_Item_Click", false), toggleState.sound_parameter_name, toggleState.sound_parameter_value);
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound(on_click_override_sound_path, false), toggleState.sound_parameter_name, toggleState.sound_parameter_value);
				return;
			}
			KFMOD.PlayUISound(GlobalAssets.GetSound(on_click_override_sound_path, false));
		}
	}

	// Token: 0x060066CC RID: 26316 RVA: 0x00265A0F File Offset: 0x00263C0F
	public virtual void OnPointerUp(PointerEventData eventData)
	{
		if (!this.allowRightClick && eventData.button == PointerEventData.InputButton.Right)
		{
			return;
		}
		this.StopHolding();
	}

	// Token: 0x060066CD RID: 26317 RVA: 0x00265A2C File Offset: 0x00263C2C
	private void StopHolding()
	{
		if (this.clickHeldDown)
		{
			if (this.play_sound_on_release && this.states[this.state].on_release_override_sound_path != "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound(this.states[this.state].on_release_override_sound_path, false));
			}
			this.clickHeldDown = false;
			if (this.onStopHold != null)
			{
				this.onStopHold();
			}
		}
		this.totalHeldTime = 0f;
	}

	// Token: 0x040046F9 RID: 18169
	[Header("Settings")]
	[SerializeField]
	public ToggleState[] states;

	// Token: 0x040046FA RID: 18170
	public bool play_sound_on_click = true;

	// Token: 0x040046FB RID: 18171
	public bool play_sound_on_release;

	// Token: 0x040046FC RID: 18172
	public Image toggle_image;

	// Token: 0x040046FD RID: 18173
	protected int state;

	// Token: 0x040046FE RID: 18174
	public System.Action onClick;

	// Token: 0x040046FF RID: 18175
	private bool stateDirty = true;

	// Token: 0x04004700 RID: 18176
	public Func<bool> onDoubleClick;

	// Token: 0x04004701 RID: 18177
	public System.Action onEnter;

	// Token: 0x04004702 RID: 18178
	public System.Action onExit;

	// Token: 0x04004703 RID: 18179
	public System.Action onHold;

	// Token: 0x04004704 RID: 18180
	public System.Action onStopHold;

	// Token: 0x04004705 RID: 18181
	public bool allowRightClick = true;

	// Token: 0x04004706 RID: 18182
	protected bool clickHeldDown;

	// Token: 0x04004707 RID: 18183
	protected float totalHeldTime;

	// Token: 0x04004708 RID: 18184
	protected float heldTimeThreshold = 0.4f;

	// Token: 0x04004709 RID: 18185
	private bool pointerOver;
}
