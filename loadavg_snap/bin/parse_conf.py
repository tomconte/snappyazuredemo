#!/usr/bin/python

import os.path
import sys
import yaml
import time

PKGNAME="azureloadavg"
CONFIG_FILE = os.path.join(os.environ["SNAPP_APP_DATA_PATH"], "conf.yaml")

# Wait for the config file to appear
# (the config hook runs only after the service is started)
while not os.path.exists(CONFIG_FILE):
    print >> sys.stderr, "waiting for " + CONFIG_FILE + " to appear"
    time.sleep(1)

with open(CONFIG_FILE) as fp:
    config = yaml.load(fp)

connectionstring = config.get("config", {}).get(PKGNAME, {}).get("connectionstring", "")
print "export CONNECTION_STRING=\"" + connectionstring + "\""

eventhubname = config.get("config", {}).get(PKGNAME, {}).get("eventhubname", "")
print "export EVENTHUB_NAME=\"" + eventhubname + "\""
