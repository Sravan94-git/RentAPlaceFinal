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
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260317064604_InitialCreate', N'10.0.5');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [Properties] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [OwnerId] int NOT NULL,
    CONSTRAINT [PK_Properties] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Properties_Users_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Properties_OwnerId] ON [Properties] ([OwnerId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260317073542_PropertyTable', N'10.0.5');

COMMIT;
GO

BEGIN TRANSACTION;
DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'Email');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [Users] ALTER COLUMN [Email] nvarchar(450) NOT NULL;

ALTER TABLE [Properties] ADD [HasGarden] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Properties] ADD [HasPool] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Properties] ADD [IsBeachFacing] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Properties] ADD [MaxGuests] int NOT NULL DEFAULT 0;

ALTER TABLE [Properties] ADD [PropertyType] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Properties] ADD [Rating] decimal(4,2) NOT NULL DEFAULT 0.0;

CREATE TABLE [Bookings] (
    [Id] int NOT NULL IDENTITY,
    [PropertyId] int NOT NULL,
    [RenterId] int NOT NULL,
    [CheckInDate] datetime2 NOT NULL,
    [CheckOutDate] datetime2 NOT NULL,
    [Guests] int NOT NULL,
    [TotalPrice] decimal(18,2) NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Bookings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bookings_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Bookings_Users_RenterId] FOREIGN KEY ([RenterId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Messages] (
    [Id] int NOT NULL IDENTITY,
    [PropertyId] int NOT NULL,
    [SenderId] int NOT NULL,
    [ReceiverId] int NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [IsRead] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Messages_Properties_PropertyId] FOREIGN KEY ([PropertyId]) REFERENCES [Properties] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_Users_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Messages_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

CREATE INDEX [IX_Bookings_PropertyId] ON [Bookings] ([PropertyId]);

CREATE INDEX [IX_Bookings_RenterId] ON [Bookings] ([RenterId]);

CREATE INDEX [IX_Messages_PropertyId] ON [Messages] ([PropertyId]);

CREATE INDEX [IX_Messages_ReceiverId] ON [Messages] ([ReceiverId]);

CREATE INDEX [IX_Messages_SenderId] ON [Messages] ([SenderId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260317114254_CapstoneCore', N'10.0.5');

COMMIT;
GO

