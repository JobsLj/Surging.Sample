namespace Surging.Core.Schedule.Configurations
{
    public class ClusterOption
    {
        public QuartzClusterDbType DbType { get; set; }

        public string DbName { get; set; }

        public string DbConnectionString { get; set; }

        public string TablePrefix { get; set; } = "QRTZ_";

        public string DataSource { get; set; } = "SurgingDataSource";

        public string DataSourceProvider { get; set; }
    }
}