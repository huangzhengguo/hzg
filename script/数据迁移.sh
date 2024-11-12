# 如果运行失败，在主项目中运行该命令
dotnet ef migrations add 名称  --context Admin.Data.AdminDbContext -p ../adminapi/admin.csproj
dotnet ef database update  --context Admin.Data.AdminDbContext -p ../adminapi/admin.csproj

dotnet ef migrations add 名称  --context Hzg.Data.AccountDbContext -p ../hzg/hzg.csproj
dotnet ef database update  --context Hzg.Data.AccountDbContext -p ../hzg/hzg.csproj