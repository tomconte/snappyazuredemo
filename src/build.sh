#!/bin/bash

# Location of the Azure IoT client library
IOTCLIENT=~/iot/azure-iot-client-sdk-for-c-develop

gcc -I$IOTCLIENT/eventhub_client/inc -I$IOTCLIENT/common/inc $IOTCLIENT/eventhub_client/src/*.o $IOTCLIENT/common/src/*.o $IOTCLIENT/common/adapters/*.o ./eventhub_demo.c -L/home/azureuser/qpid-proton/build/proton-c -lqpid-proton -lcurl -lpthread -o eventhub_demo
