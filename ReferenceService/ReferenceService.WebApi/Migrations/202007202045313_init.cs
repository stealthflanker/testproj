namespace ReferenceService.WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "sch_reference.REFERENCES",
                c => new
                    {
                        ref_number = c.String(nullable: false, maxLength: 15),
                        ref_status = c.String(nullable: false, maxLength: 15),
                        ref_code = c.String(nullable: false, maxLength: 15),
                        ref_name = c.String(maxLength: 255),
                        ref_reg_channel = c.String(nullable: false, maxLength: 15),
                        delivery_method = c.String(maxLength: 15),
                        delivery_email = c.String(maxLength: 255),
                        delivery_bo_number = c.String(maxLength: 255),
                        user_date_time = c.DateTime(),
                        user_id = c.String(maxLength: 255),
                        user_fio = c.String(maxLength: 255),
                        user_workstation_ip = c.String(maxLength: 255),
                        user_workstation_name = c.String(maxLength: 255),
                        customer_cuid = c.String(maxLength: 255),
                        customer_fio = c.String(maxLength: 255),
                        customer_passportSerNum = c.String(maxLength: 10),
                        date_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ref_number);
            
        }
        
        public override void Down()
        {
            DropTable("sch_reference.REFERENCES");
        }
    }
}
