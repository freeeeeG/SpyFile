using System;
using UnityEngine;

// Token: 0x02000B38 RID: 2872
public class OptionsMenuUIController : ScrollingListUIController
{
	// Token: 0x06003A4E RID: 14926 RVA: 0x00115BB0 File Offset: 0x00113FB0
	public void SetNames(string _title, OptionsScreenListEntry.OptionsNameData[] _nameData)
	{
		this.m_titleObject.text = _title;
		this.m_discreteEntries = new OptionsMenuUIController.DiscreteEntry[0];
		this.m_sliderEntries = new OptionsMenuUIController.SliderEntry[0];
		for (int i = 0; i < _nameData.Length; i++)
		{
			if (_nameData[i] is OptionsScreenListEntry.OptionsDiscreteNameData)
			{
				ArrayUtils.PushBack<OptionsMenuUIController.DiscreteEntry>(ref this.m_discreteEntries, new OptionsMenuUIController.DiscreteEntry(_nameData[i] as OptionsScreenListEntry.OptionsDiscreteNameData, i));
			}
			if (_nameData[i] is OptionsScreenListEntry.OptionsSliderNameData)
			{
				ArrayUtils.PushBack<OptionsMenuUIController.SliderEntry>(ref this.m_sliderEntries, new OptionsMenuUIController.SliderEntry(_nameData[i] as OptionsScreenListEntry.OptionsSliderNameData, i));
			}
		}
		base.OnSetNames();
	}

	// Token: 0x06003A4F RID: 14927 RVA: 0x00115C48 File Offset: 0x00114048
	protected override ScrollingListUIContainer.NameData[] GetNameData()
	{
		OptionsMenuUIController.Entry[] array = new OptionsMenuUIController.Entry[this.m_sliderEntries.Length + this.m_discreteEntries.Length];
		this.m_sliderEntries.CopyTo(array, 0);
		this.m_discreteEntries.CopyTo(array, this.m_sliderEntries.Length);
		Array.Sort<OptionsMenuUIController.Entry>(array, (OptionsMenuUIController.Entry x1, OptionsMenuUIController.Entry x2) => x1.Order.CompareTo(x2.Order));
		return array.ConvertAll((OptionsMenuUIController.Entry x) => x.BaseData);
	}

	// Token: 0x06003A50 RID: 14928 RVA: 0x00115CD3 File Offset: 0x001140D3
	public void MoveLeft()
	{
		this.MoveSelection(-1);
	}

	// Token: 0x06003A51 RID: 14929 RVA: 0x00115CDC File Offset: 0x001140DC
	public void MoveRight()
	{
		this.MoveSelection(1);
	}

	// Token: 0x06003A52 RID: 14930 RVA: 0x00115CE8 File Offset: 0x001140E8
	public int GetOptionValue()
	{
		int selection = base.GetSelection();
		OptionsScreenListEntry optionsScreenListEntry = base.GetUnselectedEntries()[selection] as OptionsScreenListEntry;
		return optionsScreenListEntry.SelectedValueIndex;
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x00115D10 File Offset: 0x00114110
	private void MoveSelection(int _moveValue)
	{
		int selection = base.GetSelection();
		OptionsScreenListEntry optionsScreenListEntry = base.GetUnselectedEntries()[selection] as OptionsScreenListEntry;
		optionsScreenListEntry.SelectedValueIndex += _moveValue;
		OptionsScreenListEntry optionsScreenListEntry2 = base.GetSelectedEntry() as OptionsScreenListEntry;
		optionsScreenListEntry2.SelectedValueIndex = optionsScreenListEntry.SelectedValueIndex;
	}

	// Token: 0x04002F39 RID: 12089
	[SerializeField]
	private LocalisedText m_titleObject;

	// Token: 0x04002F3A RID: 12090
	[SerializeField]
	private OptionsMenuUIController.DiscreteEntry[] m_discreteEntries = new OptionsMenuUIController.DiscreteEntry[0];

	// Token: 0x04002F3B RID: 12091
	[SerializeField]
	private OptionsMenuUIController.SliderEntry[] m_sliderEntries = new OptionsMenuUIController.SliderEntry[0];

	// Token: 0x02000B39 RID: 2873
	[Serializable]
	private class DiscreteEntry : OptionsMenuUIController.GenericEntry<OptionsScreenListEntry.OptionsDiscreteNameData>
	{
		// Token: 0x06003A56 RID: 14934 RVA: 0x00115DA5 File Offset: 0x001141A5
		public DiscreteEntry(OptionsScreenListEntry.OptionsDiscreteNameData _data, int _order) : base(_data, _order)
		{
		}
	}

	// Token: 0x02000B3A RID: 2874
	[Serializable]
	private class SliderEntry : OptionsMenuUIController.GenericEntry<OptionsScreenListEntry.OptionsSliderNameData>
	{
		// Token: 0x06003A57 RID: 14935 RVA: 0x00115DAF File Offset: 0x001141AF
		public SliderEntry(OptionsScreenListEntry.OptionsSliderNameData _data, int _order) : base(_data, _order)
		{
		}
	}

	// Token: 0x02000B3B RID: 2875
	private class GenericEntry<T> : OptionsMenuUIController.Entry where T : OptionsScreenListEntry.OptionsNameData
	{
		// Token: 0x06003A58 RID: 14936 RVA: 0x00115D82 File Offset: 0x00114182
		public GenericEntry(T _data, int _order)
		{
			this.Data = _data;
			this.Order = _order;
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06003A59 RID: 14937 RVA: 0x00115D98 File Offset: 0x00114198
		public override OptionsScreenListEntry.OptionsNameData BaseData
		{
			get
			{
				return this.Data;
			}
		}

		// Token: 0x04002F3E RID: 12094
		public T Data;
	}

	// Token: 0x02000B3C RID: 2876
	private abstract class Entry
	{
		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06003A5B RID: 14939
		public abstract OptionsScreenListEntry.OptionsNameData BaseData { get; }

		// Token: 0x04002F3F RID: 12095
		public int Order = -1;
	}
}
