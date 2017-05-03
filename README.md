# Remote executor
Simple tool to execute stuff on your PC from your smartphone.
Probably too simple, since it took less time to write the tool itself than to write the readme on Github.

# Usage
1. Run the exe.
1. Create an arbitrary amount of arbitrary-named .txt files next to the exe (see bin/Debug/Test1.txt as an example). Odd lines are captions, even lines are commands. Empty lines are ignored.
1. Point any device to "http://*your-host-name*:81/RD/*your-text-file-name-without-extension*/".
E.g., http://localhost:81/RD/Test1/  
Pushing a button will run the corresponding command.
1. Make a QR out of that link using any online QR generator and print it :)

# Notes
I had few issues with putty when trying to run a list of commands on a remote target. I.e. we had a smartphone as a controller, a PC hosting this app and another remote PC we wanted to run commands on via putty. Eventually I had to create separate .txt files for each of the command lists. So the original .txt files for the executor would look like this:

~~~~
Debug ON
plink.exe 10.0.0.5 -l root -pw 1234 -m debug-on.txt

Debug OFF
plink.exe 10.0.0.5 -l root -pw 1234 -m debug-off.txt
~~~~

Where debug-on.txt contains actual commands, e.g.:

~~~~
date
uptime
python -c "print('Hi')"
~~~~

Notice the usage on plink instead of putty.
