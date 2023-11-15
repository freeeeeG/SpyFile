using System;
using System.Collections.Generic;

// Token: 0x02000863 RID: 2147
public interface IGroupProber
{
	// Token: 0x06003ECD RID: 16077
	void Occupy(object prober, short serial_no, IEnumerable<int> cells);

	// Token: 0x06003ECE RID: 16078
	void SetValidSerialNos(object prober, short previous_serial_no, short serial_no);

	// Token: 0x06003ECF RID: 16079
	bool ReleaseProber(object prober);
}
