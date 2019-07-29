using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics;

namespace InventoryService.UnitTest.TestTools
{
    static class Its
    {
        /// <summary>
        /// Will create a <see cref="Mock"/> of <typeparamref name="T"/> where the Equals is overriden by a call to <see cref="AreEquivalent{T}(T, T)"/>.
        /// This will compare <paramref name="expected"/> with any passed object using <see cref="FluentAssertions"/>.
        /// </summary>
        /// <typeparam name="T">T of the expected object.</typeparam>
        /// <param name="expected">The expected object to use for comparison.</param>
        /// <returns><see cref="Mock"/> of <typeparamref name="T"/></returns>
        public static T EquivalentTo<T>(this T expected)
            where T : class
        {
            var mockOfExpected = new Mock<T>();

            mockOfExpected.Setup(m => m.Equals(It.IsAny<object>()))
                .Returns((object obj) => AreEquivalent(obj, expected));

            return mockOfExpected.Object;
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
                Trace.WriteLine($"Actual and expected of type {typeof(T)} are not equal. Details:");
                Trace.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
