namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 255),
                        PhoneNumber = c.String(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.Email, unique: true, name: "IX_UniqueAdminEmail")
                .Index(t => t.UserName, unique: true, name: "IX_UniqueAdminUserName")
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        QuestionSLNO = c.Int(nullable: false),
                        AnswerText = c.String(maxLength: 255),
                        AnswerDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Questions", t => t.QuestionSLNO, cascadeDelete: true)
                .Index(t => t.QuestionSLNO);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        Question = c.String(maxLength: 255),
                        Date = c.DateTime(nullable: false),
                        Answer = c.String(maxLength: 255),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuestionId);
            
            CreateTable(
                "dbo.AppliedPolicies",
                c => new
                    {
                        AppliedPolicyId = c.Int(nullable: false, identity: true),
                        PolicyNumber = c.String(nullable: false),
                        AppliedDate = c.DateTime(nullable: false),
                        Category = c.String(),
                        CustomerId = c.Int(nullable: false),
                        StatusCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppliedPolicyId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 255),
                        PhoneNumber = c.String(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.Email, unique: true, name: "IX_UniqueEmpUserEmail")
                .Index(t => t.UserName, unique: true, name: "IX_UniqueEmpUserName")
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Policies",
                c => new
                    {
                        PolicyId = c.Int(nullable: false, identity: true),
                        PolicyNumber = c.String(nullable: false),
                        AppliedDate = c.DateTime(nullable: false),
                        Category = c.String(),
                        CustomerId = c.Int(nullable: false),
                        Category_CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.PolicyId)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId)
                .Index(t => t.Category_CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Policies", "Category_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.AppliedPolicies", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Answers", "QuestionSLNO", "dbo.Questions");
            DropForeignKey("dbo.Admins", "RoleId", "dbo.Roles");
            DropIndex("dbo.Policies", new[] { "Category_CategoryId" });
            DropIndex("dbo.Customers", new[] { "RoleId" });
            DropIndex("dbo.Customers", "IX_UniqueEmpUserName");
            DropIndex("dbo.Customers", "IX_UniqueEmpUserEmail");
            DropIndex("dbo.AppliedPolicies", new[] { "CustomerId" });
            DropIndex("dbo.Answers", new[] { "QuestionSLNO" });
            DropIndex("dbo.Admins", new[] { "RoleId" });
            DropIndex("dbo.Admins", "IX_UniqueAdminUserName");
            DropIndex("dbo.Admins", "IX_UniqueAdminEmail");
            DropTable("dbo.Policies");
            DropTable("dbo.Categories");
            DropTable("dbo.Customers");
            DropTable("dbo.AppliedPolicies");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
            DropTable("dbo.Roles");
            DropTable("dbo.Admins");
        }
    }
}
