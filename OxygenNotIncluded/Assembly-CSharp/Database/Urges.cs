using System;

namespace Database
{
	// Token: 0x02000D2B RID: 3371
	public class Urges : ResourceSet<Urge>
	{
		// Token: 0x06006A37 RID: 27191 RVA: 0x00297F6C File Offset: 0x0029616C
		public Urges()
		{
			this.HealCritical = base.Add(new Urge("HealCritical"));
			this.BeIncapacitated = base.Add(new Urge("BeIncapacitated"));
			this.PacifyEat = base.Add(new Urge("PacifyEat"));
			this.PacifySleep = base.Add(new Urge("PacifySleep"));
			this.PacifyIdle = base.Add(new Urge("PacifyIdle"));
			this.EmoteHighPriority = base.Add(new Urge("EmoteHighPriority"));
			this.RecoverBreath = base.Add(new Urge("RecoverBreath"));
			this.Aggression = base.Add(new Urge("Aggression"));
			this.MoveToQuarantine = base.Add(new Urge("MoveToQuarantine"));
			this.WashHands = base.Add(new Urge("WashHands"));
			this.Shower = base.Add(new Urge("Shower"));
			this.Eat = base.Add(new Urge("Eat"));
			this.Pee = base.Add(new Urge("Pee"));
			this.RestDueToDisease = base.Add(new Urge("RestDueToDisease"));
			this.Sleep = base.Add(new Urge("Sleep"));
			this.Narcolepsy = base.Add(new Urge("Narcolepsy"));
			this.Doctor = base.Add(new Urge("Doctor"));
			this.Heal = base.Add(new Urge("Heal"));
			this.Feed = base.Add(new Urge("Feed"));
			this.PacifyRelocate = base.Add(new Urge("PacifyRelocate"));
			this.Emote = base.Add(new Urge("Emote"));
			this.MoveToSafety = base.Add(new Urge("MoveToSafety"));
			this.WarmUp = base.Add(new Urge("WarmUp"));
			this.CoolDown = base.Add(new Urge("CoolDown"));
			this.LearnSkill = base.Add(new Urge("LearnSkill"));
			this.EmoteIdle = base.Add(new Urge("EmoteIdle"));
		}

		// Token: 0x04004D70 RID: 19824
		public Urge BeIncapacitated;

		// Token: 0x04004D71 RID: 19825
		public Urge Sleep;

		// Token: 0x04004D72 RID: 19826
		public Urge Narcolepsy;

		// Token: 0x04004D73 RID: 19827
		public Urge Eat;

		// Token: 0x04004D74 RID: 19828
		public Urge WashHands;

		// Token: 0x04004D75 RID: 19829
		public Urge Shower;

		// Token: 0x04004D76 RID: 19830
		public Urge Pee;

		// Token: 0x04004D77 RID: 19831
		public Urge MoveToQuarantine;

		// Token: 0x04004D78 RID: 19832
		public Urge HealCritical;

		// Token: 0x04004D79 RID: 19833
		public Urge RecoverBreath;

		// Token: 0x04004D7A RID: 19834
		public Urge Emote;

		// Token: 0x04004D7B RID: 19835
		public Urge Feed;

		// Token: 0x04004D7C RID: 19836
		public Urge Doctor;

		// Token: 0x04004D7D RID: 19837
		public Urge Flee;

		// Token: 0x04004D7E RID: 19838
		public Urge Heal;

		// Token: 0x04004D7F RID: 19839
		public Urge PacifyIdle;

		// Token: 0x04004D80 RID: 19840
		public Urge PacifyEat;

		// Token: 0x04004D81 RID: 19841
		public Urge PacifySleep;

		// Token: 0x04004D82 RID: 19842
		public Urge PacifyRelocate;

		// Token: 0x04004D83 RID: 19843
		public Urge RestDueToDisease;

		// Token: 0x04004D84 RID: 19844
		public Urge EmoteHighPriority;

		// Token: 0x04004D85 RID: 19845
		public Urge Aggression;

		// Token: 0x04004D86 RID: 19846
		public Urge MoveToSafety;

		// Token: 0x04004D87 RID: 19847
		public Urge WarmUp;

		// Token: 0x04004D88 RID: 19848
		public Urge CoolDown;

		// Token: 0x04004D89 RID: 19849
		public Urge LearnSkill;

		// Token: 0x04004D8A RID: 19850
		public Urge EmoteIdle;
	}
}
