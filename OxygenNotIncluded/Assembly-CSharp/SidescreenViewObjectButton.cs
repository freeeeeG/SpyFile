using System;
using UnityEngine;

// Token: 0x02000330 RID: 816
public class SidescreenViewObjectButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x06001092 RID: 4242 RVA: 0x00059BB8 File Offset: 0x00057DB8
	public bool IsValid()
	{
		SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
		if (trackMode != SidescreenViewObjectButton.Mode.Target)
		{
			return trackMode == SidescreenViewObjectButton.Mode.Cell && Grid.IsValidCell(this.TargetCell);
		}
		return this.Target != null;
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06001093 RID: 4243 RVA: 0x00059BED File Offset: 0x00057DED
	public string SidescreenButtonText
	{
		get
		{
			return this.Text;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06001094 RID: 4244 RVA: 0x00059BF5 File Offset: 0x00057DF5
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.Tooltip;
		}
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x00059BFD File Offset: 0x00057DFD
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x00059C04 File Offset: 0x00057E04
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x00059C07 File Offset: 0x00057E07
	public bool SidescreenButtonInteractable()
	{
		return this.IsValid();
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x00059C0F File Offset: 0x00057E0F
	public int HorizontalGroupID()
	{
		return this.horizontalGroupID;
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x00059C18 File Offset: 0x00057E18
	public void OnSidescreenButtonPressed()
	{
		if (this.IsValid())
		{
			SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
			if (trackMode == SidescreenViewObjectButton.Mode.Target)
			{
				CameraController.Instance.CameraGoTo(this.Target.transform.GetPosition(), 2f, true);
				return;
			}
			if (trackMode == SidescreenViewObjectButton.Mode.Cell)
			{
				CameraController.Instance.CameraGoTo(Grid.CellToPos(this.TargetCell), 2f, true);
				return;
			}
		}
		else
		{
			base.gameObject.Trigger(1980521255, null);
		}
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x00059C89 File Offset: 0x00057E89
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000915 RID: 2325
	public string Text;

	// Token: 0x04000916 RID: 2326
	public string Tooltip;

	// Token: 0x04000917 RID: 2327
	public SidescreenViewObjectButton.Mode TrackMode;

	// Token: 0x04000918 RID: 2328
	public GameObject Target;

	// Token: 0x04000919 RID: 2329
	public int TargetCell;

	// Token: 0x0400091A RID: 2330
	public int horizontalGroupID = -1;

	// Token: 0x02000F97 RID: 3991
	public enum Mode
	{
		// Token: 0x0400563F RID: 22079
		Target,
		// Token: 0x04005640 RID: 22080
		Cell
	}
}
