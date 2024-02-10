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
- [x] BillboardController
- [x] CategoryController
- [x] CollectionController
- [x] CustomerController
- [x] GenericController
- [x] ProductController
- [x] ReviewController
- [x] SellerController
- [x] StoreController
- [ ] OrderController
- [ ] CartController 

## Config

### Files
- [x] `ClaimTypes.cs` - used for custom claim types.
- [x] `Policies.cs` - consistent authorization policies name
- [x] `Secrets.cs` - static class which gets all required environment variables 
- [x] `SimplePrincipal.cs` - Dto with list of claims
- [x] `UserRoles.cs` - consistent role names.

## Models
### DTOs
- [ ] Complete DTOs folder
### Entities 
- [ ] Complete Entities folder
### Enum
- [ ] Complete Enum folder

## Services
- [x] JWT Service
- [ ] Other services (based on your application requirements)

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