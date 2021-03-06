﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adrezdi.Windows.Tests
{
    [TestClass]
    public class CommandLineTests
    {
        [TestMethod, ExpectedException(typeof(CommandLineException))]
        public void FlagPropertyHasLongNameArgumentWithValue()
        {
            // Arrange
            var args = new string[] { "--flag=value" };
            var x = new CommandLine();

            // Act
            var result = x.Parse<FlagCommandLineTest>(args, automatingUsage: false);

            // Assert
        }

        [TestMethod]
        public void FlagPropertyHasShortNameArgumentWithValue()
        {
            // Arrange
            var args = new string[] { "-f", "value" };
            var x = new CommandLine();

            // Act
            var result = x.Parse<FlagCommandLineTest>(args, automatingUsage: false);

            // Assert
            Assert.IsTrue(result.FlagValue);
        }

        [TestMethod, ExpectedException(typeof(CommandLineException))]
        public void FlagPropertyHasLongNameArgumentWithEmptyValue()
        {
            // Arrange
            var args = new string[] { "--flag=" };
            var x = new CommandLine();

            // Act
            var result = x.Parse<FlagCommandLineTest>(args, automatingUsage: false);

            // Assert
        }

        [TestMethod]
        public void FlagPropertyHasShortNameArgumentWithEmptyValue()
        {
            // Arrange
            var args = new string[] { "-f", "" };
            var x = new CommandLine();

            // Act
            var result = x.Parse<FlagCommandLineTest>(args, automatingUsage: false);

            // Assert
            Assert.IsTrue(result.FlagValue);
        }

        [TestMethod]
        public void FlagPropertyHasLongNameArgumentWithNoValue()
        {
            // Arrange
            var args = new string[] { "--flag" };
            var x = new CommandLine();

            // Act
            var result = x.Parse<FlagCommandLineTest>(args, automatingUsage: false);

            // Assert
            Assert.IsTrue(result.FlagValue);
        }

        [TestMethod]
        public void FlagPropertyHasShortNameArgumentWithNoValue()
        {
            // Arrange
            var args = new string[] { "-f" };
            var x = new CommandLine();

            // Act
            var result = x.Parse<FlagCommandLineTest>(args, automatingUsage: false);

            // Assert
            Assert.IsTrue(result.FlagValue);
        }

        [TestMethod]
        public void FlagPropertyHasNoMatchingArgument()
        {
            // Arrange
            var args = new string[] { "-x" };
            var x = new CommandLine();

            // Act
            var result = x.Parse<FlagCommandLineTest>(args, automatingUsage: false);

            // Assert
            Assert.IsFalse(result.FlagValue);
            var extraOptions = x.ExtraOptions.ToList();
            Assert.AreEqual(1, extraOptions.Count);
            Assert.AreEqual(args[0], extraOptions[0]);
        }

        [TestMethod]
        public void CapturesThreeExtraArguments()
        {
            // Arrange
            var args = new string[] { "one", "two", "three" };
            var x = new CommandLine();

            // Act
            var result = x.Parse<object>(args, automatingUsage: false);

            // Assert
            Assert.IsInstanceOfType(result, typeof(object));
            var extraArguments = x.ExtraArguments.ToList();
            Assert.AreEqual(args.Length, extraArguments.Count);
            for(int i = 0; i < args.Length; ++i)
                Assert.AreEqual(args[i], extraArguments[i]);
        }

        [TestMethod]
        public void Usage()
        {
            // Arrange
            var programName = System.IO.Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            string expected = @"usage:  {0} [-f] -r value [-o value] one two three

-f,--flag        this is a flag
-r,--required    this is required
-o,--optional    this is optional
";
            expected = string.Format(expected, programName);
            var args = new string[] { "one", "two", "three" };
            var x = new CommandLine();

            // Act
            string actual = x.Usage<UsageCommandLineTest>(args);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UsageWithProlog()
        {
            // Arrange
            var programName = System.IO.Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            string expected = @"prolog

usage:  {0} [-f] -r value [-o value] one two three

-f,--flag        this is a flag
-r,--required    this is required
-o,--optional    this is optional
";
            expected = string.Format(expected, programName);
            var args = new string[] { "one", "two", "three" };
            var x = new CommandLine();

            // Act
            string actual = x.Usage<PrologUsageCommandLineTest>(args);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UsageWithEpilog()
        {
            // Arrange
            var programName = System.IO.Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            string expected = @"usage:  {0} [-f] -r value [-o value] one two three

-f,--flag        this is a flag
-r,--required    this is required
-o,--optional    this is optional

epilog
";
            expected = string.Format(expected, programName);
            var args = new string[] { "one", "two", "three" };
            var x = new CommandLine();

            // Act
            string actual = x.Usage<EpilogUsageCommandLineTest>(args);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UsageWithBoth()
        {
            // Arrange
            var programName = System.IO.Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            string expected = @"prolog

usage:  {0} [-f] -r value [-o value] one two three

-f,--flag        this is a flag
-r,--required    this is required
-o,--optional    this is optional

epilog
";
            expected = string.Format(expected, programName);
            var args = new string[] { "one", "two", "three" };
            var x = new CommandLine();

            // Act
            string actual = x.Usage<BothUsageCommandLineTest>(args);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }

    public class FlagCommandLineTest
    {
        [CommandLine.FlagArgument(LongName = "flag", ShortName = 'f')]
        public bool FlagValue { get; set; }
    }

    public class UsageCommandLineTest
    {
        [CommandLine.FlagArgument(LongName = "flag", ShortName = 'f', Usage = "this is a flag")]
        public bool Flag { get; set; }
        [CommandLine.RequiredValueArgument(LongName = "required", ShortName = 'r', Usage = "this is required")]
        public string RequiredValue { get; set; }
        [CommandLine.OptionalValueArgument(LongName = "optional", ShortName = 'o', Usage = "this is optional")]
        public string OptionalValue { get; set; }
    }

    [CommandLine.Usage(Prolog = "prolog")]
    public class PrologUsageCommandLineTest
    {
        [CommandLine.FlagArgument(LongName = "flag", ShortName = 'f', Usage = "this is a flag")]
        public bool Flag { get; set; }
        [CommandLine.RequiredValueArgument(LongName = "required", ShortName = 'r', Usage = "this is required")]
        public string RequiredValue { get; set; }
        [CommandLine.OptionalValueArgument(LongName = "optional", ShortName = 'o', Usage = "this is optional")]
        public string OptionalValue { get; set; }
    }

    [CommandLine.Usage(Epilog = "epilog")]
    public class EpilogUsageCommandLineTest
    {
        [CommandLine.FlagArgument(LongName = "flag", ShortName = 'f', Usage = "this is a flag")]
        public bool Flag { get; set; }
        [CommandLine.RequiredValueArgument(LongName = "required", ShortName = 'r', Usage = "this is required")]
        public string RequiredValue { get; set; }
        [CommandLine.OptionalValueArgument(LongName = "optional", ShortName = 'o', Usage = "this is optional")]
        public string OptionalValue { get; set; }
    }

    [CommandLine.Usage("prolog", "epilog")]
    public class BothUsageCommandLineTest
    {
        [CommandLine.FlagArgument(LongName = "flag", ShortName = 'f', Usage = "this is a flag")]
        public bool Flag { get; set; }
        [CommandLine.RequiredValueArgument(LongName = "required", ShortName = 'r', Usage = "this is required")]
        public string RequiredValue { get; set; }
        [CommandLine.OptionalValueArgument(LongName = "optional", ShortName = 'o', Usage = "this is optional")]
        public string OptionalValue { get; set; }
    }
}
