# User Managment API Introduction

This .NET API project is designed to show what user management looks like in a secure way. This API has Register, Login, Update Profile and Get Profile functions. After Registration or Authorization, you will have JWT Token which is necessary to have access to your profile. This API uses Swagger UI for easy managment and testing. For Database access, this one uses EntityFramework Core to access SQLServer.

# Technologies 

.NET 8, C# 12, EF Core Identity, SQL Server, Fluent Validator, Serilog, Swagger, JWT Token.

# Steps to run code 

To run this code you need any IDE (integrated development environment) for example : Visual Studio 2022.

After download and open this project in your IDE, you need to build whole Project.

Then find appsetting.json in UserManagmentAPI project and change this connection string by your connection string to have access your SQL Database:

"ConnectionStrings": {
   "DefaultConnection": "Server=localhost;Database=UserManagmentDB;TrustServerCertificate=True;Integrated Security=True;"
 }

Then open Package Manager Console and write this code : update-database. you can open your SQL Server and check if Database is added.

After you will run project, there will add default User with Admin role in Database.

Email : admin@example.com and Password : Admin@123

As I said in Introduction we will get JWT token after LogIn or Registration : 

![image](https://github.com/merabmos/UserManagmentAPI/assets/48407417/68433884-35f3-4410-997c-c5ab39278360)

Then we need to enter this JWT token in Authorize section.

![image](https://github.com/merabmos/UserManagmentAPI/assets/48407417/daea6fcd-d89f-4e0f-9619-679f7a25fec9)

![image](https://github.com/merabmos/UserManagmentAPI/assets/48407417/371648fe-e538-47fd-bdbd-aed681a9edda)

After this we are Authorized as trusted user and you can have access to your Profile.

Here we should say that only User and Admin roles can have access to their profile so in registration you should specify which you are.

![image](https://github.com/merabmos/UserManagmentAPI/assets/48407417/a8178f84-23fb-489c-aa85-f692c03e0a44)

So after this steps you can successfully access to your profile.

![image](https://github.com/merabmos/UserManagmentAPI/assets/48407417/cbbeaa43-0af1-4312-88f1-167023002585)

![image](https://github.com/merabmos/UserManagmentAPI/assets/48407417/9dd09556-ad1d-4e1b-a85c-e70a2eb616ec)



