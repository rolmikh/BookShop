using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API_Book_Shop.Models
{
    public partial class Book_ShopContext : DbContext
    {
        public Book_ShopContext()
        {
        }

        public Book_ShopContext(DbContextOptions<Book_ShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Basket> Baskets { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Contract> Contracts { get; set; } = null!;
        public virtual DbSet<CustomerCard> CustomerCards { get; set; } = null!;
        public virtual DbSet<DeliveryNote> DeliveryNotes { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderComposition> OrderCompositions { get; set; } = null!;
        public virtual DbSet<OrderView> OrderViews { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<StatusOrder> StatusOrders { get; set; } = null!;
        public virtual DbSet<Supply> Supplies { get; set; } = null!;
        public virtual DbSet<SupplyComposition> SupplyCompositions { get; set; } = null!;
        public virtual DbSet<SupplyView> SupplyViews { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Warehouse> Warehouses { get; set; } = null!;
        public virtual DbSet<SupplyCompositionView> SupplyCompositionViews { get; set; } = null!;

    

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            modelBuilder.Entity<Basket>(entity =>
            {
                entity.HasKey(e => e.IdBasket);

                entity.ToTable("Basket");

                

                entity.Property(e => e.IdBasket).HasColumnName("ID_Basket");

                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.IsDeletedBasket).HasColumnName("IsDeleted_Basket");

               
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory);

                entity.ToTable("Category");

                entity.HasIndex(e => e.NameCategory, "UQ_Name_Category")
                    .IsUnique();

                entity.Property(e => e.IdCategory).HasColumnName("ID_Category");

                entity.Property(e => e.NameCategory)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Name_Category");

                entity.Property(e => e.IsDeletedCategory).HasColumnName("IsDeletedCategory");

            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.IdContract);

                entity.ToTable("Contract");

                entity.HasIndex(e => e.NumberContract, "UQ_Number_Contract")
                    .IsUnique();

                entity.Property(e => e.IdContract).HasColumnName("ID_Contract");

                entity.Property(e => e.DateContract)
                    .HasColumnType("date")
                    .HasColumnName("Date_Contract")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NumberContract)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Number_Contract");

                entity.Property(e => e.IsDeletedContract).HasColumnName("IsDeletedContract");

            });

            modelBuilder.Entity<CustomerCard>(entity =>
            {
                entity.HasKey(e => e.IdCustomerCard)
                    .HasName("PK_Card");

                entity.ToTable("Customer_Card");

                entity.HasIndex(e => e.CvvCode, "UQ_CVV")
                    .IsUnique();

                entity.HasIndex(e => e.NumberCard, "UQ_Number_Card")
                    .IsUnique();

                entity.Property(e => e.IdCustomerCard).HasColumnName("ID_Customer_Card");

                entity.Property(e => e.CvvCode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CVV_Code");

                entity.Property(e => e.NumberCard)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("Number_Card");

                entity.Property(e => e.SaltCard)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("Salt_Card");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.ValidityPeriod)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Validity_Period");

              
            });

            modelBuilder.Entity<DeliveryNote>(entity =>
            {
                entity.HasKey(e => e.IdDeliveryNote);

                entity.ToTable("Delivery_Note");

                entity.HasIndex(e => e.NumberDeliveryNote, "UQ_Number_Delivery_Note")
                    .IsUnique();

                entity.Property(e => e.IdDeliveryNote).HasColumnName("ID_Delivery_Note");

                entity.Property(e => e.ContractId).HasColumnName("Contract_ID");

                entity.Property(e => e.DateDeliveryNote)
                    .HasColumnType("date")
                    .HasColumnName("Date_Delivery_Note")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NumberDeliveryNote)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Number_Delivery_Note");

                entity.Property(e => e.IsDeletedNote).HasColumnName("IsDeletedNote");


            });


            

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrder);

                entity.ToTable("Order");

                entity.HasIndex(e => e.NumberOrder, "UQ_Number_Order")
                    .IsUnique();

                entity.Property(e => e.IdOrder).HasColumnName("ID_Order");

                entity.Property(e => e.DateOrder)
                    .HasColumnType("date")
                    .HasColumnName("Date_Order");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.NumberOrder)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Number_Order");

                entity.Property(e => e.StatusOrderId).HasColumnName("Status_Order_ID");

                entity.Property(e => e.PriceOrder)
                   .HasColumnType("decimal(38, 2)")
                   .HasColumnName("Price_Order");


            });

            modelBuilder.Entity<OrderComposition>(entity =>
            {
                entity.HasKey(e => e.IdOrderComposition);

                entity.ToTable("Order_Composition");

                entity.Property(e => e.IdOrderComposition).HasColumnName("ID_Order_Composition");

                entity.Property(e => e.BasketId).HasColumnName("Basket_ID");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.OrderId).HasColumnName("Order_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

            });

            modelBuilder.Entity<OrderView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Order_View");

                entity.Property(e => e.ДатаОформления)
                    .HasColumnType("date")
                    .HasColumnName("Дата оформления");

                entity.Property(e => e.ДатаРождения)
                    .HasColumnType("date")
                    .HasColumnName("Дата рождения");

                entity.Property(e => e.Имя)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.НомерЗаказа)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Номер заказа");

                entity.Property(e => e.Отчество)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.СоставЗаказа)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Состав заказа");

                entity.Property(e => e.СтатусЗаказа)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Статус заказа");

                entity.Property(e => e.СтоимостьЗаказа)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("Стоимость заказа");

                entity.Property(e => e.Фамилия)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ЭлектроннаяПочта)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Электронная почта");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.IdProduct);

                entity.ToTable("Product");

                entity.Property(e => e.IdProduct).HasColumnName("ID_Product");

                entity.Property(e => e.AgeRestriction)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("Age_Restriction");

                entity.Property(e => e.Annotation).IsUnicode(false);

                entity.Property(e => e.ArticleProduct)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Article_Product");

                entity.Property(e => e.Author)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryId).HasColumnName("Category_ID");

                entity.Property(e => e.CoverType)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Cover_Type");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.NameBook)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Name_Book");

                entity.Property(e => e.NumberOfPages).HasColumnName("Number_Of_Pages");

                entity.Property(e => e.PhotoBook)
                    .IsUnicode(false)
                    .HasColumnName("Photo_Book");

                entity.Property(e => e.PriceBook)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("Price_Book");

                entity.Property(e => e.PublishingHouse)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Publishing_House");

                entity.Property(e => e.Series)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.YearOfPublication)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("Year_Of_Publication");

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole);

                entity.ToTable("Role");

                entity.HasIndex(e => e.NameRole, "UQ_Name_Role")
                    .IsUnique();

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.NameRole)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Name_Role");

                entity.Property(e => e.IsDeletedRole).HasColumnName("IsDeletedRole");

            });

            modelBuilder.Entity<StatusOrder>(entity =>
            {
                entity.HasKey(e => e.IdStatusOrder);

                entity.ToTable("Status_Order");

                entity.HasIndex(e => e.NameStatusOrder, "UQ_Name_Status_Order")
                    .IsUnique();

                entity.Property(e => e.IdStatusOrder).HasColumnName("ID_Status_Order");

                entity.Property(e => e.NameStatusOrder)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Name_Status_Order");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");

            });

            modelBuilder.Entity<Supply>(entity =>
            {
                entity.HasKey(e => e.IdSupply);

                entity.ToTable("Supply");

                entity.HasIndex(e => e.NumberSupply, "UQ_NUmber_Supply")
                    .IsUnique();

                entity.Property(e => e.IdSupply).HasColumnName("ID_Supply");

                entity.Property(e => e.DateSupply)
                    .HasColumnType("date")
                    .HasColumnName("Date_Supply")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeliveryNoteId).HasColumnName("Delivery_Note_ID");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.NumberSupply)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Number_Supply");

                entity.Property(e => e.WarehouseId).HasColumnName("Warehouse_ID");

            });

            modelBuilder.Entity<SupplyComposition>(entity =>
            {
                entity.HasKey(e => e.IdSupplyComposition);

                entity.ToTable("Supply_Composition");

                entity.Property(e => e.IdSupplyComposition).HasColumnName("ID_Supply_Composition");

                entity.Property(e => e.CountSupply).HasColumnName("Count_Supply");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.PriceSupply)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("Price_Supply");

                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.SupplyId).HasColumnName("Supply_ID");

            });

            modelBuilder.Entity<SupplyView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Supply_View");

                entity.Property(e => e.АдресСклада)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Адрес склада");

                entity.Property(e => e.АртикулТовара)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Артикул товара");

                entity.Property(e => e.ДатаНакладной)
                    .HasColumnType("date")
                    .HasColumnName("Дата накладной");

                entity.Property(e => e.ДатаПодписания)
                    .HasColumnType("date")
                    .HasColumnName("Дата подписания");

                entity.Property(e => e.ДатаПоставки)
                    .HasColumnType("date")
                    .HasColumnName("Дата поставки");

                entity.Property(e => e.ЗакупочнаяЦена)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("Закупочная цена");

                entity.Property(e => e.КоличествоПоставки).HasColumnName("Количество поставки");

                entity.Property(e => e.НазваниеКниги)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Название книги");

                entity.Property(e => e.НомерДоговора)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Номер договора");

                entity.Property(e => e.НомерНакладной)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Номер накладной");

                entity.Property(e => e.НомерПоставки)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Номер поставки");

                entity.Property(e => e.НомерСклада)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Номер склада");

                entity.Property(e => e.ОтпускнаяЦена)
                    .HasColumnType("decimal(38, 2)")
                    .HasColumnName("Отпускная цена");
            });

            modelBuilder.Entity<SupplyCompositionView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Supply_Composition_View");

                entity.Property(e => e.НазваниеТовара)
                   .HasMaxLength(30)
                   .IsUnicode(false)
                   .HasColumnName("Название товара");

                entity.Property(e => e.НомерПоставки)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Номер поставки");

                entity.Property(e => e.ДатаПоставки)
                    .HasColumnType("date")
                    .HasColumnName("Дата поставки");

                entity.Property(e => e.СтоимостьПоставки)
                   .HasColumnType("decimal(38, 2)")
                   .HasColumnName("Стоимость товара");

                entity.Property(e => e.КоличествоПоставки).HasColumnName("Количество поставки");


            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("User");

                entity.HasIndex(e => e.EmailUser, "UQ_Email_User")
                    .IsUnique();

                entity.HasIndex(e => e.PasswordUser, "UQ_Password")
                    .IsUnique();

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.DateBirthUser)
                    .HasColumnType("date")
                    .HasColumnName("Date_Birth_User");

                entity.Property(e => e.EmailUser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Email_User");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.MiddleNameUser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Middle_Name_User");

                entity.Property(e => e.NameUser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_User");

                entity.Property(e => e.PasswordUser)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("Password_User");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.SaltUser)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Salt_User");

                entity.Property(e => e.SurnameUser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Surname_User");

            });

            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.IdWarehouse);

                entity.ToTable("Warehouse");

                entity.HasIndex(e => e.NumberWarehouse, "UQ_Number_Warehouse")
                    .IsUnique();

                entity.Property(e => e.IdWarehouse).HasColumnName("ID_Warehouse");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumberWarehouse)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("Number_Warehouse");

                entity.Property(e => e.IsDeletedWarehouse).HasColumnName("IsDeletedWarehouse");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
