namespace VIPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSerialNumberToSelfEstimationCheckList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SelfEstimationCheckLists", "SerialNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SelfEstimationCheckLists", "SerialNumber");
        }
    }
}
