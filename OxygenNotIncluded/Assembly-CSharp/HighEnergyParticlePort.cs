using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020007F8 RID: 2040
public class HighEnergyParticlePort : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06003A2A RID: 14890 RVA: 0x001441EF File Offset: 0x001423EF
	public int GetHighEnergyParticleInputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleInputCell();
	}

	// Token: 0x06003A2B RID: 14891 RVA: 0x001441FC File Offset: 0x001423FC
	public int GetHighEnergyParticleOutputPortPosition()
	{
		return this.m_building.GetHighEnergyParticleOutputCell();
	}

	// Token: 0x06003A2C RID: 14892 RVA: 0x00144209 File Offset: 0x00142409
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003A2D RID: 14893 RVA: 0x00144211 File Offset: 0x00142411
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.HighEnergyParticlePorts.Add(this);
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x00144224 File Offset: 0x00142424
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.HighEnergyParticlePorts.Remove(this);
	}

	// Token: 0x06003A2F RID: 14895 RVA: 0x00144238 File Offset: 0x00142438
	public bool InputActive()
	{
		Operational component = base.GetComponent<Operational>();
		return this.particleInputEnabled && component != null && component.IsFunctional && (!this.requireOperational || component.IsOperational);
	}

	// Token: 0x06003A30 RID: 14896 RVA: 0x00144277 File Offset: 0x00142477
	public bool AllowCapture(HighEnergyParticle particle)
	{
		return this.onParticleCaptureAllowed == null || this.onParticleCaptureAllowed(particle);
	}

	// Token: 0x06003A31 RID: 14897 RVA: 0x0014428F File Offset: 0x0014248F
	public void Capture(HighEnergyParticle particle)
	{
		this.currentParticle = particle;
		if (this.onParticleCapture != null)
		{
			this.onParticleCapture(particle);
		}
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x001442AC File Offset: 0x001424AC
	public void Uncapture(HighEnergyParticle particle)
	{
		if (this.onParticleUncapture != null)
		{
			this.onParticleUncapture(particle);
		}
		this.currentParticle = null;
	}

	// Token: 0x06003A33 RID: 14899 RVA: 0x001442CC File Offset: 0x001424CC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.particleInputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_INPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_INPUT, Descriptor.DescriptorType.Requirement, false));
		}
		if (this.particleOutputEnabled)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.PARTICLE_PORT_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.PARTICLE_PORT_OUTPUT, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x040026B7 RID: 9911
	[MyCmpGet]
	private Building m_building;

	// Token: 0x040026B8 RID: 9912
	public HighEnergyParticlePort.OnParticleCapture onParticleCapture;

	// Token: 0x040026B9 RID: 9913
	public HighEnergyParticlePort.OnParticleCaptureAllowed onParticleCaptureAllowed;

	// Token: 0x040026BA RID: 9914
	public HighEnergyParticlePort.OnParticleCapture onParticleUncapture;

	// Token: 0x040026BB RID: 9915
	public HighEnergyParticle currentParticle;

	// Token: 0x040026BC RID: 9916
	public bool requireOperational = true;

	// Token: 0x040026BD RID: 9917
	public bool particleInputEnabled;

	// Token: 0x040026BE RID: 9918
	public bool particleOutputEnabled;

	// Token: 0x040026BF RID: 9919
	public CellOffset particleInputOffset;

	// Token: 0x040026C0 RID: 9920
	public CellOffset particleOutputOffset;

	// Token: 0x020015D8 RID: 5592
	// (Invoke) Token: 0x0600889E RID: 34974
	public delegate void OnParticleCapture(HighEnergyParticle particle);

	// Token: 0x020015D9 RID: 5593
	// (Invoke) Token: 0x060088A2 RID: 34978
	public delegate bool OnParticleCaptureAllowed(HighEnergyParticle particle);
}
