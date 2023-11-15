using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B02 RID: 2818
public class FrontEndBackground : UIDupeRandomizer
{
	// Token: 0x060056E6 RID: 22246 RVA: 0x001FC288 File Offset: 0x001FA488
	protected override void Start()
	{
		this.tuning = TuningData<FrontEndBackground.Tuning>.Get();
		base.Start();
		for (int i = 0; i < this.anims.Length; i++)
		{
			int minionIndex = i;
			KBatchedAnimController kbatchedAnimController = this.anims[i].minions[0];
			if (kbatchedAnimController.gameObject.activeInHierarchy)
			{
				kbatchedAnimController.onAnimComplete += delegate(HashedString name)
				{
					this.WaitForABit(minionIndex, name);
				};
				this.WaitForABit(i, HashedString.Invalid);
			}
		}
		this.dreckoController = base.transform.GetChild(0).Find("startmenu_drecko").GetComponent<KBatchedAnimController>();
		if (this.dreckoController.gameObject.activeInHierarchy)
		{
			this.dreckoController.enabled = false;
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minFirstDreckoInterval, this.tuning.maxFirstDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x060056E7 RID: 22247 RVA: 0x001FC376 File Offset: 0x001FA576
	protected override void Update()
	{
		base.Update();
		this.UpdateDrecko();
	}

	// Token: 0x060056E8 RID: 22248 RVA: 0x001FC384 File Offset: 0x001FA584
	private void UpdateDrecko()
	{
		if (this.dreckoController.gameObject.activeInHierarchy && Time.unscaledTime > this.nextDreckoTime)
		{
			this.dreckoController.enabled = true;
			this.dreckoController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minDreckoInterval, this.tuning.maxDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x060056E9 RID: 22249 RVA: 0x001FC403 File Offset: 0x001FA603
	private void WaitForABit(int minion_idx, HashedString name)
	{
		base.StartCoroutine(this.WaitForTime(minion_idx));
	}

	// Token: 0x060056EA RID: 22250 RVA: 0x001FC413 File Offset: 0x001FA613
	private IEnumerator WaitForTime(int minion_idx)
	{
		this.anims[minion_idx].lastWaitTime = UnityEngine.Random.Range(this.anims[minion_idx].minSecondsBetweenAction, this.anims[minion_idx].maxSecondsBetweenAction);
		yield return new WaitForSecondsRealtime(this.anims[minion_idx].lastWaitTime);
		base.GetNewBody(minion_idx);
		using (List<KBatchedAnimController>.Enumerator enumerator = this.anims[minion_idx].minions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KBatchedAnimController kbatchedAnimController = enumerator.Current;
				kbatchedAnimController.ClearQueue();
				kbatchedAnimController.Play(this.anims[minion_idx].anim_name, KAnim.PlayMode.Once, 1f, 0f);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x04003A97 RID: 14999
	private KBatchedAnimController dreckoController;

	// Token: 0x04003A98 RID: 15000
	private float nextDreckoTime;

	// Token: 0x04003A99 RID: 15001
	private FrontEndBackground.Tuning tuning;

	// Token: 0x02001A13 RID: 6675
	public class Tuning : TuningData<FrontEndBackground.Tuning>
	{
		// Token: 0x0400783B RID: 30779
		public float minDreckoInterval;

		// Token: 0x0400783C RID: 30780
		public float maxDreckoInterval;

		// Token: 0x0400783D RID: 30781
		public float minFirstDreckoInterval;

		// Token: 0x0400783E RID: 30782
		public float maxFirstDreckoInterval;
	}
}
