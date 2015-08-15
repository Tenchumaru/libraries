using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adrezdi.Windows.Tests
{
	public abstract class TypedCommandLineTests<T>
	{
		protected void RequiredValuePropertyHasLongNameArgumentWithRequiredValue(T value)
		{
			// Arrange
			var args = new string[] { "--required:" + value };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(value, result.RequiredValue);
		}

		protected void RequiredValuePropertyHasLongNameArgumentWithInvalidValue()
		{
			// Arrange
			var args = new string[] { "--required:value" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void RequiredValuePropertyHasShortNameArgumentWithRequiredValue(T value)
		{
			// Arrange
			var args = new string[] { "-r:" + value };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(value, result.RequiredValue);
		}

		protected void RequiredValuePropertyHasShortNameArgumentWithInvalidValue()
		{
			// Arrange
			var args = new string[] { "--r:value" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void RequiredValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "--required:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual("", result.RequiredValue);
		}

		protected void RequiredValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "-r:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual("", result.RequiredValue);
		}

		protected void RequiredValuePropertyHasLongNameArgumentWithNoValue()
		{
			// Arrange
			var args = new string[] { "--required" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void RequiredValuePropertyHasShortNameArgumentWithNoValue()
		{
			// Arrange
			var args = new string[] { "-r" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void RequiredValuePropertyHasNoMatchingArgument()
		{
			// Arrange
			var args = new string[] { "-x" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void OptionalValuePropertyHasLongNameArgumentWithValue(T value)
		{
			// Arrange
			var args = new string[] { "--optional:" + value };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(value, result.OptionalValue);
		}

		protected void OptionalValuePropertyHasLongNameArgumentWithInvalidValue()
		{
			// Arrange
			var args = new string[] { "--optional:value" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void OptionalValuePropertyHasShortNameArgumentWithValue(T value)
		{
			// Arrange
			var args = new string[] { "-o:" + value };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(value, result.OptionalValue);
		}

		protected void OptionalValuePropertyHasShortNameArgumentWithInvalidValue()
		{
			// Arrange
			var args = new string[] { "-o:value" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void OptionalValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "--optional:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual("", result.OptionalValue);
		}

		protected void OptionalValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "-o:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual("", result.OptionalValue);
		}

		protected void OptionalValuePropertyHasLongNameArgumentWithNoValue()
		{
			// Arrange
			var args = new string[] { "--optional" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void OptionalValuePropertyHasShortNameArgumentWithNoValue()
		{
			// Arrange
			var args = new string[] { "-o" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void OptionalValuePropertyHasNoMatchingArgument()
		{
			// Arrange
			var args = new string[] { "-x" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(default(T), result.OptionalValue);
			var extraOptions = x.ExtraOptions.ToList();
			Assert.AreEqual(1, extraOptions.Count);
			Assert.AreEqual(args[0], extraOptions[0]);
		}

		protected void FlagValuePropertyHasLongNameArgumentWithValue(T value)
		{
			// Arrange
			var args = new string[] { "--flag:" + value };
			var x = new CommandLine();

			// Act
			var result = x.Parse<FlagValueCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(value, result.FlagValue);
		}

		protected void FlagValuePropertyHasLongNameArgumentWithInvalidValue()
		{
			// Arrange
			var args = new string[] { "--flag:value" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<FlagValueCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void FlagValuePropertyHasShortNameArgumentWithValue(T value)
		{
			// Arrange
			var args = new string[] { "-f:" + value };
			var x = new CommandLine();

			// Act
			var result = x.Parse<FlagValueCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(value, result.FlagValue);
		}

		protected void FlagValuePropertyHasShortNameArgumentWithInvalidValue()
		{
			// Arrange
			var args = new string[] { "-f:value" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<FlagValueCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.Fail();
		}

		protected void FlagValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "--flag:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<FlagValueCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual("", result.FlagValue);
		}

		protected void FlagValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "-f:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<FlagValueCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual("", result.FlagValue);
		}

		protected void FlagValuePropertyHasLongNameArgumentWithNoValue()
		{
			// Arrange
			var args = new string[] { "--flag" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<FlagValueCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(default(T), result.FlagValue);
			Assert.IsTrue(result.WasSet);
		}

		protected void FlagValuePropertyHasShortNameArgumentWithNoValue()
		{
			// Arrange
			var args = new string[] { "-f" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<FlagValueCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(default(T), result.FlagValue);
			Assert.IsTrue(result.WasSet);
		}

		protected void FlagValuePropertyHasNoMatchingArgument()
		{
			// Arrange
			var args = new string[] { "-x" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<FlagValueCommandLineTest<T>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(default(T), result.FlagValue);
			Assert.IsFalse(result.WasSet);
			var extraOptions = x.ExtraOptions.ToList();
			Assert.AreEqual(1, extraOptions.Count);
			Assert.AreEqual(args[0], extraOptions[0]);
		}
	}

	public class RequiredCommandLineTest<T>
	{
		[CommandLine.RequiredValueArgument(LongName = "required", ShortName = 'r')]
		public T RequiredValue { get; set; }
	}

	public class OptionalCommandLineTest<T>
	{
		[CommandLine.OptionalValueArgument(LongName = "optional", ShortName = 'o')]
		public T OptionalValue { get; set; }
	}

	public class FlagValueCommandLineTest<T>
	{
		internal bool WasSet;
		[CommandLine.FlagValueArgument(LongName = "flag", ShortName = 'f')]
		public T FlagValue { get { return flagValue; } set { flagValue = value; WasSet = true; } }
		private T flagValue;
	}
}
