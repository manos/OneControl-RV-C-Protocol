using IDS.Portable.Common;
using OneControl.Direct.MyRvLink;

namespace OneControl.Direct.MyRvLinkTcpIp;

public interface IDirectMyRvLinkConnectionTcpIp : IEndPointConnectionTcpIp, IEndPointConnection, IDirectMyRvLinkConnection, IDirectConnection
{
	string DeviceSourceToken { get; }
}
