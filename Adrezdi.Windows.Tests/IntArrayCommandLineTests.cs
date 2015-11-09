using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adrezdi.Windows.Tests
{
	[TestClass]
	public class IntArrayCommandLineTests : TypedArrayCommandLineTests<int>
	{
		private static readonly int[] value = new[] { 2, 3, 5 };

		[TestMethod]
		public void IntArrayRequiredValuePropertyHasLongNameArgumentWithRequiredValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithRequiredValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayRequiredValuePropertyHasLongNameArgumentWithInvalidValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithInvalidValue();
		}

		[TestMethod]
		public void IntArrayRequiredValuePropertyHasShortNameArgumentWithRequiredValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithRequiredValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayRequiredValuePropertyHasShortNameArgumentWithInvalidValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithInvalidValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayRequiredValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayRequiredValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayRequiredValuePropertyHasLongNameArgumentWithNoValue()
		{
			base.RequiredValuePropertyHasLongNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayRequiredValuePropertyHasShortNameArgumentWithNoValue()
		{
			base.RequiredValuePropertyHasShortNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayRequiredValuePropertyHasNoMatchingArgument()
		{
			base.RequiredValuePropertyHasNoMatchingArgument();
		}

		[TestMethod]
		public void IntArrayOptionalValuePropertyHasLongNameArgumentWithValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayOptionalValuePropertyHasLongNameArgumentWithInvalidValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithInvalidValue();
		}

		[TestMethod]
		public void IntArrayOptionalValuePropertyHasShortNameArgumentWithValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithValue(value);
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayOptionalValuePropertyHasShortNameArgumentWithInvalidValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithInvalidValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayOptionalValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayOptionalValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithEmptyValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayOptionalValuePropertyHasLongNameArgumentWithNoValue()
		{
			base.OptionalValuePropertyHasLongNameArgumentWithNoValue();
		}

		[TestMethod, ExpectedException(typeof(CommandLineException))]
		public void IntArrayOptionalValuePropertyHasShortNameArgumentWithNoValue()
		{
			base.OptionalValuePropertyHasShortNameArgumentWithNoValue();
		}

		[TestMethod]
		public void IntArrayOptionalValuePropertyHasNoMatchingArgument()
		{
			base.OptionalValuePropertyHasNoMatchingArgument();
		}
	}
}
