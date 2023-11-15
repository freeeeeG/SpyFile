using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001A RID: 26
[CreateAssetMenu(menuName = "Attribute/LevelAttribute", fileName = "LevelAttribute")]
public class LevelAttribute : ScriptableObject
{
	// Token: 0x06000082 RID: 130 RVA: 0x00004514 File Offset: 0x00002714
	public EnemyAttribute GetRandomBoss(int level)
	{
		switch (level)
		{
		case 1:
			return this.Boss1[Random.Range(0, this.Boss1.Count)];
		case 2:
			return this.Boss2[Random.Range(0, this.Boss2.Count)];
		case 3:
			return this.Boss3[Random.Range(0, this.Boss3.Count)];
		case 4:
			return this.Boss4[Random.Range(0, this.Boss4.Count)];
		default:
			Debug.LogWarning("没有可以返回的BOSS类型");
			return null;
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x000045BC File Offset: 0x000027BC
	private void GeneateSequence()
	{
		this.SaveSequences = new List<EnemySequenceStruct>();
		foreach (List<EnemySequence> sequencesList in WaveSystem.LevelSequence)
		{
			EnemySequenceStruct enemySequenceStruct = new EnemySequenceStruct();
			enemySequenceStruct.SequencesList = sequencesList;
			this.SaveSequences.Add(enemySequenceStruct);
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x0000462C File Offset: 0x0000282C
	private void GenerateChallenge()
	{
		this.Choices.Clear();
		for (int i = 0; i < 30; i++)
		{
			this.GenerateRandomChoice(i);
		}
		this.Boss1.Clear();
		this.Boss1.Add(Singleton<StaticData>.Instance.EnemyFactory.GetRandomBoss(1));
		this.Boss2.Clear();
		this.Boss2.Add(Singleton<StaticData>.Instance.EnemyFactory.GetRandomBoss(2));
		this.Boss3.Clear();
		this.Boss3.Add(Singleton<StaticData>.Instance.EnemyFactory.GetRandomBoss(3));
	}

	// Token: 0x06000085 RID: 133 RVA: 0x000046CC File Offset: 0x000028CC
	private void GenerateRandomChoice(int wave)
	{
		ChallengeChoice challengeChoice = new ChallengeChoice();
		challengeChoice.Choices = new List<Choice>();
		ChallengeChoiceType challengeChoiceType = (wave % 10 == 4 || wave % 10 == 9) ? ChallengeChoiceType.Trap : ChallengeChoiceType.Turret;
		if (challengeChoiceType != ChallengeChoiceType.Turret)
		{
			if (challengeChoiceType == ChallengeChoiceType.Trap)
			{
				List<int> list = StaticData.SelectNoRepeat(Singleton<StaticData>.Instance.ContentFactory.TrapNames.Count, 3);
				for (int i = 0; i < 3; i++)
				{
					Choice item = default(Choice);
					item.ChoiceType = challengeChoiceType;
					item.Value1 = Singleton<StaticData>.Instance.ContentFactory.TrapNames[list[i]];
					challengeChoice.Choices.Add(item);
				}
			}
		}
		else
		{
			List<int> list2 = StaticData.SelectNoRepeat(Singleton<StaticData>.Instance.ContentFactory.RefactorTurretNames.Count, 3);
			for (int j = 0; j < 3; j++)
			{
				Choice choice = default(Choice);
				choice.ChoiceType = challengeChoiceType;
				choice.Value1 = Singleton<StaticData>.Instance.ContentFactory.RefactorTurretNames[list2[j]];
				choice.Elements = null;
				while (choice.Elements == null || (choice.Elements.Contains(0) && choice.Elements.Contains(2) && choice.Elements.Contains(4)))
				{
					choice.Elements = new List<int>();
					for (int k = 0; k < 3; k++)
					{
						int element = (int)Singleton<StaticData>.Instance.ContentFactory.GetRandomElementAttribute().element;
						choice.Elements.Add(element);
					}
				}
				challengeChoice.Choices.Add(choice);
			}
		}
		this.Choices.Add(challengeChoice);
	}

	// Token: 0x04000075 RID: 117
	public ModeType ModeType;

	// Token: 0x04000076 RID: 118
	public int ModeID;

	// Token: 0x04000077 RID: 119
	public int Level;

	// Token: 0x04000078 RID: 120
	public int StartCoin;

	// Token: 0x04000079 RID: 121
	public int PlayerHealth;

	// Token: 0x0400007A RID: 122
	public int Wave;

	// Token: 0x0400007B RID: 123
	public float LevelIntensify;

	// Token: 0x0400007C RID: 124
	public WaveSet[] WaveSets;

	// Token: 0x0400007D RID: 125
	public List<EnemyAttribute> NormalEnemies;

	// Token: 0x0400007E RID: 126
	public List<EnemyAttribute> EliteEnemies;

	// Token: 0x0400007F RID: 127
	public List<EnemyAttribute> Boss1;

	// Token: 0x04000080 RID: 128
	public List<EnemyAttribute> Boss2;

	// Token: 0x04000081 RID: 129
	public List<EnemyAttribute> Boss3;

	// Token: 0x04000082 RID: 130
	public List<EnemyAttribute> Boss4;

	// Token: 0x04000083 RID: 131
	public string LevelInfo;

	// Token: 0x04000084 RID: 132
	public int TrapCount;

	// Token: 0x04000085 RID: 133
	public int EliteWave;

	// Token: 0x04000086 RID: 134
	public float ExpIntensify;

	// Token: 0x04000087 RID: 135
	public List<ChallengeChoice> Choices;

	// Token: 0x04000088 RID: 136
	public List<EnemySequenceStruct> SaveSequences;

	// Token: 0x04000089 RID: 137
	public DialogueData[] GuideDialogues;

	// Token: 0x0400008A RID: 138
	public bool CanSaveGame;

	// Token: 0x0400008B RID: 139
	public DialogueData WinDialogue;

	// Token: 0x0400008C RID: 140
	public DialogueData LostDialogue;

	// Token: 0x0400008D RID: 141
	public DialogueData[] WaveDialogue;
}
