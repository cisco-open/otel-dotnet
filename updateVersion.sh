#!/bin/bash

echo "::set-output name=new_release_version::$1"
sed "s#<PackageVersion>.*#<PackageVersion>$1</PackageVersion>#" $2
sed "s#<Version>.*#<Version>$1</Version>#" $2
