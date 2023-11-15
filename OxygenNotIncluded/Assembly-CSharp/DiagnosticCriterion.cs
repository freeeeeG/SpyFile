using System;

// Token: 0x02000757 RID: 1879
public class DiagnosticCriterion
{
	// Token: 0x170003BC RID: 956
	// (get) Token: 0x0600341B RID: 13339 RVA: 0x0011719B File Offset: 0x0011539B
	// (set) Token: 0x0600341C RID: 13340 RVA: 0x001171A3 File Offset: 0x001153A3
	public string id { get; private set; }

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x0600341D RID: 13341 RVA: 0x001171AC File Offset: 0x001153AC
	// (set) Token: 0x0600341E RID: 13342 RVA: 0x001171B4 File Offset: 0x001153B4
	public string name { get; private set; }

	// Token: 0x0600341F RID: 13343 RVA: 0x001171BD File Offset: 0x001153BD
	public DiagnosticCriterion(string name, Func<ColonyDiagnostic.DiagnosticResult> action)
	{
		this.name = name;
		this.evaluateAction = action;
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x001171D3 File Offset: 0x001153D3
	public void SetID(string id)
	{
		this.id = id;
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x001171DC File Offset: 0x001153DC
	public ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return this.evaluateAction();
	}

	// Token: 0x04001FA2 RID: 8098
	private Func<ColonyDiagnostic.DiagnosticResult> evaluateAction;
}
