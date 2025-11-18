## QzHalleyClient
# Overview
QzHalleyClient is a .NET-based client application designed to work alongside the QzHalley server, providing users with an intuitive interface to authenticate, take quizzes, and view their results. This project is part of the QzHalley ecosystem, focusing on the user experience for completing educational or training quizzes managed by an administrator.
Requirements
```
Space: At least 2 MB of disk space.
RAM: Minimum 64 MB.
Software: .NET SDK (version 9.0 or later) must be installed on your system.
Server: The QzHalleyAPI server (from https://github.com/AkoForU/QzHalley) must be running on the local network.
```
## Installation

Clone the repository to your local machine:

    git clone https://github.com/AkoForU/QzHalleyClient.git
    cd QzHalleyClient/Main/

Build and Run

Publish the application using the .NET CLI:

    dotnet publish

You can also use the folder from the Release build to transfer the application to other laptops.

Alternatively, use:

    dotnet run

from the project directory after building to start the application directly.

## Usage

Authentication: Launch the client, enter the server IP (e.g., 192.168.X.X as shown in the server console), and authenticate with your username and password provided by the administrator.
Quiz: Once authenticated, start the test and select answers from the provided options. The result will be displayed upon completion.
Exit: Use the "Close" button to return to the main screen or exit the application.

## Notes

Ensure the QzHalleyAPI server is active before running the client. Check the server console for the correct IP and port.
The client is designed for single-use quiz completion per account, as managed by the administrator.

## Contributing
Feel free to fork this repository, submit issues, or create pull requests to enhance QzHalleyClient. Contributions are welcome!
