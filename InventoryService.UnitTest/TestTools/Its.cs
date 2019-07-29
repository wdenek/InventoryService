using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace InventoryService.UnitTest.TestTools
{
    class Its
    {
        public static bool EquivalentTo<T>(T actual, T expected)
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
