#!/bin/sh
set -e

find "$(pwd -P)/tests" -maxdepth 1 -type d -name "*${TEST_TYPE}" | while read -r i
do
    cd "$i"
    dotnet test
done
