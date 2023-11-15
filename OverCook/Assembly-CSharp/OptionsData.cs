using System;
using System.Collections.Generic;

// Token: 0x02000A2B RID: 2603
[Serializable]
public class OptionsData
{
	// Token: 0x0600338F RID: 13199 RVA: 0x000F2A80 File Offset: 0x000F0E80
	public OptionsData()
	{
		this.m_options = new IOption[11];
		this.m_options[0] = new Resolution();
		this.m_options[2] = new VSync();
		this.m_options[1] = new Windowed();
		this.m_options[3] = new Quality();
		this.m_options[4] = new MusicVolume();
		this.m_options[5] = new SFXVolume();
		this.m_options[6] = new SafeAreaX();
		this.m_options[7] = new SafeAreaY();
		this.m_options[8] = new HasSetSafeArea();
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x000F2B18 File Offset: 0x000F0F18
	public void OnAwake()
	{
		for (int i = 0; i < this.m_options.Length; i++)
		{
			if (this.m_options[i] != null)
			{
				OptionsData.IInit init = this.m_options[i] as OptionsData.IInit;
				if (init != null)
				{
					init.Init();
				}
			}
		}
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x000F2B6C File Offset: 0x000F0F6C
	public void Unload()
	{
		for (int i = 0; i < this.m_options.Length; i++)
		{
			if (this.m_options[i] != null)
			{
				OptionsData.IUnloadable unloadable = this.m_options[i] as OptionsData.IUnloadable;
				if (unloadable != null)
				{
					unloadable.Unload();
				}
			}
		}
	}

	// Token: 0x06003392 RID: 13202 RVA: 0x000F2BC0 File Offset: 0x000F0FC0
	public void Update()
	{
		for (int i = 0; i < this.m_options.Length; i++)
		{
			if (this.m_options[i] != null)
			{
				OptionsData.IUpdatable updatable = this.m_options[i] as OptionsData.IUpdatable;
				if (updatable != null)
				{
					updatable.Update();
				}
			}
		}
	}

	// Token: 0x06003393 RID: 13203 RVA: 0x000F2C14 File Offset: 0x000F1014
	public void AddToSave()
	{
		for (int i = 0; i < this.m_options.Length; i++)
		{
			if (this.m_options[i] != null)
			{
				this.m_options[i].SetOption(this.m_options[i].GetOption());
				this.m_options[i].Commit();
				Prefs.SetInt(this.m_options[i].Label, this.m_options[i].GetOption());
			}
		}
		Prefs.Save();
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x000F2C98 File Offset: 0x000F1098
	public void LoadFromSave()
	{
		for (int i = 0; i < this.m_options.Length; i++)
		{
			if (this.m_options[i] != null)
			{
				Prefs.Setup(this.m_options[i].Label, 0);
				if (Prefs.HasKey(this.m_options[i].Label))
				{
					int @int = Prefs.GetInt(this.m_options[i].Label, 0);
					this.m_options[i].SetOption(@int);
					this.m_options[i].Commit();
				}
			}
		}
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x000F2D2C File Offset: 0x000F112C
	public bool AnyChangesToCommit()
	{
		for (int i = 0; i < this.m_options.Length; i++)
		{
			if (this.m_options[i] != null)
			{
				int option = this.m_options[i].GetOption();
				int @int = Prefs.GetInt(this.m_options[i].Label, 0);
				if (option != @int)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003396 RID: 13206 RVA: 0x000F2D90 File Offset: 0x000F1190
	public IOption GetOption(OptionsData.OptionType _type)
	{
		return this.m_options[(int)_type];
	}

	// Token: 0x06003397 RID: 13207 RVA: 0x000F2D9C File Offset: 0x000F119C
	public int GetOptionsInCategory(OptionsData.Categories _categories)
	{
		int num = 0;
		foreach (IOption option in this.IterateOverCategory(_categories))
		{
			num++;
		}
		return num;
	}

	// Token: 0x06003398 RID: 13208 RVA: 0x000F2DF8 File Offset: 0x000F11F8
	public IEnumerable<IOption> IterateOverCategory(OptionsData.Categories _category)
	{
		for (int i = 0; i < this.m_options.Length; i++)
		{
			if (this.m_options[i] != null)
			{
				if (this.m_options[i].Category == _category)
				{
					yield return this.m_options[i];
				}
			}
		}
		yield break;
	}

	// Token: 0x0400297F RID: 10623
	[AssignResource("SidedAmbiControlsMappingData", Editorbility.NonEditable)]
	public AmbiControlsMappingData m_sidedMappingData;

	// Token: 0x04002980 RID: 10624
	[AssignResource("UnsidedAmbiControlsMappingData", Editorbility.NonEditable)]
	public AmbiControlsMappingData m_unsidedMappingData;

	// Token: 0x04002981 RID: 10625
	private IOption[] m_options;

	// Token: 0x02000A2C RID: 2604
	public enum Categories
	{
		// Token: 0x04002983 RID: 10627
		Graphics,
		// Token: 0x04002984 RID: 10628
		Audio,
		// Token: 0x04002985 RID: 10629
		SafeArea,
		// Token: 0x04002986 RID: 10630
		Controls,
		// Token: 0x04002987 RID: 10631
		System
	}

	// Token: 0x02000A2D RID: 2605
	public interface IEnabled
	{
		// Token: 0x06003399 RID: 13209
		bool IsEnabled();
	}

	// Token: 0x02000A2E RID: 2606
	public interface IInit
	{
		// Token: 0x0600339A RID: 13210
		void Init();
	}

	// Token: 0x02000A2F RID: 2607
	public interface IUnloadable
	{
		// Token: 0x0600339B RID: 13211
		void Unload();
	}

	// Token: 0x02000A30 RID: 2608
	public interface IUpdatable
	{
		// Token: 0x0600339C RID: 13212
		void Update();
	}

	// Token: 0x02000A31 RID: 2609
	public enum OptionType
	{
		// Token: 0x04002989 RID: 10633
		Resolution,
		// Token: 0x0400298A RID: 10634
		Windowed,
		// Token: 0x0400298B RID: 10635
		VSync,
		// Token: 0x0400298C RID: 10636
		Quality,
		// Token: 0x0400298D RID: 10637
		MusicVolume,
		// Token: 0x0400298E RID: 10638
		SFXVolume,
		// Token: 0x0400298F RID: 10639
		SafeAreaX,
		// Token: 0x04002990 RID: 10640
		SafeAreaY,
		// Token: 0x04002991 RID: 10641
		HasSetSafeArea,
		// Token: 0x04002992 RID: 10642
		Count = 11
	}
}
