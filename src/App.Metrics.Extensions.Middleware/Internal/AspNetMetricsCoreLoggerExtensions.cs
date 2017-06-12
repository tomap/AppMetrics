﻿// <copyright file="AspNetMetricsCoreLoggerExtensions.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Logging
    // ReSharper restore CheckNamespace
{
    [ExcludeFromCodeCoverage]
    internal static class AspNetMetricsCoreLoggerExtensions
    {
        public static void MiddlewareExecuted(this ILogger logger, Type middleware)
        {
            logger.LogTrace(AspNetMetricsEventIds.Middleware.MiddlewareExecutedId, $"Executed AspNet Metrics Middleware {middleware.FullName}");
        }

        public static void MiddlewareExecuting(this ILogger logger, Type middleware)
        {
            logger.LogTrace(AspNetMetricsEventIds.Middleware.MiddlewareExecutingId, $"Executing AspNet Metrics Middleware {middleware.FullName}");
        }

        private static class AspNetMetricsEventIds
        {
            public static class Middleware
            {
                public const int MiddlewareExecutedId = MiddlewareStart + 1;
                public const int MiddlewareExecutingId = MiddlewareStart + 2;
                private const int MiddlewareStart = 3000;
            }
        }
    }
}
