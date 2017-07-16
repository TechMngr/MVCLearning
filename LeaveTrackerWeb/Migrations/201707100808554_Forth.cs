namespace LeaveTrackerWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Forth : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HolidayLists",
                c => new
                    {
                        HolidayId = c.Int(nullable: false, identity: true),
                        HolidayDate = c.DateTime(nullable: false),
                        DateDescription = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.HolidayId);
            
            CreateTable(
                "dbo.LeaveMappings",
                c => new
                    {
                        LeaveMappingId = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        EmployeeId = c.String(maxLength: 128),
                        LeaveTypeId = c.Int(nullable: false),
                        UxLeaveTypeId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        LeaveStatusId = c.Int(nullable: false),
                        LeaveDescription = c.String(),
                        LeaveCount = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.LeaveMappingId)
                .ForeignKey("dbo.MyUsers", t => t.EmployeeId)
                .ForeignKey("dbo.LeaveStatus", t => t.LeaveStatusId, cascadeDelete: true)
                .ForeignKey("dbo.LeaveTypes", t => t.LeaveTypeId, cascadeDelete: true)
                .ForeignKey("dbo.UxLeaveTypes", t => t.UxLeaveTypeId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.LeaveTypeId)
                .Index(t => t.UxLeaveTypeId)
                .Index(t => t.LeaveStatusId);
            
            CreateTable(
                "dbo.LeaveStatus",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Status = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "dbo.LeaveTypes",
                c => new
                    {
                        LeaveTypeId = c.Int(nullable: false, identity: true),
                        LeaveTypeName = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.LeaveTypeId);
            
            CreateTable(
                "dbo.UxLeaveTypes",
                c => new
                    {
                        UxLeaveTypeId = c.Int(nullable: false, identity: true),
                        UxLeaveTypeName = c.String(maxLength: 200),
                        LeaveTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.UxLeaveTypeId)
                .ForeignKey("dbo.LeaveTypes", t => t.LeaveTypeId)
                .Index(t => t.LeaveTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeaveMappings", "UxLeaveTypeId", "dbo.UxLeaveTypes");
            DropForeignKey("dbo.UxLeaveTypes", "LeaveTypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.LeaveMappings", "LeaveTypeId", "dbo.LeaveTypes");
            DropForeignKey("dbo.LeaveMappings", "LeaveStatusId", "dbo.LeaveStatus");
            DropForeignKey("dbo.LeaveMappings", "EmployeeId", "dbo.MyUsers");
            DropIndex("dbo.UxLeaveTypes", new[] { "LeaveTypeId" });
            DropIndex("dbo.LeaveMappings", new[] { "LeaveStatusId" });
            DropIndex("dbo.LeaveMappings", new[] { "UxLeaveTypeId" });
            DropIndex("dbo.LeaveMappings", new[] { "LeaveTypeId" });
            DropIndex("dbo.LeaveMappings", new[] { "EmployeeId" });
            DropTable("dbo.UxLeaveTypes");
            DropTable("dbo.LeaveTypes");
            DropTable("dbo.LeaveStatus");
            DropTable("dbo.LeaveMappings");
            DropTable("dbo.HolidayLists");
        }
    }
}
