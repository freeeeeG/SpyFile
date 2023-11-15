using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000B1E RID: 2846
public class IncrementorToggle : MultiToggle
{
	// Token: 0x06005796 RID: 22422 RVA: 0x00200E38 File Offset: 0x001FF038
	protected override void Update()
	{
		if (this.clickHeldDown)
		{
			this.totalHeldTime += Time.unscaledDeltaTime;
			if (this.timeToNextIncrement <= 0f)
			{
				this.PlayClickSound();
				this.onClick();
				this.timeToNextIncrement = Mathf.Lerp(this.timeBetweenIncrementsMax, this.timeBetweenIncrementsMin, this.totalHeldTime / 2.5f);
				return;
			}
			this.timeToNextIncrement -= Time.unscaledDeltaTime;
		}
	}

	// Token: 0x06005797 RID: 22423 RVA: 0x00200EB4 File Offset: 0x001FF0B4
	private void PlayClickSound()
	{
		if (this.play_sound_on_click)
		{
			if (this.states[this.state].on_click_override_sound_path == "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
				return;
			}
			KFMOD.PlayUISound(GlobalAssets.GetSound(this.states[this.state].on_click_override_sound_path, false));
		}
	}

	// Token: 0x06005798 RID: 22424 RVA: 0x00200F1D File Offset: 0x001FF11D
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		this.timeToNextIncrement = this.timeBetweenIncrementsMax;
	}

	// Token: 0x06005799 RID: 22425 RVA: 0x00200F34 File Offset: 0x001FF134
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!this.clickHeldDown)
		{
			this.clickHeldDown = true;
			this.PlayClickSound();
			if (this.onClick != null)
			{
				this.onClick();
			}
		}
		if (this.states.Length - 1 < this.state)
		{
			global::Debug.LogWarning("Multi toggle has too few / no states");
		}
		base.RefreshHoverColor();
	}

	// Token: 0x0600579A RID: 22426 RVA: 0x00200F8B File Offset: 0x001FF18B
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.RefreshHoverColor();
	}

	// Token: 0x04003B35 RID: 15157
	private float timeBetweenIncrementsMin = 0.033f;

	// Token: 0x04003B36 RID: 15158
	private float timeBetweenIncrementsMax = 0.25f;

	// Token: 0x04003B37 RID: 15159
	private const float incrementAccelerationScale = 2.5f;

	// Token: 0x04003B38 RID: 15160
	private float timeToNextIncrement;
}
