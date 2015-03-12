# Snappy Azure Demo

Preparing the environment: read [cross-compilation on Ubuntu using sbuild](http://hypernephelist.com/2015/03/09/cross-compilation-made-easy-on-ubuntu-with-sbuild.html)

Before you can compile the Ubuntu Core Snappy demo, you need to compile the Azure IoT C library for both ARM and X64.

## Compiling the Azure IoT C library for ARM

Let's compile the Azure C library in the chroot environment described in the article above. This will give us an ARM build of the library.

Same process as described in the article for Proton, copy the C library zip file to the `/build` directory in the chroot, via the mirror directory in `/var/lib/sbuild/build`.

Use the shell command to enter the chroot:

```
sudo sbuild-shell vivid-armhf
```

You can then build the C library as usual:

```
bash ./build.sh -c
```

Everything should compile nicely for ARM.

Now let's compile and run the Event Hubs sample to make sure everything works; in the chroot:

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

To build the executables:

```
x64:
cd src
./build.sh x64
```

```
arm:
sudo sbuild-shell vivid-armhf
cd /build/src
./build.sh arm
```

To build the snaps, from the top-level directory, just run "make".

This will produce two snapps. You can install them on an Ubuntu Core machine manually or using snappy-remote.

