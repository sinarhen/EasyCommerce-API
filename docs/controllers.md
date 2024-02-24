# 📚 Controllers Documentation

Detailed API documentation for the controller endpoints can be found after running the application and visiting the `/swagger` endpoint. 🚀

How to run the application is described in the [README](../README.md). 📖

## 📝 Some General Notes

- `[ServiceFilter(typeof(ValidationServiceFilter))]` is used to validate the request body. 🧾 The `ValidationService` implementation can be found [here](../src/Api/Services/ValidationService.cs).

- `[Authorize]` is used to protect the endpoint. The user must be authenticated to access the endpoint. 🔐

- `[Authorize(Policy = Policies.AdminPolicy)]` is used to protect the endpoint. The user must be authenticated and have the `Admin` role to access the endpoint. 👮‍♂️ Available policies can be found [here](../src/Api/Config/Policies.cs).