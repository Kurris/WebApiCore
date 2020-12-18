namespace WebApiCore.EF.DataBase
{
    /// <summary>
    /// SqlServer数据库
    /// </summary>
    internal class SqlServerDB : BaseDatabase
    {
        internal SqlServerDB(string provider, string connStr) : base(provider, connStr)
        {
        }
    }
}
