namespace VIPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserIdType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Participants", "IX_UserId");
            DropIndex("dbo.SelfEstimationCheckLists", "IX_UserId");
            DropIndex("dbo.SelfEstimationResumeFromUsers", "IX_UserId");
            DropIndex("dbo.SelfEstimationFeedbackToUsers", "IX_UserId");
            DropForeignKey("dbo.Participants", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SelfEstimationCheckLists", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SelfEstimationResumeFromUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SelfEstimationFeedbackToUsers", "UserId", "dbo.AspNetUsers");
            AlterColumn("dbo.SelfEstimationCheckLists", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.SelfEstimationFeedbackToUsers", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Participants", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.SelfEstimationResumeFromUsers", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddForeignKey("dbo.Participants", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SelfEstimationCheckLists", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SelfEstimationResumeFromUsers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SelfEstimationFeedbackToUsers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Participants", "IX_UserId");
            DropIndex("dbo.SelfEstimationCheckLists", "IX_UserId");
            DropIndex("dbo.SelfEstimationResumeFromUsers", "IX_UserId");
            DropIndex("dbo.SelfEstimationFeedbackToUsers", "IX_UserId");
            DropForeignKey("dbo.Participants", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SelfEstimationCheckLists", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SelfEstimationResumeFromUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SelfEstimationFeedbackToUsers", "UserId", "dbo.AspNetUsers");
            AlterColumn("dbo.SelfEstimationResumeFromUsers", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Participants", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.SelfEstimationFeedbackToUsers", "UserId", c => c.Guid(nullable: false));
            AlterColumn("dbo.SelfEstimationCheckLists", "UserId", c => c.Guid(nullable: false));
            AddForeignKey("dbo.Participants", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SelfEstimationCheckLists", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SelfEstimationResumeFromUsers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SelfEstimationFeedbackToUsers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
