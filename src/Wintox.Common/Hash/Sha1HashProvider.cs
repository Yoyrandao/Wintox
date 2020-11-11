using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Wintox.Common.Hash
{
	public class Sha1HashProvider : IHashProvider
	{
		public string Create(string data)
		{
			using var hashFunc = new SHA1Managed();

			var hash = hashFunc.ComputeHash(Encoding.UTF8.GetBytes(data));
			return string.Join(string.Empty, hash.Select(x => x.ToString("X2"))).ToLower();
		}
	}
}