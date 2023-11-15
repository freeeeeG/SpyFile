using System;
using System.Collections.Generic;

// Token: 0x0200014A RID: 330
public class ChallengeSystem : IGameSystem
{
	// Token: 0x17000332 RID: 818
	// (get) Token: 0x060008EF RID: 2287 RVA: 0x00018838 File Offset: 0x00016A38
	public int ChoiceRemained
	{
		get
		{
			return this.challengeChoices.Count;
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00018848 File Offset: 0x00016A48
	public override void Initialize()
	{
		base.Initialize();
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge)
		{
			this.challengeChoices = new Queue<ChallengeChoice>();
			for (int i = 0; i < Singleton<LevelManager>.Instance.CurrentLevel.Choices.Count; i++)
			{
				this.challengeChoices.Enqueue(Singleton<LevelManager>.Instance.CurrentLevel.Choices[i]);
			}
			this.MaxChoice = this.challengeChoices.Count;
		}
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x000188C8 File Offset: 0x00016AC8
	public void LoadSaveGame()
	{
		int num = Singleton<LevelManager>.Instance.LastGameSave.ChallengeChoicePicked ? GameRes.CurrentWave : (GameRes.CurrentWave - 1);
		for (int i = 0; i < num; i++)
		{
			this.challengeChoices.Dequeue();
		}
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0001890D File Offset: 0x00016B0D
	public ChallengeChoice GetCurrentChoice()
	{
		if (this.challengeChoices.Count > 0)
		{
			return this.challengeChoices.Dequeue();
		}
		return null;
	}

	// Token: 0x040004A9 RID: 1193
	private Queue<ChallengeChoice> challengeChoices;

	// Token: 0x040004AA RID: 1194
	public int MaxChoice;
}
