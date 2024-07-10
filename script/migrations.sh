dotnet ef migrations add AddAccountInfo  --context Hzg.Data.AccountDbContext -p ../hzg/hzg.csproj
dotnet ef database update  --context Hzg.Data.AccountDbContext -p ../hzg/hzg.csproj