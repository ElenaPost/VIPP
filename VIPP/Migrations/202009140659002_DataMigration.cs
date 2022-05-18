using VIPP.Models;

namespace VIPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMigration : DbMigration
    {
        public override void Up()
        {
        //    CreateTable(
        //        "dbo.Books",
        //        c => new
        //        {
        //            Id = c.Int(nullable: false, identity: true),
        //            Name = c.String(),
        //            Author = c.String(),
        //            UserId = c.Int(nullable: false, identity: true),
        //        })
        //        .PrimaryKey(t => t.Id)
        //        .ForeignKey("dbo.AspNetUsers", t => t.Id, cascadeDelete: true);
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
        }
    }
}
