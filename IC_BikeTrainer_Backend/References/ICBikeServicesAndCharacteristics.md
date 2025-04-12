### Generic Access Profile
 - UUID: ``00001800-0000-1000-8000-00805f9b34fb``
 - Characteristics:
   - Device Name
     - UUID: ``00002a00-0000-1000-8000-00805f9b34fb``
     - Properties: read, write
   - Appearance
     - UUID: ``00002a01-0000-1000-8000-00805f9b34fb``
     - Properties: read, write
   - Peripheral Preferred Connection Parameters
     - UUID: ``00002a04-0000-1000-8000-00805f9b34fb``
     - Properties: read
   - Resolvable Private Address Only
     - UUID: ``00002ac9-0000-1000-8000-00805f9b34fb``
     - Properties: read

### Generic Attribute Profile
 - UUID: ``00001801-0000-1000-8000-00805f9b34fb``
 - Characteristics:
   - Service Changed
     - UUID: ``00002a05-0000-1000-8000-00805f9b34fb``
     - Properties: read, indicate

### Device Information
 - UUID: ``0000180a-0000-1000-8000-00805f9b34fb``
 - Characteristics:
   - Manufacturer Name String
     - UUID: ``00002a29-0000-1000-8000-00805f9b34fb``
     - Properties: read
   - Model Number String
     - UUID: ``00002a24-0000-1000-8000-00805f9b34fb``
     - Properties: read
   - Hardware Revision String
     - UUID: ``00002a27-0000-1000-8000-00805f9b34fb``
     - Properties: read
   - Firmware Revision String
     - UUID: ``00002a26-0000-1000-8000-00805f9b34fb``
     - Properties: read
   - Software Revision String
     - UUID: ``00002a28-0000-1000-8000-00805f9b34fb``
     - Properties: read
   - PnP ID
     - UUID: ``00002a50-0000-1000-8000-00805f9b34fb``
     - Properties: read

### Heart Rate
 - UUID: ``0000180d-0000-1000-8000-00805f9b34fb``
 - Characteristics:
   - Heart Rate Measurement
     - UUID: ``00002a37-0000-1000-8000-00805f9b34fb``
     - Properties: notify
   - Body Sensor Location
     - UUID: ``00002a38-0000-1000-8000-00805f9b34fb``
     - Properties: read

### Cycling Speed and Cadence
 - UUID: ``00001816-0000-1000-8000-00805f9b34fb``
 - Characteristics:
   - CSC Measurement
     - UUID: ``00002a5b-0000-1000-8000-00805f9b34fb``
     - Properties: notify
   - CSC Feature
     - UUID: ``00002a5c-0000-1000-8000-00805f9b34fb``
     - Properties: read
   - Sensor Location
     - UUID: ``00002a5d-0000-1000-8000-00805f9b34fb``
     - Properties: read

### Fitness Machine
 - UUID: ``00001826-0000-1000-8000-00805f9b34fb``
 - Characteristics:
   - Fitness Machine Feature
     - UUID: ``00002acc-0000-1000-8000-00805f9b34fb``
     - Properties: read
   - Indoor Bike Data
     - UUID: ``00002ad2-0000-1000-8000-00805f9b34fb``
     - Properties: notify
   - Training Status
     - UUID: ``00002ad3-0000-1000-8000-00805f9b34fb``
     - Properties: read, notify
   - Supported Resistance Level Range
     - UUID: ``00002ad6-0000-1000-8000-00805f9b34fb``
     - Properties: read
   - Fitness Machine Control Point
     - UUID: ``00002ad9-0000-1000-8000-00805f9b34fb``
     - Properties: write-without-response, write, indicate
   - Fitness Machine Status
     - UUID: ``00002ada-0000-1000-8000-00805f9b34fb``
     - Properties: notify

### Vendor specific
 - UUID: ``0000fff0-0000-1000-8000-00805f9b34fb``
 - Characteristics:
   - Vendor specific
     - UUID: ``0000fff1-0000-1000-8000-00805f9b34fb``
     - Properties: notify
   - Vendor specific
     - UUID: ``0000fff2-0000-1000-8000-00805f9b34fb``
     - Properties: write-without-response, write

### Unknown
 - UUID: ``02f00000-0000-0000-0000-00000000fe00``
 - Characteristics:
   - Unknown
     - UUID: ``02f00000-0000-0000-0000-00000000ff03``
     - Properties: read
   - OTA Response
     - UUID: ``02f00000-0000-0000-0000-00000000ff02``
     - Properties: read, notify
   - Unknown
     - UUID: ``02f00000-0000-0000-0000-00000000ff00``
     - Properties: read
   - Unknown
     - UUID: ``02f00000-0000-0000-0000-00000000ff01``
     - Properties: write-without-response, write
