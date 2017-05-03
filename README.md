# Remote executor
Simple tool to execute stuff on your PC from your smartphone.
Probably too simple, since it took less time to write the tool itself than to write the readme on Github.

# Usage
1. Run exe
1. Create an arbitrary amount of arbitrary-named .txt files next to the exe (see default Test1 as an example). Odd lines are captions, even lines are commands. Empty lines are ignored.
1. Point any device to "http://{your-host-name}:81/RD/{you-text-file-name-without-extension}/". E.g., http://localhost:81/RD/Test1/
Pushing a button will run that command.
