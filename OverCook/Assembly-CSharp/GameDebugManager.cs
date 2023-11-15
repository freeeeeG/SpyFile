using System;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public class GameDebugManager : Manager
{
	// Token: 0x060015CB RID: 5579 RVA: 0x00074DF0 File Offset: 0x000731F0
	private void Start()
	{
		AudioSource component = Camera.main.GetComponent<AudioSource>();
		if (component != null && DebugManager.Instance.GetOption("Mute music"))
		{
			component.enabled = false;
		}
		OnScreenDebugDisplay onScreenDebugDisplay = base.gameObject.AddComponent<OnScreenDebugDisplay>();
		if (onScreenDebugDisplay != null)
		{
			onScreenDebugDisplay.AddDisplay(new VersionDisplay());
		}
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x00074E54 File Offset: 0x00073254
	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			Time.timeScale = this.m_manualTimeMod * this.m_config.m_timeScale;
			Time.fixedDeltaTime = this.m_manualTimeMod * this.m_config.m_timeScale * 0.02f;
		}
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x00074EA4 File Offset: 0x000732A4
	public GameDebugConfig GetDebugConfig()
	{
		return this.m_config;
	}

	// Token: 0x0400107F RID: 4223
	[SerializeField]
	private GameDebugConfig m_config;

	// Token: 0x04001080 RID: 4224
	private float m_manualTimeMod = 1f;
}
