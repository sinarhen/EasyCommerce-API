# TODO

## Controllers
- [x] AdminController
	- [x] `GetAllUsers`: Retrieves all users in the system.
	- [x] `DeleteUser`: Deletes a user from the system based on the provided user ID.
	- [x] `GetUserById`: Retrieves a specific user based on the provided user ID.
	- [x] `BanUser`: Bans a specific user based on the provided user ID.
	- [x] `UnbanUser`: Unbans a specific user based on the provided user ID.
	- [x] `GetBannedUsers`: Retrieves all banned users in the system.
	- [x] `UpdateUserRole`: Updates the role of a specific user based on the provided user ID and role.
	- [x] `GetUpgradeRequests`: Retrieves all seller upgrade requests in the system.
	- [x] `GetUpgradeRequestById`: Retrieves a specific seller upgrade request based on the provided request ID.
	- [x] `UpdateUpgradeRequestStatus`: Updates the status of a specific seller upgrade request based on the provided request ID and status.
- [x] AuthController
	- [x] `Register`: Registers a new user in the system.
	- [x] `Login`: Logs in a user. Finds the user by email, checks if the password is correct, generates a JWT token for the user and returns it.
	- [x] `ValidateToken`: Validates a JWT token. Retrieves the token from the Authorization header, validates it and returns the principal.
	- [x] `ChangePassword`: Changes the password of a user.
	- [x] `ChangeEmail`: Changes the email of a user.
- [x] BillboardController
	- [x] `GetBillboardsForCollection: Retrieves all billboards for a specific collection.
	- [x] `CreateBillboardForCollection`: Creates a new billboard for a specific collection. 
	- [x] `UpdateBillboard` :  Updates a specific billboard.
	- [x] `DeleteBillboard`: Deletes a specific billboard from a specific collection. 
- [x] CategoryController
	- [x] `GetCategories`: Retrieves all categories. 
	- [x] `GetCategory`: Retrieves a specific category based on the provided ID. 
	- [x] `CreateCategory`: Creates a new category.
	- [x] `UpdateCategory` - Updates a specific category based on the provided ID and DTO.
	- [x] `DeleteCategory`- Deletes a specific category based on the provided ID.
- [x] CollectionRepository
	- [x] `CreateCollectionAsync`: Creates a new collection.
	- [x] `DeleteCollectionAsync`: Deletes a specific collection based on the provided ID. 
	- [x] `GetCollectionByIdAsync`: Retrieves a specific collection based on the provided ID. 
	- [x] `GetRandomCollectionsAsync`: Retrieves a random set of collections. It returns the collections as a list.
	- [x] `UpdateCollectionAsync`: Updates a specific collection based on the provided ID and DTO. 
	- [x] `GetCollectionsAsync`: Retrieves collections based on the provided search parameters.
- [x] `CustomerController`
	- [x] `GetReviewsForUser`: Retrieves all reviews for the currently authenticated user.
	- [x] `UpgradeToSeller`: Allows the currently authenticated user to request an upgrade to a seller.
	- [X] GetCartForUser - Retrieves cart for user.
    - [X] AddToCart - Adds product to cart.
    - [X] RemoveFromCart - Removes product from cart.
    - [X] UpdateProductCart - Updates cart.
	- [X] ClearCart - Clears cart.
    - [X] ConfirmCart - Changes order status to confirmed.
	- [ ] GetOrdersForUser - Retrieves all orders made by user.
- [x] GenericController
- [x] `ProductController`
	- [x] `GetProducts`: Retrieves a list of products based on the provided search parameters.
	- [x] `GetProduct`: Retrieves a specific product by its ID.
	- [x] `CreateProduct`: Creates a new product. This action is only allowed for users with the Seller role.
	- [x] `UpdateProduct`: Updates a specific product by its ID. This action is only allowed for users with the Seller role.
	- [x] `DeleteProduct`: Deletes a specific product by its ID. This action is only allowed for users with the Seller role.
- [x] `ReviewController`
	- [x] `CreateReviewForProduct`: Creates a new review for a specific product. This action is only allowed for authenticated users.
	- [x] `DeleteReview`: Deletes a specific review. This action is only allowed for authenticated users.
- [x] StoreController
- [ ] SellerController
	- [X] GetSellerInfo - Retrieves all products for a specific seller.
    - [ ] GetOrdersForSeller - Retrieves all orders made by user.
    - [ ] GetSellsInfoForSeller - Retrieves all sells info for seller.
- [ ] OrderController
- [ ] CartController 

## Config

### Files
- [x] `ClaimTypes.cs` - used for custom claim types.
- [x] `Policies.cs` - consistent authorization policies name
- [x] `Secrets.cs` - static class which gets all required environment variables 
- [x] `SimplePrincipal.cs` - Dto with list of claims
- [x] `UserRoles.cs` - consistent role names.

## Services
- [x] JWT Service

## Data
- [x] Database context and migrations
- [x] Seed data

## Repositories
### Admin
- [x] IAdminRepository.cs (Interface)
- [x] AdminRepository.cs (Interface implementation)
### Auth
- [x] IAuthRepository.cs (Interface)
- [x] AuthRepository.cs (Interface implementation)
### Billboard
- [x] IBillboardRepository.cs (Interface)
- [x] BillboardRepository.cs (Interface implementation)
### Category
- [x] ICategoryRepository.cs (Interface)
- [x] CategoryRepository.cs (Interface implementation)
### Collection
- [x] ICollectionRepository.cs (Interface)
- [x] CollectionRepository.cs (Interface implementation)
### Customer
- [x] ICustomerRepository.cs (Interface)
- [x] CustomerRepository.cs (Interface implementation)
### Generic
- [x] IGenericRepository.cs (Interface)
- [x] GenericRepository.cs (Interface implementation)
### Product
- [x] IProductRepository.cs (Interface)
- [x] ProductRepository.cs (Interface implementation)
### Review
- [x] IReviewRepository.cs (Interface)
- [x] ReviewRepository.cs (Interface implementation)
### Seller
- [x] ISellerRepository.cs (Interface)
- [x] SellerRepository.cs (Interface implementation)
### Store
- [x] IStoreRepository.cs (Interface)
- [x] StoreRepository.cs (Interface implementation)
### Order
- [ ] IOrderRepository.cs (Interface)
- [ ] OrderRepository.cs (Interface implementation)
### Cart
- [ ] ICartRepository.cs (Interface)
- [ ] CartRepository.cs (Interface implementation)

## Other
- [x] Error handling and logging
- [ ] API documentation (Swagger, etc.)

## Notes
- No need for separate controllers for `ProductStock`, `ProductCategory`, `ProductImage`, `ProductMaterial`, `Material`, `CategorySize`, `CartProduct`, `OrderDetail`, `Color`, `Size`, `Review`, `Occasion` as these are handled by related controllers.