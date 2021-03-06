using System;

namespace SqlPatch
{
    public abstract class Migration
    {
        public String ConnectionString { get; private set; }

        public Migration()
        {
            ConnectionString = SchemaHelpers.CreateConnectionString();
        }

        public abstract void Execute();
    }
}
