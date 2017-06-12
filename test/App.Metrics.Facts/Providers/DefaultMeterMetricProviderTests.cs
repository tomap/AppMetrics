﻿// <copyright file="DefaultMeterMetricProviderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System.Linq;
using App.Metrics.Core;
using App.Metrics.Core.Filtering;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Filters;
using App.Metrics.Meter;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Providers
{
    public class DefaultMeterMetricProviderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IFilterMetrics _filter = new DefaultMetricsFilter().WhereType(MetricType.Meter);
        private readonly MetricCoreTestFixture _fixture;
        private readonly IProvideMeterMetrics _provide;

        public DefaultMeterMetricProviderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _provide = _fixture.Providers.Meter;
        }

        [Fact]
        public void Can_add_add_new_instance_to_registry()
        {
            var metricName = "meter_provider_metric_test";
            var options = new MeterOptions
                          {
                              Name = metricName
                          };

            var meterMetric = _fixture.Builder.Meter.Build(_fixture.Clock);

            _provide.Instance(options, () => meterMetric);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Meters.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_add_new_multidimensional_to_registry()
        {
            var metricName = "meter_provider_metric_test_multi";
            var options = new MeterOptions
                          {
                              Name = metricName
                          };

            var meterMetric = _fixture.Builder.Meter.Build(_fixture.Clock);

            _provide.Instance(options, _fixture.Tags[0], () => meterMetric);

            _filter.WhereMetricName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Meters.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_instance_to_registry()
        {
            var metricName = "meter_provider_test";
            var options = new MeterOptions
                          {
                              Name = metricName
                          };

            _provide.Instance(options);

            _filter.WhereMetricName(name => name == metricName);

            _fixture.Registry.GetData(_filter).Contexts.First().Meters.Count().Should().Be(1);
        }

        [Fact]
        public void Can_add_multidimensional_to_registry()
        {
            var metricName = "meter_provider_test_multi";
            var options = new MeterOptions
                          {
                              Name = metricName
                          };

            _provide.Instance(options, _fixture.Tags[0]);

            _filter.WhereMetricName(name => name == _fixture.Tags[0].AsMetricName(metricName));

            _fixture.Registry.GetData(_filter).Contexts.First().Meters.Count().Should().Be(1);
        }
    }
}