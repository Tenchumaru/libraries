using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adrezdi.Windows.Tests
{
	[TestClass]
	public class StringCommandLineTests : TypedCommandLineTests<string>
	{
		private const string value = "value";

		[TestMethod]
		public void StringRequiredValuePropertyHasLongNameArgumentWithRequiredValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithRequiredValue(value);
		}

		[TestMethod]
		public void StringRequiredValuePropertyHasShortNameArgumentWithRequiredValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithRequiredValue(value);
		}

		[TestMethod]
		public void StringRequiredValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithEmptyValue();
		}

		[TestMethod]
		public void StringRequiredValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringRequiredValuePropertyHasLongNameArgumentWithNoValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringRequiredValuePropertyHasShortNameArgumentWithNoValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringRequiredValuePropertyHasNoMatchingArgument()
		{
			base.RequiredValuePropertyHasNoMatchingArgument();
		}

		[TestMethod]
		public void StringOptionalValuePropertyHasLongNameArgumentWithValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithValue(value);
		}

		[TestMethod]
		public void StringOptionalValuePropertyHasShortNameArgumentWithValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithValue(value);
		}

		[TestMethod]
		public void StringOptionalValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithEmptyValue();
		}

		[TestMethod]
		public void StringOptionalValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringOptionalValuePropertyHasLongNameArgumentWithNoValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void StringOptionalValuePropertyHasShortNameArgumentWithNoValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithNoValue();
		}

		[TestMethod]
		public void StringOptionalValuePropertyHasNoMatchingArgument()
		{
			base.OptionalValuePropertyHasNoMatchingArgument();
		}
	}
}
