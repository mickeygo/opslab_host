using LiteDB;

namespace Ops.Host.Common.Management;

internal sealed class LiteDBMananger : LiteDatabase, ILiteDBMananger
{
	public LiteDBMananger(IConfiguration configuration) : base(GetConnectionString(configuration, "LiteDB"))
	{
	}

	static string GetConnectionString(IConfiguration configuration, string name)
	{
		return configuration.GetConnectionString(name);
	}
}
