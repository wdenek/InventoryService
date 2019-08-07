using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics;

namespace InventoryService.UnitTest.TestTools
{
    /// <summary>
    /// Contains helper methods that combine fuctionality of Moq and FluentAssertions
    /// to make it easier to work with complex input parameters in mocks.
    /// </summary>
    static class Its
    {
        /// <summary>
        /// Matches any value that is equivalent to <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of the argument to check.</typeparam>
        /// <param name="expected">The expected object to match.</param>
        public static TValue AsExpectedObject<TValue>(this TValue expected)
        {
            return Its.EquivalentTo(expected);
        }

        /// <summary>
        /// Matches any value that is equivalent to <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of the argument to check.</typeparam>
        /// <param name="expected">The expected object to match.</param>
        public static TValue EquivalentTo<TValue>(TValue expected)
        {
            return Match.Create(
                actual => AreEquivalent(actual, expected, options => options),
                () => Its.EquivalentTo<TValue>(expected)
            );
        }

        /// <summary>
        /// Matches any value that is equivalent to <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of the argument to check.</typeparam>
        /// <param name="expected">The expected object to match.</param>
        public static TValue EquivalentTo<TValue>(TValue expected,
            Func<EquivalencyAssertionOptions<TValue>, EquivalencyAssertionOptions<TValue>> config)
        {
            return Match.Create(
                actual => AreEquivalent(actual, expected, config),
                () => Its.EquivalentTo<TValue>(expected)
            );
        }

        private static bool AreEquivalent<TValue>(TValue actual, TValue expected,
            Func<EquivalencyAssertionOptions<TValue>, EquivalencyAssertionOptions<TValue>> config)
        {
            try
            {
                actual.Should().BeEquivalentTo(expected, config);
                return true;
            }
            catch (AssertFailedException ex)
            {
                // Although catching an Exception to return false is a bit ugly
                // the great advantage is that we can log the error message of FluentAssertions.
                // This makes it easier to troubleshoot why a Mock was not called with the expected parameters.

                Trace.WriteLine($"Actual and expected of type {typeof(TValue)} are not equal. Details:");
                Trace.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
