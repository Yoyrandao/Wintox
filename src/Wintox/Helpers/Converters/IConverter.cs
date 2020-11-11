using System;

namespace Wintox.Helpers.Converters
{
	public interface IConverter<in T, out U>
	{
		U Convert(T @object, EventHandler callback);
	}
}