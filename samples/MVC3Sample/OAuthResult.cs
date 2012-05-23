using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OAuth2Provider.Response;

namespace MVC3Sample
{
    public class OAuthResult : ActionResult
    {
        public OAuthResult(OAuthResponse response)
        {
            OAuthResponse = response;
        }

        public OAuthResponse OAuthResponse { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ClearHeaders();

            foreach (var header in OAuthResponse.Headers)
                context.HttpContext.Response.AddHeader(header.Key, header.Value);

            context.HttpContext.Response.ContentType = OAuthResponse.ContentType;
            context.HttpContext.Response.ClearContent();

            var json = OAuthResponse.Body;
            string callback = context.HttpContext.Request.QueryString["callback"];
            if (!string.IsNullOrWhiteSpace(callback))
            {
                context.HttpContext.Response.Write(callback + "(" + json + ");");
                context.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                context.HttpContext.Response.Write(json);
                context.HttpContext.Response.StatusCode = OAuthResponse.StatusCode;
            }
        }
    }
}