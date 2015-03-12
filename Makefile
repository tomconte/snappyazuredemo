all:
	cp src/eventhub_demo_arm loadavg_arm_snap/bin/eventhub_demo
	cp src/eventhub_demo_x64 loadavg_snap/bin/eventhub_demo
	snappy build loadavg_arm_snap
	snappy build loadavg_snap

clean:
	rm -f src/eventhub_demo_arm loadavg_arm_snap/bin/eventhub_demo
	rm -f src/eventhub_demo_x64 loadavg_snap/bin/eventhub_demo
	rm -f loadavg_1.0_all.snap loadavgarm_1.0_all.snap
