using System;

namespace Wintox.Helpers
{
	public interface IConverter<in T, out U>
	{
		U Convert(T @object, EventHandler callback);
	}
}