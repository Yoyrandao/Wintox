namespace Wintox.Common.Hash
{
	public interface IHashProvider
	{
		string Create(string data);
	}
}