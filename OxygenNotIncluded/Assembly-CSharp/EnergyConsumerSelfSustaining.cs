using System;
using System.Diagnostics;
using KSerialization;

// Token: 0x0200078E RID: 1934
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
public class EnergyConsumerSelfSustaining : EnergyConsumer
{
	// Token: 0x1400001A RID: 26
	// (add) Token: 0x060035C5 RID: 13765 RVA: 0x00122D28 File Offset: 0x00120F28
	// (remove) Token: 0x060035C6 RID: 13766 RVA: 0x00122D60 File Offset: 0x00120F60
	public event System.Action OnConnectionChanged;

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x060035C7 RID: 13767 RVA: 0x00122D95 File Offset: 0x00120F95
	public override bool IsPowered
	{
		get
		{
			return this.isSustained || this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x060035C8 RID: 13768 RVA: 0x00122DAA File Offset: 0x00120FAA
	public bool IsExternallyPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x060035C9 RID: 13769 RVA: 0x00122DB5 File Offset: 0x00120FB5
	public void SetSustained(bool isSustained)
	{
		this.isSustained = isSustained;
	}

	// Token: 0x060035CA RID: 13770 RVA: 0x00122DC0 File Offset: 0x00120FC0
	public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		CircuitManager.ConnectionStatus connectionStatus = this.connectionStatus;
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.connectionStatus = CircuitManager.ConnectionStatus.NotConnected;
			break;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.connectionStatus == CircuitManager.ConnectionStatus.Powered && base.GetComponent<Battery>() == null)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Unpowered;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (this.connectionStatus != CircuitManager.ConnectionStatus.Powered)
			{
				this.connectionStatus = CircuitManager.ConnectionStatus.Powered;
			}
			break;
		}
		this.UpdatePoweredStatus();
		if (connectionStatus != this.connectionStatus && this.OnConnectionChanged != null)
		{
			this.OnConnectionChanged();
		}
	}

	// Token: 0x060035CB RID: 13771 RVA: 0x00122E43 File Offset: 0x00121043
	public void UpdatePoweredStatus()
	{
		this.operational.SetFlag(EnergyConsumer.PoweredFlag, this.IsPowered);
	}

	// Token: 0x040020DC RID: 8412
	private bool isSustained;

	// Token: 0x040020DD RID: 8413
	private CircuitManager.ConnectionStatus connectionStatus;
}
