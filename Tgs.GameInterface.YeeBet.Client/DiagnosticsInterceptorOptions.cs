using System.Diagnostics;

namespace Tgs.GameInterface.YeeBet.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.YeeBet.Client.DiagnosticsInterceptor");
}
