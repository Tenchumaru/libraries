using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adrezdi.Windows.Tests
{
	[TestClass]
	public class StringArrayCommandLineTests : TypedArrayCommandLineTests<string>
	{
		private static readonly string[] value = new[] { "one", "two", "three" };

		[TestMethod]
		public void StringArrayRequiredValuePropertyHasLongNameArgumentWithRequiredValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithRequiredValue(value);
		}

		[TestMethod]
		public void StringArrayRequiredValuePropertyHasShortNameArgumentWithRequiredValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithRequiredValue(value);
		}

		[TestMethod]
		public void StringArrayRequiredValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithEmptyValue();
		}

		[TestMethod]
		public void StringArrayRequiredValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringArrayRequiredValuePropertyHasLongNameArgumentWithNoValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringArrayRequiredValuePropertyHasShortNameArgumentWithNoValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringArrayRequiredValuePropertyHasNoMatchingArgument()
		{
			base.RequiredValuePropertyHasNoMatchingArgument();
		}

		[TestMethod]
		public void StringArrayOptionalValuePropertyHasLongNameArgumentWithValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithValue(value);
		}

		[TestMethod]
		public void StringArrayOptionalValuePropertyHasShortNameArgumentWithValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithValue(value);
		}

		[TestMethod]
		public void StringArrayOptionalValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithEmptyValue();
		}

		[TestMethod]
		public void StringArrayOptionalValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringArrayOptionalValuePropertyHasLongNameArgumentWithNoValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringArrayOptionalValuePropertyHasShortNameArgumentWithNoValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithNoValue();
		}

		[TestMethod]
		public void StringArrayOptionalValuePropertyHasNoMatchingArgument()
		{
			base.OptionalValuePropertyHasNoMatchingArgument();
		}
	}
}
