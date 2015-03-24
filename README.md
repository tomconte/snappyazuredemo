# Snappy Azure Demo

# Quick start

If you want to get started quickly, download the pre-built Snappy binary from the Releases tab.

https://github.com/tomconte/snappyazuredemo/releases/download/1.2/azureloadavg_1.2_all.snap

Create a configuration file e.g. `conf.yaml` that contains your Service Bus connection string and Event Hub name. There is a sample `example-conf.yaml` file at the root of this repository.

Install the Snap on an Ubuntu Core system:

```
sudo snappy install azureloadavg_1.0_all.snap --config conf.yaml
```

It should start right away and send load average data to your Event Hub.

# Compiling the demo from source code

Before you can compile the Ubuntu Core Snappy demo, you need to compile the Azure IoT C library (and its pre-requisites) for both ARM and X64. To that end, you should first prepare a cross-compiling environment that will allow you to build for both hardware architectures from a single Ubuntu VM.

The article "[cross-compilation on Ubuntu using sbuild](http://hypernephelist.com/2015/03/09/cross-compilation-made-easy-on-ubuntu-with-sbuild.html)" contains the instructions on how to setup this environment and compile the Apache Qpid Proton for ARM. Please first read this article and follow the instructions!

## Compiling the Azure IoT C library for ARM

Let's compile the Azure C library for the ARM target, using the chroot environment described in the article above. This will give us an ARM build of the library.

You should follow the same process as described in the article for Proton:

- copy the C library zip file to the `/build` directory in the chroot, via the mirror directory in `/var/lib/sbuild/build`.
- Use the `sbuild-shell` command to enter the chroot:

```
sudo sbuild-shell vivid-armhf
```

- You can then build the C library as usual from the `build_all/linux` directory:

```
bash ./build.sh -c
```

Everything should compile nicely for ARM.

Now let's compile and run the Event Hubs sample to make sure everything works; in the chroot, go to the sample directory and use the following command:

```
cd /build/az*/eventhub_client/samples/send
cc -I../../inc -I../../../common/inc send.c ../../../common/build/linux/common.a ../../../eventhub_client/build/linux/eventhub_client.a -L/usr/local/lib -lpthread -lcurl -lqpid-proton
```

Feedback: add an install target to Makefile to gather all include and lib files in one location, this will simplify the compiler options.

## Compiling the Azure IoT C library for x64

Building for x64 is of course straightforward. You only have to make sure to keep both arm and x64 binaries in separate locations so you can reference them separately, e.g.:

```
/var/lib/sbuild/build
	/arm/
		azure-iot-client-sdk-for-c-develop
		qpid-proton
	/x64/
		azure-iot-client-sdk-for-c-develop
		qpid-proton
```

e.g. build Proton like this:

```
bash build_proton.sh -s /var/lib/sbuild/build/x64/qpid-proton -i /usr/local
```

"Make install" will fail because of privileges, go to `/var/lib/sbuild/build/x64/qpid-proton/build` and
`sudo make install`.

Now Proton is installed in the same location in both environments (`/usr/local`).

You can compile the C library in the same way as before.

```
bash ./build.sh -c
```

Now you can go to the Event Hub sample and the same command line as above should produce an x64 executable:

```
cc -I../../inc -I../../../common/inc send.c ../../../common/build/linux/common.a ../../../eventhub_client/build/linux/eventhub_client.a -L/usr/local/lib -lpthread -lcurl -lqpid-proton
```

You now have two copies of the Azure IoT client libraries, one compiled for ARM and one for x64.

## Building the Snappy demo clients

Based on the above organisation, the Snappy demo repo has a sample build script you can use to build the arm and x64 executables from the same machine.

Install the Snappy development tools:

```
sudo add-apt-repository ppa:snappy-dev/beta
sudo apt-get update
sudo apt-get upgrade
sudo apt-get install snappy-tools
```

Get the demo source code from [https://github.com/tomconte/snappyazuredemo](https://github.com/tomconte/snappyazuredemo)

Check the paths in `src/build.sh` to make sure they point to the right locations for both arm (chroot) and x64.

To build the executables for x64:

```
cd src
./build.sh x64
```

To build the executables for ARM:

```
sudo sbuild-shell vivid-armhf
cd /build/snappyazuredemo/src
./build.sh arm
```

To build the snaps, from the top-level directory, just run "make". This should produce two packages:

- `loadavg_1.0_all.snap`
- `loadavgarm_1.0_all.snap`

You can install these packages on an Ubuntu Core machine manually or using snappy-remote.

The packages are configured to run as services and should start automatically sending telemetry data to Event Hubs.
