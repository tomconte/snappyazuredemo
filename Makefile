CHROOTNAME = vivid-armhf
CHROOTPATH = /build/snappyazuredemo/src

all: compile
	cp src/eventhub_demo_x64 loadavg_snap/bin/x86_64-linux-gnu/eventhub_demo
	cp src/eventhub_demo_arm loadavg_snap/bin/arm-linux-gnueabihf/eventhub_demo
	snappy build loadavg_snap

compile:
	cd src; ./build.sh x64
	schroot -c $(CHROOTNAME) -d $(CHROOTPATH) -- build.sh arm

clean:
	rm -f src/eventhub_demo_arm loadavg_arm_snap/bin/arm-linux-gnueabihf/eventhub_demo
	rm -f src/eventhub_demo_x64 loadavg_snap/bin/x86_64-linux-gnu/eventhub_demo
	rm -f azureloadavg_1.0_all.snap
