using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adrezdi.Windows.Tests
{
	[TestClass]
	public class IntCommandLineTests : TypedCommandLineTests<int>
	{
		private const int value = 2;

		[TestMethod]
		public void IntRequiredValuePropertyHasLongNameArgumentWithRequiredValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithRequiredValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntRequiredValuePropertyHasLongNameArgumentWithInvalidValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithInvalidValue();
		}

		[TestMethod]
		public void IntRequiredValuePropertyHasShortNameArgumentWithRequiredValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithRequiredValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntRequiredValuePropertyHasShortNameArgumentWithInvalidValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithInvalidValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntRequiredValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntRequiredValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntRequiredValuePropertyHasLongNameArgumentWithNoValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntRequiredValuePropertyHasShortNameArgumentWithNoValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntRequiredValuePropertyHasNoMatchingArgument()
		{
			base.RequiredValuePropertyHasNoMatchingArgument();
		}

		[TestMethod]
		public void IntOptionalValuePropertyHasLongNameArgumentWithValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntOptionalValuePropertyHasLongNameArgumentWithInvalidValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithInvalidValue();
		}

		[TestMethod]
		public void IntOptionalValuePropertyHasShortNameArgumentWithValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntOptionalValuePropertyHasShortNameArgumentWithInvalidValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithInvalidValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntOptionalValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntOptionalValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntOptionalValuePropertyHasLongNameArgumentWithNoValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntOptionalValuePropertyHasShortNameArgumentWithNoValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithNoValue();
		}

		[TestMethod]
		public void IntOptionalValuePropertyHasNoMatchingArgument()
		{
			base.OptionalValuePropertyHasNoMatchingArgument();
		}

		[TestMethod]
		public void IntFlagValuePropertyHasLongNameArgumentWithValue()
		{
			base.FlagValuePropertyHasLongNameArgumentWithValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntFlagValuePropertyHasLongNameArgumentWithInvalidValue()
		{
			base.FlagValuePropertyHasLongNameArgumentWithInvalidValue();
		}

		[TestMethod]
		public void IntFlagValuePropertyHasShortNameArgumentWithValue()
		{
			base.FlagValuePropertyHasShortNameArgumentWithValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntFlagValuePropertyHasShortNameArgumentWithInvalidValue()
		{
			base.FlagValuePropertyHasShortNameArgumentWithInvalidValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntFlagValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			base.FlagValuePropertyHasLongNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntFlagValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			base.FlagValuePropertyHasShortNameArgumentWithEmptyValue();
		}

		[TestMethod]
		public void IntFlagValuePropertyHasLongNameArgumentWithNoValue()
		{
			base.FlagValuePropertyHasLongNameArgumentWithNoValue();
		}

		[TestMethod]
		public void IntFlagValuePropertyHasShortNameArgumentWithNoValue()
		{
			base.FlagValuePropertyHasShortNameArgumentWithNoValue();
		}

		[TestMethod]
		public void IntFlagValuePropertyHasNoMatchingArgument()
		{
			base.FlagValuePropertyHasNoMatchingArgument();
		}
	}
}
