# kafka-windows-service-wrapper
A Windows service wrapper for Apache Kafka

[![Build status](https://ci.appveyor.com/api/projects/status/mtk4aiu8ur6u0sqd?svg=true)](https://ci.appveyor.com/project/ngeor/kafka-windows-service-wrapper)

![Kafka and ZooKeeper installed as Windows Services](./services.png?raw=true "Kafka and ZooKeeper installed as Windows Services")

## Installation

- Build the project using Visual Studio
- Install with installutil.exe (launch Developer Command Prompt for VS2015 as Administrator)

```
installutil KafkaWindowsServiceWrapper.exe
```

## Uninstall

- Stop the services if they're running
- Uninstall completely with installutil.exe

```
installutil /u KafkaWindowsServiceWrapper.exe
```
