using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace monacos.us.web.services.model.Models.DB
{
    public partial class monacosusContext : DbContext
    {

        private string _ConnectionString = null;


        public monacosusContext(string ConnectionString )
        {

            

            this._ConnectionString = ConnectionString;


        }

        public monacosusContext(DbContextOptions<monacosusContext> options)
            : base(options)
        {

            
        }

        public virtual DbSet<AlexaSkillAccount> AlexaSkillAccounts { get; set; }
        public virtual DbSet<AlexaSkillAuthorization> AlexaSkillAuthorizations { get; set; }
        public virtual DbSet<AlexaSkillConfigurationDatum> AlexaSkillConfigurationData { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<ContentArea> ContentAreas { get; set; }
        public virtual DbSet<Dtproperty> Dtproperties { get; set; }
        public virtual DbSet<NavigationItem> NavigationItems { get; set; }
        public virtual DbSet<NavigationItemResourceRoleXref> NavigationItemResourceRoleXrefs { get; set; }
        public virtual DbSet<NavigationType> NavigationTypes { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<ResourceRole> ResourceRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRoleXref> UserRoleXrefs { get; set; }
        public virtual DbSet<WebSession> WebSessions { get; set; }
        public virtual DbSet<WebSessionInformation> WebSessionInformations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this._ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AlexaSkillAccount>(entity =>
            {
                entity.ToTable("alexa_skill_account", "Alexa");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AmazonAlexaSkillName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("amazon_alexa_skill_name");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.Delete).HasColumnName("delete");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("First_Name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Last_Name");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");
            });

            modelBuilder.Entity<AlexaSkillAuthorization>(entity =>
            {
                entity.ToTable("alexa_skill_authorization", "Alexa");

                entity.HasIndex(e => e.Id, "IX_alexa_skill_authorization");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AlexaSkillAccountId).HasColumnName("alexa_skill_account_id");

                entity.Property(e => e.AmazonAlexaAuthRedirectUri)
                    .HasColumnType("text")
                    .HasColumnName("amazon_alexa_auth_redirect_uri");

                entity.Property(e => e.AmazonAlexaClientId)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasColumnName("amazon_alexa_client_id");

                entity.Property(e => e.AmazonAlexaSkillAccessToken)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("amazon_alexa_skill_access_token");

                entity.Property(e => e.AmazonAlexaSkillScope)
                    .HasMaxLength(4000)
                    .HasColumnName("amazon_alexa_skill_scope");

                entity.Property(e => e.AmazonAlexaState)
                    .IsRequired()
                    .HasMaxLength(4000)
                    .HasColumnName("amazon_alexa_state");

                entity.Property(e => e.AmazonAlexaUserId)
                    .HasMaxLength(4000)
                    .HasColumnName("amazon_alexa_user_id");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.Delete).HasColumnName("delete");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.AlexaSkillAccount)
                    .WithMany(p => p.AlexaSkillAuthorizations)
                    .HasForeignKey(d => d.AlexaSkillAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_alexa_skill_authentication_alexa_skill_account_data1");
            });

            modelBuilder.Entity<AlexaSkillConfigurationDatum>(entity =>
            {
                entity.ToTable("alexa_skill_configuration_data", "Alexa");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AlexaSkillAccountId).HasColumnName("alexa_skill_account_id");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creation_date");

                entity.Property(e => e.Delete).HasColumnName("delete");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("update_date");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.AlexaSkillAccount)
                    .WithMany(p => p.AlexaSkillConfigurationData)
                    .HasForeignKey(d => d.AlexaSkillAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_alexa_skill_configuration_data_alexa_skill_account_data");
            });

            modelBuilder.Entity<Content>(entity =>
            {
                entity.ToTable("Content");

                entity.Property(e => e.ContentId).HasColumnName("Content_ID");

                entity.Property(e => e.ContentAreaId).HasColumnName("ContentArea_ID");

                entity.Property(e => e.ContentValue)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Create_Date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Expiration_Date");

                entity.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Publish_Date");
            });

            modelBuilder.Entity<ContentArea>(entity =>
            {
                entity.ToTable("ContentArea");

                entity.Property(e => e.ContentAreaId)
                    .ValueGeneratedNever()
                    .HasColumnName("ContentArea_ID");

                entity.Property(e => e.ContentAreaDescription)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ContentArea_Description");
            });

            modelBuilder.Entity<Dtproperty>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dtproperties");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Lvalue)
                    .HasColumnType("image")
                    .HasColumnName("lvalue");

                entity.Property(e => e.Objectid).HasColumnName("objectid");

                entity.Property(e => e.Property)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("property");

                entity.Property(e => e.Uvalue)
                    .HasMaxLength(255)
                    .HasColumnName("uvalue");

                entity.Property(e => e.Value)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("value");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            modelBuilder.Entity<NavigationItem>(entity =>
            {
                entity.ToTable("Navigation_Items");

                entity.Property(e => e.NavigationItemId)
                    .ValueGeneratedNever()
                    .HasColumnName("Navigation_Item_ID");

                entity.Property(e => e.NavigationItemCaption)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Navigation_Item_Caption");

                entity.Property(e => e.NavigationItemImage)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Navigation_Item_Image");

                entity.Property(e => e.NavigationItemLevelSortOrder).HasColumnName("Navigation_Item_Level_Sort_Order");

                entity.Property(e => e.NavigationItemMenuVisible).HasColumnName("Navigation_Item_Menu_Visible");

                entity.Property(e => e.NavigationItemSiteMapCaption)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Navigation_Item_SiteMap_Caption");

                entity.Property(e => e.NavigationItemTreeViewVisible).HasColumnName("Navigation_Item_TreeView_Visible");

                entity.Property(e => e.NavigationItemUri)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Navigation_Item_URI");

                entity.Property(e => e.NavigationItemUriTarget)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Navigation_Item_URI_Target")
                    .IsFixedLength(true);

                entity.Property(e => e.NavigationTypeId).HasColumnName("Navigation_Type_ID");

                entity.Property(e => e.ParentNavigationItemId).HasColumnName("Parent_Navigation_Item_ID");
            });

            modelBuilder.Entity<NavigationItemResourceRoleXref>(entity =>
            {
                entity.HasKey(e => e.NavigationRoleXrefId);

                entity.ToTable("NavigationItem_ResourceRole_Xref");

                entity.Property(e => e.NavigationRoleXrefId)
                    .ValueGeneratedNever()
                    .HasColumnName("Navigation_Role_Xref_ID");

                entity.Property(e => e.NavigationItemId).HasColumnName("Navigation_Item_ID");

                entity.Property(e => e.ResourceRoleId).HasColumnName("Resource_Role_ID");
            });

            modelBuilder.Entity<NavigationType>(entity =>
            {
                entity.ToTable("Navigation_Types");

                entity.Property(e => e.NavigationTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("Navigation_Type_ID");

                entity.Property(e => e.NavigationTypeDescription)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("Navigation_Type_Description");

                entity.Property(e => e.NavigationTypeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Navigation_Type_Name");
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.ToTable("Resource");

                entity.Property(e => e.ResourceId)
                    .ValueGeneratedNever()
                    .HasColumnName("Resource_ID");

                entity.Property(e => e.ResourceDescription)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("Resource_Description");

                entity.Property(e => e.ResourceName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Resource_Name");
            });

            modelBuilder.Entity<ResourceRole>(entity =>
            {
                entity.ToTable("Resource_Roles");

                entity.Property(e => e.ResourceRoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("Resource_Role_ID");

                entity.Property(e => e.ResourceId).HasColumnName("Resource_ID");

                entity.Property(e => e.RoleDescription)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("Role_Description");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Role_Name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("User_ID");

                entity.Property(e => e.UserLogin)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("User_Login")
                    .IsFixedLength(true);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("User_Password")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<UserRoleXref>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ResourceRoleId });

                entity.ToTable("User_Role_Xref");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.ResourceRoleId).HasColumnName("Resource_Role_ID");
            });

            modelBuilder.Entity<WebSession>(entity =>
            {
                entity.Property(e => e.WebSessionId)
                    .ValueGeneratedNever()
                    .HasColumnName("WebSessionID");

                entity.Property(e => e.EntryDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Entry_DateTime");

                entity.Property(e => e.SessionId)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("SessionID");
            });

            modelBuilder.Entity<WebSessionInformation>(entity =>
            {
                entity.ToTable("WebSessionInformation");

                entity.Property(e => e.WebSessionInformationId).HasColumnName("WebSessionInformationID");

                entity.Property(e => e.EntryDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Entry_DateTime");

                entity.Property(e => e.WebSessionId).HasColumnName("WebSessionID");

                entity.Property(e => e.WebSessionServerVariableName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WebSessionServerVariableValue)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
