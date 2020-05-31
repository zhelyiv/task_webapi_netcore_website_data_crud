Prerequisite:
VS 2019
.NET Core 3.1
dotnet-ef if not: dotnet tool install --global dotnet-ef
In case of problems with running the application or creating its database see https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=netcore-cli

Creating Database: 
1)Open WebsiteManagement.sln in VS 2019
2)Open Package Manager Console and execute:

	dotnet ef database update --project DAL.Ef

Or open command prompt and execute:
	cd [<project location>]\WebsiteManagement\DAL.Ef
	dotnet ef database update
	
Running the project:
1) Set WebsiteManagement.WebAPI as startup project
2) Select "WebsiteManagement.WebAPI" launch configuration
3) The project will start as self hosted app on http://localhost:5000/swagger/index.html in your default browser
The open api json could be found here http://localhost:5000/swagger/v1/swagger.json
