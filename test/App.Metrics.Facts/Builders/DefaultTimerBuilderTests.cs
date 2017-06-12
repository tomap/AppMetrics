﻿// <copyright file="DefaultTimerBuilderTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using App.Metrics.Core.ReservoirSampling.Uniform;
using App.Metrics.Facts.Fixtures;
using App.Metrics.Histogram;
using App.Metrics.Meter;
using App.Metrics.ReservoirSampling;
using App.Metrics.Timer;
using FluentAssertions;
using Moq;
using Xunit;

namespace App.Metrics.Facts.Builders
{
    public class DefaultTimerBuilderTests : IClassFixture<MetricCoreTestFixture>
    {
        private readonly IBuildTimerMetrics _builder;
        private readonly MetricCoreTestFixture _fixture;

        public DefaultTimerBuilderTests(MetricCoreTestFixture fixture)
        {
            _fixture = fixture;
            _builder = _fixture.Builder.Timer;
        }

        [Fact]
        public void Can_build_with_histogram()
        {
            var histogram = new Mock<IHistogramMetric>();
            histogram.Setup(r => r.Update(It.IsAny<long>(), null));
            histogram.Setup(r => r.Reset());

            var timer = _builder.Build(histogram.Object, _fixture.Clock);

            timer.Should().NotBeNull();
        }

        [Fact]
        public void Can_build_with_histogram_and_meter()
        {
            var histogramMock = new Mock<IHistogramMetric>();
            var meterMock = new Mock<IMeterMetric>();
            histogramMock.Setup(r => r.Update(It.IsAny<long>(), null));
            histogramMock.Setup(r => r.Reset());
            meterMock.Setup(r => r.GetValue(false));

            var timer = _builder.Build(histogramMock.Object, meterMock.Object, _fixture.Clock);

            timer.Should().NotBeNull();
        }

        [Fact]
        public void Can_build_with_reservoir()
        {
            var reservoirMock = new Mock<IReservoir>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());

            var timer = _builder.Build(() => reservoirMock.Object, _fixture.Clock);

            timer.Should().NotBeNull();
        }

        [Fact]
        public void Can_build_with_reservoir_and_meter()
        {
            var reservoirMock = new Mock<IReservoir>();
            var meterMock = new Mock<IMeterMetric>();
            reservoirMock.Setup(r => r.Update(It.IsAny<long>()));
            reservoirMock.Setup(r => r.GetSnapshot()).Returns(() => new UniformSnapshot(100, 100.0, new long[100]));
            reservoirMock.Setup(r => r.Reset());
            meterMock.Setup(r => r.GetValue(false));

            var timer = _builder.Build(() => reservoirMock.Object, meterMock.Object, _fixture.Clock);

            timer.Should().NotBeNull();
        }
    }
}