#!/bin/bash

current_build=$(grep -o "<Version>0.2.\(.*\)<\/Version>" ./src/Initium/Initium.csproj | sed 's/<Version>0.2.\(.*\)<\/Version>/\1/')
new_build=$(printf "%d" $((10#$current_build + 1)))
sed -i "s/<Version>0.2.$current_build<\/Version>/<Version>0.2.$new_build<\/Version>/" ./src/Initium/Initium.csproj

# Write the new build to the environment file
echo "new_build=0.2.$new_build" >> $GITHUB_ENV

echo "Version updated to 0.2.$new_build"
