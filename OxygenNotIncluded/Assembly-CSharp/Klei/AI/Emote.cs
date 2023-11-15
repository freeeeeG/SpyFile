using System;
using System.Collections.Generic;

namespace Klei.AI
{
	// Token: 0x02000E08 RID: 3592
	public class Emote : Resource
	{
		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06006E32 RID: 28210 RVA: 0x002B6439 File Offset: 0x002B4639
		public int StepCount
		{
			get
			{
				if (this.emoteSteps != null)
				{
					return this.emoteSteps.Count;
				}
				return 0;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06006E33 RID: 28211 RVA: 0x002B6450 File Offset: 0x002B4650
		public KAnimFile AnimSet
		{
			get
			{
				if (this.animSetName != HashedString.Invalid && this.animSet == null)
				{
					this.animSet = Assets.GetAnim(this.animSetName);
				}
				return this.animSet;
			}
		}

		// Token: 0x06006E34 RID: 28212 RVA: 0x002B6489 File Offset: 0x002B4689
		public Emote(ResourceSet parent, string emoteId, EmoteStep[] defaultSteps, string animSetName = null) : base(emoteId, parent, null)
		{
			this.emoteSteps.AddRange(defaultSteps);
			this.animSetName = animSetName;
		}

		// Token: 0x06006E35 RID: 28213 RVA: 0x002B64C4 File Offset: 0x002B46C4
		public bool IsValidForController(KBatchedAnimController animController)
		{
			bool flag = true;
			int num = 0;
			while (flag && num < this.StepCount)
			{
				flag = animController.HasAnimation(this.emoteSteps[num].anim);
				num++;
			}
			KAnimFileData kanimFileData = (this.animSet == null) ? null : this.animSet.GetData();
			int num2 = 0;
			while (kanimFileData != null && flag && num2 < this.StepCount)
			{
				bool flag2 = false;
				int num3 = 0;
				while (!flag2 && num3 < kanimFileData.animCount)
				{
					flag2 = (kanimFileData.GetAnim(num2).id == this.emoteSteps[num2].anim);
					num3++;
				}
				flag = flag2;
				num2++;
			}
			return flag;
		}

		// Token: 0x06006E36 RID: 28214 RVA: 0x002B657C File Offset: 0x002B477C
		public void ApplyAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.AddAnimOverrides(kanimFile, 0f);
		}

		// Token: 0x06006E37 RID: 28215 RVA: 0x002B65BC File Offset: 0x002B47BC
		public void RemoveAnimOverrides(KBatchedAnimController animController, KAnimFile overrideSet)
		{
			KAnimFile kanimFile = (overrideSet != null) ? overrideSet : this.AnimSet;
			if (kanimFile == null || animController == null)
			{
				return;
			}
			animController.RemoveAnimOverrides(kanimFile);
		}

		// Token: 0x06006E38 RID: 28216 RVA: 0x002B65F8 File Offset: 0x002B47F8
		public void CollectStepAnims(out HashedString[] emoteAnims, int iterations)
		{
			emoteAnims = new HashedString[this.emoteSteps.Count * iterations];
			for (int i = 0; i < emoteAnims.Length; i++)
			{
				emoteAnims[i] = this.emoteSteps[i % this.emoteSteps.Count].anim;
			}
		}

		// Token: 0x06006E39 RID: 28217 RVA: 0x002B664D File Offset: 0x002B484D
		public bool IsValidStep(int stepIdx)
		{
			return stepIdx >= 0 && stepIdx < this.emoteSteps.Count;
		}

		// Token: 0x170007D0 RID: 2000
		public EmoteStep this[int stepIdx]
		{
			get
			{
				if (!this.IsValidStep(stepIdx))
				{
					return null;
				}
				return this.emoteSteps[stepIdx];
			}
		}

		// Token: 0x06006E3B RID: 28219 RVA: 0x002B667C File Offset: 0x002B487C
		public int GetStepIndex(HashedString animName)
		{
			int i = 0;
			bool condition = false;
			while (i < this.emoteSteps.Count)
			{
				if (this.emoteSteps[i].anim == animName)
				{
					condition = true;
					break;
				}
				i++;
			}
			Debug.Assert(condition, string.Format("Could not find emote step {0} for emote {1}!", animName, this.Id));
			return i;
		}

		// Token: 0x04005291 RID: 21137
		private HashedString animSetName = null;

		// Token: 0x04005292 RID: 21138
		private KAnimFile animSet;

		// Token: 0x04005293 RID: 21139
		private List<EmoteStep> emoteSteps = new List<EmoteStep>();
	}
}
