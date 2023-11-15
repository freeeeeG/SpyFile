using System;

// Token: 0x0200013D RID: 317
public class MenuState : ISceneState
{
	// Token: 0x06000835 RID: 2101 RVA: 0x000156F1 File Offset: 0x000138F1
	public MenuState(SceneStateController Controller) : base(Controller)
	{
		base.StateName = "MenuState";
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00015708 File Offset: 0x00013908
	public override void StateBegin()
	{
		Singleton<Game>.Instance.InitializeNetworks();
		Singleton<LevelManager>.Instance.Initialize();
		Singleton<MenuManager>.Instance.Initinal();
		Singleton<GameManager>.Instance = null;
		Singleton<Sound>.Instance.PlayBg("menu");
		Singleton<Game>.Instance.Tutorial = false;
		TechnologyFactory.ResetAllTech();
		StaticData.LockKeyboard = false;
		GameParam.ResetGameParam();
		Singleton<TipsManager>.Instance.SetCanvasCam();
		RuleFactory.Release();
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x00015773 File Offset: 0x00013973
	public override void StateEnd()
	{
		Singleton<MenuManager>.Instance.Release();
		Singleton<TipsManager>.Instance.HideTips();
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00015789 File Offset: 0x00013989
	public override void StateUpdate()
	{
		Singleton<MenuManager>.Instance.GameUpdate();
	}
}
