using System;

namespace Database
{
	// Token: 0x02000D03 RID: 3331
	public class Faces : ResourceSet<Face>
	{
		// Token: 0x060069AD RID: 27053 RVA: 0x0028D310 File Offset: 0x0028B510
		public Faces()
		{
			this.Neutral = base.Add(new Face("Neutral", null));
			this.Happy = base.Add(new Face("Happy", null));
			this.Uncomfortable = base.Add(new Face("Uncomfortable", null));
			this.Cold = base.Add(new Face("Cold", null));
			this.Hot = base.Add(new Face("Hot", "headfx_sweat"));
			this.Tired = base.Add(new Face("Tired", null));
			this.Sleep = base.Add(new Face("Sleep", null));
			this.Hungry = base.Add(new Face("Hungry", null));
			this.Angry = base.Add(new Face("Angry", null));
			this.Suffocate = base.Add(new Face("Suffocate", null));
			this.Sick = base.Add(new Face("Sick", "headfx_sick"));
			this.SickSpores = base.Add(new Face("Spores", "headfx_spores"));
			this.Zombie = base.Add(new Face("Zombie", null));
			this.SickFierySkin = base.Add(new Face("Fiery", "headfx_fiery"));
			this.SickCold = base.Add(new Face("SickCold", "headfx_sickcold"));
			this.Pollen = base.Add(new Face("Pollen", "headfx_pollen"));
			this.Dead = base.Add(new Face("Death", null));
			this.Productive = base.Add(new Face("Productive", null));
			this.Determined = base.Add(new Face("Determined", null));
			this.Sticker = base.Add(new Face("Sticker", null));
			this.Sparkle = base.Add(new Face("Sparkle", null));
			this.Balloon = base.Add(new Face("Balloon", null));
			this.Tickled = base.Add(new Face("Tickled", null));
			this.Music = base.Add(new Face("Music", null));
			this.Radiation1 = base.Add(new Face("Radiation1", "headfx_radiation1"));
			this.Radiation2 = base.Add(new Face("Radiation2", "headfx_radiation2"));
			this.Radiation3 = base.Add(new Face("Radiation3", "headfx_radiation3"));
			this.Radiation4 = base.Add(new Face("Radiation4", "headfx_radiation4"));
		}

		// Token: 0x04004BB7 RID: 19383
		public Face Neutral;

		// Token: 0x04004BB8 RID: 19384
		public Face Happy;

		// Token: 0x04004BB9 RID: 19385
		public Face Uncomfortable;

		// Token: 0x04004BBA RID: 19386
		public Face Cold;

		// Token: 0x04004BBB RID: 19387
		public Face Hot;

		// Token: 0x04004BBC RID: 19388
		public Face Tired;

		// Token: 0x04004BBD RID: 19389
		public Face Sleep;

		// Token: 0x04004BBE RID: 19390
		public Face Hungry;

		// Token: 0x04004BBF RID: 19391
		public Face Angry;

		// Token: 0x04004BC0 RID: 19392
		public Face Suffocate;

		// Token: 0x04004BC1 RID: 19393
		public Face Dead;

		// Token: 0x04004BC2 RID: 19394
		public Face Sick;

		// Token: 0x04004BC3 RID: 19395
		public Face SickSpores;

		// Token: 0x04004BC4 RID: 19396
		public Face Zombie;

		// Token: 0x04004BC5 RID: 19397
		public Face SickFierySkin;

		// Token: 0x04004BC6 RID: 19398
		public Face SickCold;

		// Token: 0x04004BC7 RID: 19399
		public Face Pollen;

		// Token: 0x04004BC8 RID: 19400
		public Face Productive;

		// Token: 0x04004BC9 RID: 19401
		public Face Determined;

		// Token: 0x04004BCA RID: 19402
		public Face Sticker;

		// Token: 0x04004BCB RID: 19403
		public Face Balloon;

		// Token: 0x04004BCC RID: 19404
		public Face Sparkle;

		// Token: 0x04004BCD RID: 19405
		public Face Tickled;

		// Token: 0x04004BCE RID: 19406
		public Face Music;

		// Token: 0x04004BCF RID: 19407
		public Face Radiation1;

		// Token: 0x04004BD0 RID: 19408
		public Face Radiation2;

		// Token: 0x04004BD1 RID: 19409
		public Face Radiation3;

		// Token: 0x04004BD2 RID: 19410
		public Face Radiation4;
	}
}
