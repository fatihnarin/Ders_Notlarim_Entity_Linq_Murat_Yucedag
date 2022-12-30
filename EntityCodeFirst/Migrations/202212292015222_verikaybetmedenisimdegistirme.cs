namespace EntityCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class verikaybetmedenisimdegistirme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Kategoris", "KategoriAdi", c => c.String());
            Sql("Update Kategoris Set KategoriAdi=KategoriAd");//Verilerin kaybolmaması için
            DropColumn("dbo.Kategoris", "KategoriAd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Kategoris", "KategoriAd", c => c.String());
            Sql("Update Kategoris Set KategoriAd=KategoriAdi");//Verilerin kaybolmaması için
            DropColumn("dbo.Kategoris", "KategoriAdi");
        }
    }
}
