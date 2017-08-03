﻿// <copyright file="MetricsJsonOptionsSetup.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using Microsoft.Extensions.Options;

namespace App.Metrics.Formatters.Json.Internal
{
    /// <summary>
    ///     Sets up default ASCII options for <see cref="MetricsOptions"/>.
    /// </summary>
    public class MetricsJsonOptionsSetup : IConfigureOptions<MetricsOptions>
    {
        private readonly MetricsJsonOptions _jsonOptions;

        public MetricsJsonOptionsSetup(IOptions<MetricsJsonOptions> asciiOptions)
        {
            _jsonOptions = asciiOptions.Value ?? throw new ArgumentNullException(nameof(asciiOptions));
        }

        public void Configure(MetricsOptions options)
        {
            var formatter = new JsonOutputFormatter(_jsonOptions.SerializerSettings);
            var envFormatter = new JsonEnvOutputFormatter(_jsonOptions.SerializerSettings);

            if (options.DefaultEnvOutputFormatter == null)
            {
                options.DefaultEnvOutputFormatter = envFormatter;
            }

            options.OutputFormatters.Add(formatter);
            options.EnvOutputFormatters.Add(envFormatter);
        }
    }
}