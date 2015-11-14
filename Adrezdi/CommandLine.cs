using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Adrezdi
{
    public class CommandLine
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
        public class UsageAttribute : Attribute
        {
            public string Prolog { get; set; }
            public string Epilog { get; set; }

            public UsageAttribute() { }

            public UsageAttribute(string prolog, string epilog)
            {
                Prolog = prolog;
                Epilog = epilog;
            }
        }

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
            /// Gets the display value of how to use this argument on the
            /// command line.
            /// </summary>
            internal abstract string CommandLine { get; }

            internal abstract bool WantsValue { get; }
        }

        /// <summary>
        /// The option takes no value.  Its presence or absence determines its
        /// Boolean value.
        /// </summary>
        public class FlagArgumentAttribute : ArgumentAttribute
        {
            internal override string CommandLine
            {
                get { return "[-" + ShortName + ']'; }
            }

            internal override bool WantsValue
            {
                get { return false; }
            }
        }

        /// <summary>
        /// The option need not appear on the command line but, if it does, it
        /// must have a value.
        /// </summary>
        public class OptionalValueArgumentAttribute : ArgumentAttribute
        {
            internal override string CommandLine
            {
                get { return "[-" + ShortName + " value]"; }
            }

            internal override bool WantsValue
            {
                get { return true; }
            }
        }

        /// <summary>
        /// The option must appear on the command line with a value.
        /// </summary>
        public class RequiredValueArgumentAttribute : ArgumentAttribute
        {
            internal override string CommandLine
            {
                get { return "-" + ShortName + " value"; }
            }

            internal override bool WantsValue
            {
                get { return true; }
            }
        }

        public IEnumerable<string> ExtraArguments { get { return extraArguments; } }
        private List<string> extraArguments = new List<string>();
        public IEnumerable<string> ExtraOptions { get { return extraOptions; } }
        private List<string> extraOptions = new List<string>();

        public T Parse<T>(string[] args, bool automatingUsage) where T : new()
        {
            var t = new T();

            // Check for the help option.
            if(args.Any(s => s == "-h" || s == "--help"))
            {
                var usage = Usage<T>();
                if(automatingUsage)
                {
                    Console.Write(usage);
                    Environment.Exit(2);
                }
                throw new CommandLineException(usage);
            }

            // Collect the argument attributes with their properties of the type.
            var q = from p in typeof(T).GetProperties()
                    from a in p.GetCustomAttributes(typeof(ArgumentAttribute), true).Cast<ArgumentAttribute>()
                    from k in new[] { a.LongName, a.ShortName.ToString() }
                    select new { Key = k, Property = p, Attribute = a };
            var pairs = q.ToDictionary(a => a.Key, a => new { a.Property, a.Attribute });

            // Collect required argument attributes.
            var required = new HashSet<RequiredValueArgumentAttribute>(pairs.Select(a => a.Value.Attribute).OfType<RequiredValueArgumentAttribute>());

            // Collect only the options from the command line.
            var invalid = new List<ArgumentAttribute>();
            for(int i = 0, n = args.Length; i < n; ++i)
            {
                var arg = args[i];
                if(arg == "--")
                {
                    extraArguments.AddRange(args.Skip(i + 1).ToList());
                    break;
                }
                if(arg.StartsWith("--"))
                {
                    // Check for a long name option.
                    var parts = arg.Substring(2).Split('=');
                    var name = parts[0];
                    if(pairs.ContainsKey(name))
                    {
                        required.RemoveWhere(a => a.LongName == name);
                        var pair = pairs[name];
                        if(pair.Attribute.WantsValue && parts.Length >= 2)
                        {
                            var value = String.Join("=", parts.Skip(1));
                            SetPropertyValue(t, pair.Property, value);
                        }
                        else if(!pair.Attribute.WantsValue && parts.Length == 1)
                            SetPropertyValue(t, pair.Property, true);
                        else
                            invalid.Add(pair.Attribute);
                    }
                    else
                        extraOptions.Add(arg);
                }
                else if(arg.StartsWith("-"))
                {
                    // Check for a short name option.
                    var name = arg.Substring(1);
                    if(pairs.ContainsKey(name))
                    {
                        required.RemoveWhere(a => a.ShortName == name[0]);
                        var pair = pairs[name];
                        if(pair.Attribute.WantsValue && i + 1 < args.Length)
                        {
                            var value = args[++i];
                            SetPropertyValue(t, pair.Property, value);
                        }
                        else if(!pair.Attribute.WantsValue)
                            SetPropertyValue(t, pair.Property, true);
                        else
                            invalid.Add(pair.Attribute);
                    }
                    else
                        extraOptions.Add(arg);
                }
                else
                    extraArguments.Add(arg);
            }

            // Check for invalid options.
            if(invalid.Count > 0 || required.Count > 0)
            {
                if(automatingUsage)
                {
                    Console.Write(Usage<T>());
                    Environment.Exit(2);
                }
                throw new CommandLineException(invalid.Concat(required));
            }

            return t;
        }

        public string Usage<T>(params string[] additionalArguments)
        {
            var sb = new StringBuilder();
            var usageAttribute = typeof(T).GetCustomAttribute<UsageAttribute>();
            if(usageAttribute != null && !String.IsNullOrWhiteSpace(usageAttribute.Prolog))
                sb.AppendFormat("{1}{0}{0}", Environment.NewLine, usageAttribute.Prolog);
            sb.Append("usage:  ");
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
            var epilogAttribute = typeof(T).GetCustomAttribute<UsageAttribute>();
            if(usageAttribute != null && !String.IsNullOrWhiteSpace(usageAttribute.Epilog))
                sb.AppendFormat("{0}{1}{0}", Environment.NewLine, usageAttribute.Epilog);
            return sb.ToString();
        }

        private static void SetPropertyValue(object obj, PropertyInfo property, object value)
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
