namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string AppId,
	string SecretKey,
	string PlayerCode,
	long OrderId);
