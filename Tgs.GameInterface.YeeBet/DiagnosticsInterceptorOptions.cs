using System.Diagnostics;

namespace Tgs.GameInterface.YeeBet;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.YeeBet.DiagnosticsInterceptor");
}
