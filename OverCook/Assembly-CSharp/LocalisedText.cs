using System;
using UnityEngine;

// Token: 0x02000142 RID: 322
[ExecutionDependency(typeof(BootstrapManager))]
[ExecutionDependency(typeof(MetaEnvironmentFactory))]
[ExecutionDependency(typeof(Localization))]
public class LocalisedText : TextEx
{
	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x0002AC99 File Offset: 0x00029099
	// (set) Token: 0x060005C0 RID: 1472 RVA: 0x0002ACA4 File Offset: 0x000290A4
	public override string text
	{
		get
		{
			return this.m_Text;
		}
		set
		{
			this.m_tag = value;
			if (Application.isPlaying)
			{
				base.text = Localization.Get(this.m_tag, new LocToken[0]);
			}
			else
			{
				base.text = "<" + this.m_tag + ">";
			}
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0002ACF9 File Offset: 0x000290F9
	// (set) Token: 0x060005C2 RID: 1474 RVA: 0x0002AD01 File Offset: 0x00029101
	public string literalText
	{
		get
		{
			return this.m_Text;
		}
		set
		{
			this.m_Text = value;
		}
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x0002AD0A File Offset: 0x0002910A
	protected override void Awake()
	{
		base.Awake();
		this.m_Text = Localization.Get(this.m_tag, new LocToken[0]);
	}

	// Token: 0x040004C2 RID: 1218
	[SerializeField]
	private string m_tag;
}
