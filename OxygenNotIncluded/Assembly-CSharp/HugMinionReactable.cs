using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class HugMinionReactable : Reactable
{
	// Token: 0x060003AC RID: 940 RVA: 0x0001C9B0 File Offset: 0x0001ABB0
	public HugMinionReactable(GameObject gameObject) : base(gameObject, "HugMinionReactable", Db.Get().ChoreTypes.Hug, 1, 1, true, 1f, 0f, float.PositiveInfinity, 0f, ObjectLayer.Minion)
	{
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001C9F8 File Offset: 0x0001ABF8
	public override bool InternalCanBegin(GameObject newReactor, Navigator.ActiveTransition transition)
	{
		if (this.reactor != null)
		{
			return false;
		}
		Navigator component = newReactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving();
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0001CA32 File Offset: 0x0001AC32
	public override void Update(float dt)
	{
		this.gameObject.GetComponent<Facing>().SetFacing(this.reactor.GetComponent<Facing>().GetFacing());
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0001CA54 File Offset: 0x0001AC54
	protected override void InternalBegin()
	{
		KAnimControllerBase component = this.reactor.GetComponent<KAnimControllerBase>();
		component.AddAnimOverrides(Assets.GetAnim("anim_react_pip_kanim"), 0f);
		component.Play("hug_dupe_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("hug_dupe_loop", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("hug_dupe_pst", KAnim.PlayMode.Once, 1f, 0f);
		component.onAnimComplete += this.Finish;
		this.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(new HashedString[]
		{
			"hug_dupe_pre",
			"hug_dupe_loop",
			"hug_dupe_pst"
		});
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0001CB34 File Offset: 0x0001AD34
	private void Finish(HashedString anim)
	{
		if (anim == "hug_dupe_pst")
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KAnimControllerBase>().onAnimComplete -= this.Finish;
				this.ApplyEffects();
			}
			else
			{
				DebugUtil.DevLogError("HugMinionReactable finishing without adding a Hugged effect.");
			}
			base.End();
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0001CB98 File Offset: 0x0001AD98
	private void ApplyEffects()
	{
		this.reactor.GetComponent<Effects>().Add("Hugged", true);
		HugMonitor.Instance smi = this.gameObject.GetSMI<HugMonitor.Instance>();
		if (smi != null)
		{
			smi.EnterHuggingFrenzy();
		}
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0001CBD1 File Offset: 0x0001ADD1
	protected override void InternalEnd()
	{
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0001CBD3 File Offset: 0x0001ADD3
	protected override void InternalCleanup()
	{
	}
}
