# Forum Website Readme

This repository contains the source code for a Forum website developed using .NET Core, ASP.NET, Entity Framework, Microsoft SQL Server, and MVC architecture.

## Functionality

The Forum website provides the following functionality:

### User Roles

- **Admin**: Admin users have elevated privileges, including the ability to manage users, accept or decline posts, and perform administrative actions.
- **User**: Registered users can post topics, write comments, and engage in discussions.
- **Guest**: Guests can only view topics and discussions without the ability to post or comment.

### Features

- **User Authentication**: Users can register, log in, and log out securely.
- **Post Topics**: Both admins and users can create new discussion topics.
- **Write Comments**: Users and admins can participate in discussions by writing comments.
- **Admin Privileges**: Admins have special permissions such as banning users, accepting or declining posts, and managing user accounts.
- **Guest Access**: Guests can browse existing topics and discussions without the need to register.

## Technologies Used

- **.NET Core**: Provides the framework for building web applications.
- **ASP.NET**: Used for building web applications and APIs using the .NET framework.
- **Entity Framework**: An ORM (Object-Relational Mapping) framework for .NET Core.
- **Microsoft SQL Server**: Used as the backend database to store user data, posts, and comments.
- **MVC Architecture**: Used to organize the application structure into Model, View, and Controller components.

## Setup Instructions

To run the Forum website locally, follow these steps:

1. Clone this repository to your local machine.
2. Install .NET Core SDK if you haven't already (https://dotnet.microsoft.com/download).
3. Open the solution file in Visual Studio or your preferred IDE.
4. Update the connection string in the `appsettings.json` file to point to your local SQL Server instance.
5. Build the solution and resolve any dependencies.
6. Run the application.

## Contributing

Contributions are welcome! If you have any suggestions, feature requests, or bug reports, please open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code for your own purposes.
