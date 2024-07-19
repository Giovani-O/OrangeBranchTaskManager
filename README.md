# Orange Branch Task Manager
.NET Web API developed for the Orange Branch program at FCamara. This API allows sign up of a user that will be able to add tasks to a list, 
there is also a worker watching a RabbitMQ queue, this worker is responsible for sending emails when a new user signs up or a task is created,
updated or deleted.

## How to run
There are a few steps you need to follow in order to execute the API and the worker.
First, you need [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), [RabbitMQ](https://www.rabbitmq.com/docs/download) and [MySQL 8.0.38](https://dev.mysql.com/downloads/mysql/8.0.html).

You will also need to configure an app password for the email you will be using as sender.

### Environment variables
You will need to set some environment variables that are used to send the emails:
```
ORANGE_TASKS_SMTP_SERVER 
ORANGE_TASKS_SMTP_PORT
ORANGE_TASKS_SENDER_EMAIL
ORANGE_TASKS_SENDER_APP_PASSWORD
```
You can find more information about SMTP server configuration [here](https://support.google.com/a/answer/176600?hl=en).

### Connection string and migrations
You will need to change the connection string according to your MySQL settings. The connection string is in `src/OrangeBranchTaskManager.Api/appsettings.json`.

Also, inside the `src` directory, you will have to run the following command to apply the database migration:
```bash
dotnet ef database update --startup-project .\OrangeBranchTaskManager.Api\ --project .\OrangeBranchTaskManager.Infrastructure\
```

### Execution
Finally, you will need to start both `OrangeBranchTaskManager.Api` and `Emailing.Worker`. You can do that with Visual Studio or JetBrains Rider, or you can run them in the command line by using `dotnet watch run` inside their respective directories.
