using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AEC RID: 2796
public class OptionsScreenListEntry : ScrollingListEntry
{
	// Token: 0x170003DE RID: 990
	// (get) Token: 0x060038A7 RID: 14503 RVA: 0x0010B6CE File Offset: 0x00109ACE
	// (set) Token: 0x060038A8 RID: 14504 RVA: 0x0010B6DB File Offset: 0x00109ADB
	public int SelectedValueIndex
	{
		get
		{
			return this.m_optionsNameData.Value;
		}
		set
		{
			this.m_optionsNameData.Value = value;
			this.SetNameData(this.m_optionsNameData);
		}
	}

	// Token: 0x060038A9 RID: 14505 RVA: 0x0010B6F5 File Offset: 0x00109AF5
	public override void SetNameData(ScrollingListUIContainer.NameData _nameData)
	{
		this.m_optionsNameData = (_nameData as OptionsScreenListEntry.OptionsNameData);
		this.m_optionsNameData.Setup(this);
	}

	// Token: 0x04002D55 RID: 11605
	[SerializeField]
	[AssignChild("LabelText", Editorbility.NonEditable)]
	private Text m_optionName;

	// Token: 0x04002D56 RID: 11606
	[SerializeField]
	[AssignChild("ValueText", Editorbility.NonEditable)]
	private Text m_optionValue;

	// Token: 0x04002D57 RID: 11607
	[SerializeField]
	[AssignChild("ValueNumber", Editorbility.NonEditable)]
	private Text m_optionNumber;

	// Token: 0x04002D58 RID: 11608
	private OptionsScreenListEntry.OptionsNameData m_optionsNameData;

	// Token: 0x02000AED RID: 2797
	[Serializable]
	public class OptionsDiscreteNameData : OptionsScreenListEntry.OptionsNameData
	{
		// Token: 0x060038AA RID: 14506 RVA: 0x0010B717 File Offset: 0x00109B17
		public OptionsDiscreteNameData(string _label, string[] _options, int _option)
		{
			this.Label = _label;
			this.Options = _options;
			this.SelectedIndex = _option;
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x0010B734 File Offset: 0x00109B34
		public override void Setup(OptionsScreenListEntry _entry)
		{
			_entry.m_optionName.text = this.Label;
			_entry.m_optionValue.text = this.Options[this.SelectedIndex];
			_entry.m_optionNumber.enabled = false;
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060038AC RID: 14508 RVA: 0x0010B76B File Offset: 0x00109B6B
		// (set) Token: 0x060038AD RID: 14509 RVA: 0x0010B774 File Offset: 0x00109B74
		public override int Value
		{
			get
			{
				return this.SelectedIndex;
			}
			set
			{
				int num = this.Options.Length;
				this.SelectedIndex = (value + num) % num;
			}
		}

		// Token: 0x04002D59 RID: 11609
		public string Label;

		// Token: 0x04002D5A RID: 11610
		public string[] Options;

		// Token: 0x04002D5B RID: 11611
		public int SelectedIndex;
	}

	// Token: 0x02000AEE RID: 2798
	[Serializable]
	public class OptionsSliderNameData : OptionsScreenListEntry.OptionsNameData
	{
		// Token: 0x060038AE RID: 14510 RVA: 0x0010B795 File Offset: 0x00109B95
		public OptionsSliderNameData(string _label, int _value, int _quanta)
		{
			this.Label = _label;
			this.Value = _value;
			this.Quanta = _quanta;
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x0010B7BC File Offset: 0x00109BBC
		public override void Setup(OptionsScreenListEntry _entry)
		{
			_entry.m_optionName.text = this.Label;
			_entry.m_optionValue.enabled = false;
			_entry.m_optionNumber.text = Mathf.RoundToInt(this.Prop * 100f).ToString();
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060038B0 RID: 14512 RVA: 0x0010B810 File Offset: 0x00109C10
		// (set) Token: 0x060038B1 RID: 14513 RVA: 0x0010B825 File Offset: 0x00109C25
		public override int Value
		{
			get
			{
				return Mathf.RoundToInt(this.Prop * (float)this.Quanta);
			}
			set
			{
				this.Prop = Mathf.Clamp01((float)value / (float)this.Quanta);
			}
		}

		// Token: 0x04002D5C RID: 11612
		public string Label;

		// Token: 0x04002D5D RID: 11613
		public float Prop;

		// Token: 0x04002D5E RID: 11614
		public int Quanta = 10;
	}

	// Token: 0x02000AEF RID: 2799
	[Serializable]
	public abstract class OptionsNameData : ScrollingListUIContainer.NameData
	{
		// Token: 0x060038B3 RID: 14515
		public abstract void Setup(OptionsScreenListEntry _entry);

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060038B4 RID: 14516
		// (set) Token: 0x060038B5 RID: 14517
		public abstract int Value { get; set; }
	}
}
