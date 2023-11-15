using System;
using STRINGS;

// Token: 0x020009D8 RID: 2520
public class ConditionPassengersOnBoard : ProcessCondition
{
	// Token: 0x06004B58 RID: 19288 RVA: 0x001A7BAB File Offset: 0x001A5DAB
	public ConditionPassengersOnBoard(PassengerRocketModule module)
	{
		this.module = module;
	}

	// Token: 0x06004B59 RID: 19289 RVA: 0x001A7BBC File Offset: 0x001A5DBC
	public override ProcessCondition.Status EvaluateCondition()
	{
		global::Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
		if (crewBoardedFraction.first != crewBoardedFraction.second)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B5A RID: 19290 RVA: 0x001A7BE6 File Offset: 0x001A5DE6
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.FAILURE;
	}

	// Token: 0x06004B5B RID: 19291 RVA: 0x001A7C04 File Offset: 0x001A5E04
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		global::Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
		if (status == ProcessCondition.Status.Ready)
		{
			if (crewBoardedFraction.second != 0)
			{
				return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.READY, crewBoardedFraction.first, crewBoardedFraction.second);
			}
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.NONE, crewBoardedFraction.first, crewBoardedFraction.second);
		}
		else
		{
			if (crewBoardedFraction.first == 0)
			{
				return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.FAILURE, crewBoardedFraction.first, crewBoardedFraction.second);
			}
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.WARNING, crewBoardedFraction.first, crewBoardedFraction.second);
		}
	}

	// Token: 0x06004B5C RID: 19292 RVA: 0x001A7CC8 File Offset: 0x001A5EC8
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400314A RID: 12618
	private PassengerRocketModule module;
}
