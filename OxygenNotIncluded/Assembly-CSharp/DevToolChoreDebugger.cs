using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000552 RID: 1362
public class DevToolChoreDebugger : DevTool
{
	// Token: 0x06002131 RID: 8497 RVA: 0x000B151D File Offset: 0x000AF71D
	protected override void RenderTo(DevPanel panel)
	{
		this.Update();
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000B1528 File Offset: 0x000AF728
	public void Update()
	{
		if (!Application.isPlaying || SelectTool.Instance == null || SelectTool.Instance.selected == null || SelectTool.Instance.selected.gameObject == null)
		{
			return;
		}
		GameObject gameObject = SelectTool.Instance.selected.gameObject;
		if (this.Consumer == null || (!this.lockSelection && this.selectedGameObject != gameObject))
		{
			this.Consumer = gameObject.GetComponent<ChoreConsumer>();
			this.selectedGameObject = gameObject;
		}
		if (this.Consumer != null)
		{
			ImGui.InputText("Filter:", ref this.filter, 256U);
			this.DisplayAvailableChores();
			ImGui.Text("");
		}
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000B15F0 File Offset: 0x000AF7F0
	private void DisplayAvailableChores()
	{
		ImGui.Checkbox("Lock selection", ref this.lockSelection);
		ImGui.Checkbox("Show Last Successful Chore Selection", ref this.showLastSuccessfulPreconditionSnapshot);
		ImGui.Text("Available Chores:");
		ChoreConsumer.PreconditionSnapshot target_snapshot = this.Consumer.GetLastPreconditionSnapshot();
		if (this.showLastSuccessfulPreconditionSnapshot)
		{
			target_snapshot = this.Consumer.GetLastSuccessfulPreconditionSnapshot();
		}
		this.ShowChores(target_snapshot);
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x000B1650 File Offset: 0x000AF850
	private void ShowChores(ChoreConsumer.PreconditionSnapshot target_snapshot)
	{
		ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollX | ImGuiTableFlags.ScrollY;
		this.rowIndex = 0;
		if (ImGui.BeginTable("Available Chores", this.columns.Count, flags))
		{
			foreach (object obj in this.columns.Keys)
			{
				ImGui.TableSetupColumn(obj.ToString(), ImGuiTableColumnFlags.WidthFixed);
			}
			ImGui.TableHeadersRow();
			for (int i = target_snapshot.succeededContexts.Count - 1; i >= 0; i--)
			{
				this.ShowContext(target_snapshot.succeededContexts[i]);
			}
			if (target_snapshot.doFailedContextsNeedSorting)
			{
				target_snapshot.failedContexts.Sort();
				target_snapshot.doFailedContextsNeedSorting = false;
			}
			for (int j = target_snapshot.failedContexts.Count - 1; j >= 0; j--)
			{
				this.ShowContext(target_snapshot.failedContexts[j]);
			}
			ImGui.EndTable();
		}
	}

	// Token: 0x06002135 RID: 8501 RVA: 0x000B1754 File Offset: 0x000AF954
	private void ShowContext(Chore.Precondition.Context context)
	{
		string text = "";
		Chore chore = context.chore;
		if (!context.IsSuccess())
		{
			text = context.chore.GetPreconditions()[context.failedPreconditionId].id;
		}
		string text2 = "";
		if (chore.driver != null)
		{
			text2 = chore.driver.name;
		}
		string text3 = "";
		if (chore.overrideTarget != null)
		{
			text3 = chore.overrideTarget.name;
		}
		string text4 = "";
		if (!chore.isNull)
		{
			text4 = chore.gameObject.name;
		}
		if (Chore.Precondition.Context.ShouldFilter(this.filter, chore.GetType().ToString()) && Chore.Precondition.Context.ShouldFilter(this.filter, chore.choreType.Id) && Chore.Precondition.Context.ShouldFilter(this.filter, text) && Chore.Precondition.Context.ShouldFilter(this.filter, text2) && Chore.Precondition.Context.ShouldFilter(this.filter, text3) && Chore.Precondition.Context.ShouldFilter(this.filter, text4))
		{
			return;
		}
		this.columns["Id"] = chore.id.ToString();
		this.columns["Class"] = chore.GetType().ToString().Replace("`1", "");
		this.columns["Type"] = chore.choreType.Id;
		this.columns["PriorityClass"] = context.masterPriority.priority_class.ToString();
		this.columns["PersonalPriority"] = context.personalPriority.ToString();
		this.columns["PriorityValue"] = context.masterPriority.priority_value.ToString();
		this.columns["Priority"] = context.priority.ToString();
		this.columns["PriorityMod"] = context.priorityMod.ToString();
		this.columns["ConsumerPriority"] = context.consumerPriority.ToString();
		this.columns["Cost"] = context.cost.ToString();
		this.columns["Interrupt"] = context.interruptPriority.ToString();
		this.columns["Precondition"] = text;
		this.columns["Override"] = text3;
		this.columns["Assigned To"] = text2;
		this.columns["Owner"] = text4;
		this.columns["Details"] = "";
		ImGui.TableNextRow();
		string format = "ID_row_{0}";
		int num = this.rowIndex;
		this.rowIndex = num + 1;
		ImGui.PushID(string.Format(format, num));
		for (int i = 0; i < this.columns.Count; i++)
		{
			ImGui.TableSetColumnIndex(i);
			ImGui.Text(this.columns[i].ToString());
		}
		ImGui.PopID();
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x000B1A72 File Offset: 0x000AFC72
	public void ConsumerDebugDisplayLog()
	{
	}

	// Token: 0x040012B4 RID: 4788
	private string filter = "";

	// Token: 0x040012B5 RID: 4789
	private bool showLastSuccessfulPreconditionSnapshot;

	// Token: 0x040012B6 RID: 4790
	private bool lockSelection;

	// Token: 0x040012B7 RID: 4791
	private ChoreConsumer Consumer;

	// Token: 0x040012B8 RID: 4792
	private GameObject selectedGameObject;

	// Token: 0x040012B9 RID: 4793
	private OrderedDictionary columns = new OrderedDictionary
	{
		{
			"BP",
			""
		},
		{
			"Id",
			""
		},
		{
			"Class",
			""
		},
		{
			"Type",
			""
		},
		{
			"PriorityClass",
			""
		},
		{
			"PersonalPriority",
			""
		},
		{
			"PriorityValue",
			""
		},
		{
			"Priority",
			""
		},
		{
			"PriorityMod",
			""
		},
		{
			"ConsumerPriority",
			""
		},
		{
			"Cost",
			""
		},
		{
			"Interrupt",
			""
		},
		{
			"Precondition",
			""
		},
		{
			"Override",
			""
		},
		{
			"Assigned To",
			""
		},
		{
			"Owner",
			""
		},
		{
			"Details",
			""
		}
	};

	// Token: 0x040012BA RID: 4794
	private int rowIndex;

	// Token: 0x020011EB RID: 4587
	public class EditorPreconditionSnapshot
	{
		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06007B24 RID: 31524 RVA: 0x002DD676 File Offset: 0x002DB876
		// (set) Token: 0x06007B25 RID: 31525 RVA: 0x002DD67E File Offset: 0x002DB87E
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> SucceededContexts { get; set; }

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06007B26 RID: 31526 RVA: 0x002DD687 File Offset: 0x002DB887
		// (set) Token: 0x06007B27 RID: 31527 RVA: 0x002DD68F File Offset: 0x002DB88F
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> FailedContexts { get; set; }

		// Token: 0x020020A5 RID: 8357
		public struct EditorContext
		{
			// Token: 0x17000A72 RID: 2674
			// (get) Token: 0x0600A656 RID: 42582 RVA: 0x0036E33A File Offset: 0x0036C53A
			// (set) Token: 0x0600A657 RID: 42583 RVA: 0x0036E342 File Offset: 0x0036C542
			public string Chore { readonly get; set; }

			// Token: 0x17000A73 RID: 2675
			// (get) Token: 0x0600A658 RID: 42584 RVA: 0x0036E34B File Offset: 0x0036C54B
			// (set) Token: 0x0600A659 RID: 42585 RVA: 0x0036E353 File Offset: 0x0036C553
			public string ChoreType { readonly get; set; }

			// Token: 0x17000A74 RID: 2676
			// (get) Token: 0x0600A65A RID: 42586 RVA: 0x0036E35C File Offset: 0x0036C55C
			// (set) Token: 0x0600A65B RID: 42587 RVA: 0x0036E364 File Offset: 0x0036C564
			public string FailedPrecondition { readonly get; set; }

			// Token: 0x17000A75 RID: 2677
			// (get) Token: 0x0600A65C RID: 42588 RVA: 0x0036E36D File Offset: 0x0036C56D
			// (set) Token: 0x0600A65D RID: 42589 RVA: 0x0036E375 File Offset: 0x0036C575
			public int WorldId { readonly get; set; }
		}
	}
}
