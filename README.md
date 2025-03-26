# OrderAPI

OrderAPI is an API developed for handling order operations. The project supports functionalities such as product listing, order creation, and asynchronous email sending via RabbitMQ.


## Technologies
- **Framework**: .NET 6  
- **Language**: C#  
- **Database**: MySQL  
- **ORM**: Entity Framework (Code First)  
- **Memory Cache**: Redis  
- **Message Queue**: RabbitMQ  
- **Tools**: AutoMapper, Serilog  

## Workflow
1. **Product Listing**: The API provides a GET method to list products. It first checks Redis cache; if not found, it fetches the data from the database and caches it.
2. **Order Creation**: An order is created via a POST method. Order details are saved in the database, and an email sending request is added to the RabbitMQ queue.
3. **Email Sending**: Email requests in the RabbitMQ queue are processed by a background service.

## Project Structure
The project follows the Onion Architecture:
- **Domain**: Entity classes
- **Application**: Business logic, service interfaces and DTOs
- **Infrastructure**: RabbitMQ services, background services and other infrastructure components
- **Persistence**: Handles all database operations
- **WebAPI**: Hosts the API endpoints

## RabbitMQ Usage
- **PublishToQueue**: Sends messages to the specified queue
- **Consume**: Listens to the specified queue and processes incoming messages

## Setup
### Requirements
- .NET SDK 6+
- Docker (for Redis)
- MySQL

### Run the Project
1. Clone the repository:
   ```bash
   git clone https://github.com/huseyinerdin/OrderAPI.git
   cd OrderAPI
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Set up Redis using Docker
   ```bash
   docker run -d --name my-redis -p 6379:6379 redis
   ```
4. Configuration : Edit  `appsettings.json` with your RabbitMQ, MySQL, and Redis connection settings:
    ```json
    "ConnectionStrings": {
            "MySqlConnection": "server=localhost;port=3306;database=OrderDb;user=your_username;password=your_password;"
        },
    "RabbitMq": {
            "HostName": "localhost",
            "Port": "5672",
            "UserName": "guest",
            "Password": "guest"
        },
    "Redis": {
            "ConnectionString": "localhost:6379"
        }
    ```
5. Run the application:
   ```bash
   dotnet run
   ```

## Contributing
Feel free to open an issue or submit a pull request if you'd like to contribute.

## License
This project is licensed under the MIT License.