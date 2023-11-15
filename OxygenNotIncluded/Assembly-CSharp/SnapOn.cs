using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000506 RID: 1286
[AddComponentMenu("KMonoBehaviour/scripts/SnapOn")]
public class SnapOn : KMonoBehaviour
{
	// Token: 0x06001E3A RID: 7738 RVA: 0x000A1358 File Offset: 0x0009F558
	protected override void OnPrefabInit()
	{
		this.kanimController = base.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x06001E3B RID: 7739 RVA: 0x000A1368 File Offset: 0x0009F568
	protected override void OnSpawn()
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.automatic)
			{
				this.DoAttachSnapOn(snapPoint);
			}
		}
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x000A13C4 File Offset: 0x0009F5C4
	public void AttachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					this.DoAttachSnapOn(snapPoint);
				}
			}
		}
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x000A1458 File Offset: 0x0009F658
	public void DetachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					base.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(snapPoint.overrideSymbol, 5);
					this.kanimController.SetSymbolVisiblity(snapPoint.overrideSymbol, false);
					break;
				}
			}
		}
	}

	// Token: 0x06001E3E RID: 7742 RVA: 0x000A1510 File Offset: 0x0009F710
	private void DoAttachSnapOn(SnapOn.SnapPoint point)
	{
		SnapOn.OverrideEntry overrideEntry = null;
		KAnimFile buildFile = point.buildFile;
		string symbol_name = "";
		if (this.overrideMap.TryGetValue(point.pointName, out overrideEntry))
		{
			buildFile = overrideEntry.buildFile;
			symbol_name = overrideEntry.symbolName;
		}
		KAnim.Build.Symbol symbol = SnapOn.GetSymbol(buildFile, symbol_name);
		base.GetComponent<SymbolOverrideController>().AddSymbolOverride(point.overrideSymbol, symbol, 5);
		this.kanimController.SetSymbolVisiblity(point.overrideSymbol, true);
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x000A1584 File Offset: 0x0009F784
	private static KAnim.Build.Symbol GetSymbol(KAnimFile anim_file, string symbol_name)
	{
		KAnim.Build.Symbol result = anim_file.GetData().build.symbols[0];
		KAnimHashedString y = new KAnimHashedString(symbol_name);
		foreach (KAnim.Build.Symbol symbol in anim_file.GetData().build.symbols)
		{
			if (symbol.hash == y)
			{
				result = symbol;
				break;
			}
		}
		return result;
	}

	// Token: 0x06001E40 RID: 7744 RVA: 0x000A15E5 File Offset: 0x0009F7E5
	public void AddOverride(string point_name, KAnimFile build_override, string symbol_name)
	{
		this.overrideMap[point_name] = new SnapOn.OverrideEntry
		{
			buildFile = build_override,
			symbolName = symbol_name
		};
	}

	// Token: 0x06001E41 RID: 7745 RVA: 0x000A1606 File Offset: 0x0009F806
	public void RemoveOverride(string point_name)
	{
		this.overrideMap.Remove(point_name);
	}

	// Token: 0x040010F3 RID: 4339
	private KAnimControllerBase kanimController;

	// Token: 0x040010F4 RID: 4340
	public List<SnapOn.SnapPoint> snapPoints = new List<SnapOn.SnapPoint>();

	// Token: 0x040010F5 RID: 4341
	private Dictionary<string, SnapOn.OverrideEntry> overrideMap = new Dictionary<string, SnapOn.OverrideEntry>();

	// Token: 0x020011AD RID: 4525
	[Serializable]
	public class SnapPoint
	{
		// Token: 0x04005D2F RID: 23855
		public string pointName;

		// Token: 0x04005D30 RID: 23856
		public bool automatic = true;

		// Token: 0x04005D31 RID: 23857
		public HashedString context;

		// Token: 0x04005D32 RID: 23858
		public KAnimFile buildFile;

		// Token: 0x04005D33 RID: 23859
		public HashedString overrideSymbol;
	}

	// Token: 0x020011AE RID: 4526
	public class OverrideEntry
	{
		// Token: 0x04005D34 RID: 23860
		public KAnimFile buildFile;

		// Token: 0x04005D35 RID: 23861
		public string symbolName;
	}
}
