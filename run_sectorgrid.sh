#!/bin/bash

# Run 'dotnet build' and capture its output
output=$(dotnet build 2>&1)

# Check if 'Build FAILED' is in the output
if echo "$output" | grep -q "Build FAILED"; then
    # If 'Build FAILED' is found, print the output and do not run the second command
    echo "Build failed. Output:"
    echo "$output"
else
    # If 'Build FAILED' is not found, run the second command
    godot --path /home/sina/code/ldjam_2024 --scene res://main.tscn --verbose
fi

