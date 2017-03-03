# kafka-windows-service-wrapper
A Windows service wrapper for Apache Kafka

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
