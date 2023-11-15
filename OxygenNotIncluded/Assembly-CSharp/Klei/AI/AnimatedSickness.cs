using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DEB RID: 3563
	public class AnimatedSickness : Sickness.SicknessComponent
	{
		// Token: 0x06006D88 RID: 28040 RVA: 0x002B36B0 File Offset: 0x002B18B0
		public AnimatedSickness(HashedString[] kanim_filenames, Expression expression)
		{
			this.kanims = new KAnimFile[kanim_filenames.Length];
			for (int i = 0; i < kanim_filenames.Length; i++)
			{
				this.kanims[i] = Assets.GetAnim(kanim_filenames[i]);
			}
			this.expression = expression;
		}

		// Token: 0x06006D89 RID: 28041 RVA: 0x002B36FC File Offset: 0x002B18FC
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().AddAnimOverrides(this.kanims[i], 10f);
			}
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().AddExpression(this.expression);
			}
			return null;
		}

		// Token: 0x06006D8A RID: 28042 RVA: 0x002B3750 File Offset: 0x002B1950
		public override void OnCure(GameObject go, object instace_data)
		{
			if (this.expression != null)
			{
				go.GetComponent<FaceGraph>().RemoveExpression(this.expression);
			}
			for (int i = 0; i < this.kanims.Length; i++)
			{
				go.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(this.kanims[i]);
			}
		}

		// Token: 0x04005223 RID: 21027
		private KAnimFile[] kanims;

		// Token: 0x04005224 RID: 21028
		private Expression expression;
	}
}
