using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adrezdi
{
	public class EventArgs<T> : EventArgs
	{
		public T Value { get; private set; }

		public EventArgs(T value)
		{
			Value = value;
		}
	}
}
