using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adrezdi
{
	public class CommandLine
	{
		[AttributeUsage(AttributeTargets.Property)]
		public abstract class ArgumentAttribute : Attribute
		{
			public char ShortName { get; set; }
			public string LongName { get; set; }
			public string Usage { get; set; }

			public bool Matches(string name)
			{
				if(name.Length < 2 || "-/".IndexOf(name[0]) < 0)
					return false;
				if(name.Length > 2)
				{
					if("-/".IndexOf(name[1]) >= 0)
						return string.Equals(name.Substring(2), LongName, StringComparison.InvariantCultureIgnoreCase);
					throw new ArgumentException("Multiple short options are not supported.");
				}
				return char.ToLowerInvariant(name[1]) == ShortName;
			}

			/// <summary>
			/// Checks if the arguments on the command line satisfy this attribute.
			/// </summary>
			/// <param name="args">The values of the argument or null if there are no arguments.</param>
			/// <returns></returns>
			internal abstract bool IsValid(IEnumerable<string> args);

			/// <summary>
			/// Gets the display value of how to use this argument on the
			/// command line.
			/// </summary>
			internal abstract string CommandLine { get; }
		}

		/// <summary>
		/// The option takes no value.  Its presence or absence determines its
		/// Boolean value.
		/// </summary>
		public class FlagArgumentAttribute : ArgumentAttribute
		{
			internal override bool IsValid(IEnumerable<string> args)
			{
				return args == null || args.First() == null;
			}

			internal override string CommandLine
			{
				get { return "[-" + ShortName + ']'; }
			}
		}

		/// <summary>
		/// The option need not appear on the command line and, if it does,
		/// need not have a value.
		/// </summary>
		public class FlagValueArgumentAttribute : ArgumentAttribute
		{
			internal override bool IsValid(IEnumerable<string> args)
			{
				return true;
			}

			internal override string CommandLine
			{
				get { return "[-" + ShortName + "[:value]]"; }
			}
		}

		/// <summary>
		/// The option need not appear on the command line but, if it does, it
		/// must have a value.
		/// </summary>
		public class OptionalValueArgumentAttribute : ArgumentAttribute
		{
			internal override bool IsValid(IEnumerable<string> args)
			{
				return args == null || args.First() != null;
			}

			internal override string CommandLine
			{
				get { return "[-" + ShortName + ":value]"; }
			}
		}

		/// <summary>
		/// The option must appear on the command line with a value.
		/// </summary>
		public class RequiredValueArgumentAttribute : ArgumentAttribute
		{
			internal override bool IsValid(IEnumerable<string> args)
			{
				return args != null && args.First() != null;
			}

			internal override string CommandLine
			{
				get { return "-" + ShortName + ":value"; }
			}
		}

		public IEnumerable<string> ExtraArguments { get { return extraArguments; } }
		private List<string> extraArguments;
		public IEnumerable<string> ExtraOptions { get { return extraOptions; } }
		private List<string> extraOptions;

		public T Parse<T>(string[] args, bool automatingUsage) where T : new()
		{
			var t = new T();

			// TODO:  support separating an option from its value.
			Func<int, bool> AcceptsNext = i => i + 1 < args.Length && args[i].StartsWith("-");
			Func<int, bool> IsRetained = i => i < 1 || !AcceptsNext(i - 1);
			var q = from i in args.Select((s, i) => i)
					let b = IsRetained(i)
					where b
					select AcceptsNext(i) ? string.Format("{0}:{1}", args[i], args[i + 1]) : args[i];
			q = q.ToList();

			// Compose all arguments in a convenient format.
			var cargs = from s in args
						let o = s.Length > 1 && "-/".IndexOf(s[0]) >= 0
						let p = s.Split(new[] { ':' }, 2)
						let n = o ? p[0] : null
						let v = o ? p.Length > 1 ? p[1] : null : s
						select new { IsOption = o, Name = n, Value = v, Argument = s };
			cargs = cargs.ToList();

			// Match properties with their arguments.
			var matchings = from p in typeof(T).GetProperties()
							from a in p.GetCustomAttributes(typeof(ArgumentAttribute), true).Cast<ArgumentAttribute>()
							let v = from c in cargs
									where c.IsOption && a.Matches(c.Name)
									select c
							select new { Property = p, Attribute = a, Arguments = v };
			matchings = matchings.ToList();

			// Extract those with invalid specifications.
			// TODO:  consider allowing multiple flag (and possibly other) options.
			var invalid = matchings.Where(i => i.Arguments.Skip(1).Any() || !i.Attribute.IsValid(i.Arguments.Any() ? i.Arguments.Select(a => a.Value) : null)).ToList();
			if(invalid.Count > 0)
			{
				if(automatingUsage)
				{
					Console.WriteLine(Usage<T>());
					Environment.Exit(2);
				}
				throw new CommandLineException(invalid.Select(a => a.Attribute));
			}

			// Set those properties' values.
			foreach(var m in matchings.Where(i => i.Arguments.Any()))
				SetPropertyValue(t, m.Property, m.Attribute is FlagArgumentAttribute ? (object)true : m.Arguments.First().Value); // TODO:  refactor as a virtual method.

			// Put all other arguments into the "extra arguments" property.
			var xargs = from v in cargs
						where !v.IsOption
						select v.Value;
			extraArguments = new List<string>(xargs);

			// Put unmatched options into the "extra options" property.
			var xopts = from c in cargs
						where c.IsOption
						join b in
							from m in matchings from a in m.Arguments select a on c equals b into j
						where !j.Any()
						select c.Argument;
			extraOptions = new List<string>(xopts);

			return t;
		}

		public string Usage<T>(params string[] additionalArguments)
		{
			var sb = new StringBuilder("usage:  ");
			sb.Append(System.IO.Path.GetFileName(Environment.GetCommandLineArgs()[0]));
			var q = from p in typeof(T).GetProperties()
					from a in p.GetCustomAttributes(typeof(ArgumentAttribute), true)
					select (ArgumentAttribute)a;
			q = q.ToList();
			foreach(var attribute in q)
				sb.Append(' ').Append(attribute.CommandLine);
			foreach(string arg in additionalArguments)
				sb.Append(' ').Append(arg);
			sb.Append(Environment.NewLine).Append(Environment.NewLine);
			int longestOption = q.Max(a => a.LongName.Length);
			var format = string.Format("-{{0}},--{0}1,-{1}{2}{{2}}{3}", '{', longestOption + 4, '}', Environment.NewLine);
			foreach(var attribute in q)
				sb.AppendFormat(format, attribute.ShortName, attribute.LongName, attribute.Usage);
			return sb.ToString();
		}

		private static void SetPropertyValue(object obj, System.Reflection.PropertyInfo property, object value)
		{
			try
			{
				string s = value != null ? value.ToString() : null;
				switch(property.PropertyType.Name)
				{
				case "Boolean":
					value = bool.Parse(s);
					break;
				case "Char":
					if(s != null && s.Length != 1)
						throw new FormatException();
					value = value != null ? s[0] : default(char);
					break;
				case "Int32":
					value = value != null ? int.Parse(s) : 0;
					break;
				case "Int64":
					value = value != null ? long.Parse(s) : 0L;
					break;
				case "String":
					value = value != null ? s : null;
					break;
				case "Int32[]":
					value = value != null ? Array.ConvertAll(s.Split(','), v => int.Parse(v)) : null;
					break;
				case "Int64[]":
					value = value != null ? Array.ConvertAll(s.Split(','), v => long.Parse(v)) : null;
					break;
				case "String[]":
					value = value != null ? s.Split(',') : null;
					break;
				default:
					throw new FormatException();
				}
				property.SetValue(obj, value, null);
			}
			catch(FormatException)
			{
				var message = string.Format("Cannot extract value for type {0} from '{1}'", property.PropertyType, value);
				throw new CommandLineException(message);
			}
		}
	}

	public class CommandLineException : Exception
	{
		public IEnumerable<CommandLine.ArgumentAttribute> Invalid { get; private set; }

		public CommandLineException(string message) : base(message) { }

		public CommandLineException(IEnumerable<CommandLine.ArgumentAttribute> invalid)
			: this(FormatMessage(invalid))
		{
			Invalid = new List<CommandLine.ArgumentAttribute>(invalid);
		}

		private static string FormatMessage(IEnumerable<CommandLine.ArgumentAttribute> invalid)
		{
			var sb = new StringBuilder("These arguments where not specified correctly: ");
			foreach(CommandLine.ArgumentAttribute arg in invalid)
				sb.AppendFormat(" {0} ({1}),", arg.LongName, arg.ShortName);
			--sb.Length;
			return sb.ToString();
		}
	}
}
