using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000449 RID: 1097
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimSequencer")]
public class KAnimSequencer : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060017A7 RID: 6055 RVA: 0x00079D25 File Offset: 0x00077F25
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.mb = base.GetComponent<MinionBrain>();
		if (this.autoRun)
		{
			this.PlaySequence();
		}
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x00079D53 File Offset: 0x00077F53
	public void Reset()
	{
		this.currentIndex = 0;
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x00079D5C File Offset: 0x00077F5C
	public void PlaySequence()
	{
		if (this.sequence != null && this.sequence.Length != 0)
		{
			if (this.mb != null)
			{
				this.mb.Suspend("AnimSequencer");
			}
			this.kbac.onAnimComplete += this.PlayNext;
			this.PlayNext(null);
		}
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x00079DBC File Offset: 0x00077FBC
	private void PlayNext(HashedString name)
	{
		if (this.sequence.Length > this.currentIndex)
		{
			this.kbac.Play(new HashedString(this.sequence[this.currentIndex].anim), this.sequence[this.currentIndex].mode, this.sequence[this.currentIndex].speed, 0f);
			this.currentIndex++;
			return;
		}
		this.kbac.onAnimComplete -= this.PlayNext;
		if (this.mb != null)
		{
			this.mb.Resume("AnimSequencer");
		}
	}

	// Token: 0x04000D19 RID: 3353
	[Serialize]
	public bool autoRun;

	// Token: 0x04000D1A RID: 3354
	[Serialize]
	public KAnimSequencer.KAnimSequence[] sequence = new KAnimSequencer.KAnimSequence[0];

	// Token: 0x04000D1B RID: 3355
	private int currentIndex;

	// Token: 0x04000D1C RID: 3356
	private KBatchedAnimController kbac;

	// Token: 0x04000D1D RID: 3357
	private MinionBrain mb;

	// Token: 0x020010D6 RID: 4310
	[SerializationConfig(MemberSerialization.OptOut)]
	[Serializable]
	public class KAnimSequence
	{
		// Token: 0x04005A38 RID: 23096
		public string anim;

		// Token: 0x04005A39 RID: 23097
		public float speed = 1f;

		// Token: 0x04005A3A RID: 23098
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}
}
