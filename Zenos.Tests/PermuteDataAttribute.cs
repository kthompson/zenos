using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using Xunit.Sdk;

namespace Zenos.Tests
{
    /// <summary>
    /// Provides a data source for a data theory, with the data coming from inline values.
    /// </summary>
    [DataDiscoverer("Zenos.Tests.PermuteDataDiscoverer", "Zenos.Tests")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class PermuteDataAttribute : DataAttribute
    {
        private readonly object[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineDataAttribute"/> class.
        /// </summary>
        /// <param name="data">The data values to pass to the theory.</param>
        public PermuteDataAttribute(params object[] data)
        {
            this.data = data;
        }

        /// <inheritdoc/>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // This is called by the WPA81 version as it does not have access to attribute ctor params

            return new[] {data};
        }
    }
}