using System;
using System.Collections.Generic;
using FMODUnity;

namespace Database
{
	// Token: 0x02000D66 RID: 3430
	public class ColonyAchievement : Resource
	{
		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06006B2A RID: 27434 RVA: 0x0029CCF1 File Offset: 0x0029AEF1
		// (set) Token: 0x06006B2B RID: 27435 RVA: 0x0029CCF9 File Offset: 0x0029AEF9
		public EventReference victoryNISSnapshot { get; private set; }

		// Token: 0x06006B2C RID: 27436 RVA: 0x0029CD04 File Offset: 0x0029AF04
		public ColonyAchievement(string Id, string platformAchievementId, string Name, string description, bool isVictoryCondition, List<ColonyAchievementRequirement> requirementChecklist, string messageTitle = "", string messageBody = "", string videoDataName = "", string victoryLoopVideo = "", Action<KMonoBehaviour> VictorySequence = null, EventReference victorySnapshot = default(EventReference), string icon = "", string[] dlcIds = null) : base(Id, Name)
		{
			this.Id = Id;
			this.platformAchievementId = platformAchievementId;
			this.Name = Name;
			this.description = description;
			this.isVictoryCondition = isVictoryCondition;
			this.requirementChecklist = requirementChecklist;
			this.messageTitle = messageTitle;
			this.messageBody = messageBody;
			this.shortVideoName = videoDataName;
			this.loopVideoName = victoryLoopVideo;
			this.victorySequence = VictorySequence;
			this.victoryNISSnapshot = (victorySnapshot.IsNull ? AudioMixerSnapshots.Get().VictoryNISGenericSnapshot : victorySnapshot);
			this.icon = icon;
			this.dlcIds = dlcIds;
			if (this.dlcIds == null)
			{
				this.dlcIds = DlcManager.AVAILABLE_ALL_VERSIONS;
			}
		}

		// Token: 0x04004DF9 RID: 19961
		public string description;

		// Token: 0x04004DFA RID: 19962
		public bool isVictoryCondition;

		// Token: 0x04004DFB RID: 19963
		public string messageTitle;

		// Token: 0x04004DFC RID: 19964
		public string messageBody;

		// Token: 0x04004DFD RID: 19965
		public string shortVideoName;

		// Token: 0x04004DFE RID: 19966
		public string loopVideoName;

		// Token: 0x04004DFF RID: 19967
		public string platformAchievementId;

		// Token: 0x04004E00 RID: 19968
		public string icon;

		// Token: 0x04004E01 RID: 19969
		public List<ColonyAchievementRequirement> requirementChecklist = new List<ColonyAchievementRequirement>();

		// Token: 0x04004E02 RID: 19970
		public Action<KMonoBehaviour> victorySequence;

		// Token: 0x04004E04 RID: 19972
		public string[] dlcIds;
	}
}
