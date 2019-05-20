﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UrlShortenr.Models.Exeptions;

namespace UrlShortenr.Filters
{
    public class ShortnrErrorFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError;
            var ex = filterContext.Exception;
            string viewName = "Error500";

            if (ex is ShortnrNotFoundException)
            {
                code = HttpStatusCode.NotFound;
                viewName = "Error404";
            }
            if (ex is ShortnrConflictException)
            {
                code = HttpStatusCode.Conflict;
                viewName = "Error409";
            }
            if (ex is ArgumentException)
            {
                code = HttpStatusCode.BadRequest;
                viewName = "Error400";
            }

            filterContext.Result = new ViewResult()
            {
                ViewName = viewName
            };

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = (int)code;
        }
    }
}
