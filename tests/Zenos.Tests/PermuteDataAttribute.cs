using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
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

    /// <summary>
    /// Implementation of <see cref="IDataDiscoverer"/> used to discover the data
    /// provided by <see cref="InlineDataAttribute"/>.
    /// </summary>
    public class PermuteDataDiscoverer : IDataDiscoverer
    {
        /// <inheritdoc/>
        public virtual IEnumerable<object[]> GetData(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            // The data from GetConstructorArguments does not maintain its original form (in particular, collections
            // end up as generic IEnumerable<T>). So we end up needing to call .ToArray() on the enumerable so that
            // we can restore the correct argument type from InlineDataAttribute.
            //
            // In addition, [InlineData(null)] gets translated into passing a null array, not a single array with a null
            // value in it, which is why the null coalesce operator is required (this is covered by the acceptance test
            // in Xunit2TheoryAcceptanceTests.InlineDataTests.SingleNullValuesWork).
            var count = testMethod.GetParameters().Count();

            var arguments = dataAttribute.GetConstructorArguments();
            var args = (IEnumerable<object>)arguments.Single() ?? new object[] { null };
            var objects = args.ToArray();
            var results = objects.Select(x => Enumerable.Empty<object>().Concat(new[] { x }));

            while (--count > 0)
            {
                results = results.Join(objects, o => true, o => true, (enumerable, item) => enumerable.Concat(new[] { item }).ToArray());
            }

            var array = results.Select(x => x.ToArray()).ToArray();
            return array;
        }

        public bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            return true;
        }
    }
}