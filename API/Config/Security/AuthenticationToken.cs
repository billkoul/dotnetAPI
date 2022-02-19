using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Config
{
    public class AuthenticationToken
    {
        private readonly RequestDelegate _next;
        private readonly List<string> _tokens;
        private readonly List<string> _trackTokens;
        private readonly List<string> _swaggerTokens;

        public AuthenticationToken(RequestDelegate next)
        {
            _tokens = new List<string>();
            _trackTokens = new List<string>();
            _swaggerTokens = new List<string>();

            try
            {
                var jAppSettings = JToken.Parse(
                    File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json"))
                );

                foreach (var token in jAppSettings["tokens"])
                    _tokens.Add(token["key"].ToString());

                foreach (var token in jAppSettings["trackTokens"])
                    _trackTokens.Add(token["key"].ToString());

                foreach (var token in jAppSettings["swaggerTokens"])
                    _swaggerTokens.Add(token["key"].ToString());
            }
            catch { }

            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string token = context.Request.Headers["Authorization"];
            
            if (context.Request.Path.Value.Contains("/api/Home"))
                await _next(context);


            //disable for autoPi for debug
            if (context.Request.Path.Value.ToLower().Contains("/api/acc") ||
                context.Request.Path.Value.ToLower().Contains("/api/ble") ||
                context.Request.Path.Value.ToLower().Contains("/api/position") ||
                context.Request.Path.Value.ToLower().Contains("/api/update") ||
                context.Request.Path.Value.ToLower().Contains("/api/version") ||
                context.Request.Path.Value.ToLower().Contains("/api/voltage"))
            {
                if (_trackTokens.Contains(token))
                    await _next(context);
            }

            else if (!context.Request.Path.Value.ToLower().Contains("/api/home") && context.Request.Method.ToLower() == "get")
            {
                if (_swaggerTokens.Contains(token))
                    await _next(context);
                else if (_tokens.Contains(token))
                    await _next(context);
            }

            else if(_tokens.Contains(token))
                await _next(context);

            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Access denied!");
                return;
            }
        }
    }
}
