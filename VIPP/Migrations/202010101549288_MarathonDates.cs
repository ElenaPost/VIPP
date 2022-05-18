namespace VIPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarathonDates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MarathonDates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MarathonId = c.Guid(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Marathons", t => t.Id, cascadeDelete: true);
            
            AddColumn("dbo.Marathons", "Cost", c => c.Int(nullable: false));
            AddColumn("dbo.Marathons", "Count", c => c.Int(nullable: false));
            AddColumn("dbo.Participants", "MarathonDateId", c => c.Guid(nullable: false));
            AddForeignKey("dbo.Participants", "MarathonDateId", "dbo.MarathonDates", "Id", cascadeDelete: true);
            DropForeignKey("dbo.Participants", "MarathonId", "dbo.Marathons");
            DropIndex("dbo.Participants", new[] { "MarathonId" });
            DropColumn("dbo.Marathons", "StartDate");
            DropColumn("dbo.Participants", "MarathonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Participants", "MarathonDateId", "dbo.MarathonDates");
            AddForeignKey("dbo.Participants", "MarathonId", "dbo.Marathons", "Id", cascadeDelete: true);
            AddColumn("dbo.Participants", "MarathonId", c => c.Guid(nullable: false));
            AddColumn("dbo.Marathons", "StartDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Participants", "MarathonDateId");
            DropColumn("dbo.Marathons", "Count");
            DropColumn("dbo.Marathons", "Cost");
            DropTable("dbo.MarathonDates");
        }
    }
}
