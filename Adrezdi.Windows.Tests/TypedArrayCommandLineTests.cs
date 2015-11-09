using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adrezdi.Windows.Tests
{
	public abstract class TypedArrayCommandLineTests<T> : TypedCommandLineTests<T[]>
	{
		protected new void RequiredValuePropertyHasLongNameArgumentWithRequiredValue(T[] value)
		{
			// Arrange
			var args = new string[] { "--required:" + string.Join(",", value) };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T[]>>(args, automatingUsage: false);

			// Assert
			Assert.IsTrue(Enumerable.SequenceEqual(value, result.RequiredValue));
		}

		protected new void RequiredValuePropertyHasShortNameArgumentWithRequiredValue(T[] value)
		{
			// Arrange
			var args = new string[] { "-r:" + string.Join(",", value) };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T[]>>(args, automatingUsage: false);

			// Assert
			Assert.IsTrue(Enumerable.SequenceEqual(value, result.RequiredValue));
		}

		protected new void RequiredValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "--required:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T[]>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(typeof(T) == typeof(string) ? 1 : 0, result.RequiredValue.Length);
		}

		protected new void RequiredValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "-r:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<RequiredCommandLineTest<T[]>>(args, automatingUsage: false);

			// Assert
			Assert.AreEqual(typeof(T) == typeof(string) ? 1 : 0, result.RequiredValue.Length);
		}

		protected new void OptionalValuePropertyHasLongNameArgumentWithValue(T[] value)
		{
			// Arrange
			var args = new string[] { "--optional:" + string.Join(",", value) };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T[]>>(args, automatingUsage: false);

			// Assert
			Assert.IsTrue(Enumerable.SequenceEqual(value, result.OptionalValue));
		}

		protected new void OptionalValuePropertyHasShortNameArgumentWithValue(T[] value)
		{
			// Arrange
			var args = new string[] { "-o:" + string.Join(",", value) };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T[]>>(args, automatingUsage: false);

			// Assert
			Assert.IsTrue(Enumerable.SequenceEqual(value, result.OptionalValue));
		}

		protected new void OptionalValuePropertyHasLongNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "--optional:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T[]>>(args, automatingUsage: false);

			// Assert
			if(typeof(T) == typeof(string))
			{
				Assert.AreEqual(1, result.OptionalValue.Length);
				Assert.AreEqual("", result.OptionalValue[0].ToString());
			}
			else
				Assert.Fail();
		}

		protected new void OptionalValuePropertyHasShortNameArgumentWithEmptyValue()
		{
			// Arrange
			var args = new string[] { "-o:" };
			var x = new CommandLine();

			// Act
			var result = x.Parse<OptionalCommandLineTest<T[]>>(args, automatingUsage: false);

			// Assert
			if(typeof(T) == typeof(string))
			{
				Assert.AreEqual(1, result.OptionalValue.Length);
				Assert.AreEqual("", result.OptionalValue[0].ToString());
			}
			else
				Assert.Fail();
		}
	}
}
