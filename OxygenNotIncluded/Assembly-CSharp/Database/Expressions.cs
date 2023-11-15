using System;

namespace Database
{
	// Token: 0x02000D02 RID: 3330
	public class Expressions : ResourceSet<Expression>
	{
		// Token: 0x060069AC RID: 27052 RVA: 0x0028CFF4 File Offset: 0x0028B1F4
		public Expressions(ResourceSet parent) : base("Expressions", parent)
		{
			Faces faces = Db.Get().Faces;
			this.Angry = new Expression("Angry", this, faces.Angry);
			this.Suffocate = new Expression("Suffocate", this, faces.Suffocate);
			this.RecoverBreath = new Expression("RecoverBreath", this, faces.Uncomfortable);
			this.RedAlert = new Expression("RedAlert", this, faces.Hot);
			this.Hungry = new Expression("Hungry", this, faces.Hungry);
			this.Radiation1 = new Expression("Radiation1", this, faces.Radiation1);
			this.Radiation2 = new Expression("Radiation2", this, faces.Radiation2);
			this.Radiation3 = new Expression("Radiation3", this, faces.Radiation3);
			this.Radiation4 = new Expression("Radiation4", this, faces.Radiation4);
			this.SickSpores = new Expression("SickSpores", this, faces.SickSpores);
			this.Zombie = new Expression("Zombie", this, faces.Zombie);
			this.SickFierySkin = new Expression("SickFierySkin", this, faces.SickFierySkin);
			this.SickCold = new Expression("SickCold", this, faces.SickCold);
			this.Pollen = new Expression("Pollen", this, faces.Pollen);
			this.Sick = new Expression("Sick", this, faces.Sick);
			this.Cold = new Expression("Cold", this, faces.Cold);
			this.Hot = new Expression("Hot", this, faces.Hot);
			this.FullBladder = new Expression("FullBladder", this, faces.Uncomfortable);
			this.Tired = new Expression("Tired", this, faces.Tired);
			this.Unhappy = new Expression("Unhappy", this, faces.Uncomfortable);
			this.Uncomfortable = new Expression("Uncomfortable", this, faces.Uncomfortable);
			this.Productive = new Expression("Productive", this, faces.Productive);
			this.Determined = new Expression("Determined", this, faces.Determined);
			this.Sticker = new Expression("Sticker", this, faces.Sticker);
			this.Balloon = new Expression("Sticker", this, faces.Balloon);
			this.Sparkle = new Expression("Sticker", this, faces.Sparkle);
			this.Music = new Expression("Music", this, faces.Music);
			this.Tickled = new Expression("Tickled", this, faces.Tickled);
			this.Happy = new Expression("Happy", this, faces.Happy);
			this.Relief = new Expression("Relief", this, faces.Happy);
			this.Neutral = new Expression("Neutral", this, faces.Neutral);
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.resources[i].priority = 100 * (this.Count - i);
			}
		}

		// Token: 0x04004B98 RID: 19352
		public Expression Neutral;

		// Token: 0x04004B99 RID: 19353
		public Expression Happy;

		// Token: 0x04004B9A RID: 19354
		public Expression Uncomfortable;

		// Token: 0x04004B9B RID: 19355
		public Expression Cold;

		// Token: 0x04004B9C RID: 19356
		public Expression Hot;

		// Token: 0x04004B9D RID: 19357
		public Expression FullBladder;

		// Token: 0x04004B9E RID: 19358
		public Expression Tired;

		// Token: 0x04004B9F RID: 19359
		public Expression Hungry;

		// Token: 0x04004BA0 RID: 19360
		public Expression Angry;

		// Token: 0x04004BA1 RID: 19361
		public Expression Unhappy;

		// Token: 0x04004BA2 RID: 19362
		public Expression RedAlert;

		// Token: 0x04004BA3 RID: 19363
		public Expression Suffocate;

		// Token: 0x04004BA4 RID: 19364
		public Expression RecoverBreath;

		// Token: 0x04004BA5 RID: 19365
		public Expression Sick;

		// Token: 0x04004BA6 RID: 19366
		public Expression SickSpores;

		// Token: 0x04004BA7 RID: 19367
		public Expression Zombie;

		// Token: 0x04004BA8 RID: 19368
		public Expression SickFierySkin;

		// Token: 0x04004BA9 RID: 19369
		public Expression SickCold;

		// Token: 0x04004BAA RID: 19370
		public Expression Pollen;

		// Token: 0x04004BAB RID: 19371
		public Expression Relief;

		// Token: 0x04004BAC RID: 19372
		public Expression Productive;

		// Token: 0x04004BAD RID: 19373
		public Expression Determined;

		// Token: 0x04004BAE RID: 19374
		public Expression Sticker;

		// Token: 0x04004BAF RID: 19375
		public Expression Balloon;

		// Token: 0x04004BB0 RID: 19376
		public Expression Sparkle;

		// Token: 0x04004BB1 RID: 19377
		public Expression Music;

		// Token: 0x04004BB2 RID: 19378
		public Expression Tickled;

		// Token: 0x04004BB3 RID: 19379
		public Expression Radiation1;

		// Token: 0x04004BB4 RID: 19380
		public Expression Radiation2;

		// Token: 0x04004BB5 RID: 19381
		public Expression Radiation3;

		// Token: 0x04004BB6 RID: 19382
		public Expression Radiation4;
	}
}
