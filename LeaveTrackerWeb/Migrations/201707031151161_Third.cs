namespace LeaveTrackerWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Third : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MyUsers", "LastName", c => c.String());
            AddColumn("dbo.MyUsers", "FirstName", c => c.String());
            AddColumn("dbo.MyUsers", "EmployeeNo", c => c.String(nullable: false));
            AddColumn("dbo.MyUsers", "ProjectId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MyUsers", "ProjectId");
            DropColumn("dbo.MyUsers", "EmployeeNo");
            DropColumn("dbo.MyUsers", "FirstName");
            DropColumn("dbo.MyUsers", "LastName");
        }
    }
}
