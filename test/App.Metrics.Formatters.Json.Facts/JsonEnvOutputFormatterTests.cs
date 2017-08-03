// <copyright file="JsonEnvOutputFormatterTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.Formatters.Json.Facts.Helpers;
using App.Metrics.Formatters.Json.Facts.TestFixtures;
using App.Metrics.Infrastructure;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace App.Metrics.Formatters.Json.Facts
{
    public class JsonEnvOutputFormatterTests : IClassFixture<MetricProviderTestFixture>
    {
        private readonly EnvironmentInfo _env;
        private readonly ITestOutputHelper _output;
        private readonly JsonEnvOutputFormatter _formatter;

        public JsonEnvOutputFormatterTests(ITestOutputHelper output, MetricProviderTestFixture fixture)
        {
            _output = output;
            _formatter = new JsonEnvOutputFormatter();
            _env = fixture.Env;
        }

        [Fact]
        public async Task Produces_expected_json()
        {
            // Arrange
            JToken result;
            var expected = _env.SampleJson();

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, _env, Encoding.UTF8);

                result = Encoding.UTF8.GetString(stream.ToArray()).ParseAsJson();
            }

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public async Task Produces_valid_Json()
        {
            // Arrange
            string result;

            // Act
            using (var stream = new MemoryStream())
            {
                await _formatter.WriteAsync(stream, _env, Encoding.UTF8);

                result = Encoding.UTF8.GetString(stream.ToArray());
            }

            _output.WriteLine("Json Env Info: {0}", result);

            // Assert
            Action action = () => JToken.Parse(result);
            action.ShouldNotThrow<Exception>();
        }
    }
}