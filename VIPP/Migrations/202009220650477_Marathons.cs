namespace VIPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Marathons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SelfEstimationCheckLists",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Achievement = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SelfEstimationFeedbackToUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Feedback = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Marathons",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "MarathonNameIndex");
            
            CreateTable(
                "dbo.Participants",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        MarathonId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Marathons", t => t.MarathonId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.MarathonId);
            
            CreateTable(
                "dbo.SelfEstimationResumeFromUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Resume = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SelfEstimationCheckLists", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SelfEstimationFeedbackToUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Participants", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Participants", "MarathonId", "dbo.Marathons");
            DropForeignKey("dbo.SelfEstimationResumeFromUsers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SelfEstimationCheckLists", new[] { "UserId" });
            DropIndex("dbo.SelfEstimationFeedbackToUsers", new[] { "UserId" });
            DropIndex("dbo.Participants", new[] { "UserId" });
            DropIndex("dbo.Participants", new[] { "MarathonId" });
            DropIndex("dbo.SelfEstimationResumeFromUsers", new[] { "UserId" });
            DropIndex("dbo.Marathons", "MarathonNameIndex");
            DropTable("dbo.SelfEstimationResumeFromUsers");
            DropTable("dbo.Participants");
            DropTable("dbo.Marathons");
            DropTable("dbo.SelfEstimationFeedbackToUsers");
            DropTable("dbo.SelfEstimationCheckLists");
        }
    }
}
