using System;
using KSerialization;
using STRINGS;

// Token: 0x02000197 RID: 407
public class FossilBits : FossilExcavationWorkable, ISidescreenButtonControl
{
	// Token: 0x060007F8 RID: 2040 RVA: 0x0002F126 File Offset: 0x0002D326
	protected override bool IsMarkedForExcavation()
	{
		return this.MarkedForDig;
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0002F12E File Offset: 0x0002D32E
	public void SetEntombStatusItemVisibility(bool visible)
	{
		this.entombComponent.SetShowStatusItemOnEntombed(visible);
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0002F13C File Offset: 0x0002D33C
	public void CreateWorkableChore()
	{
		if (this.chore == null && this.operational.IsOperational)
		{
			this.chore = new WorkChore<FossilBits>(Db.Get().ChoreTypes.ExcavateFossil, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x0002F18A File Offset: 0x0002D38A
	public void CancelWorkChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("FossilBits.CancelChore");
			this.chore = null;
		}
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x0002F1AC File Offset: 0x0002D3AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sculpture_kanim")
		};
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		base.SetWorkTime(30f);
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x0002F200 File Offset: 0x0002D400
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		base.SetShouldShowSkillPerkStatusItem(this.IsMarkedForExcavation());
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x0002F220 File Offset: 0x0002D420
	private void OnOperationalChanged(object state)
	{
		if ((bool)state)
		{
			if (this.MarkedForDig)
			{
				this.CreateWorkableChore();
				return;
			}
		}
		else if (this.MarkedForDig)
		{
			this.CancelWorkChore();
		}
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x0002F248 File Offset: 0x0002D448
	private void DropLoot()
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element != null)
		{
			float num = component.Mass;
			int num2 = 0;
			while ((float)num2 < component.Mass / 400f)
			{
				float num3 = num;
				if (num > 400f)
				{
					num3 = 400f;
					num -= 400f;
				}
				int disease_count = (int)((float)component.DiseaseCount * (num3 / component.Mass));
				element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num3, component.Temperature, component.DiseaseIdx, disease_count, false, false, false);
				num2++;
			}
		}
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0002F2FE File Offset: 0x0002D4FE
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.DropLoot();
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0002F318 File Offset: 0x0002D518
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000802 RID: 2050 RVA: 0x0002F31B File Offset: 0x0002D51B
	public string SidescreenButtonText
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000803 RID: 2051 RVA: 0x0002F33A File Offset: 0x0002D53A
	public string SidescreenButtonTooltip
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0002F359 File Offset: 0x0002D559
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x0002F360 File Offset: 0x0002D560
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0002F363 File Offset: 0x0002D563
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0002F368 File Offset: 0x0002D568
	public void OnSidescreenButtonPressed()
	{
		this.MarkedForDig = !this.MarkedForDig;
		base.SetShouldShowSkillPerkStatusItem(this.MarkedForDig);
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		if (this.MarkedForDig)
		{
			this.CreateWorkableChore();
		}
		else
		{
			this.CancelWorkChore();
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x0002F3B9 File Offset: 0x0002D5B9
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000532 RID: 1330
	[Serialize]
	public bool MarkedForDig;

	// Token: 0x04000533 RID: 1331
	private Chore chore;

	// Token: 0x04000534 RID: 1332
	[MyCmpGet]
	private EntombVulnerable entombComponent;

	// Token: 0x04000535 RID: 1333
	[MyCmpGet]
	private Operational operational;
}
