# dotnetapi
DDD API in .NET Core

A simple .NET Core API following the Domain Driven Design Pattern

Dapper is used as ORM.

A RabbitMQ mechanism to push messages to queue is also given.

The infrastructure includes the find, create, update, and delete abstract methods for each model in database. These methods can also be overridden when specific functionality is needed.

This project consists of one model, the upload model, which represents an uploaded file.

The infrastructure assumes a connection to Postgres but can be easily modified.

This project can be easily scaled to meet any needs by following the same logic for each model in our Domain.

Swagger is used for documentation. The documentation is produced automatically by following the logic in Uploads controller. Controllers can also be exluded from swagger.

Custom error codes are included and can be extended in Config/CustomErrorCodes.cs

Authentication is also provided through API tokens listed in appsettings.json

The database assumes a column named 'active' when finding or removing objects for demo purposes instead of actually removing entries.


### API calling - Get supposed uploaded file with id 1 from our database
Method: Get<br />
URL: /api/Uploads/1<br />
content-type: application/json<br />
authorization: apitesting


To setup a RabbitMQ listener use the following example:<br>
```C#
private void InitializeRabbitMqListener()
{
    var factory = new ConnectionFactory
    {
        HostName = _hostname,
        UserName = _username,
        Password = _password
    };

    _connection = factory.CreateConnection();
    _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    _channel = _connection.CreateModel();
    _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
} 
protected override Task ExecuteAsync(CancellationToken stoppingToken)
{
    stoppingToken.ThrowIfCancellationRequested();

    var consumer = new EventingBasicConsumer(_channel);
    consumer.Received += (ch, ea) =>
    {
        var content = Encoding.UTF8.GetString(ea.Body.ToArray());
        var updateFileNameModel = JsonConvert.DeserializeObject<UpdateFileNameModel>(content);

        HandleMessage(UpdateFileNameModel);

        _channel.BasicAck(ea.DeliveryTag, false);
    };
    consumer.Shutdown += OnConsumerShutdown;
    consumer.Registered += OnConsumerRegistered;
    consumer.Unregistered += OnConsumerUnregistered;
    consumer.ConsumerCancelled += OnConsumerCancelled;

    _channel.BasicConsume(_queueName, false, consumer);

    return Task.CompletedTask;
} 
```
