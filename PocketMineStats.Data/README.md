## How to update the database schema
Database schema is managed by [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/). To update the database schema, update the DB models, then run the following command from the PocketMineStats.Web directory:
```
dotnet ef migrations add DescriptionHere --project ..\PocketMineStats.Data\
```

The DB changes will be automatically applied when the server is started, or to manually
apply them, run the following command from the PocketMineStats.Web directory:
```
dotnet ef database update --project ..\PocketMineStats.Data\
```

## Mapping between Database and API models
[AutoMapper](https://github.com/AutoMapper/AutoMapper) is used to map between the database models and the API models.  

The mapping configuration is located in the `PocketMineStats.Web\Profiles\MappingProfile.cs` file.
AutoMapper is configured to make sure there's a mapping for every source > destination model configured in the `Profile`.  

When updating the DB Schema, either the API models will need to be updated to add the property to the model, or the mapping configuration will need to be updated to ignore or map the property. If the name on API model matches the name on the DB model, it will be automatically mapped.