﻿using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace API.Config
{
	public class ClientIpCheckActionFilter : ActionFilterAttribute
	{
		private readonly ILogger _logger;
		private readonly string _safelist;

		public ClientIpCheckActionFilter(
			string safelist,
			ILogger logger)
		{
			_safelist = safelist;
			_logger = logger;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
			_logger.LogDebug(
				"Remote IpAddress: {RemoteIp}", remoteIp);

			var ip = _safelist.Split(';');

			var badIp = true;
			foreach (var address in ip)
			{
				if (remoteIp.IsIPv4MappedToIPv6)
				{
					remoteIp = remoteIp.MapToIPv4();
				}
				var testIp = IPAddress.Parse(address);
				if (!testIp.Equals(remoteIp)) continue;
				badIp = false;
				break;
			}

			if (badIp)
			{
				_logger.LogWarning(
					"Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
				context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
				return;
			}

			base.OnActionExecuting(context);
		}
	}
}
