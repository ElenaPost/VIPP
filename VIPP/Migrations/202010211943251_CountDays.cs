namespace VIPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountDays : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SelfEstimationCheckLists", "Day", c => c.Int(nullable: false));
            AddColumn("dbo.SelfEstimationFeedbackToUsers", "Day", c => c.Int(nullable: false));
            AddColumn("dbo.Marathons", "CountDays", c => c.Int(nullable: false));
            AddColumn("dbo.SelfEstimationResumeFromUsers", "Day", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SelfEstimationResumeFromUsers", "Day");
            DropColumn("dbo.Marathons", "CountDays");
            DropColumn("dbo.SelfEstimationFeedbackToUsers", "Day");
            DropColumn("dbo.SelfEstimationCheckLists", "Day");
        }
    }
}