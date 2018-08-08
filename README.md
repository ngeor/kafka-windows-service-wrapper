# kafka-windows-service-wrapper
A Windows service wrapper for Apache Kafka

[![Build status](https://ci.appveyor.com/api/projects/status/mtk4aiu8ur6u0sqd?svg=true)](https://ci.appveyor.com/project/ngeor/kafka-windows-service-wrapper)

![Kafka and ZooKeeper installed as Windows Services](./services.png?raw=true "Kafka and ZooKeeper installed as Windows Services")

## Prerequisites
It is expected that you've already downloaded and unzipped the Kafka zip bundle from the official website.
Additionally, it is assumed that you haven't modified any files or the folder structure.

## Installing using the prebuilt binaries
AppVeyor is a continuous integration tool that produces binaries for this project.

- Download the [latest binary and configuration file](https://github.com/ngeor/kafka-windows-service-wrapper/releases/latest).
- Place them in a folder together
- Make sure you **edit** the config file (see following section about Configuration)
- Open a command prompt with Administrator privileges
- From the folder where you placed the binary, run `KafkaWindowsServiceWrapper -install`

## Installation using Visual Studio
This method is more useful for developers who want to modify the code.

- Build the project using Visual Studio
- Install with installutil.exe (launch Developer Command Prompt for VS2015 as Administrator)

```
installutil KafkaWindowsServiceWrapper.exe
```

## Configuration Settings

The configuration file contains various settings. The most important is the location where you have installed Kafka.

- KafkaInstallationDirectory: The directory in which Kafka is installed. It is assumed that you have simply unziped the Kafka zip bundle with no modifications.

The following settings are used to detect that ZooKeeper is listening for connections. This way, we don't start Kafka prematurely:

- ZooKeeperHost: The host that ZooKeeper is expected to be listening for connections (127.0.0.1 by default).
- ZooKeeperPort: The port of ZooKeeper (2181 by default).
- ZooKeeperConnectTimeout: How long to wait, in seconds, to establish a connection to ZooKeeper (10 seconds by default).
- ZooKeeperMaxConnectAttemptCount: How many times should the service attempt to connect to ZooKeeper (3 times by default).

The following setting is used to avoid race conditions when restarting services too fast:

- CoolingTimeout: How long to wait, in seconds, after launching an external process such as ZooKeeper and Kafka (5 seconds by default).

## Uninstall
Uninstall will remove the services from your system.

- Stop the services if they're running
- Open a command prompt with Administrator privileges
- Run `KafkaWindowsServiceWrapper -uninstall`
