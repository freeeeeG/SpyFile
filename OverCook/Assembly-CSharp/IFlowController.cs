using System;

// Token: 0x020006BE RID: 1726
public interface IFlowController
{
	// Token: 0x060020A6 RID: 8358
	LevelConfigBase GetLevelConfig();

	// Token: 0x060020A7 RID: 8359
	GameConfig GetGameConfig();

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x060020A8 RID: 8360
	bool InRound { get; }

	// Token: 0x14000021 RID: 33
	// (add) Token: 0x060020A9 RID: 8361
	// (remove) Token: 0x060020AA RID: 8362
	event CallbackVoid RoundActivatedCallback;

	// Token: 0x14000022 RID: 34
	// (add) Token: 0x060020AB RID: 8363
	// (remove) Token: 0x060020AC RID: 8364
	event CallbackVoid RoundDeactivatedCallback;
}
