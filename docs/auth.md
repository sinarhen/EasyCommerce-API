## 🔒 Authentication and Authorization

In the EasyCommerce-API project, we handle authentication and authorization using a combination of ASP.NET Core Identity and JWT (JSON Web Tokens).

- **ASP.NET Core Identity** 📝: Manages user registration, data validation, and role management.

- **JWT** 🎫: Handles user authentication. A JWT is generated upon user login and is included in the header of subsequent requests for authentication.

- **Authorization** 🔐: Managed through policies based on user roles. The roles, defined in the [`UserRoles.cs`](../src/Api/Config/UserRoles.cs) file, include `Admin`, `Seller`, `Customer`, and `SuperAdmin`.

- **Policies** 📜: Defined in the [`Policies.cs`](../src/Api/Config/Policies.cs) file, include `AdminPolicy`, `SellerPolicy`, `CustomerPolicy`, and `SuperAdminPolicy`. These policies restrict access to certain parts of the application based on the user's role.

- **JwtService** 🛠️: Responsible for generating, writing, and validating JWTs. It uses the `JwtSecrets` record to store the JWT issuer, key, and audience. The `GenerateToken`, `WriteToken`, and `ValidateToken` methods are used for these purposes. More details can be found in the [`JwtService.cs`](../src/Api/Services/JwtService.cs) file.

