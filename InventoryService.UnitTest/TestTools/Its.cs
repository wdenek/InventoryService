using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics;

namespace InventoryService.UnitTest.TestTools
{
    /// <summary>
    /// Contains helper methods that combine fuctionality of Moq and FluentAssertions
    /// to make it easier to work with expected input parameters in mocks.
    /// </summary>
    static class Its
    {
        /// <summary>
        /// Will create a <see cref="Mock"/> of <typeparamref name="T"/> where the Equals is overriden by a call to <see cref="AreEquivalent{T}(T, T)"/>.
        /// This will compare <paramref name="expected"/> with any passed object using <see cref="FluentAssertions"/>.
        /// </summary>
        /// <typeparam name="T">T of the expected object.</typeparam>
        /// <param name="expected">The expected object to use for comparison.</param>
        /// <returns><see cref="Mock"/> of <typeparamref name="T"/></returns>
        public static T AsExpectedObject<T>(this T expected)
            where T : class
        {
            return Its.EquivalentTo(expected);
        }
        
        /// <summary>
        /// Matches any value that is equivalent to <paramref name="expected"/>.
        /// </summary>
        /// <typeparam name="TValue">Type of the argument to check.</typeparam>
        /// <param name="expected">The expected object to match.</param>
        public static TValue EquivalentTo<TValue>(TValue expected)
            where TValue : class
        {
            return Match<TValue>.Create(
                actual => AreEquivalent(actual, expected),
                () => Its.EquivalentTo<TValue>(expected)
            );
        }

        private static bool AreEquivalent<T>(T actual, T expected)
        {
            try
            {
                actual.Should().BeEquivalentTo(expected);
                return true;
            }
            catch (AssertFailedException ex)
            {
                // Although catching an Exception to return false is a bit ugly
                // the great advantage is that we can log the error message of FluentAssertions.
                // This makes it easier to troubleshoot why a Mock was not called with the expected parameters.

                Trace.WriteLine($"Actual and expected of type {typeof(T)} are not equal. Details:");
                Trace.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
