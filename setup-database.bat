@echo off
echo Setting up database...
dotnet ef migrations add InitialCreate
dotnet ef database update
echo Database setup complete!
pause