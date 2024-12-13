# Overview

The `CreateServerSocketLogicAnalyzer` will create a TCP server socket.  
`Miscellaneous.Logic_Probe` are attached to ports and pins of the micro emulation.  
When the emulation is running and a transition of the `Logic_Probe` is detected, a TCP message is transmitted containing a timestamp, probe identifier, and the current state of the pin.

## What's Missing

You need to write an application that will open a TCP socket to the server and capture the messages being sent.  
You could save them to a file to analyze later.  
My intention was to feed the data into Sigrok PulseView, but I have not gotten very far with this.  
The main stumbling block is that PulseView doesn’t seem to have many TCP SUMP devices, and I would have to add that to PulseView first.

## Quickstart

1. Run `Runme.sh`.
2. In another terminal, open a TCP client to the LogicAnalyzer TCP server:
   ```
   nc localhost 4567
   ``` 
3. Run the emulation by typing start in the Renode terminal.
4. You should see the output on the nc terminal.

## Details on the Configuration Files
ToDo
