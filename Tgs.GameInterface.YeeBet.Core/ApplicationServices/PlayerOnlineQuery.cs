using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.GameInterface.YeeBet.Core.ApplicationServices;
public record PlayerOnlineQuery(
	Uri ApiUrl,
	string AppId,
	string SecretKey,
	string Username);
