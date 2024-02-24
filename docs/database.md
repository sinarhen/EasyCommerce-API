# 💾 Database Documentation

This document provides detailed information about the database schema.
- [Database Documentation](#database-documentation)
    - [Tables](#tables)
        - [`__EFMigrationsHistory`](#__efmigrationshistory)
        - [`AspNetRoles`](#aspnetroles)
        - [`AspNetRoleClaims`](#aspnetroleclaims)
        - [`AspNetUsers`](#aspnetusers)
        - [`AspNetUserClaims`](#aspnetuserclaims)
        - [`AspNetUserLogins`](#aspnetuserlogins)
        - [`AspNetUserRoles`](#aspnetuserroles)
        - [`AspNetUserTokens`](#aspnetusertokens)
        - [`BannedUsers`](#bannedusers)
        - [`Orders`](#orders)
        - [`SellerUpgradeRequests`](#sellerupgraderequests)
        - [`Stores`](#stores)
        - [`BannedStores`](#bannedstores)
        - [`Collections`](#collections)
        - [`Billboards`](#billboards)
        - [`Products`](#products)
        - [`OrderItems`](#orderitems)
        - [`ProductCategories`](#productcategories)
        - [`ProductImages`](#productimages)
        - [`ProductMaterials`](#productmaterials)
        - [`ProductStocks`](#productstocks)
        - [`Reviews`](#reviews)
    - [Indexes](#indexes)
    - [Relationships](#relationships)
## Tables

### `__EFMigrationsHistory`

This table is used by Entity Framework Core to track migrations.

Fields:
- `MigrationId`: The ID of the migration.
- `ProductVersion`: The version of EF Core used to apply the migration.

### `AspNetRoles`

This table stores information about roles.

Fields:
- `Id`: The ID of the role.
- `Name`: The name of the role.
- `NormalizedName`: The normalized name of the role.
- `ConcurrencyStamp`: A concurrency token used to ensure that two users editing the same data do not overwrite each other's changes.

### `AspNetRoleClaims`

This table links roles and claims.

Fields:
- `Id`: The ID of the role claim.
- `RoleId`: The ID of the role associated with this claim.
- `ClaimType`: The claim type.
- `ClaimValue`: The claim value.

### `AspNetUsers`

This table stores information about users.

Fields:
- `Id`: The ID of the user.
- `UserName`: The username of the user.
- `NormalizedUserName`: The normalized username of the user.
- `Email`: The email of the user.
- `NormalizedEmail`: The normalized email of the user.
- `EmailConfirmed`: A flag indicating if the user's email is confirmed.
- `PasswordHash`: The hash of the user's password.
- `SecurityStamp`: A random value that should change whenever a user's credentials have changed.
- `ConcurrencyStamp`: A concurrency token used to ensure that two users editing the same data do not overwrite each other's changes.
- `PhoneNumber`: The user's phone number.
- `PhoneNumberConfirmed`: A flag indicating if the user's phone number is confirmed.
- `TwoFactorEnabled`: A flag indicating if two-factor authentication is enabled for this user.
- `LockoutEnd`: The date and time when any current lockout ends.
- `LockoutEnabled`: A flag indicating if the user can be locked out.
- `AccessFailedCount`: The number of failed login attempts.

### `AspNetUserClaims`

This table links users and claims.

Fields:
- `Id`: The ID of the user claim.
- `UserId`: The ID of the user associated with this claim.
- `ClaimType`: The claim type.
- `ClaimValue`: The claim value.

### `AspNetUserLogins`

This table stores information about user logins.

Fields:
- `LoginProvider`: The login provider (e.g., Facebook, Google).
- `ProviderKey`: The unique identifier for the user provided by the login provider.
- `ProviderDisplayName`: The display name of the login provider.
- `UserId`: The ID of the user associated with this login.

### `AspNetUserRoles`

This table links users and roles.

Fields:
- `UserId`: The ID of the user.
- `RoleId`: The ID of the role.

### `AspNetUserTokens`

This table stores tokens for users.

Fields:
- `UserId`: The ID of the user.
- `LoginProvider`: The login provider (e.g., Facebook, Google).
- `Name`: The name of the token.
- `Value`: The value of the token.

### `BannedUsers`

This table stores information about banned users.

Fields:
- `UserId`: The ID of the user.
- `Reason`: The reason for the ban.
- `BanStartTime`: The start time of the ban.
- `BanEndTime`: The end time of the ban.

### `Orders`

This table stores information about orders.

Fields:
- `Id`: The ID of the order.
- `CustomerId`: The ID of the customer who placed the order.
- `Status`: The status of the order.
- `CreatedAt`: The date and time when the order was created.
- `UpdatedAt`: The date and time when the order was last updated.

### `SellerUpgradeRequests`

This table stores information about seller upgrade requests.

Fields:
- `Id`: The ID of the request.
- `UserId`: The ID of the user who made the request.
- `Status`: The status of the request.
- `Message`: The message associated with the request.
- `DecidedAt`: The date and time when the request was decided.
- `SellerInfoId`: The ID of the seller info associated with the request.
- `CreatedAt`: The date and time when the request was created.
- `UpdatedAt`: The date and time when the request was last updated.

### `Stores`

This table stores information about stores.

Fields:
- `Id`: The ID of the store.
- `Name`: The name of the store.
- `Description`: The description of the store.
- `BannerUrl`: The URL of the store's banner.
- `LogoUrl`: The URL of the store's logo.
- `Address`: The address of the store.
- `Contacts`: The contacts of the store.
- `Email`: The email of the store.
- `OwnerId`: The ID of the user who owns the store.
- `IsVerified`: A flag indicating if the store is verified.
- `CreatedAt`: The date and time when the store was created.
- `UpdatedAt`: The date and time when the store was last updated.

### `BannedStores`

This table stores information about banned stores.

Fields:
- `Id`: The ID of the ban.
- `StoreId`: The ID of the store.
- `Reason`: The reason for the ban.
- `BanStartDate`: The start date of the ban.
- `BanEndDate`: The end date of the ban.
- `CreatedAt`: The date and time when the ban was created.
- `UpdatedAt`: The date and time when the ban was last updated.

### `Collections`

This table stores information about collections.

Fields:
- `Id`: The ID of the collection.
- `Name`: The name of the collection.
- `Description`: The description of the collection.
- `StoreId`: The ID of the store that owns the collection.
- `CreatedAt`: The date and time when the collection was created.
- `UpdatedAt`: The date and time when the collection was last updated.

### `Billboards`

This table stores information about billboards.
Billboard are used to be displayed on the home page of the store for the purpose of getting new client.

Fields:
- `Id`: The ID of the billboard.
- `Title`: The title of the billboard.
- `Subtitle`: The subtitle of the billboard.
- `ImageUrl`: The URL of the billboard's image.
- `CollectionId`: The ID of the collection that the billboard belongs to.
- `BillboardFilterId`: The ID of the billboard filter associated with the billboard.
- `CreatedAt`: The date and time when the billboard was created.
- `UpdatedAt`: The date and time when the billboard was last updated.

### `Products`

This table stores information about products.

Fields:
- `Id`: The ID of the product.
- `Name`: The name of the product.
- `Description`: The description of the product.
- `OccasionId`: The ID of the occasion associated with the product.
- `SizeChartImageUrl`: The URL of the product's size chart image.
- `Gender`: The gender for which the product is intended.
- `Season`: The season for which the product is intended.
- `SellerId`: The ID of the user who sells the product.
- `CollectionId`: The ID of the collection that the product belongs to.
- `SizeId`: The ID of the size of the product.
- `CreatedAt`: The date and time when the product was created.
- `UpdatedAt`: The date and time when the product was last updated.

### `OrderItems`

This table stores information about order items.

Fields:
- `Id`: The ID of the order item.
- `OrderId`: The ID of the order that the item belongs to.
- `ProductId`: The ID of the product.
- `ColorId`: The ID of the color of the product.
- `Quantity`: The quantity of the product.
- `SizeId`: The ID of the size of the product.
- `Status`: The status of the order item.
- `CreatedAt`: The date and time when the order item was created.
- `UpdatedAt`: The date and time when the order item was last updated.

### `ProductCategories`

This table links products and categories.

Fields:
- `ProductId`: The ID of the product.
- `CategoryId`: The ID of the category.
- `Order`: The order of the product in the category.
- `CreatedAt`: The date and time when the product was added to the category.
- `UpdatedAt`: The date and time when the product was last updated in the category.

### `ProductImages`

This table stores images for products.

Fields:
- `ProductId`: The ID of the product.
- `ColorId`: The ID of the color of the product.
- `ImageUrls`: The URLs of the images of the product.
- `CreatedAt`: The date and time when the image was added.
- `UpdatedAt`: The date and time when the image was last updated.

### `ProductMaterials`

This table links products and materials.

Fields:
- `ProductId`: The ID of the product.
- `MaterialId`: The ID of the material.
- `Percentage`: The percentage of the material in the product.
- `CreatedAt`: The date and time when the material was added to the product.
- `UpdatedAt`: The date and time when the material was last updated in the product.

### `ProductStocks`

This table stores stock information for products.

Fields:
- `ProductId`: The ID of the product.
- `ColorId`: The ID of the color of the product.
- `SizeId`: The ID of the size of the product.
- `Price`: The price of the product.
- `Stock`: The quantity of the product in stock.
- `Discount`: The discount on the product.
- `CreatedAt`: The date and time when the stock information was added.
- `UpdatedAt`: The date and time when the stock information was last updated.

### `Reviews`

This table stores reviews for products.

Fields:
- `Id`: The ID of the review.
- `ProductId`: The ID of the product that the review is for.
- `CustomerId`: The ID of the customer who wrote the review.
- `Title`: The title of the review.
- `Content`: The content of the review.
- `Rating`: The rating given by the customer.
- `CreatedAt`: The date and time when the review was created.
- `UpdatedAt`: The date and time when the review was last updated.

## Indexes

Several tables have indexes to improve query performance. For example, the `AspNetUsers` table has an index on the `NormalizedUserName` field, and the `AspNetRoleClaims` table has an index on the `RoleId` field.

## Relationships

There are several relationships between tables. For example, the `AspNetUserClaims` table has a foreign key to the `AspNetUsers` table, and the `AspNetUserRoles` table has foreign keys to both the `AspNetUsers` and `AspNetRoles` tables.