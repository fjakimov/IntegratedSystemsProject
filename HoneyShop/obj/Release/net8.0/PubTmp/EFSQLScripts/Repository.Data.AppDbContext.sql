IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Stock] int NOT NULL,
        [ImageUrl] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(max) NOT NULL,
        [DateBirth] datetime2 NULL,
        [Email] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Role] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE TABLE [Carts] (
        [CartId] uniqueidentifier NOT NULL,
        [SessionId] nvarchar(max) NULL,
        [UserId] uniqueidentifier NULL,
        CONSTRAINT [PK_Carts] PRIMARY KEY ([CartId]),
        CONSTRAINT [FK_Carts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE TABLE [Orders] (
        [OrderId] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [TotalPrice] decimal(18,2) NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
        CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE TABLE [CartProducts] (
        [CartItemId] uniqueidentifier NOT NULL,
        [CartId] uniqueidentifier NOT NULL,
        [ProductId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        CONSTRAINT [PK_CartProducts] PRIMARY KEY ([CartItemId]),
        CONSTRAINT [FK_CartProducts_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([CartId]) ON DELETE CASCADE,
        CONSTRAINT [FK_CartProducts_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE TABLE [OrderProducts] (
        [ProductId] uniqueidentifier NOT NULL,
        [OrderId] uniqueidentifier NOT NULL,
        [Quantity] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Discount] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_OrderProducts] PRIMARY KEY ([OrderId], [ProductId]),
        CONSTRAINT [FK_OrderProducts_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderProducts_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DateBirth', N'Email', N'FullName', N'Password', N'Phone', N'Role') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] ON;
    EXEC(N'INSERT INTO [Users] ([Id], [DateBirth], [Email], [FullName], [Password], [Phone], [Role])
    VALUES (''11111111-1111-1111-1111-111111111111'', NULL, N''admin@honeyshop.com'', N''Admin User'', N''Admin123'', N''1234567890'', N''Admin'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DateBirth', N'Email', N'FullName', N'Password', N'Phone', N'Role') AND [object_id] = OBJECT_ID(N'[Users]'))
        SET IDENTITY_INSERT [Users] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE INDEX [IX_CartProducts_CartId] ON [CartProducts] ([CartId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE INDEX [IX_CartProducts_ProductId] ON [CartProducts] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Carts_UserId] ON [Carts] ([UserId]) WHERE [UserId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE INDEX [IX_OrderProducts_ProductId] ON [OrderProducts] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250129001048_init'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250129001048_init', N'9.0.1');
END;

COMMIT;
GO

