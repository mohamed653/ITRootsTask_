# ITRootsTask

## Installation
To run the project, you will need to have the following installed:

* Visual Studio 2019 or later
* .NET Core SDK 3.1 or later
* Microsoft SQL Server 2014 or later

## Packages Used
![image](https://user-images.githubusercontent.com/61664713/228144310-85f4f480-e33d-4c55-be7f-89207b129e27.png)

Once you have these installed, you can follow these steps to set up the system:

1. Clone the repository to your local machine.
2. Open the solution file in Visual Studio.
3. Build the solution to restore any missing packages.
4. In the appsettings.json file, update the connection string to match your SQL Server configuration.
5. Open the Package Manager Console and run the following command to create the database:


```
Add-Migration -outputdir data/migration
```

```
Update-Database
```
6. Run the application in Visual Studio.


## Technologies Used

- ASP.NET Core
- C#
- Entity Framework core
- Microsoft SQL Server
- HTML
- CSS
- Bootstrap
- JavaScript
- Ajax
-Jquery
- Code First approach
