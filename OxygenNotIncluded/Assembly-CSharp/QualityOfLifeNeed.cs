using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008B5 RID: 2229
[SkipSaveFileSerialization]
public class QualityOfLifeNeed : Need, ISim4000ms
{
	// Token: 0x06004080 RID: 16512 RVA: 0x00168E28 File Offset: 0x00167028
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.breakBlocks = new List<bool>(24);
		Attributes attributes = base.gameObject.GetAttributes();
		this.expectationAttribute = attributes.Add(Db.Get().Attributes.QualityOfLifeExpectation);
		base.Name = DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME;
		base.ExpectationTooltip = string.Format(DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_TOOLTIP, Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this).GetTotalValue());
		this.stressBonus = new Need.ModifierType
		{
			modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0f, DUPLICANTS.NEEDS.QUALITYOFLIFE.GOOD_MODIFIER, false, false, false)
		};
		this.stressNeutral = new Need.ModifierType
		{
			modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.008333334f, DUPLICANTS.NEEDS.QUALITYOFLIFE.NEUTRAL_MODIFIER, false, false, true)
		};
		this.stressPenalty = new Need.ModifierType
		{
			modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0f, DUPLICANTS.NEEDS.QUALITYOFLIFE.BAD_MODIFIER, false, false, false),
			statusItem = Db.Get().DuplicantStatusItems.PoorQualityOfLife
		};
		this.qolAttribute = Db.Get().Attributes.QualityOfLife.Lookup(base.gameObject);
	}

	// Token: 0x06004081 RID: 16513 RVA: 0x00168FAC File Offset: 0x001671AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		while (this.breakBlocks.Count < 24)
		{
			this.breakBlocks.Add(false);
		}
		while (this.breakBlocks.Count > 24)
		{
			this.breakBlocks.RemoveAt(this.breakBlocks.Count - 1);
		}
		base.Subscribe<QualityOfLifeNeed>(1714332666, QualityOfLifeNeed.OnScheduleBlocksTickDelegate);
	}

	// Token: 0x06004082 RID: 16514 RVA: 0x00169018 File Offset: 0x00167218
	public void Sim4000ms(float dt)
	{
		if (this.skipUpdate)
		{
			return;
		}
		float num = 0.004166667f;
		float b = 0.041666668f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Morale);
		if (currentQualitySetting.id == "Disabled")
		{
			base.SetModifier(null);
			return;
		}
		if (currentQualitySetting.id == "Easy")
		{
			num = 0.0033333334f;
			b = 0.016666668f;
		}
		else if (currentQualitySetting.id == "Hard")
		{
			num = 0.008333334f;
			b = 0.05f;
		}
		else if (currentQualitySetting.id == "VeryHard")
		{
			num = 0.016666668f;
			b = 0.083333336f;
		}
		float totalValue = this.qolAttribute.GetTotalValue();
		float totalValue2 = this.expectationAttribute.GetTotalValue();
		float num2 = totalValue2 - totalValue;
		if (totalValue < totalValue2)
		{
			this.stressPenalty.modifier.SetValue(Mathf.Min(num2 * num, b));
			base.SetModifier(this.stressPenalty);
			return;
		}
		if (totalValue > totalValue2)
		{
			this.stressBonus.modifier.SetValue(Mathf.Max(-num2 * -0.016666668f, -0.033333335f));
			base.SetModifier(this.stressBonus);
			return;
		}
		base.SetModifier(this.stressNeutral);
	}

	// Token: 0x06004083 RID: 16515 RVA: 0x00169150 File Offset: 0x00167350
	private void OnScheduleBlocksTick(object data)
	{
		Schedule schedule = (Schedule)data;
		ScheduleBlock block = schedule.GetBlock(Schedule.GetLastBlockIdx());
		ScheduleBlock block2 = schedule.GetBlock(Schedule.GetBlockIdx());
		bool flag = block.IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		bool flag2 = block2.IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		this.breakBlocks[Schedule.GetLastBlockIdx()] = flag;
		if (flag && !flag2)
		{
			int num = 0;
			using (List<bool>.Enumerator enumerator = this.breakBlocks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current)
					{
						num++;
					}
				}
			}
			this.ApplyBreakBonus(num);
		}
	}

	// Token: 0x06004084 RID: 16516 RVA: 0x0016920C File Offset: 0x0016740C
	private void ApplyBreakBonus(int numBlocks)
	{
		string breakBonus = QualityOfLifeNeed.GetBreakBonus(numBlocks);
		if (breakBonus != null)
		{
			base.GetComponent<Effects>().Add(breakBonus, true);
		}
	}

	// Token: 0x06004085 RID: 16517 RVA: 0x00169234 File Offset: 0x00167434
	public static string GetBreakBonus(int numBlocks)
	{
		int num = numBlocks - 1;
		if (num >= QualityOfLifeNeed.breakLengthEffects.Count)
		{
			return QualityOfLifeNeed.breakLengthEffects[QualityOfLifeNeed.breakLengthEffects.Count - 1];
		}
		if (num >= 0)
		{
			return QualityOfLifeNeed.breakLengthEffects[num];
		}
		return null;
	}

	// Token: 0x04002A0B RID: 10763
	private AttributeInstance qolAttribute;

	// Token: 0x04002A0C RID: 10764
	public bool skipUpdate;

	// Token: 0x04002A0D RID: 10765
	[Serialize]
	private List<bool> breakBlocks;

	// Token: 0x04002A0E RID: 10766
	private static readonly EventSystem.IntraObjectHandler<QualityOfLifeNeed> OnScheduleBlocksTickDelegate = new EventSystem.IntraObjectHandler<QualityOfLifeNeed>(delegate(QualityOfLifeNeed component, object data)
	{
		component.OnScheduleBlocksTick(data);
	});

	// Token: 0x04002A0F RID: 10767
	private static List<string> breakLengthEffects = new List<string>
	{
		"Break1",
		"Break2",
		"Break3",
		"Break4",
		"Break5"
	};
}
