using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BCF RID: 3023
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenEntryRow")]
public class ReportScreenEntryRow : KMonoBehaviour
{
	// Token: 0x06005F04 RID: 24324 RVA: 0x0022E10C File Offset: 0x0022C30C
	private List<ReportManager.ReportEntry.Note> Sort(List<ReportManager.ReportEntry.Note> notes, ReportManager.ReportEntry.Order order)
	{
		if (order == ReportManager.ReportEntry.Order.Ascending)
		{
			notes.Sort((ReportManager.ReportEntry.Note x, ReportManager.ReportEntry.Note y) => x.value.CompareTo(y.value));
		}
		else if (order == ReportManager.ReportEntry.Order.Descending)
		{
			notes.Sort((ReportManager.ReportEntry.Note x, ReportManager.ReportEntry.Note y) => y.value.CompareTo(x.value));
		}
		return notes;
	}

	// Token: 0x06005F05 RID: 24325 RVA: 0x0022E16E File Offset: 0x0022C36E
	public static void DestroyStatics()
	{
		ReportScreenEntryRow.notes = null;
	}

	// Token: 0x06005F06 RID: 24326 RVA: 0x0022E178 File Offset: 0x0022C378
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.added.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnPositiveNoteTooltip);
		this.removed.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNegativeNoteTooltip);
		this.net.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNetNoteTooltip);
		this.name.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.OnNetNoteTooltip);
	}

	// Token: 0x06005F07 RID: 24327 RVA: 0x0022E1FC File Offset: 0x0022C3FC
	private string OnNoteTooltip(float total_accumulation, string tooltip_text, ReportManager.ReportEntry.Order order, ReportManager.FormattingFn format_fn, Func<ReportManager.ReportEntry.Note, bool> is_note_applicable_cb, ReportManager.GroupFormattingFn group_format_fn = null)
	{
		ReportScreenEntryRow.notes.Clear();
		this.entry.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
		{
			if (is_note_applicable_cb(note))
			{
				ReportScreenEntryRow.notes.Add(note);
			}
		});
		string text = "";
		float num = 0f;
		if (this.entry.contextEntries.Count > 0)
		{
			num = (float)this.entry.contextEntries.Count;
		}
		else
		{
			num = (float)ReportScreenEntryRow.notes.Count;
		}
		num = Mathf.Max(num, 1f);
		foreach (ReportManager.ReportEntry.Note note2 in this.Sort(ReportScreenEntryRow.notes, this.reportGroup.posNoteOrder))
		{
			string arg = format_fn(note2.value);
			if (this.toggle.gameObject.activeInHierarchy && group_format_fn != null)
			{
				arg = group_format_fn(note2.value, num);
			}
			text = string.Format(UI.ENDOFDAYREPORT.NOTES.NOTE_ENTRY_LINE_ITEM, text, note2.note, arg);
		}
		string arg2 = format_fn(total_accumulation);
		if (this.entry.context != null)
		{
			return string.Format(tooltip_text + "\n" + text, arg2, this.entry.context);
		}
		if (group_format_fn != null)
		{
			arg2 = group_format_fn(total_accumulation, num);
			return string.Format(tooltip_text + "\n" + text, arg2, UI.ENDOFDAYREPORT.MY_COLONY);
		}
		return string.Format(tooltip_text + "\n" + text, arg2, UI.ENDOFDAYREPORT.MY_COLONY);
	}

	// Token: 0x06005F08 RID: 24328 RVA: 0x0022E398 File Offset: 0x0022C598
	private string OnNegativeNoteTooltip()
	{
		return this.OnNoteTooltip(-this.entry.Negative, this.reportGroup.negativeTooltip, this.reportGroup.negNoteOrder, this.reportGroup.formatfn, (ReportManager.ReportEntry.Note note) => this.IsNegativeNote(note), this.reportGroup.groupFormatfn);
	}

	// Token: 0x06005F09 RID: 24329 RVA: 0x0022E3F0 File Offset: 0x0022C5F0
	private string OnPositiveNoteTooltip()
	{
		return this.OnNoteTooltip(this.entry.Positive, this.reportGroup.positiveTooltip, this.reportGroup.posNoteOrder, this.reportGroup.formatfn, (ReportManager.ReportEntry.Note note) => this.IsPositiveNote(note), this.reportGroup.groupFormatfn);
	}

	// Token: 0x06005F0A RID: 24330 RVA: 0x0022E446 File Offset: 0x0022C646
	private string OnNetNoteTooltip()
	{
		if (this.entry.Net > 0f)
		{
			return this.OnPositiveNoteTooltip();
		}
		return this.OnNegativeNoteTooltip();
	}

	// Token: 0x06005F0B RID: 24331 RVA: 0x0022E467 File Offset: 0x0022C667
	private bool IsPositiveNote(ReportManager.ReportEntry.Note note)
	{
		return note.value > 0f;
	}

	// Token: 0x06005F0C RID: 24332 RVA: 0x0022E479 File Offset: 0x0022C679
	private bool IsNegativeNote(ReportManager.ReportEntry.Note note)
	{
		return note.value < 0f;
	}

	// Token: 0x06005F0D RID: 24333 RVA: 0x0022E48C File Offset: 0x0022C68C
	public void SetLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup)
	{
		this.entry = entry;
		this.reportGroup = reportGroup;
		ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList pos_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
		entry.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
		{
			if (this.IsPositiveNote(note))
			{
				pos_notes.Add(note);
			}
		});
		ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList neg_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
		entry.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
		{
			if (this.IsNegativeNote(note))
			{
				neg_notes.Add(note);
			}
		});
		LayoutElement component = this.name.GetComponent<LayoutElement>();
		if (entry.context == null)
		{
			component.minWidth = (component.preferredWidth = this.nameWidth);
			if (entry.HasContextEntries())
			{
				this.toggle.gameObject.SetActive(true);
				this.spacer.minWidth = this.groupSpacerWidth;
			}
			else
			{
				this.toggle.gameObject.SetActive(false);
				this.spacer.minWidth = this.groupSpacerWidth + this.toggle.GetComponent<LayoutElement>().minWidth;
			}
			this.name.text = reportGroup.stringKey;
		}
		else
		{
			this.toggle.gameObject.SetActive(false);
			this.spacer.minWidth = this.contextSpacerWidth;
			this.name.text = entry.context;
			component.minWidth = (component.preferredWidth = this.nameWidth - this.indentWidth);
			if (base.transform.GetSiblingIndex() % 2 != 0)
			{
				this.bgImage.color = this.oddRowColor;
			}
		}
		if (this.addedValue != entry.Positive)
		{
			string text = reportGroup.formatfn(entry.Positive);
			if (reportGroup.groupFormatfn != null && entry.context == null)
			{
				float num;
				if (entry.contextEntries.Count > 0)
				{
					num = (float)entry.contextEntries.Count;
				}
				else
				{
					num = (float)pos_notes.Count;
				}
				num = Mathf.Max(num, 1f);
				text = reportGroup.groupFormatfn(entry.Positive, num);
			}
			this.added.text = text;
			this.addedValue = entry.Positive;
		}
		if (this.removedValue != entry.Negative)
		{
			string text2 = reportGroup.formatfn(-entry.Negative);
			if (reportGroup.groupFormatfn != null && entry.context == null)
			{
				float num2;
				if (entry.contextEntries.Count > 0)
				{
					num2 = (float)entry.contextEntries.Count;
				}
				else
				{
					num2 = (float)neg_notes.Count;
				}
				num2 = Mathf.Max(num2, 1f);
				text2 = reportGroup.groupFormatfn(-entry.Negative, num2);
			}
			this.removed.text = text2;
			this.removedValue = entry.Negative;
		}
		if (this.netValue != entry.Net)
		{
			string text3 = (reportGroup.formatfn == null) ? entry.Net.ToString() : reportGroup.formatfn(entry.Net);
			if (reportGroup.groupFormatfn != null && entry.context == null)
			{
				float num3;
				if (entry.contextEntries.Count > 0)
				{
					num3 = (float)entry.contextEntries.Count;
				}
				else
				{
					num3 = (float)(pos_notes.Count + neg_notes.Count);
				}
				num3 = Mathf.Max(num3, 1f);
				text3 = reportGroup.groupFormatfn(entry.Net, num3);
			}
			this.net.text = text3;
			this.netValue = entry.Net;
		}
		pos_notes.Recycle();
		neg_notes.Recycle();
	}

	// Token: 0x04004036 RID: 16438
	[SerializeField]
	public new LocText name;

	// Token: 0x04004037 RID: 16439
	[SerializeField]
	public LocText added;

	// Token: 0x04004038 RID: 16440
	[SerializeField]
	public LocText removed;

	// Token: 0x04004039 RID: 16441
	[SerializeField]
	public LocText net;

	// Token: 0x0400403A RID: 16442
	private float addedValue = float.NegativeInfinity;

	// Token: 0x0400403B RID: 16443
	private float removedValue = float.NegativeInfinity;

	// Token: 0x0400403C RID: 16444
	private float netValue = float.NegativeInfinity;

	// Token: 0x0400403D RID: 16445
	[SerializeField]
	public MultiToggle toggle;

	// Token: 0x0400403E RID: 16446
	[SerializeField]
	private LayoutElement spacer;

	// Token: 0x0400403F RID: 16447
	[SerializeField]
	private Image bgImage;

	// Token: 0x04004040 RID: 16448
	public float groupSpacerWidth;

	// Token: 0x04004041 RID: 16449
	public float contextSpacerWidth;

	// Token: 0x04004042 RID: 16450
	private float nameWidth = 164f;

	// Token: 0x04004043 RID: 16451
	private float indentWidth = 6f;

	// Token: 0x04004044 RID: 16452
	[SerializeField]
	private Color oddRowColor;

	// Token: 0x04004045 RID: 16453
	private static List<ReportManager.ReportEntry.Note> notes = new List<ReportManager.ReportEntry.Note>();

	// Token: 0x04004046 RID: 16454
	private ReportManager.ReportEntry entry;

	// Token: 0x04004047 RID: 16455
	private ReportManager.ReportGroup reportGroup;
}
